// Тема: Асинхронное программирование
// Автор: Попов Адександр Павлович
// Уровень: Middle
// Основной вопрос: Что будет выведено на консоль при выполнении данного кода? Почему?
// Ответ: Будет выведено
// False
// True
// True
// поскольку FirstTaskAsync() отработает асинхронно, поэтому произойдет переключение потоков и продолжение работы будет выполнено в другом потоке
// SecondTaskAsync() выполнится синхронно, поэтому переключение потоков не произойдет и продолжение выполнится в том же потоке
// ThirdTaskAsync() запустится и отработает асинхронно, но так как после запуска основной поток заснет, на время большое чем время ожидания внутри задачи,
// то к моменту его пробуждения задача уже будет выполнена, результат будет получен синхронно, и продолжение выполнится в том же потоке
// Дополнительные вопросы:
// 1) Кратко расскажи что происходит при использовании async/await?
// 2) Для чего используется конструкция ConfigureAwait? Нужно ли её использовать в .net core 3.1 и более новых фреймворках? Ответ пояснить. 
// 3) Безопасно ли использовать void в качестве возращаемого типа в асинхронных методах?

using System;
using System.Threading;
using System.Threading.Tasks;

var initialThreadId = Environment.CurrentManagedThreadId;

var firstTask = FirstTaskAsync();
var secondTask = SecondTaskAsync();

await firstTask.ConfigureAwait(false);
var afterFirstTaskThreadId = Environment.CurrentManagedThreadId;

await secondTask.ConfigureAwait(false);
var afterSecondTaskThreadId = Environment.CurrentManagedThreadId;

var t3 = ThirdTaskAsync();
Thread.Sleep(100);
await t3;
var afterThirdTaskThreadId = Environment.CurrentManagedThreadId;

Console.WriteLine(initialThreadId == afterFirstTaskThreadId);
Console.WriteLine(afterSecondTaskThreadId == afterFirstTaskThreadId);
Console.WriteLine(afterThirdTaskThreadId == afterSecondTaskThreadId);


async ValueTask<int?> FirstTaskAsync()
{
    // Simulate complicated computations
    await Task.Delay(1000);
    return 2 + 2;
}

Task<double> SecondTaskAsync() => Task.FromResult(Math.Pow(2, 11));

async Task<double> ThirdTaskAsync()
{
    await Task.Delay(10);
    return Math.Pow(2, 5);
}









