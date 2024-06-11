using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Simpl.Snippets.Service.Domain.CodeRunner.Abstract;
using Simpl.Snippets.Service.Domain.CodeRunner.Models;
using Simpl.Snippets.Service.Exceptions.Models;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading.Tasks.Dataflow;

namespace Simpl.Snippets.Service.Domain.CodeRunner.Services
{
    public class CSharpCodeRunnerStrategy : ICodeRunnerStrategy
    {
        private ILogger<CSharpCodeRunnerStrategy> Logger { get; }

        private ActionBlock<RunSnippetRequest> RunningQueue { get; }

        public CSharpCodeRunnerStrategy(ILogger<CSharpCodeRunnerStrategy> logger)
        {
            Logger = logger;

            RunningQueue = new ActionBlock<RunSnippetRequest>(r =>
            {
                try
                {
                    var output = RunSnippetCode(r.SnippetCode);
                    r.OutputTaskSource.SetResult(output);
                }
                catch (Exception e)
                {
                   r.OutputTaskSource.SetException(e);
                }                              
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
        }

        public async Task<string> RunSnippetCodeAsync(string codeSnippet, CancellationToken cancellationToken = default)
        {
            var outputTaskSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            var request = new RunSnippetRequest(codeSnippet, outputTaskSource);
            await RunningQueue.SendAsync(request, cancellationToken);

            var output = await outputTaskSource.Task;

            return output;
        }

        private string RunSnippetCode(string snippetCode)
        {
            Logger.LogInformation("Start compiling snippet");
            var compilation = CompileCodeSnippet(snippetCode);
            Logger.LogInformation("Snippet is compiled");

            using var peStream = GetCompilationStreamOrThrow(compilation);
            Logger.LogInformation("Compiled assembly is valid");

            var output = string.Empty;
            var assemblyReference = LoadAndExecuteAssembly(peStream, Array.Empty<string>(), s => output = s);

            Logger.LogInformation("Compiled assembly is executed");

            DisposeAssemby(assemblyReference);

            return output;
        }

        private static CSharpCompilation CompileCodeSnippet(string codeSnippet)
        {
            var parsedSyntaxTree = GetSyntaxTree(codeSnippet, default);
            var references = GetReferences();

            var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithOverflowChecks(true)
                .WithAssemblyIdentityComparer(AssemblyIdentityComparer.Default);

            var compilation = CSharpCompilation.Create(
                "Custom.dll",
                syntaxTrees: new[] { parsedSyntaxTree },
                references: references,
                options: compilationOptions);

            return compilation;
        }

        private static SyntaxTree GetSyntaxTree(string codeSnippet, CancellationToken cancellationToken)
        {
            var codeSource = SourceText.From(codeSnippet);
            var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp10);
            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeSource, parseOptions, cancellationToken: cancellationToken);

            return parsedSyntaxTree;
        }

        private static List<MetadataReference> GetReferences()
        {
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            };

            foreach (var assembly in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                references.Add(MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            return references;
        }

        private static MemoryStream GetCompilationStreamOrThrow(CSharpCompilation compilation)
        {
            var peStream = new MemoryStream();
            var result = compilation.Emit(peStream);
            peStream.Position = 0;

            if (result.Success)
                return peStream;

            var failures = result.Diagnostics
                .Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error)
                .Select(d => $"{d.Id}: {d.GetMessage()}");

            throw new BusinessLogicException(string.Join("\n", failures));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference LoadAndExecuteAssembly(Stream compiledAssembly, string[] args, Action<string> outputFunc)
        {
            var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();
            var assembly = assemblyLoadContext.LoadFromStream(compiledAssembly);
            var entry = assembly.EntryPoint;

            var output = string.Empty;
            using (_ = new ConsoleOutputInterceptor(s => outputFunc(s)))
            {
                _ = entry != null && entry.GetParameters().Length > 0
                    ? entry.Invoke(null, new object[] { args })
                    : entry.Invoke(null, null);
            }

            assemblyLoadContext.Unload();

            return new WeakReference(assemblyLoadContext);
        }

        private void DisposeAssemby(WeakReference assemblyReference)
        {
            Logger.LogInformation("Start assembly unloading");

            for (var i = 0; i < 16 && assemblyReference.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Logger.LogInformation(assemblyReference.IsAlive ? "Unloading failed!" : "Unloading successful!");
        }



        private class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
        {
            public SimpleUnloadableAssemblyLoadContext()
                : base(true)
            {
            }

            protected override Assembly Load(AssemblyName assemblyName) => null;
        }

        private record RunSnippetRequest(string SnippetCode, TaskCompletionSource<string> OutputTaskSource);
    }
}
