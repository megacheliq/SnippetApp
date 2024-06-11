// Тема: LINQ
// Автор: Попов Адександр Павлович
// Уровень: Middle
// Основной вопрос: Что будет выведено на консоль при выполнении данного кода?
// Ответ:
// Simpl: 3
// Microsoft: 1
// Дополнительные вопросы:
// 1) Как переписать данный запрос, используя методы расширения?
// 2) В чем разница отложенного и немедленного выполнения LINQ? Приведите примеры функций.
// 3) Приходилось ли вам использовать операторы запросов LINQ? Если да, то для каких задач

using System;
using System.Collections.Generic;
using System.Linq;

var person = new List<(int Id, string Name)>() { (1, "Tom"), (2, "John"), (3, "Jack"), (4, "Sean") };
var companies = new List<(int Id, string Name)>() { (1, "Simpl"), (2, "Microsoft") };
var mapping = new List<(int PersonId, int CompanyId)> { (1, 1), (2, 1), (3, 1), (4, 2) };

var query = from p in person
            join m in mapping on p.Id equals m.PersonId
            join c in companies on m.CompanyId equals c.Id
            let i = (Person: p.Name, Company: c.Name)
            group i by i.Company into gr
            where gr.Key.StartsWith("S") || gr.Key.EndsWith("t")
            let r = new { Company = gr.Key, Count = gr.Count() }
            orderby r.Count descending
            select r;

foreach (var item in query)
{
    Console.WriteLine($"{item.Company}: {item.Count}");
}
