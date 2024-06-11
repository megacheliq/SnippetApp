// Тема: Dependency Injection and Minimal API
// Автор: Варзаносов Павел Викторович
// Уровень: Middle
// Основной вопрос: Что будет выведено при запросе метода /GetGuids? Что будет в результате запроса?
// Ответ: true, потому что зарегистрированы разные инстансы одного и того же класса под разными интерфейсами
// Дополнительные вопросы:
// 1) Что можно сделать, чтобы получить гуиды из класса DefaultGuids? (сменить имя, зарегать отдельно)

//using System.Collections.Generic;
//using System;
//using System.Text;

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
//app.MapGet("/GetGuids", (IEnumerable<IGuid> guids, IInstallDefault defGuid) =>
//{
//    StringBuilder sb = new StringBuilder();
//    foreach (var guid in guids)
//    {
//        sb.Append(guid.GetGuid().ToString());
//        sb.Append(" ");
//    }
//    defGuid.InstallDefault();
//    return $"Guids: {sb}";
//});

//app.Run();

//public interface IGuid
//{
//    Guid GetGuid();
//}
//public interface IInstallDefault
//{
//    void InstallDefault();
//}

//public class RandomGuid : IGuid, IInstallDefault
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

//    public void InstallDefault()
//    {
//        _value = new Guid("00000000-0000-0000-0000-000000000000");
//    }
//}

//public class DefaultGuids : IGuid
//{
//    private Guid _value;
//    public DefaultGuids()
//    {
//        _value = new Guid("00000000-0000-0000-0000-000000000000");
//    }

//    public Guid GetGuid()
//    {
//        return _value;
//    }
//}