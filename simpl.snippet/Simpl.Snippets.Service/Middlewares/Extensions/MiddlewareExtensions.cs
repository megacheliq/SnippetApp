namespace Simpl.Snippets.Service.Middlewares.Extensions
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Интеграция <see cref="ExceptionMidleware"/> в конвеер asp
        /// </summary>
        /// <param name="app">Приложение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UseJsonException(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware<ExceptionMidleware>();
        }

        /// <summary>
        /// Использовать переопределение базового пути запроса
        /// </summary>
        /// <param name="app">Приложение</param>
        /// <param name="configuration">Конфигурация</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UseConfigPathBase(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            app.UsePathBase(new PathString(configuration["AppName"]));
        }
    }
}
