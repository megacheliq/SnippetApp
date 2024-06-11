using System.ComponentModel;

namespace Simpl.Snippets.Service.DataAccess.Models
{
    /// <summary>
    /// Перечисление, представляющее направление
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Бэкэнд
        /// </summary>
        [Description("Бэкэнд")]
        Backend = 1,

        /// <summary>
        /// Фронтэнд
        /// </summary>
        [Description("Фронтэнд")]
        Frontend = 2,

        /// <summary>
        /// База данных
        /// </summary>
        [Description("База данных")]
        DB = 3
    }

}
