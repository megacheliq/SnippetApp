//Тема - Ссылочные и значимые типы, оператор ==, Equals
// Автор: Карташов Д.С.
// Уровень: Middle
// Основной вопрос: Что будет выведено на консоль при выполнении данного кода? Почему?
// Ответ: Будет выведено
// Пример 1. False, по умолчанию оператором == сравниваются ссылки
// Пример 2. True, вопрос со * - объяснить почему в итоге происходит сравнение по значению
// Пример 3. True, вопросы - почему true, что требует определить IEquatable (Equals), вопрос со * - зачем переопределили Equals(object),
// вопрос с ** - зачем метод GetHashCode 

using System;

object o1 = 5, o2 = 5;
var isEqual1 = o1 == o2;
Console.WriteLine($"Пример 1. {isEqual1}");

var isEqual2 = o1.Equals(o2);
Console.WriteLine($"Пример 2. {isEqual2}");

Subject s1 = new Subject()
{
    Id = 1,
    Name = "aaa"
};

Subject s2 = new Subject()
{
    Id = 1,
    Name = "aaa"
};
var isEqual3 = s1.Equals(s2);
Console.WriteLine($"Пример 3. {isEqual3}");

public class Subject : IEquatable<Subject>
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Уникальный Id
    /// </summary>
    public ulong Id { get; set; }

    public bool Equals(Subject? other)
    {
        if (other == null)
            return false;

        if (Name == other.Name && Id == other.Id)
            return true;
        else
            return false;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Subject);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
