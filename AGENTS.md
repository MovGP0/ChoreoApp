## General
- This project uses .NET MAUI and ReactiveUI
- Build targets C# 13 on .NET 10

## Coding Style & Naming
- Avoid nested classes (except private records for tests); follow SOLID and DI preferences.
- Allman braces everywhere, including single-line blocks.
- Indentation: C# 4 spaces; XML/props 2 spaces; use `end_of_line = crlf`, `charset = utf-8`.
- Names: public members/consts PascalCase; instance fields `_camelCase`; static fields `s_camelCase`; `System.*` usings first; prefer file-scoped namespaces and implicit usings.
- Use `var` when type obvious; prefer object/collection initializers; null-propagation.
- Place the usings before the namespace:
```csharp
using Some.Namespace;
using Some.Other.Namespace;

using Local.Namespace;
```
- prefer the new array syntax:
```csharp
int[] values = [1, 2, 3, 4];
IList<int> values = [1, 2, 3, 4];
IEnumerable<int> values = [1, 2, 3, 4];
```
- Set properties after initializing an object when using `using var`
```csharp
// Wrong
using var font = new SKFont() { Size = 12 };

// Correct
using var font = new SKFont();
font.Size = 12;
```
- Never return `async void`. Use `async Task` instead.

## Testing Guidelines
- Prefer xUnit + Shouldy assertions. Test class names: `<Subject>Tests`; method names start with `Should...` (set `DisplayName`).
- Structure tests with `// Arrange`, `// Act`, `// Assert`; name the main instance `subject` and the outcome `result`.
- Prefer xUnit + Shouldy; one assertion or `ShouldSatisfyAllConditions`.
- Test class naming: `<TypeUnderTest>Tests`; facts/theories start with `Should...`.
- Arrange/Act/Assert comments; name subject/result accordingly.

## Commit & Pull Request Guidelines
- Commits: short, imperative subjects (≈50 chars); group logical changes; include context in body when needed. Current history is minimal—keep it tidy.
- PRs: describe motivation and behavior changes; link issues/work items; include screenshots or recordings for UI changes (Windows/Android). Note required workloads or tooling changes in the description.

## Security & Configuration Tips
- Do not commit secrets or platform keystores. Use user-level env vars for API keys.
- Android: ensure `JAVA_HOME` points to `C:\Program Files\Microsoft\jdk-21.0.x` and `ANDROID_HOME`/SDK path is set; accept SDK licenses when prompted.
- Keep workloads current: `dotnet workload update` before major builds.

## Tool usage
- When available, prefer tools (e.g. Rider) for creating, listing, inspecting and editing files.

## Scripts
- `scripts/Create-DevCert.ps1` creates the development signing certificate and prints its thumbprint.
- `scripts/Publish-All.ps1` publishes all supported platforms, skipping targets that require a different OS.

## Dependency Updates
- Use `dotnet outdated` to scan for dependency updates.
- Apply version updates in `Directory.Packages.props` (central package management).

## Implicit Usings
- Implicit global usings are enabled for all projects; common namespaces are auto-included:
```csharp
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
global using Microsoft.Maui;
global using Microsoft.Maui.Controls;
global using Microsoft.Maui.Graphics;
global using Microsoft.Maui.Storage;
global using ReactiveUI;
global using ReactiveUI.SourceGenerators;
global using Microsoft.Extensions.DependencyInjection;
```
- do not import namespaces (`using` keyword) that are already imported globally

## MAUI Specific
- Use `SemanticProperties` for accessibility (`SemanticProperties.Hint`, `SemanticProperties.Description`, `SemanticProperties.HeadingLevel`).
- Prefer compiled bindings:
  - Add `x:DataType` on views and `DataTemplate` roots where bindings are used.
  - For bindings that use `Source=...`, keep `MauiEnableXamlCBindingWithSourceCompilation` set to `true` in project files.
  - If a binding cannot be compiled safely (e.g., attached-property bindings), opt out with `x:DataType="{x:Null}"` on that binding or container.

