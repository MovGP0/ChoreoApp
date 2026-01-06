using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes;
using MaterialDesignThemes.Maui;
using ChoreoApp.AudioPlayer.Messages;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using ChoreoApp.Settings;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Dancer = ChoreoApp.Models.DancerModel;
using Position = ChoreoApp.Models.PositionModel;
using Role = ChoreoApp.Models.RoleModel;
using Scene = ChoreoApp.Models.SceneModel;

namespace ChoreoApp.Floor.Behaviors;

public sealed class DrawFloorBehavior(
    Global.GlobalStateModel globalState,
    ApplicationStateMachine stateMachine,
    ISubscriber<DrawFloorCommand> drawFloorCommandSubscriber,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber,
    ISubscriber<AudioPlayerPositionChangedEvent> audioPositionSubscriber,
    IFloorRenderGate renderGate):
    IBehavior<FloorCanvasViewModel>
{
    private readonly Dictionary<int, SKColor> _roleBorderColors = new();
    private FloorCanvasViewModel? _viewModel;
    private SceneViewModel? _selectedScene;
    private double? _currentAudioSeconds;
    private bool _hasRendered;

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
        _viewModel = viewModel;
        Disposable
            .Create(() => _viewModel = null)
            .DisposeWith(disposables);

        drawFloorCommandSubscriber
            .Subscribe(command => DrawFloor(command.SurfaceEventArgs))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(evt => _selectedScene = evt.SelectedScene)
            .DisposeWith(disposables);

        audioPositionSubscriber
            .Subscribe(evt => _currentAudioSeconds = evt.PositionSeconds)
            .DisposeWith(disposables);
    }

    private void DrawFloor(SKPaintSurfaceEventArgs args)
    {
        if (!_hasRendered)
        {
            _hasRendered = true;
            renderGate.MarkRendered();
        }

        if (stateMachine.State is not ViewSceneState
            && stateMachine.State is not PlacePositionsState
            && stateMachine.State is not MovePositionsState)
        {
            return;
        }

        var canvas = args.Surface.Canvas;
        SKColor surfaceColor = GetColor(MaterialDesignColorKey.Surface);
        canvas.Clear(surfaceColor);

        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        var floor = choreography.Floor;
        var settings = choreography.Settings;

        float sizeFromCenterToFront = floor.SizeFront;
        float sizeFromCenterToBack = floor.SizeBack;
        float sizeFromCenterToLeft = floor.SizeLeft;
        float sizeFromCenterToRight = floor.SizeRight;

        float canvasWidth = args.Info.Width;
        float canvasHeight = args.Info.Height;
        float headerHeight = CalculateHeaderHeight();
        float contentHeight = canvasHeight - headerHeight;
        if (contentHeight <= 0f)
        {
            return;
        }

        var scale = CalculateScaleFactor(contentHeight);

        float centerX = canvasWidth / 2f;
        float centerY = headerHeight + contentHeight / 2f;
        float left = centerX - sizeFromCenterToLeft * scale;
        float right = centerX + sizeFromCenterToRight * scale;
        float top = centerY - sizeFromCenterToFront * scale;
        float bottom = centerY + sizeFromCenterToBack * scale;

        var floorRect = new SKRect(left, top, right, bottom);
        _viewModel?.UpdateFloorBounds(floorRect, new SKSize(canvasWidth, canvasHeight));

        var transformationMatrix = _viewModel?.TransformationMatrix ?? SKMatrix.CreateIdentity();
        canvas.Save();
        canvas.SetMatrix(in transformationMatrix);

        // draw floor
        DrawHeader();
        DrawFloorRectangle();
        if (settings.GridLines)
        {
            DrawGridLines();
        }
        DrawCenter();
        DrawFloorBorder();
        DrawSvgOverlay();

        // draw positions and labels
        var scenePositions = GetScenePositions();
        var (previousScene, currentScene, nextScene) = GetAdjacentScenes();
        var remainingPositions = stateMachine.State is PlacePositionsState
            ? GetRemainingPositions(currentScene)
            : 0;

        if (scenePositions is not null && settings.PositionsAtSide)
        {
            DrawAxisLabels(scenePositions);
        }
        DrawSceneCurves(previousScene, currentScene, nextScene);
        var selectedPositions = globalState.SelectedPositions;
        var selectedPositionsSet = selectedPositions.Count > 0
            ? new HashSet<Position>(selectedPositions)
            : null;
        DrawPositions(scenePositions, currentScene, nextScene, _currentAudioSeconds, selectedPositionsSet);
        DrawSelectionRectangle(globalState.SelectionRectangle);

        canvas.Restore();

        if (stateMachine.State is PlacePositionsState
            && globalState.IsPlaceMode
            && remainingPositions > 0)
        {
            DrawPlacementOverlay(remainingPositions);
        }

        void DrawGridLines()
        {
            canvas.Save();
            canvas.ClipRect(floorRect, SKClipOperation.Intersect, true);

            using var gridPaint = new SKPaint();

            SKColor secondaryColor = GetColor(MaterialDesignColorKey.Secondary);
            gridPaint.Color = secondaryColor;
            gridPaint.Style = SKPaintStyle.Stroke;
            gridPaint.IsAntialias = true;
            gridPaint.StrokeWidth = 1f;

            int maxHorizontalMeters = Math.Max((int)floor.SizeLeft, (int)floor.SizeRight);
            for (int meter = 1; meter <= maxHorizontalMeters; meter++)
            {
                float offset = meter * scale;

                if (offset <= sizeFromCenterToLeft * scale)
                {
                    float x = centerX - offset;
                    canvas.DrawLine(x, top, x, bottom, gridPaint);
                }

                if (offset <= sizeFromCenterToRight * scale)
                {
                    float x = centerX + offset;
                    canvas.DrawLine(x, top, x, bottom, gridPaint);
                }
            }

            int maxVerticalMeters = Math.Max((int)floor.SizeFront, (int)floor.SizeBack);
            for (int meter = 1; meter <= maxVerticalMeters; meter++)
            {
                float offset = meter * scale;

                if (offset <= sizeFromCenterToFront * scale)
                {
                    float y = centerY - offset;
                    canvas.DrawLine(left, y, right, y, gridPaint);
                }

                if (offset <= sizeFromCenterToBack * scale)
                {
                    float y = centerY + offset;
                    canvas.DrawLine(left, y, right, y, gridPaint);
                }
            }

            canvas.Restore();
        }

        float CalculateScaleFactor(float availableHeight)
        {
            float padding = 46f; // pixels
            float scaleX = (canvasWidth - 2 * padding) /
                           (sizeFromCenterToLeft + sizeFromCenterToRight);

            float scaleY = (availableHeight - 2 * padding) /
                           (sizeFromCenterToFront + sizeFromCenterToBack);

            return Math.Min(scaleX, scaleY);
        }

        float CalculateHeaderHeight()
        {
            const float padding = 16f;
            const float spacing = 4f;

            using var titleFont = new SKFont();
            titleFont.Size = 20f;
            titleFont.Edging = SKFontEdging.Antialias;

            using var subtitleFont = new SKFont();
            subtitleFont.Size = 16f;
            subtitleFont.Edging = SKFontEdging.Antialias;

            float titleHeight = titleFont.Metrics.Descent - titleFont.Metrics.Ascent;
            float subtitleHeight = subtitleFont.Metrics.Descent - subtitleFont.Metrics.Ascent;

            return padding + titleHeight + spacing + subtitleHeight + padding;
        }

        void DrawHeader()
        {
            const float padding = 16f;
            const float spacing = 4f;

            var choreographyName = choreography.Name ?? string.Empty;
            var sceneName = _selectedScene?.Name ?? string.Empty;

            using var titlePaint = new SKPaint();
            titlePaint.Color = GetColor(MaterialDesignColorKey.OnSurface);
            titlePaint.IsAntialias = true;

            using var subtitlePaint = new SKPaint();
            subtitlePaint.Color = GetColor(MaterialDesignColorKey.OnSurfaceVariant);
            subtitlePaint.IsAntialias = true;

            using var titleFont = new SKFont();
            titleFont.Size = 20f;
            titleFont.Edging = SKFontEdging.Antialias;

            using var subtitleFont = new SKFont();
            subtitleFont.Size = 14f;
            subtitleFont.Edging = SKFontEdging.Antialias;

            float titleHeight = titleFont.Metrics.Descent - titleFont.Metrics.Ascent;
            float titleBaseline = padding - titleFont.Metrics.Ascent;
            canvas.DrawText(choreographyName, centerX, titleBaseline, SKTextAlign.Center, titleFont, titlePaint);

            float subtitleBaseline = padding + titleHeight + spacing - subtitleFont.Metrics.Ascent;
            canvas.DrawText(sceneName, centerX, subtitleBaseline, SKTextAlign.Center, subtitleFont, subtitlePaint);
        }

        void DrawCenter()
        {
            using var centerPaint = new SKPaint();
            centerPaint.Color = GetColor(MaterialDesignColorKey.Primary);
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
            borderPaint.Color = GetColor(MaterialDesignColorKey.Primary);
            borderPaint.Style = SKPaintStyle.Stroke;
            borderPaint.IsAntialias = true;
            borderPaint.StrokeWidth = 2f;
            canvas.DrawRect(floorRect, borderPaint);
        }

        void DrawAxisLabels(IReadOnlyList<Position> positions)
        {
            SKColor labelColor = GetColor(MaterialDesignColorKey.OnSurfaceVariant);
            using var labelPaint = new SKPaint();
            labelPaint.Color = labelColor;
            labelPaint.IsAntialias = true;

            using var font = new SKFont();
            font.Size = 16f;
            font.Edging = SKFontEdging.Antialias;

            const float labelOffset = 12f;
            float topLabelY = top - labelOffset;
            float bottomLabelY = bottom + labelOffset;
            float leftLabelX = left - labelOffset;
            float rightLabelX = right + labelOffset;

            var xValues = new HashSet<double>();
            var yValues = new HashSet<double>();

            foreach (var position in positions)
            {
                xValues.Add(position.X);
                yValues.Add(position.Y);
            }

            foreach (var x in xValues.OrderBy(value => value))
            {
                float labelX = centerX + (float)x * scale;
                var text = x.ToString("0.##", CultureInfo.CurrentUICulture);
                DrawLabel(text, labelX, topLabelY, SKTextAlign.Center);
                DrawLabel(text, labelX, bottomLabelY, SKTextAlign.Center);
            }

            foreach (var y in yValues.OrderBy(value => value))
            {
                float labelY = centerY - (float)y * scale;
                var text = y.ToString("0.##", CultureInfo.CurrentUICulture);
                DrawLabel(text, leftLabelX, labelY, SKTextAlign.Right);
                DrawLabel(text, rightLabelX, labelY, SKTextAlign.Left);
            }

            void DrawLabel(string text, float x, float y, SKTextAlign align)
            {
                float textY = y + font.Metrics.CapHeight / 2f;
                canvas.DrawText(text, x, textY, align, font, labelPaint);
            }
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

        void DrawSvgOverlay()
        {
            var svgDocument = globalState.SvgDocument;
            if (svgDocument is null)
            {
                return;
            }

            var bounds = svgDocument.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            var scaleX = floorRect.Width / bounds.Width;
            var scaleY = floorRect.Height / bounds.Height;
            var svgScale = Math.Min(scaleX, scaleY);
            if (svgScale <= 0f || float.IsNaN(svgScale) || float.IsInfinity(svgScale))
            {
                return;
            }

            var svgCenterX = bounds.Left + bounds.Width / 2f;
            var svgCenterY = bounds.Top + bounds.Height / 2f;

            canvas.Save();
            canvas.Translate(centerX, centerY);
            canvas.Scale(svgScale, svgScale);
            canvas.Translate(-svgCenterX, -svgCenterY);
            canvas.DrawPicture(svgDocument.Picture);
            canvas.Restore();
        }

        void DrawPositions(
            IReadOnlyList<Position>? positions,
            Scene? currentScene,
            Scene? nextScene,
            double? currentAudioSeconds,
            IReadOnlySet<Position>? selectedPositions)
        {
            if (positions is null)
            {
                return;
            }

            double? interpolationT = null;
            Dictionary<int, Position>? nextPositionsByDancer = null;

            if (currentAudioSeconds.HasValue
                && currentScene?.Timestamp is { } currentTimestamp
                && nextScene?.Timestamp is { } nextTimestamp
                && nextScene.Positions is not null)
            {
                double startSeconds = currentTimestamp.TotalSeconds;
                double endSeconds = nextTimestamp.TotalSeconds;
                double duration = endSeconds - startSeconds;
                if (duration > 0d)
                {
                    double rawT = (currentAudioSeconds.Value - startSeconds) / duration;
                    if (rawT >= 0d && rawT <= 1d)
                    {
                        interpolationT = rawT;
                        nextPositionsByDancer = BuildPositionsByDancerId(nextScene);
                    }
                }
            }

            var diameter = (float)settings.DancerSize;
            float radius = diameter / 2f * scale;

            using var fillPaint = new SKPaint();
            fillPaint.Style = SKPaintStyle.Fill;
            fillPaint.IsAntialias = true;

            using var borderPaint = new SKPaint();
            borderPaint.Style = SKPaintStyle.Stroke;
            borderPaint.IsAntialias = true;
            borderPaint.StrokeWidth = 2f;

            using var selectionPaint = new SKPaint();
            selectionPaint.Style = SKPaintStyle.Stroke;
            selectionPaint.IsAntialias = true;
            selectionPaint.StrokeWidth = 3f;
            selectionPaint.Color = GetColor(MaterialDesignColorKey.Secondary);

            using var textPaint = new SKPaint();
            textPaint.Color = SKColors.White;
            textPaint.IsAntialias = true;

            using var font = new SKFont();
            font.Size = 24f;
            font.Edging = SKFontEdging.Antialias;

            foreach (var position in positions)
            {
                DrawPosition(position);
            }

            void DrawPosition(Position position)
            {
                if (position.Dancer is null)
                {
                    fillPaint.Color = ApplyTransparency(GetColor(MaterialDesignColorKey.SurfaceVariant), settings.Transparency);
                    borderPaint.Color = ApplyTransparency(GetColor(MaterialDesignColorKey.OutlineVariant), settings.Transparency);

                    var x = centerX + (float)position.X * scale;
                    var y = centerY - (float)position.Y * scale;

                    canvas.DrawCircle(x, y, radius, fillPaint);
                    canvas.DrawCircle(x, y, radius, borderPaint);
                    return;
                }

                double drawX = position.X;
                double drawY = position.Y;

                if (interpolationT is { } t
                    && nextPositionsByDancer is not null
                    && nextPositionsByDancer.TryGetValue(position.Dancer.DancerId.Value, out var nextPosition))
                {
                    var curve1X = position.Curve1X;
                    var curve1Y = position.Curve1Y;
                    var curve2X = position.Curve2X;
                    var curve2Y = position.Curve2Y;

                    if (curve1X is null || curve1Y is null)
                    {
                        drawX = Lerp(position.X, nextPosition.X, t);
                        drawY = Lerp(position.Y, nextPosition.Y, t);
                    }
                    else if (curve2X is null || curve2Y is null)
                    {
                        drawX = QuadraticBezier(position.X, curve1X.Value, nextPosition.X, t);
                        drawY = QuadraticBezier(position.Y, curve1Y.Value, nextPosition.Y, t);
                    }
                    else
                    {
                        drawX = CubicBezier(position.X, curve1X.Value, curve2X.Value, nextPosition.X, t);
                        drawY = CubicBezier(position.Y, curve1Y.Value, curve2Y.Value, nextPosition.Y, t);
                    }
                }

                var cx = centerX + (float)drawX * scale;
                var cy = centerY - (float)drawY * scale;

                fillPaint.Color = ApplyTransparency(position.Dancer.Color.ToSKColor(), settings.Transparency);
                textPaint.Color = PickBlackOrWhite(fillPaint.Color) switch
                {
                    BlackOrWhite.White => GetColor(MaterialDesignColorKey.White),
                    BlackOrWhite.Black => GetColor(MaterialDesignColorKey.Black),
                    _ => throw new ArgumentOutOfRangeException()
                };
                borderPaint.Color = ApplyTransparency(GetRoleBorderColor(position.Dancer.Role), settings.Transparency);

                canvas.DrawCircle(cx, cy, radius, fillPaint);
                canvas.DrawCircle(cx, cy, radius, borderPaint);

                if (selectedPositions is not null && selectedPositions.Contains(position))
                {
                    canvas.DrawCircle(cx, cy, radius + 4f, selectionPaint);
                }

                var shortcut = position.Dancer.Shortcut;
                if (string.IsNullOrWhiteSpace(shortcut))
                {
                    return;
                }

                var textY = cy + font.Metrics.CapHeight / 2f;
                canvas.DrawText(shortcut, cx, textY, SKTextAlign.Center, font, textPaint);
            }
        }

        void DrawSelectionRectangle(Global.SelectionRectangle? selectionRectangle)
        {
            if (stateMachine.State is not MovePositionsSelectionState)
            {
                return;
            }

            if (selectionRectangle is not { } rectangle)
            {
                return;
            }

            using var selectionPaint = new SKPaint();
            selectionPaint.Style = SKPaintStyle.Stroke;
            selectionPaint.IsAntialias = true;
            selectionPaint.StrokeWidth = 2f;
            selectionPaint.PathEffect = SKPathEffect.CreateDash([6f, 6f], 0f);
            selectionPaint.Color = GetColor(MaterialDesignColorKey.OnSurfaceVariant);

            var start = ToCanvasPoint(rectangle.Start.X, rectangle.Start.Y);
            var end = ToCanvasPoint(rectangle.End.X, rectangle.End.Y);

            var left = Math.Min(start.X, end.X);
            var top = Math.Min(start.Y, end.Y);
            var right = Math.Max(start.X, end.X);
            var bottom = Math.Max(start.Y, end.Y);

            var rect = new SKRect(left, top, right, bottom);
            canvas.DrawRect(rect, selectionPaint);
        }

        void DrawSceneCurves(Scene? previous, Scene? current, Scene? next)
        {
            if (current?.Positions is null)
            {
                return;
            }

            using var paint = new SKPaint();
            paint.Style = SKPaintStyle.Stroke;
            paint.IsAntialias = true;
            paint.StrokeWidth = 2f;

            if (previous?.Positions is not null
                && Preferences.Default.Get(SettingsPreferenceKeys.DrawPathFrom, true))
            {
                paint.PathEffect = SKPathEffect.CreateDash([6f, 6f], 0f);
                DrawCurvesBetweenScenes(previous, current, paint, useDarkerColor: true);
            }

            if (next?.Positions is not null
                && Preferences.Default.Get(SettingsPreferenceKeys.DrawPathTo, true))
            {
                paint.PathEffect = null;
                DrawCurvesBetweenScenes(current, next, paint, useDarkerColor: false);
            }
        }

        void DrawCurvesBetweenScenes(Scene fromScene, Scene toScene, SKPaint paint, bool useDarkerColor)
        {
            var fromByDancer = BuildPositionsByDancerId(fromScene);
            if (fromByDancer.Count == 0 || toScene.Positions is null)
            {
                return;
            }

            foreach (var toPosition in toScene.Positions)
            {
                if (toPosition.Dancer is not { } dancer)
                {
                    continue;
                }

                int dancerId = dancer.DancerId.Value;
                if (dancerId <= 0 || !fromByDancer.TryGetValue(dancerId, out var fromPosition))
                {
                    continue;
                }

                var dancerColor = dancer.Color.ToSKColor();
                paint.Color = useDarkerColor
                    ? DarkenColor(dancerColor, 0.7f)
                    : dancerColor;
                DrawCurve(fromPosition, toPosition, paint);
            }
        }

        void DrawCurve(Position fromPosition, Position toPosition, SKPaint paint)
        {
            var start = ToCanvasPoint(fromPosition.X, fromPosition.Y);
            var end = ToCanvasPoint(toPosition.X, toPosition.Y);

            var curve1X = fromPosition.Curve1X;
            var curve1Y = fromPosition.Curve1Y;
            if (curve1X is null || curve1Y is null)
            {
                canvas.DrawLine(start, end, paint);
                return;
            }

            var curve2X = fromPosition.Curve2X;
            var curve2Y = fromPosition.Curve2Y;

            var control1 = ToCanvasPoint(curve1X.Value, curve1Y.Value);
            if (curve2X is null || curve2Y is null)
            {
                using var quadraticPath = new SKPath();
                quadraticPath.MoveTo(start);
                quadraticPath.QuadTo(control1, end);
                canvas.DrawPath(quadraticPath, paint);
                return;
            }

            var control2 = ToCanvasPoint(curve2X.Value, curve2Y.Value);

            using var path = new SKPath();
            path.MoveTo(start);
            path.CubicTo(control1, control2, end);
            canvas.DrawPath(path, paint);
        }

        SKPoint ToCanvasPoint(double x, double y)
        {
            return new SKPoint(
                centerX + (float)x * scale,
                centerY - (float)y * scale);
        }

        static double Lerp(double start, double end, double t)
            => start + (end - start) * t;

        static double QuadraticBezier(double p0, double p1, double p2, double t)
        {
            double u = 1d - t;
            return u * u * p0 + 2d * u * t * p1 + t * t * p2;
        }

        static double CubicBezier(double p0, double p1, double p2, double p3, double t)
        {
            double u = 1d - t;
            double uu = u * u;
            double tt = t * t;
            return uu * u * p0 + 3d * uu * t * p1 + 3d * u * tt * p2 + tt * t * p3;
        }

        SKColor DarkenColor(SKColor color, float lightnessScale)
        {
            color.ToHsl(out float h, out float s, out float l);
            float newLightness = Math.Clamp(l * lightnessScale, 0f, 100f);
            return SKColor.FromHsl(h, s, newLightness, color.Alpha);
        }

        static SKColor ApplyTransparency(SKColor color, decimal transparency)
        {
            var clamped = Math.Clamp(transparency, 0m, 1m);
            var opacity = 1m - clamped;
            var alpha = (byte)Math.Clamp((int)Math.Round(opacity * 255m), 0, 255);
            return color.WithAlpha(alpha);
        }

        Dictionary<int, Position> BuildPositionsByDancerId(Scene scene)
        {
            var lookup = new Dictionary<int, Position>();
            if (scene.Positions is null)
            {
                return lookup;
            }

            foreach (var position in scene.Positions)
            {
                if (position.Dancer is not { } dancer)
                {
                    continue;
                }

                int dancerId = dancer.DancerId.Value;
                if (dancerId > 0)
                {
                    lookup[dancerId] = position;
                }
            }

            return lookup;
        }

        IReadOnlyList<Position>? GetScenePositions()
        {
            if (_selectedScene is null
                || choreography.Scenes is not { } scenes)
            {
                return null;
            }

            var scene = FindScene(scenes);
            if (scene?.Positions is null)
            {
                return [];
            }

            return scene.Positions.ToList();
        }

        (Scene? Previous, Scene? Current, Scene? Next) GetAdjacentScenes()
        {
            if (_selectedScene is null
                || choreography.Scenes is not { } scenes)
            {
                return (null, null, null);
            }

            var currentScene = FindScene(scenes);
            if (currentScene is null)
            {
                return (null, null, null);
            }

            var index = scenes.IndexOf(currentScene);
            var previous = index > 0 ? scenes[index - 1] : null;
            var next = index >= 0 && index + 1 < scenes.Count ? scenes[index + 1] : null;
            return (previous, currentScene, next);
        }

        Scene? FindScene(IList<Scene> scenes)
        {
            if (_selectedScene is null)
            {
                return null;
            }

            var scene = scenes.FirstOrDefault(s => s.SceneId == _selectedScene.SceneId);
            return scene ?? scenes.FirstOrDefault(s => string.Equals(s.Name, _selectedScene.Name, StringComparison.Ordinal));
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

        int GetRemainingPositions(Scene? scene)
        {
            if (scene is null || choreography.Dancers.Count == 0)
            {
                return 0;
            }

            var positions = scene.Positions ?? [];
            return Math.Max(0, choreography.Dancers.Count - positions.Count);
        }

        void DrawPlacementOverlay(int remainingPositions)
        {
            if (remainingPositions <= 0)
            {
                return;
            }

            const float padding = 12f;
            const float spacing = 4f;
            const float cornerRadius = 8f;

            using var titlePaint = new SKPaint();
            titlePaint.Color = GetColor(MaterialDesignColorKey.OnSurface);
            titlePaint.IsAntialias = true;

            using var bodyPaint = new SKPaint();
            bodyPaint.Color = GetColor(MaterialDesignColorKey.OnSurfaceVariant);
            bodyPaint.IsAntialias = true;

            using var titleFont = new SKFont();
            titleFont.Size = 16f;
            titleFont.Edging = SKFontEdging.Antialias;

            using var bodyFont = new SKFont();
            bodyFont.Size = 14f;
            bodyFont.Edging = SKFontEdging.Antialias;

            string title = "Placement mode";
            string line1 = "Tap to place a position";
            string line2 = $"Remaining: {remainingPositions}";

            float titleWidth = titleFont.MeasureText(title);
            float line1Width = bodyFont.MeasureText(line1);
            float line2Width = bodyFont.MeasureText(line2);

            float maxWidth = Math.Max(titleWidth, Math.Max(line1Width, line2Width));
            float titleHeight = titleFont.Metrics.Descent - titleFont.Metrics.Ascent;
            float bodyHeight = bodyFont.Metrics.Descent - bodyFont.Metrics.Ascent;

            float rectWidth = maxWidth + padding * 2f;
            float rectHeight = padding * 2f + titleHeight + bodyHeight * 2f + spacing * 2f;

            float rectX = padding;
            float rectY = padding;

            using var backgroundPaint = new SKPaint();
            backgroundPaint.Color = GetColor(MaterialDesignColorKey.SurfaceVariant).WithAlpha(230);
            backgroundPaint.IsAntialias = true;

            var rect = new SKRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            var roundedRect = new SKRoundRect(rect, cornerRadius, cornerRadius);
            canvas.DrawRoundRect(roundedRect, backgroundPaint);

            float textX = rectX + padding;
            float titleBaseline = rectY + padding - titleFont.Metrics.Ascent;
            canvas.DrawText(title, textX, titleBaseline, titleFont, titlePaint);

            float line1Baseline = titleBaseline + titleHeight + spacing;
            canvas.DrawText(line1, textX, line1Baseline, bodyFont, bodyPaint);

            float line2Baseline = line1Baseline + bodyHeight + spacing;
            canvas.DrawText(line2, textX, line2Baseline, bodyFont, bodyPaint);

        }
    }

    private enum BlackOrWhite
    {
        Black,
        White
    }

    private static BlackOrWhite PickBlackOrWhite(SKColor color)
    {
        float luminance = RelativeLuminance(color.Red, color.Green, color.Blue);

        // explicit contrast check
        float contrastBlack = (luminance + 0.05f) / 0.05f;
        float contrastWhite = 1.05f / (luminance + 0.05f);
        return contrastWhite > contrastBlack
            ? BlackOrWhite.White
            : BlackOrWhite.Black;

        // Equivalent shortcut threshold:
        // return luminance < 0.179
        //     ? BlackOrWhite.White
        //     : BlackOrWhite.Black;
    }

    private static float RelativeLuminance(byte r8, byte g8, byte b8)
    {
        float r = LinearizeChannel(r8 / 255.0f);
        float g = LinearizeChannel(g8 / 255.0f);
        float b = LinearizeChannel(b8 / 255.0f);

        return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }

    private static float LinearizeChannel(float srgb)
        => srgb <= 0.04045f
            ? srgb / 12.92f
            : MathF.Pow((srgb + 0.055f) / 1.055f, 2.4f);
}
