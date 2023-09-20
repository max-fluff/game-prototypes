# Omega IoC
A lightweight IoC framework presenting functionality for automatic dependency injections using IoC containers. Created for pure architecture development on the `Unity3d` engine

## Features and requirements 
### IoC
- ✅ (supported) Dependency injection
- ✅ (supported) Scoped IoC Containers
- ⏰ (planned **V2**) IoC extensions based on `GetInstance` hook and interface decorators
- ⏰ (planned **V2**) Automatic disposing of services (via `IDisposable` interface)
- ⏰ (planned **V3**) Transfer of end-to-end functionality into separate modules through the principles of '[AOP](https://ru.wikipedia.org/wiki/Аспектно-ориентированное_программирование)'

### Platforms
- ✅ (supported) All unity backends: `IL2CPP`, `Mono`
- ✅ (supported) All unity bcl: `.NET Standard 2.x`, `.NET Framework 4.x`
- ✅ (supported) All unity-supported OS 

### Dependency injection
- ✅ (supported) Named dependencies (via tags)
- ✅ (supported) Constructor injection
- ⏰ (planned **V2**) Method injection (via injection interface method)
- ❌ (not planned) Fields injection
- ❌ (not planned) Properties injection
- ❌ (not planned) support `Lazy<T>`

### Examples
#### Simple injection and getting instance
```csharp
var constants = new DefaultConstants();
var app = IoContainer.Configure(c => {
    c.AddSingleton<IConstants>(constants);
    c.AddSingleton<ILogger, FileLogger>();
    c.Add<IFileSystem, FileSystemService>();
});

var view = FindObjectOfType<GeneralView>();
var mainMenu = app.ConfigureScoped(c => {
    c.AddSingleton(view);
    c.Add<IPresenter, Presenter>();
});

var presenter = mainMenu.Resolve<IPresenter>();
```

#### Tags
```csharp
var app = IoContainer.Configure(c => {
    c.AddSingleton<ICamera>(generalCamera);
    c.AddSingleton<ICamera>(uiCamera, "UICamera");
});

var generalCamera = app.Resolve<ICamera>();
var uiCamera = app.Resolve<ICamera>("UICamera");
var vfxCamera = app.Resolve<ICamera>("VFXCamera"); // <- InvalidOperationException will be thrown
```