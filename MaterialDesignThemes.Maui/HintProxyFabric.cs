namespace MaterialDesignThemes.Maui;

public static partial class HintProxyFabric
{
    private sealed class HintProxyBuilder
    {
        private readonly Func<BindableObject?, bool> _canBuild;
        private readonly Func<BindableObject, IHintProxy> _build;

        public HintProxyBuilder(Func<BindableObject?, bool> canBuild, Func<BindableObject, IHintProxy> build)
        {
            ArgumentNullException.ThrowIfNull(canBuild);
            ArgumentNullException.ThrowIfNull(build);
            _canBuild = canBuild;
            _build = build;
        }

        public bool CanBuild(BindableObject? control) => _canBuild(control);

        public IHintProxy Build(BindableObject control) => _build(control);
    }

    private static readonly List<HintProxyBuilder> Builders = [];

    static HintProxyFabric()
    {
        Builders.Add(new HintProxyBuilder(c => c is Entry { IsPassword: true }, c => new PasswordEntryHintProxy((Entry)c)));
        Builders.Add(new HintProxyBuilder(c => c is Entry, c => new TextEntryHintProxy((Entry)c)));
        Builders.Add(new HintProxyBuilder(c => c is Editor, c => new EditorHintProxy((Editor)c)));
        Builders.Add(new HintProxyBuilder(c => c is Picker, c => new PickerHintProxy((Picker)c)));
    }

    public static void RegisterBuilder(Func<BindableObject?, bool> canBuild, Func<BindableObject, IHintProxy> build) =>
        Builders.Add(new HintProxyBuilder(canBuild, build));

    public static IHintProxy? Get(BindableObject? control)
    {
        if (control is null)
        {
            return null;
        }

        var builder = Builders.FirstOrDefault(v => v.CanBuild(control));
        return builder?.Build(control);
    }
}
