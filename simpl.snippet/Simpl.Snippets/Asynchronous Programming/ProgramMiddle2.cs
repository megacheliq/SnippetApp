// Тема: Асинхронное программирование
// Автор: Попов Адександр Павлович
// Уровень: Middle
// Основной вопрос: Что будет выведено на консоль при выполнении данного кода? Почему?
// Ответ: Будет выведено 24832
// 2 Будет выведено из-за Task.WhenAny - t1 завершится успешно, остальные будут отменены первым CancellationToken
// 4832 Будет выведено из-за Task.WhenAll - так как выполнятся все задачи, второй CancellationToken не повлияет на работу программы
// Дополнительные вопросы:
// 1) Измениться ли консольный вывод, если не вызывать отмену через tokenSource1? Почему?
// 2) Кратко расскажи что происходит при использовании async/await?
// 3) Приходилось ли тебе использовать различные паттерны отмены и/или ожидания задач? Расскажи самый интересный кейс с которым пришлось столкнуться.

using System;
using System.Threading;
using System.Threading.Tasks;

using var tokenSource1 = new CancellationTokenSource();
using var tokenSource2 = new CancellationTokenSource();

var t1 = CalculateAsync(1, tokenSource1.Token);
var t2 = CalculateAsync(2, tokenSource1.Token);
var t3 = CalculateAsync(3, tokenSource1.Token);
var t4 = CalculateAsync(2, tokenSource2.Token);
var t5 = CalculateAsync(5, tokenSource2.Token);
var t6 = CalculateAsync(3, tokenSource2.Token);

await Task.WhenAny(t1, t2, t3);
tokenSource1.Cancel();

await Task.WhenAll(t4, t5, t6);
tokenSource2.Cancel();


async Task CalculateAsync(int number, CancellationToken cancellationToken)
{
    await Task.Delay(number * 1000, cancellationToken);

    Console.Write(Math.Pow(2, number));
}
