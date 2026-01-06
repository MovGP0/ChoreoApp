namespace MaterialDesignThemes.Maui;

public class FlipperClassic : Flipper
{
    public const string Plane3DPartName = "PART_Plane3D";
    private const uint FlipAnimationDuration = 400;
    private const string FlipRotationAnimationName = "MaterialDesignFlipperClassicRotation";

    private Plane3D? _plane3D;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _plane3D = GetTemplateChild(Plane3DPartName) as Plane3D;
    }

    protected override async Task AnimateFlipAsync(bool isFlipped)
    {
        var plane = _plane3D;
        var front = FrontContentHost;
        var back = BackContentHost;
        if (plane is null || front is null || back is null)
        {
            return;
        }

        front.IsVisible = true;
        back.IsVisible = true;

        plane.AbortAnimation(FlipRotationAnimationName);

        if (isFlipped)
        {
            await AnimateRotationY(plane, 0, 90, FlipAnimationDuration / 2);
            back.IsVisible = true;
            front.IsVisible = false;
            await AnimateRotationY(plane, 90, 180, FlipAnimationDuration / 2);
        }
        else
        {
            await AnimateRotationY(plane, 180, 90, FlipAnimationDuration / 2);
            front.IsVisible = true;
            back.IsVisible = false;
            await AnimateRotationY(plane, 90, 0, FlipAnimationDuration / 2);
        }
    }

    private static Task AnimateRotationY(Plane3D plane, double from, double to, uint duration)
    {
        var tcs = new TaskCompletionSource();
        plane.RotationY = from;

        var animation = new Animation(
            callback: value => plane.RotationY = value,
            start: from,
            end: to,
            easing: Easing.CubicInOut);

        animation.Commit(plane, FlipRotationAnimationName, 16, duration, Easing.CubicInOut, (_, _) =>
        {
            plane.RotationY = to;
            tcs.TrySetResult();
        });

        return tcs.Task;
    }
}
