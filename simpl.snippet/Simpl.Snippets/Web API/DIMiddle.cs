// Тема: Dependency Injection and Minimal API
// Автор: Варзаносов Павел Викторович
// Уровень: Middle
// Основной вопрос: Что будет выведено при запросе метода /GetGuids? Что будет в результате запроса?
// Ответ: true, потому что зарегистрированы разные инстансы одного и того же класса под разными интерфейсами
// Дополнительные вопросы:
// 1) Как получить разные guid?

//using System;

//var builder = WebApplication.CreateBuilder(args);

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<IGuid, RandomGuid>();
//builder.Services.AddTransient<IInstallDefault, RandomGuid>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.MapGet("/GetGuids", (IGuid guid, IInstallDefault defaultGuid) =>
//{
//    var guid1 = guid.GetGuid();
//    defaultGuid.InstallDefault();
//    var guid2 = guid.GetGuid();
//    return $"Guid from IHasGuid: {guid1 == guid2}";
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
