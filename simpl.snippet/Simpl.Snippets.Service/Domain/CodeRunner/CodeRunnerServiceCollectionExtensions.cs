using Simpl.Snippets.Service.Domain.CodeRunner.Abstract;
using Simpl.Snippets.Service.Domain.CodeRunner.Services;

namespace Simpl.Snippets.Service.Domain.CodeRunner
{
    public static class CodeRunnerServiceCollectionExtensions
    {
        public static IServiceCollection AddCodeRunnerFactory(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<CSharpCodeRunnerStrategy>();
            services.AddTransient<ICodeRunnerFactory, CodeRunnerFactory>();

            return services;
        }
    }
}
