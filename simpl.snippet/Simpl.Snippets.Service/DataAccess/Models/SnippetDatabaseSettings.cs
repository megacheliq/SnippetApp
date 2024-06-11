namespace Simpl.Snippets.Service.DataAccess.Models
{
    /// <summary>
    /// Класс для конфигурации базы данных MongoDB
    /// </summary>
    public class SnippetDatabaseSettings
    {
        /// <summary>
        /// Строка подключения к базе данных
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Название базы данных
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Название коллекции в базе данных
        /// </summary>
        public string CollectionName { get; set; }
    }

}
