// Тема: Асинхронное программирование
// Автор: Попов Адександр Павлович
// Уровень: Junior
// Основной вопрос: Что будет выведено на консоль при выполнении данного кода? Почему?
// Ответ: Будет выведено
// Start main
// Hello from second Task
// End main
// поскольку у вызова PrintFirstAsync() отсутствует await (программа заврешится раньше ожидания),
// а PrintSecondAsync() выполнится синхронно
// Дополнительные вопросы:
// 1) Как сделать так чтобы был вывод на консоль из метода PrintFirstAsync()?
// 2) Какие типы данных можно использовать в качестве возвращаемого значения асинхронных методов?
// 3) Для каких операций использование асинхронности принесет максимальную выгоду в производительности?
// 4) Считается ли метод асинхронным, если в определении метода используется ключевое слово async? 

using System;
using System.Threading.Tasks;

Console.WriteLine("Start main");
PrintFirstAsync();
PrintSecondAsync();
Console.WriteLine("End main");

async Task PrintFirstAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Hello from the first Task");
}

async Task PrintSecondAsync()
{
    Console.WriteLine("Hello from the second Task");
}



