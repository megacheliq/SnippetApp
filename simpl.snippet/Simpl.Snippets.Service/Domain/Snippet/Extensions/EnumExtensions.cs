using System.ComponentModel;
using System.Reflection;
using Simpl.Snippets.Service.Domain.Snippet.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.Extensions
{
    /// <summary>
    /// Класс расширений для перечислений
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Получить описание значения перечисления
        /// </summary>
        /// <param name="value">Значение перечисления</param>
        /// <returns>Описание значения перечисления</returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Получить описания всех элементов перечисления
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления</typeparam>
        /// <returns>Коллекция объектов EnumValueDto, содержащих идентификаторы и названия элементов перечисления</returns>
        public static IEnumerable<ItemDto> GetDescriptions<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().Select(item => new ItemDto { Id = Convert.ToInt64(item), Name = item.GetDescription() });
        }
    }
}
