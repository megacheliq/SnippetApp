using Simpl.Snippets.Service.Domain.CodeRunner.Abstract;
using Simpl.Snippets.Service.Domain.CodeRunner.Models;

namespace Simpl.Snippets.Service.Domain.CodeRunner.Services
{
    public class CodeRunnerFactory : ICodeRunnerFactory
    {
        private IServiceProvider ServiceProvider { get; }

        public CodeRunnerFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async Task<string> RunCodeSnippetAsync(SnippetLanguage snippetLanguage, string snippetCode, CancellationToken cancellationToken = default)
        {
            ICodeRunnerStrategy strategy = snippetLanguage switch
            {
                SnippetLanguage.CSharp => ServiceProvider.GetRequiredService<CSharpCodeRunnerStrategy>(),

                _ => throw new NotImplementedException(),
            };

            var result = await strategy.RunSnippetCodeAsync(snippetCode, cancellationToken);

            return result;
        }
    }
}
