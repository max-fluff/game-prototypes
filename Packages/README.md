# Packages

| Название пакета                                       | Назначение в проекте                                                                                              | Место применения                                                                                                             |
|-------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------|
| `com.omega.unity.amplitude`                           | Предоставляет обертку для отправки аналитических данных в сервис Amplitude                                        | Классы `Amplitude` и `AmplitudeService` используется по всему проекту, преимущественно при обработке пользовательского ввода |
| `com.omega.unity.toolbar-configurable-enter-playmode` | Предоставляет удобный инструмент для быстрой конфигурации перезагрузки домена                                     | Верхний тулбар в редакторе Unity                                                                                             |
| `com.omega.unity.toolbar-video-capturing`             | Предоставляет удобный инструмент для быстрой записи видео из GameView                                             | Верхний тулбар в редакторе Unity                                                                                             |
| `com.omega.unity.toolbar-extender`                    | Зависимость для `com.omega.unity.toolbar-configurable-enter-playmode` и `com.omega.unity.toolbar-teamcity-button` | Верхний тулбар в редакторе Unity                                                                                             |
| `com.omega.unity.toolbar-teamcity-button`             | Инструмент для запуска билдов на Teamcity                                                                         | Верхний тулбар в редакторе Unity                                                                                             |
| `com.unity.nuget.newtonsoft-json`                     | Зависимость для `com.omega.unity.toolbar-teamcity-button`                                                         | Верхний тулбар в редакторе Unity                                                                                             |
| `com.omega.unity.integration-kit`                     | Предоставляет удобный инструмент интеграции с инфраструктурой команды                                             | Основное меню `Tools -> Omega -> ...`                                                                                        |
| `com.unity.render-pipelines.core`                     | Рендеринг                                                                                                         |                                                                                                                              |
| `com.unity.ext.nunit`                                 | Зависимость для `com.unity.test-framework`                                                                        | **Пока не используется**                                                                                                     |
| `com.unity.ide.rider`                                 | Предоставляет средства интеграции Unity c IDE JetBrains Rider                                                     | -                                                                                                                            |
| `com.unity.mathematics`                               | Зависимость для `com.unity.render-pipelines.universal`                                                            |                                                                                                                              |
| `com.unity.searcher`                                  | Зависимость для `com.unity.shadergraph`                                                                           |                                                                                                                              |
| `com.unity.shadergraph`                               | Зависимость для `com.unity.render-pipelines.universal`                                                            |                                                                                                                              |
| `com.unity.test-framework`                            | Предоставляет средства для автоматического тестирования                                                           | **Пока не используется**                                                                                                     |
| `com.unity.sharp-zip-lib`                             | Предоставляет инструменты для создания архивов, в том числе архивов на основе папки                               | `TeamcityEntryPoint` для сжатия артифактов                                                                                   |
| `com.unity.textmeshpro`                               | -                                                                                                                 |                                                                                                                              |
| `com.unity.ugui`                                      | -                                                                                                                 |                                                                                                                              |
| `com.unity.recorder`                                  | Зависимость для `com.unity.recorder`                                                                              |                                                                                                                              |
| `com.unity.timeline`                                  | Зависимость для `com.omega.unity.toolbar-video-capturing`                                                         |                                                                                                                              |
| `com.unity.render-pipelines.universal`                | Рендеринг                                                                                                         |                                                                                                                              |
| `com.eflatun.scenereference`                          | Сериализация сцен через инспектор                                                                                 | В `AppScenes` хранятся основные сцены приложения (`StartScreen`, `Workspace`). В `EnvironmentScenes` хранятся окружения      |
| `com.omega.unity.tab-navigation`                      | Предоставляет удобный инструмент для навигации по Selectable полям                                                | Префабы блоков BlockEngine                                                                                                   |

# Built-in Modules 

| Название пакета                                       | Стаус        | Назначение в проекте               | Место применения |
|-------------------------------------------------------|--------------|------------------------------------|------------------|
| `com.unity.modules.ai`                                | **disabled** |                                    |                  |
| `com.unity.modules.androidjni`                        | **disabled** |                                    |                  |
| `com.unity.modules.animation`                         | -            |                                    |                  |
| `com.unity.modules.assetbundle`                       | -            |                                    |                  |
| `com.unity.modules.audio`                             | -            |                                    |                  |
| `com.unity.modules.cloth`                             | **disabled** |                                    |                  |
| `com.unity.modules.director`                          | -            |                                    |                  |
| `com.unity.modules.imageconversion`                   | -            |                                    |                  |
| `com.unity.modules.imgui`                             | -            |                                    |                  |
| `com.unity.modules.jsonserialize`                     | -            |                                    |                  |
| `com.unity.modules.particlesystem`                    | -            |                                    |                  |
| `com.unity.modules.physics`                           | -            |                                    |                  |
| `com.unity.modules.physics2d`                         | **enabled**  | Зависимость для ассета `LeanTouch` |                  |
| `com.unity.modules.screencapture`                     | -            |                                    |                  |
| `com.unity.modules.terrain`                           | -            |                                    |                  |
| `com.unity.modules.terrainphysics`                    | -            |                                    |                  |
| `com.unity.modules.tilemap`                           | **disabled** |                                    |                  |
| `com.unity.modules.ui`                                | -            |                                    |                  |
| `com.unity.modules.uielements`                        | -            |                                    |                  |
| `com.unity.modules.umbra`                             | -            |                                    |                  |
| `com.unity.modules.unityanalytics`                    | **disabled** |                                    |                  |
| `com.unity.modules.unitywebrequest`                   | -            |                                    |                  |
| `com.unity.modules.unitywebrequestassetbundle`-       |              |                                    |
| `com.unity.modules.unitywebrequestaudio`              | -            |                                    |                  |
| `com.unity.modules.unitywebrequesttexture`            | -            |                                    |                  |
| `com.unity.modules.unitywebrequestwww`                | -            |                                    |                  |
| `com.unity.modules.vehicles`                          | -            |                                    |                  |
| `com.unity.modules.video`                             | -            |                                    |                  |
| `com.unity.modules.vr`                                | **disabled** |                                    |                  |
| `com.unity.modules.wind`                              | **disabled** |                                    |                  |
| `com.unity.modules.xr`                                | **disabled** |                                    |                  |