### Haptics & vibration
- `IHapticFeedback` and `IVibration` are registered in `ChoreoApp/MauiProgram.cs`. Inject into ViewModels/Behaviors that handle input.
- Use `HapticFeedbackType.Click` for click actions and `HapticFeedbackType.LongPress` for long-press actions.
  - For drag operations, use `vibration.Vibrate(TimeSpan)` and call `vibration.Cancel()` when the drag completes or cancels.
- Android requires `<uses-permission android:name="android.permission.VIBRATE" />` in `ChoreoApp/Platforms/Android/AndroidManifest.xml`.
- `IHapticFeedback` and `IVibration` may not be supported on all platforms and might throw exceptions when called; always check `IsSupported` before use.

**Example:**
```csharp
if (hapticFeedback.IsSupported)
{
    hapticFeedback.Perform(HapticFeedbackType.Click);
}
```
```csharp
if (vibration.IsSupported)
{
    vibration.Vibrate(TimeSpan.FromMilliseconds(100));
}

if (vibration.IsSupported)
{
    vibration.Cancel();
}
```

## ReactiveUI Specific
- The documentation for ReactiveUI can be found at https://reactiveui.net/docs/
- We use source generators for properties and commands. Make sure to read the documentation in https://github.com/reactiveui/ReactiveUI.SourceGenerators first.
- Prefer the command pattern and data binding over event handlers. The name of the command should start with a verb and indicate the intention clearly. 
```xaml
<!-- incorrect -->
<Button OnClick="OnSettingsClicked" />

<!-- correct -->
<Button Command="{Binding NavigateToSettingsCommand}" />
```
and in the ViewModel:
```csharp
// generates "CanNavigateToSettings" property
// see ReactiveUI.SourceGenerators documentation
[Reactive]
private bool _canNavigateToSettings = true;

// generates the "NavigateToSettingsCommand" command for binding
// see ReactiveUI.SourceGenerators documentation
[ReactiveCommand(CanExecute = nameof(CanNavigateToSettings))]
private async Task NavigateToSettingsAsync()
{
    // ...
}
```
Important: make sure the binding context of the control points to the view model, where the method is located, instead of to the control itself.

To crate a command that returns a value that can be subscribed to, you need to use the following pattern:
```csharp
[ReactiveCommand]
private Task<TResult> SomeCommandAsync(TArgument argument)
{
    TResult result = ...;
    return Task.FromResult(result);
}

// subscription (e.g. in a Behavior)
viewModel.SomeCommand
    .Subscribe(result => ...)
    .DisposeWith(disposables);
```

- All Views (e.g. Pages, Controls) should either have the `[IViewFor<TViewModel>]` attribute, or derive from a `Reactive*` control type (e.g. `ReactiveContentPage`).
- All ViewModels should derive from `ReactiveObject` and implement `IActivatableViewModel`.
- Make sure the set the `TypeArguments`, `Class` and `DataType` on the control as needed:
```xaml
<maui:ReactiveContentView x:TypeArguments="local:MyViewModel"
xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
xmlns:maui="using:ReactiveUI.Maui"
x:Class="local:MyView"
x:DataType="local:MyViewModel">
```
- the source generators replace Fody. We do not use Fody.

## Software Architecture
- Use Screaming Architecture
- Views, ViewModels, and Behaviors that belong (change) together should be located in the same folder.
- Use different folders for different pages.
- Example: `SettingsPage.xaml`, `SettingsPage.xaml.cs`, `SettingsViewModel.cs`, `DependencyInjection.cs`, etc. should be located in the `Settings/` folder.
- Behaviors of the settings page should be located in the `Settings/Behaviors/` folder.
- there should also be a dedicated `DependencyInjection.cs` for every component:
```charp
public static IServiceCollection AddSettings(this IServiceCollection services)
{
    services.AddTransient<IViewFor<SettingsViewModel>, SettingsPage>();
    services.AddTransient<SettingsViewModel>();
    services.AddTransient<IBehavior<SettingsViewModel>>, Behaviors.SomeSettingsBehavior>();
    // other behaviors come here
    return services;
} 
```
- the services need to be registered in the service collection found in `MauiProgram.cs`:
```csharp
builder.Services.AddSettings();
```

