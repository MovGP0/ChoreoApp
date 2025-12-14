using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes;
using ChoreoApp.Styling;
using ChoreoMasterMobile.Json;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using UnitsNet;

namespace ChoreoApp.Floor.Behaviors;

public sealed class DrawFloorBehavior(
    GlobalStateModel globalState,
    ISubscriber<DrawFloorCommand> drawFloorCommandSubscriber,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber) : IBehavior<FloorCanvasViewModel>
{
    private readonly Dictionary<int, SKColor> _roleBorderColors = new();
    private SceneViewModel? _selectedScene;

    private static SKColor GetColor(string resourceKey)
    {
        var resources = Application.Current?.Resources;
        if (resources is null
            || !resources.TryGetValue(resourceKey, out var resource)
            || resource is not Color color)
        {
            return SKColors.Transparent;
        }

        return color.ToSKColor();
    }

    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        drawFloorCommandSubscriber
            .Subscribe(command => DrawFloor(command.SurfaceEventArgs))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(evt => _selectedScene = evt.SelectedScene)
            .DisposeWith(disposables);
    }

    private void DrawFloor(SKPaintSurfaceEventArgs args)
    {
        var canvas = args.Surface.Canvas;

        // get colors for the current application theme
        SKColor primaryColor = GetColor(MaterialDesignColorKey.Primary);

        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        var floor = choreography.Floor;
        var settings = choreography.Settings;

        Length sizeFromCenterToFront = Length.FromMeters(floor.SizeFront);
        Length sizeFromCenterToBack = Length.FromMeters(floor.SizeBack);
        Length sizeFromCenterToLeft = Length.FromMeters(floor.SizeLeft);
        Length sizeFromCenterToRight = Length.FromMeters(floor.SizeRight);

        float canvasWidth = args.Info.Width;
        float canvasHeight = args.Info.Height;
        var scale = CalculateScaleFactor();

        float centerX = canvasWidth / 2f;
        float centerY = canvasHeight / 2f;
        float left = centerX - (float)sizeFromCenterToLeft.Meters * scale;
        float right = centerX + (float)sizeFromCenterToRight.Meters * scale;
        float top = centerY - (float)sizeFromCenterToFront.Meters * scale;
        float bottom = centerY + (float)sizeFromCenterToBack.Meters * scale;

        var floorRect = new SKRect(left, top, right, bottom);

        DrawFloorRectangle();
        if (settings.GridLines)
        {
            DrawGridLines();
        }
        DrawCenter();
        DrawFloorBorder();
        DrawPositions();

        void DrawGridLines()
        {
            canvas.Save();
            canvas.ClipRect(floorRect, SKClipOperation.Intersect, true);

            using var gridPaint = new SKPaint();

            SKColor secondaryColor = GetColor(MaterialDesignColorKey.Secondary);
            gridPaint.Color = secondaryColor.WithAlpha(96);
            gridPaint.Style = SKPaintStyle.Stroke;
            gridPaint.IsAntialias = true;
            gridPaint.StrokeWidth = 1f;

            int maxHorizontalMeters = Math.Max(floor.SizeLeft, floor.SizeRight);
            for (int meter = 1; meter <= maxHorizontalMeters; meter++)
            {
                float offset = meter * scale;

                if (offset <= (float)sizeFromCenterToLeft.Meters * scale)
                {
                    float x = centerX - offset;
                    canvas.DrawLine(x, top, x, bottom, gridPaint);
                }

                if (offset <= (float)sizeFromCenterToRight.Meters * scale)
                {
                    float x = centerX + offset;
                    canvas.DrawLine(x, top, x, bottom, gridPaint);
                }
            }

            int maxVerticalMeters = Math.Max(floor.SizeFront, floor.SizeBack);
            for (int meter = 1; meter <= maxVerticalMeters; meter++)
            {
                float offset = meter * scale;

                if (offset <= (float)sizeFromCenterToFront.Meters * scale)
                {
                    float y = centerY - offset;
                    canvas.DrawLine(left, y, right, y, gridPaint);
                }

                if (offset <= (float)sizeFromCenterToBack.Meters * scale)
                {
                    float y = centerY + offset;
                    canvas.DrawLine(left, y, right, y, gridPaint);
                }
            }

            canvas.Restore();
        }

        float CalculateScaleFactor()
        {
            float padding = 46f; // pixels
            float scaleX = (canvasWidth - 2 * padding) /
                           (float)(sizeFromCenterToLeft + sizeFromCenterToRight).Meters;

            float scaleY = (canvasHeight - 2 * padding) /
                           (float)(sizeFromCenterToFront + sizeFromCenterToBack).Meters;

            return Math.Min(scaleX, scaleY);
        }

        void DrawCenter()
        {
            using var centerPaint = new SKPaint();
            centerPaint.Color = primaryColor;
            centerPaint.Style = SKPaintStyle.StrokeAndFill;
            centerPaint.IsAntialias = true;
            centerPaint.StrokeWidth = 2f;

            canvas.DrawLine(left, centerY, right, centerY, centerPaint);
            canvas.DrawLine(centerX, top, centerX, bottom, centerPaint);
            canvas.DrawCircle(centerX, centerY, 4f, centerPaint);
        }

        void DrawFloorBorder()
        {
            using var borderPaint = new SKPaint();
            borderPaint.Color = primaryColor;
            borderPaint.Style = SKPaintStyle.Stroke;
            borderPaint.IsAntialias = true;
            borderPaint.StrokeWidth = 2f;
            canvas.DrawRect(floorRect, borderPaint);
        }

        void DrawFloorRectangle()
        {
            SKColor floorColor = settings.FloorColor.ToSKColor();
            using var floorPaint = new SKPaint();
            floorPaint.Color = floorColor;
            floorPaint.Style = SKPaintStyle.Fill;
            floorPaint.IsAntialias = true;
            canvas.DrawRect(floorRect, floorPaint);
        }

        void DrawPositions()
        {
            if (_selectedScene is null
                || choreography.Scenes is not { } scenes)
            {
                return;
            }

            var scene = scenes.FirstOrDefault(s => string.Equals(s.Name, _selectedScene.Name, StringComparison.Ordinal));
            if (scene?.Positions is null)
            {
                return;
            }

            var diameter = (float)settings.DancerSize;
            float radius = diameter / 2f * scale;

            using var fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            using var borderPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 2f
            };

            using var textPaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            };

            using var font = new SKFont
            {
                Size = 14f,
                Edging = SKFontEdging.Antialias
            };

            foreach (var position in scene.Positions)
            {
                DrawPosition(position);
            }

            void DrawPosition(Position position)
            {
                var x = centerX + (float)position.X * scale;
                var y = centerY - (float)position.Y * scale;

                fillPaint.Color = position.Dancer.Color.ToSKColor();
                borderPaint.Color = GetRoleBorderColor(position.Dancer.Role);

                canvas.DrawCircle(x, y, radius, fillPaint);
                canvas.DrawCircle(x, y, radius, borderPaint);

                var shortcut = position.Dancer.Shortcut;
                if (string.IsNullOrWhiteSpace(shortcut))
                {
                    return;
                }

                var textY = y + font.Metrics.CapHeight / 2f;
                canvas.DrawText(shortcut, x, textY, font, textPaint);
            }
        }

        SKColor GetRoleBorderColor(Role role)
        {
            if (_roleBorderColors.TryGetValue(role.ZIndex, out var cached))
            {
                return cached;
            }

            var color = role.Color.ToSKColor();
            _roleBorderColors[role.ZIndex] = color;
            return color;
        }
    }
}
