// Тема: LINQ
// Автор: Попов Адександр Павлович
// Уровень: Junior
// Основной вопрос: Что будет выведено на консоль при выполнении данного кода?
// Ответ: Будет выведено \nIntersected: [5, 6, 7]\nExcepted: [1, 2, 3, 4, 8]
// Дополнительные вопросы:
// С какими типами можно использовать Except() и Intersect()? Нужны ли какие-то дополнительные действия для корректной работы?
// Сталкивался ли ты с использованием этих методов? В каких ситуациях применял?

using System;
using System.Collections.Generic;
using System.Linq;

var firstCollection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
var secondCollection = new int[] { 5, 6, 7 };

var intersected = firstCollection.Intersect(secondCollection)
    .ToArray();

Console.WriteLine($"Intersected: [{string.Join(", ", intersected)}]");

var excepted = firstCollection.Except(secondCollection)
    .ToArray();
Console.WriteLine($"Excepted: [{string.Join(", ", excepted)}]");
