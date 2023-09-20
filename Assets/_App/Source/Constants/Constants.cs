using System.IO;
using UnityEngine;

// ReSharper disable CommentTypo

namespace Omega.Kulibin
{
    public sealed class Constants
    {
        #region ПУТИ
        /// <summary> Путь до файла, содержащего уникальный идентификатор компьютера пользователя (для аналитики) </summary>
        public string PathToUserId => Application.persistentDataPath + "/UserId.txt";
        /// <summary> Путь до папки с сохранениями </summary>
        public string PathToSaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");
        /// <summary> Путь до файла с информацией о билде </summary>
        public string PathToBuildInfo => Application.dataPath + "/buildinfo";
        /// <summary>Путь до файлов заданий для чемпионата</summary>
        public string PathToContestTasks => Path.Combine(Application.persistentDataPath, "TaskFile");
        #endregion
        
        #region АВТООБНОВЛЕНИЕ КЛИЕНТА
        /// <summary> Путь до установщика при автоматическом обновлении приложения </summary>
        public string PathToInstaller => Path.Combine(Application.persistentDataPath, "updater", "installer.exe");
        #endregion

        #region ИНТЕРФЕЙС
        /// <summary>Скейл для высокого разрешения</summary>
        public float HighResolutionScale => 1.5f;
        /// <summary>Скейл для среднего разрешения</summary>
        public float MediumResolutionScale => 1f;
        /// <summary>Скейл для низкого разрешения</summary>
        public float LowResolutionScale => 0.75f;
        /// <summary>Ширина высокого разрешения</summary>
        public int HighResolution => 2560;
        /// <summary>Ширина среднего разрешения</summary>
        public int MediumResolution => 1920;
        /// <summary> Минимальная ширина окна, px </summary>
        public int MinScreenWidth => 1200; 
        /// <summary> Минимальная высота окна, px </summary>
        public int MinScreenHeight => 675;
        /// <summary> Задежка установки разрешения, миллисекунды </summary>
        public int DelaySettingResolution => 50;
        /// <summary> Стандартный размер окна, относительно размера экрана </summary>
        public float WindowDefaultSize => 0.95f;
        /// <summary> Стандартный коэффициент чувствительности у скролла </summary>
        public float ScrollSensitivityDefaultCoefficient => 1f;
        /// <summary> Коэффициент чувствительности у скролла на MacOS </summary>
        public float ScrollSensitivityMacOSCoefficient => 0.25f;
        #endregion

        #region УПРАВЛЕНИЕ ОМЕГАБОТОМ
        /// <summary> Крутящий момент колес при движении омегабота прямо вперед, ньютон-метры </summary>
        public int DirectControllerForwardSpeed => 150;
        /// <summary> Крутящий момент колес при движении омегабота прямо назад, ньютон-метры </summary>
        public int DirectControllerBackSpeed => 150;
        /// <summary> Крутящий момент колес при повороте омегабота на месте, ньютон-метры </summary>
        public int DirectControllerTurningSpeed => 200;
        /// <summary> Коэффициент, используемый при повороте с движением для поворота</summary>
        public float DirectControllerTurnMoveCoefficient => 1.65f; 
        /// <summary> Коэффициент, используемый при повороте с движением для движения вреред/назад </summary>
        public float DirectControllerStraightMoveCoefficient => 6f;
        #endregion
        
        #region КАМЕРА
        /// <summary> Множитель зума камеры при нажатии на кнопку </summary>
        public float ZoomByButton => 5f;
        /// <summary> Угол камеры для вида сбоку, deg </summary>
        public float SideCameraAngle => 30f;
        /// <summary> Угол камеры для вида сверху, deg </summary>
        public float TopCameraAngle => 90f;
        /// <summary> Множитель скорости интерполяции движения камеры при перемещении бота </summary>
        public float SpeedCameraWhenBotMoving => 0.3f;
        #endregion

        #region KULIBIN FILE FORMAT
        /// <summary> Начальное значение суффикса для добавления к названию нового файла </summary>
        public int InitialValueSuffixToFileName => 2;
        /// <summary> Актуальная мажорная версия KulibinProjectFormat </summary>
        public int ActualMajorVersionKulibinProjectFormat => 2;
        /// <summary> Актуальная минорная версия KulibinProjectFormat </summary>
        public int ActualMinorVersionKulibinProjectFormat => 0;
        /// <summary> Актуальная мажорная версия KulibinLevelFormat </summary>
        public int ActualMajorVersionKulibinLevelFileFormat => 2;
        /// <summary> Актуальная минорная версия KulibinLevelFormat </summary>
        public int ActualMinorVersionKulibinLevelFileFormat => 0;
        #endregion
        
        /// <summary> Период отправки частоты кадров в аналитику, сек </summary>
        public float FpsRefreshRate => 60f; 

        /// <summary> Расстояние от экрана до перетаскиваемого объекта </summary>
        public float DragDistanceFromCamera => 5f;
        public int ConstructorAreaSize => 7;

        public int SubprogramRecursionMaxLevel => 100;
        public int MaxListCapacity => 1000;
    }
}