## SkiaSharp Specific
- `SKPaint` does not have font properties like `TextSize` anymore. You need an `SKFont` and set the `Size` property instead.
    - Use `skFont.MeasureText(string)`
    - Use `canvas.DrawText(string, x, y, font, paint);` for drawing text

## MessagePipe Specific
- When one component needs to communicate with another component, we use the Publisher/Subscriber pattern.
- The documentation can be found here: https://github.com/Cysharp/MessagePipe
- Typically, you create a `public sealed record` type that represents the message and inject the `IAsyncPublisher<T>` and `IAsyncPublisher<T>` services into the behavior of the sending/receiving view model.
- The same of the record should be prefixed with `Command`, `Event`, `Query`, or `Response` - based on their role.
  - Examples: `UpdateSceneCommand`, `SceneUpdatedEvent`, `GetScenesQuery`, `ScenesListResponse`

## Behaviors
- Behaviors contain the business logic of the view model.
- Behaviors bind to ReactiveUI properties and commands, MessagePipe messages, and/or other observables.
- A behavior follows the following structure:
```csharp
// sealed class implementing IBehavior<T>
// you can also inject the required services in the constructor
public sealed class SearchSceneBehavior(
    ISubscriber<UpdateScenesCommand> updateScenesCommandSubsriber):
    IBehavior<SceneViewModel>
{
    // attached to a view model
    // subscriptions are disposed with the dispsoables collection
    void Activate(SceneViewModel viewModel, CompositeDisposable disposables)
    {
        // Example: subcribing to a observable property
        viewModel
            .WhenAnyValue(vm => vm.SearchText)
            .Subcribe(searchText => DoSomething())
            .DisposeWith(disposables);

        // Example: subscribe to a observable command
        viewModel.SearchTextCommand
            .Subsribe(_ => DoSomething())
            .DisposeWith(disposables);
        
        // Example: subscribe to a message
        updateScenesCommandSubsriber
            .Subsribe(commandMessage => DoSomething())
            .DisposeWith(disposables);
    }
} 
```
- behaviors need to be injected as `IEnumerable<IBehavior<T>>` in the constructor and activated in the ViewModel's `WhenActivated` extension method.
```csharp
public sealed class SomeViewModel(
    IEnumerable<IBehavior<SomeViewModel>> behaviors):
    ReactiveObject,
    IActivatableViewModel
{
    // .ctor
    public SomeViewModel()
    {
        this.WhenActivated(disposables => 
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }
}
```

## Event Handler
- When registering an event handler, the handler also needs to be disposed.
```csharp
public partial class SomeControl: IDisposable
{
    // implement dispose pattern with a disposable collection
    private CompositeDisposable Disposables { get; } = new();
    public void Dispose() => Disposables.Dispose();

    public SomeControl()
    {
        // register the event handler
        someEventSource.SomeEvent += OnSomeEvent;

        // unregister the event handler on dispose
        Disposable
            .Create(() => someEventSource.SomeEvent -= OnSomeEvent)
            .DisposeWith(Disposables);
    }

    private void OnSomeEvent(object? sender, EventArgs e)
    {
        // handle the event
    }
}
```

## Bindings
- Example for dynamic bindings:
```xaml
BackgroundColor="{DynamicResource Key={x:Static styling:MaterialDesignColorKey.Surface}}">
```
- Example for static bindings:
```xaml
Style="{StaticResource MaterialDesignHamburgerToggleButton}"
```

# Issue tracking
Use 'bd' for issue tracking.

## Remember
- use Rider tools when possible. Avoid `pwsh` and `python` where possible.
- do not use namespaces that are already implicit
