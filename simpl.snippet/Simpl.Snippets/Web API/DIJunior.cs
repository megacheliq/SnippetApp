// Тема: Dependency Injection and Minimal API
// Автор: Варзаносов Павел Викторович
// Уровень: Junior
// Основной вопрос: Что будет выведено при запросе метода /GetGuids? Будут ли совпадать значения "Guid from IHasGuid" and "Guid from Service" при первом вызове? а при втором?
// Ответ: AddTransient - разные при каждом вызове; AddScoped - одинаковые в рамках одного вызова, но разные в рамках разных вызовов; AddSingleton - одинаковые во всех вызовах, пока запущено приложение;
//AddScoped и AddSingleton - ошибка; AddSingleton, AddScoped - аналогично AddSingleton
// Дополнительные вопросы:
// 1) Почему 4 вариант регистрации выдаст ошибку?
// 2) Почему 5 вариант регистрации работает аналогично 3 варианту?
// 3) Расскажи самый интересный кейс с которым пришлось столкнуться при регистрации зависимостей.

//var builder = WebApplication.CreateBuilder(args);

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddTransient<IHasGuid, RandomGuid>();
//builder.Services.AddTransient<GuidService>();

//builder.Services.AddScoped<IHasGuid, RandomGuid>();
//builder.Services.AddScoped<GuidService>();

//builder.Services.AddSingleton<IHasGuid, RandomGuid>();
//builder.Services.AddSingleton<GuidService>();

//Доп. кейсы
//builder.Services.AddScoped<IHasGuid, RandomGuid>();
//builder.Services.AddSingleton<GuidService>();

//builder.Services.AddSingleton<IHasGuid, RandomGuid>();
//builder.Services.AddScoped<GuidService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//app.UseSwagger();
//app.UseSwaggerUI();
//}
//app.MapGet("/GetGuids", (IHasGuid guid, GuidService counterService) =>
//{
//return $"Guid from IHasGuid: {guid.Value}; Guid from Service: {counterService.Guid.Value}";
//});

//app.Run();

//public interface IHasGuid
//{
//    Guid Value { get; }
//}

//public class RandomGuid : IHasGuid
//{
//    private Guid _value;
//    public RandomGuid()
//    {
//        _value = Guid.NewGuid();
//    }
//    public Guid Value
//    {
//        get { return _value; }
//    }
//}
//public class GuidService
//{
//    protected internal IHasGuid Guid { get; }
//    public GuidService(IHasGuid guid)
//    {
//        Guid = guid;
//    }
//}

