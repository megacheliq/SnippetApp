// Тема: SOLID
// Автор: Попов Адександр Павлович
// Уровень: Middle
// Основной вопрос: Ознакомьтесь со снипетом и скажите какие нарушения принципов SOLID там имеются
// Ответ: Нарушены все буквы акронима
// S - ProductionOilfieldService и отдает данные и считает показатели и кэширует значения
// O - нереализованный метод в InjectionOilfiledService, при появлении в GetKeyIndicatorByWellId
// сервиса ProductionOilfieldService новых показателей постоянно нужно модифицировать код
// L - ProductionOilfieldService и InjectionOilfiledService нельзя использовать через абстракцию (интерфейс),
// т.к. имеется нереализованный метод и может упасть ошибка
// I - через интерфейс сервиса можно выполнять много несвязанных одной логикой операций (и достать скважины и посчитать какие-то значения)
// D - оба сервиса зависят от репозитория. При смене его реализации придется менять код
// Дополнительные вопросы:
// 1) Как бы ты отрефаторил код, чтобы усранить нарушения принципов? 
// 2) Привиди примеры нарушения принципов SOLID с которыми вы сталкивались. 
// 3) Какие еще принципы проектирования кроме SOLID ты знаешь?
// 4) Расскажи что такое инверсия зависимостей?

using System;
using System.Collections.Generic;
using System.Linq;

namespace Simpl.Snippets.SOLID;

public record Well(int Id, string Code, double OilProducion, double WaterProduction, double WaterInjection);

public interface IOilfieldService
{
    Well GetWell(int id);
    double ComputeAverageWatercut();
    double GetKeyIndicatorByWellId(int id, long type);
}

public class ProductionOilfieldService : IOilfieldService
{
    private WellRepository Repository { get; }

    private Dictionary<int, Well> Cache { get; } = new Dictionary<int, Well>();

    public ProductionOilfieldService()
    {
        Repository = new WellRepository();
    }

    public double ComputeAverageWatercut()
    {
        FillCacheIfEmpty();

        var oil = Cache.Values.Sum(x => x.OilProducion);
        var water = Cache.Values.Sum(x => x.WaterProduction);

        return water / (water + oil) * 100;
    }

    public double GetKeyIndicatorByWellId(int id, long type)
    {
        var well = GetWell(id);

        return type switch
        {
            1 => well.OilProducion,
            2 => well.WaterProduction,
            _ => throw new NotImplementedException()
        };
        ;
    }

    public Well GetWell(int id)
    {
        FillCacheIfEmpty();

        return Cache[id];
    }

    public void FillCacheIfEmpty()
    {
        if (Cache.Count != 0)
            return;

        foreach (var well in Repository.GetAll())
        {
            Cache.Add(well.Id, well);
        } 
    }
}

public class InjectionOilfiledService : IOilfieldService
{
    private WellRepository Repository { get; }

    public InjectionOilfiledService()
    {
        Repository = new WellRepository();
    }

    public double ComputeAverageWatercut()
    {
        throw new NotSupportedException("На месторождении нет добывающих скважин");
    }

    public double GetKeyIndicatorByWellId(int id, long type)
    {
        return GetWell(id).WaterInjection;
    }

    public Well GetWell(int id)
    {
        if (id % 5 != 0)
            throw new Exception("Некорректный Id скважины");

        return Repository.Get(id);
    }
}

public class WellRepository
{
    private List<Well> Wells => Enumerable.Range(0, 10)
        .Select(id => new Well(
            id,
            $"ABC{id}",
            id % 5 == 0 ? 0 : 10,
            id % 5 == 0 ? 0 : 20,
            id % 5 == 0 ? 500 : 0))
        .ToList();

    public Well Get(int id) => Wells[id];

    public IEnumerable<Well> GetAll() => Wells;
}
