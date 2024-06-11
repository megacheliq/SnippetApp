namespace Simpl.Snippets.Service.Domain.CodeRunner.Models
{
    public class ConsoleOutputInterceptor : IDisposable
    {
        private static readonly TextWriter DefaultConsoleOut = Console.Out;

        private StringWriter Writer { get; }
        private Action<string> LogFunction { get; }

        public ConsoleOutputInterceptor(Action<string> logFunction)
        {
            Writer = new StringWriter();
            Console.SetOut(Writer);
            LogFunction = logFunction;
        }

        public void Dispose()
        {
            Console.SetOut(DefaultConsoleOut);
            Writer.Flush();
            LogFunction?.Invoke(Writer.ToString());
            Writer.Dispose();
        }
    }
}
