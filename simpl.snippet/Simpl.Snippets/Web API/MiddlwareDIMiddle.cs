// Тема: Dependency Injection and Middleware
// Автор: Варзаносов Павел Викторович
// Уровень: Middle
// Основной вопрос: Что будет выведено в консоль при запросе метода /GetGuids?
// Ответ: 1 сообщение из LoggerConsoleMiddleware, затем сообщение из LoggerTimeMiddleware, затем сообщение из метода, затем сообщение из LoggerTimeMiddleware, затем сообщение из LoggerConsoleMiddleware
// Дополнительные вопросы:
// 1) Как поменять порядок вызовов?
// 2) Можно ли получить экземпляр сервиса RandomGuid в middleware?
// 3) Что нужно изменить в коде, чтобы получение Guid сделать в одном из middleware?

//using System.Collections.Generic;
//using System.Net.Http;
//using System;
//using System.Text;
//using System.Threading.Tasks;

//var builder = WebApplication.CreateBuilder(args);

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.Scan(scan =>
//    scan.FromAssemblyOf<Program>()
//    .AddClasses(c => c.Where(type => type.Name.EndsWith("Guid", StringComparison.InvariantCultureIgnoreCase)))
//    .AsImplementedInterfaces()
//    .WithTransientLifetime());

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.MapGet("/GetGuids", (IEnumerable<IGuid> guids) =>
//{
//    StringBuilder sb = new();
//    foreach (var guid in guids)
//    {
//        sb.Append(guid.GetGuid().ToString());
//        sb.Append(" ");
//    }
//    Console.WriteLine($"Guids: {sb}");
//    return $"Guids: {sb}";
//});

//app.UseMiddleware<LoggerConsoleMiddleware>();
//app.UseMiddleware<LoggerTimeMiddleware>();
//app.Run();

//public interface IGuid
//{
//    Guid GetGuid();
//}

//public class RandomGuid : IGuid
//{
//    private Guid _value;
//    public RandomGuid()
//    {
//        _value = Guid.NewGuid();
//    }

//    public Guid GetGuid()
//    {
//        return _value;
//    }
//}
//public class DefaultGuid : IGuid
//{
//    private Guid _value;
//    public DefaultGuid()
//    {
//        _value = new Guid("00000000-0000-0000-0000-000000000000");
//    }

//    public Guid GetGuid()
//    {
//        return _value;
//    }
//}

//public class LoggerConsoleMiddleware
//{
//    private readonly RequestDelegate _next;

//    public LoggerConsoleMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        Console.WriteLine($"Выполнен запрос по пути: {context.Request.Path}");
//        await _next.Invoke(context);
//        Console.WriteLine($"Закончено выполнение запроса по пути: {context.Request.Path}");
//    }
//}
//public class LoggerTimeMiddleware
//{
//    private readonly RequestDelegate _next;

//    public LoggerTimeMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Path.Value == "/GetGuids")
//        {
//            Console.WriteLine($"Начало выполнения запроса: {DateTime.Now.ToString()}");
//            await _next.Invoke(context);
//            Console.WriteLine($"Окончание выполнения запроса: {DateTime.Now.ToString()}");
//        }
//        else
//        {
//            await _next.Invoke(context);
//        }
//    }
//}