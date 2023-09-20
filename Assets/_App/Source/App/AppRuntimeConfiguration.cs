using System;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace MaxFluff.Prototypes
{
    /// <summary>
    /// Параметры определяющие поведение приложения во время выполнения
    ///
    /// При значениях по умолчанию приложение ДОЛЖНО работать корректно 
    /// </summary>
    [JsonObject(MemberSerialization.Fields)]
    [Serializable]
    public struct AppRuntimeConfiguration
    {
        /// <summary>
        /// Конфигурация с которой было запущенно приложение
        /// </summary>
        [JsonIgnore, NonSerialized]
        public AppStartupConfiguration StartupConfiguration;

        /// <summary>
        /// Необходимо ли включить сервис обновлений
        /// </summary>
        public bool UpdateServiceEnabled;

        /// <summary>
        /// Путь до сервера обновлений https://git.jet-omega.com/jet-omega/kulibin-updater
        ///
        /// Используется только когда <see cref="UpdateServiceEnabled"/> == true
        /// </summary>
        public string UpdateServerUrl;

        /// <summary>
        /// Ветка обновлений
        ///
        /// Используется только когда <see cref="UpdateServiceEnabled"/> == true
        /// </summary>
        public string UpdateServiceBranch;

        /// <summary>
        /// Верхняя граница кол-ва кадров в секунду. Значения меньшие или равные нулю снимают ограничение
        /// </summary>
        public int LimitFps;

        /// <summary>
        /// Необходимо ли включить сервис аналитики
        /// </summary>
        public bool AnalyticsServiceEnabled;

        /// <summary>
        /// Необходимо ли принудительно требовать от пользователя устанавливать обновление, в случае его наличия и готовности к установки
        /// </summary>
        public bool ForceRequireUpdate;

        /// <summary>
        /// Включает или выключает функционал выбора окружения и робота
        /// </summary>
        public bool EnableFeaturePreviewPolygonSelector;

        /// <summary>
        /// `api_key` для идентификации проекта сервером аналитики  
        /// https://www.docs.developers.amplitude.com/analytics/apis/http-v2-api/#authorization
        /// 
        /// Используется только когда <see cref="AnalyticsServiceEnabled"/> == true
        /// </summary>
        public string AnalyticsApiKey;

        /// <summary>
        /// Включает или выключает функционал копирования в буфер обмена блоков программы как картинки 
        /// </summary>
        public bool EnableFeatureCopyAsImage;
        
        #region Ссылки
        
        /// <summary>
        /// Ссылка на ТГ чат учитилей
        /// </summary>
        public string CommunityTelegramLink;
        
        /// <summary>
        /// Ссылка на ТГ канал обновлений
        /// </summary>
        public string UpdatesTelegramLink;
        
        /// <summary>
        /// Адрес пользовательского сайта омегабота
        /// </summary>
        public string OmegaBotSiteUrl;
        
        #endregion

        #region Авторизация

        /// <summary>
        /// Необходима ли авторизация в приложении
        /// </summary>
        public bool AuthorizationEnabled;

        /// <summary>
        /// Путь до сервера авторизации
        ///
        /// Используется только когда <see cref="AuthorizationEnabled"/> == true
        /// </summary>
        public string AuthorizationServerUrl;

        /// <summary>
        /// Адрес страницы регистрации
        ///
        /// Используется только когда <see cref="AuthorizationEnabled"/> == true
        /// </summary>
        public string RegistrationUrl;

        /// <summary>
        /// Адрес страницы восстановления пароля
        ///
        /// Используется только когда <see cref="AuthorizationEnabled"/> == true
        /// </summary>
        public string PasswordResetUrl;

        #endregion

        #region Соревнования

        /// <summary>
        /// Включает или выключает функционал соревнований
        /// </summary>
        public bool ContestsFeatureEnabled;
        
        /// <summary>
        /// ID чемпионата Юный Кулибин
        /// </summary>
        public string ContestId;

        /// <summary>
        /// Адрес страницы регистрации на чемпионате Юный Кулибин
        /// </summary>
        public string RegistrationOnContestUrl;

        /// <summary>
        /// Необходимо ли открывать выезжающее окно чемпионата
        /// </summary>
        public bool ContestAdWindowEnabled;        
        
        /// <summary>
        /// Можно ли отправлять результаты в зачёт
        /// </summary>
        public bool AllowSendingForStanding;

        #endregion
    }
}