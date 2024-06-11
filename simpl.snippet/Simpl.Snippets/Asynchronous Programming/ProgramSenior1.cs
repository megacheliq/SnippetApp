// Тема: Асинхронное программирование
// Автор: Попов Адександр Павлович
// Уровень: Senior
// Основной вопрос: Скомпилируется ли данный код? Почему?
// Ответ: Код скомпилируется, так как благодаря утиной типизации в .NET при поиске методов во время работы CLR.
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
        await TimeSpan.FromSeconds(2);
        await 500.Milliseconds();
        await 1000;
    }
}

public static class Extensions
{
    public static TaskAwaiter GetAwaiter(this TimeSpan time)
    {
        return Task.Delay(time).GetAwaiter();
    }

    public static TimeSpan Milliseconds(this int number)
    {
        return TimeSpan.FromMilliseconds(number);
    }

    public static ValueTaskAwaiter GetAwaiter(this int number)
    {
        var task = new ValueTask(Task.Delay(number));

        return task.GetAwaiter();
    }
}









