using System;
using Newtonsoft.Json;

namespace MaxFluff.Prototypes
{
    /// <summary>
    /// Параметры определяющие поведение приложения на момент запуска
    ///
    /// При значениях по умолчанию приложение ДОЛЖНО работать корректно  
    /// </summary>
    [JsonObject(MemberSerialization.Fields)]
    [Serializable]
    public sealed class AppStartupConfiguration
    {
        /// <summary>
        /// Указывает на необходимость использования удаленной конфигурации 
        /// </summary>
        public bool RemoteConfigEnabled;
        
        /// <summary>
        /// Путь до сервера удаленных конфигурации.
        ///
        /// Используется только когда <see cref="RemoteConfigEnabled"/> == true
        /// </summary>
        public string RemoteConfigServerUrl;
        
        /// <summary>
        /// Идентификатор удаленной конфигурации  
        ///
        /// Используется только когда <see cref="RemoteConfigEnabled"/> == true
        /// </summary>
        public string RemoteConfigStateId;

        /// <summary>
        /// Максимальное время ответа сервера удаленной конфигурации,
        /// по достижению которого операция стягивания конфигурации будет отменена,
        /// а в качестве <see cref="AppRuntimeConfiguration"/> будет использовано значение по умолчанию
        /// 
        /// Используется только когда <see cref="RemoteConfigEnabled"/> == true
        /// </summary>
        public int RemoteConfigPullTimeoutMilliseconds;

        /// <summary>
        /// ID таблицы, которая подставляется как ресурс для локализации перед созданием билда
        /// </summary>
        public string GoogleSpreadsheetKeyForBuild;
        
        /// <summary>
        /// Используется для определния режима разработчика или пользовательского
        /// </summary>
        public bool DeveloperMode;
        
        /// <summary>
        /// Конфигурация приложения во время выполнения, которая используется в случае если удаленная конфигурация выключена
        /// или стягивание удаленной конфигурации завершилось неудачей 
        /// </summary>
        public AppRuntimeConfiguration DefaultRuntimeConfiguration;
    }
}