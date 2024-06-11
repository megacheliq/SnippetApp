// Тема: Асинхронное программирование
// Автор: Попов Адександр Павлович
// Уровень: Senior
// Основной вопрос: Скомпилируется ли данный код? Почему?
// Ответ: Код скомпилируется из-за утиной типизации (duck typing) в .NET при поиске реализованных методов во время работы CLR.
// Благодаря этому можно сделать асинхронным либой класс или структуру, если явно или неявно (через методы расширения) реализуется метод GetAwaiter(),
// возвращающий TaskAwaiter
// Дополнительные вопросы:
// 1) В чем отличие Task и ValueTask? Какие есть нюансы в использовании ValueTask?
// 2) Для чего используется класс TaskCompletionSource?  
// 3) Что такое контекст синхронизации и как используется в .Net? Почему от его использования отказались в .Net Core?
// 4) Расскажите с чем вы сталкивались и с чем работали при использовании асинхронного программирования.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Simpl.Snippets;

public class Program
{
    public static async Task Main(string[] args)
    {
        var well = new Well();
        well.StartProduction();

        var oilProduced = await well;
        Console.WriteLine(oilProduced);
        Console.ReadKey();
    }
}

public class Well
{
    private const int PupmProduction = 100;

    private TaskCompletionSource<double> TaskCompletionSource { get; }
    private double OilDebit { get; }

    public Well()
    {
        OilDebit = Random.Shared.Next(1, 1001);
        TaskCompletionSource = new TaskCompletionSource<double>();
    }

    public void StartProduction()
    {
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(OilDebit / PupmProduction));
            TaskCompletionSource.SetResult(OilDebit);
        });
    }

    public TaskAwaiter<double> GetAwaiter() => TaskCompletionSource.Task.GetAwaiter();
}










