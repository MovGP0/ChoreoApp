using ChoreoMasterMobile.Json;
using DynamicData.Binding;
using Shouldly;
using Xunit;

namespace ChoreoApp.Models.Tests;

public sealed class ChoreographyModelMapperTests
{
    public ChoreographyModelMapperTests()
    {
        TestDefaults.Initialize();
    }

    [Fact(DisplayName = "Should map JSON choreography to model when invoked")]
    public void ShouldMapJsonChoreographyToModelWhenInvoked()
    {
        // Arrange
        var mapper = new ChoreographyModelMapper();
        var source = BuildJsonChoreography();

        // Act
        var result = mapper.Map(source);

        // Assert
        result.ShouldSatisfyAllConditions(
            () => result.Comment.ShouldBe("comment"),
            () => result.Name.ShouldBe("Choreo"),
            () => result.Subtitle.ShouldBe("Subtitle"),
            () => result.Date.ShouldBe("2026-01-10"),
            () => result.Variation.ShouldBe("Variation"),
            () => result.Author.ShouldBe("Author"),
            () => result.Description.ShouldBe("Description"),
            () => result.LastSaveDate.ShouldBe(new DateTimeOffset(2026, 1, 2, 12, 0, 0, TimeSpan.Zero)),
            () => result.Settings.AnimationMilliseconds.ShouldBe(250),
            () => result.Settings.FrontPosition.ShouldBe(FrontPosition.Left),
            () => result.Settings.DancerPosition.ShouldBe(FrontPosition.Right),
            () => result.Settings.Resolution.ShouldBe(12),
            () => result.Settings.Transparency.ShouldBe(0.75m),
            () => result.Settings.PositionsAtSide.ShouldBeTrue(),
            () => result.Settings.GridLines.ShouldBeTrue(),
            () => result.Settings.FloorColor.ShouldBe(Colors.Blue),
            () => result.Settings.DancerSize.ShouldBe(0.9m),
            () => result.Settings.ShowTimestamps.ShouldBeFalse(),
            () => result.Settings.MusicPathAbsolute.ShouldBe("C:\\music\\track.mp3"),
            () => result.Settings.MusicPathRelative.ShouldBe("track.mp3"),
            () => result.Floor.SizeFront.ShouldBe(10),
            () => result.Floor.SizeBack.ShouldBe(11),
            () => result.Floor.SizeLeft.ShouldBe(12),
            () => result.Floor.SizeRight.ShouldBe(13),
            () => result.Roles.Count.ShouldBe(2),
            () => result.Dancers.Count.ShouldBe(2),
            () => result.Scenes.Count.ShouldBe(2),
            () => ReferenceEquals(result.Dancers[0].Role, result.Roles[0]).ShouldBeTrue(),
            () => ReferenceEquals(result.Dancers[1].Role, result.Roles[1]).ShouldBeTrue(),
            () => result.Scenes[0].Name.ShouldBe("Scene 1"),
            () => result.Scenes[0].Text.ShouldBe("Text"),
            () => result.Scenes[0].FixedPositions.ShouldBeTrue(),
            () => result.Scenes[0].Timestamp.ShouldBe(TimeSpan.FromSeconds(12)),
            () => result.Scenes[0].VariationDepth.ShouldBe(1),
            () => result.Scenes[0].Color.ShouldBe(Colors.Green),
            () => result.Scenes[0].Positions.Count.ShouldBe(2),
            () => ReferenceEquals(result.Scenes[0].Positions[0].Dancer, result.Dancers[0]).ShouldBeTrue(),
            () => result.Scenes[0].Positions[0].Orientation.ShouldBe(90.0),
            () => result.Scenes[0].Positions[0].X.ShouldBe(1.25),
            () => result.Scenes[0].Positions[0].Y.ShouldBe(2.5),
            () => result.Scenes[0].Positions[0].Curve1X.ShouldBe(0.1),
            () => result.Scenes[0].Positions[0].Curve1Y.ShouldBe(0.2),
            () => result.Scenes[0].Positions[0].Curve2X.ShouldBe(0.3),
            () => result.Scenes[0].Positions[0].Curve2Y.ShouldBe(0.4),
            () => result.Scenes[0].Positions[0].Movement1X.ShouldBe(0.5),
            () => result.Scenes[0].Positions[0].Movement1Y.ShouldBe(0.6),
            () => result.Scenes[0].Positions[0].Movement2X.ShouldBe(0.7),
            () => result.Scenes[0].Positions[0].Movement2Y.ShouldBe(0.8),
            () => result.Scenes[0].Variations.Count.ShouldBe(1),
            () => result.Scenes[0].Variations[0].Count.ShouldBe(1),
            () => result.Scenes[0].Variations[0][0].Name.ShouldBe("Variation Scene"),
            () => result.Scenes[0].CurrentVariation.Count.ShouldBe(1),
            () => result.Scenes[0].CurrentVariation[0].Name.ShouldBe("Current Variation"),
            () => result.Scenes[1].Positions.Count.ShouldBe(0)
        );
    }

    [Fact(DisplayName = "Should map model choreography to JSON when invoked")]
    public void ShouldMapModelChoreographyToJsonWhenInvoked()
    {
        // Arrange
        var mapper = new ChoreographyModelMapper();
        var source = BuildModelChoreography();

        // Act
        var result = mapper.Map(source);

        // Assert
        result.ShouldSatisfyAllConditions(
            () => result.Comment.ShouldBe("comment"),
            () => result.Name.ShouldBe("Choreo"),
            () => result.Subtitle.ShouldBe("Subtitle"),
            () => result.Date.ShouldBe("2026-01-10"),
            () => result.Variation.ShouldBe("Variation"),
            () => result.Author.ShouldBe("Author"),
            () => result.Description.ShouldBe("Description"),
            () => result.LastSaveDate.ShouldBe(new DateTimeOffset(2026, 1, 2, 12, 0, 0, TimeSpan.Zero)),
            () => result.Settings.AnimationMilliseconds.ShouldBe(250),
            () => result.Settings.FrontPosition.ShouldBe(FrontPosition.Left),
            () => result.Settings.DancerPosition.ShouldBe(FrontPosition.Right),
            () => result.Settings.Resolution.ShouldBe(12),
            () => result.Settings.Transparency.ShouldBe(0.75m),
            () => result.Settings.PositionsAtSide.ShouldBeTrue(),
            () => result.Settings.GridLines.ShouldBeTrue(),
            () => result.Settings.FloorColor.ShouldBe(Colors.Blue),
            () => result.Settings.DancerSize.ShouldBe(0.9m),
            () => result.Settings.ShowTimestamps.ShouldBeFalse(),
            () => result.Settings.MusicPathAbsolute.ShouldBe("C:\\music\\track.mp3"),
            () => result.Settings.MusicPathRelative.ShouldBe("track.mp3"),
            () => result.Floor.SizeFront.ShouldBe(10),
            () => result.Floor.SizeBack.ShouldBe(11),
            () => result.Floor.SizeLeft.ShouldBe(12),
            () => result.Floor.SizeRight.ShouldBe(13),
            () => result.Roles.Count.ShouldBe(2),
            () => result.Dancers.Count.ShouldBe(2),
            () => result.Scenes.Count.ShouldBe(2),
            () => ReferenceEquals(result.Dancers[0].Role, result.Roles[0]).ShouldBeTrue(),
            () => ReferenceEquals(result.Dancers[1].Role, result.Roles[1]).ShouldBeTrue(),
            () => result.Scenes[0].Name.ShouldBe("Scene 1"),
            () => result.Scenes[0].Text.ShouldBe("Text"),
            () => result.Scenes[0].FixedPositions.ShouldBeTrue(),
            () => result.Scenes[0].Timestamp.ShouldBe(TimeSpan.FromSeconds(12)),
            () => result.Scenes[0].VariationDepth.ShouldBe(1),
            () => result.Scenes[0].Color.ShouldBe(Colors.Green),
            () => result.Scenes[0].Positions.ShouldNotBeNull(),
            () => result.Scenes[0].Positions!.Count.ShouldBe(2),
            () => ReferenceEquals(result.Scenes[0].Positions![0].Dancer, result.Dancers[0]).ShouldBeTrue(),
            () => result.Scenes[0].Positions![0].Orientation.ShouldBe(90.0),
            () => result.Scenes[0].Positions![0].X.ShouldBe(1.25),
            () => result.Scenes[0].Positions![0].Y.ShouldBe(2.5),
            () => result.Scenes[0].Positions![0].Curve1X.ShouldBe(0.1),
            () => result.Scenes[0].Positions![0].Curve1Y.ShouldBe(0.2),
            () => result.Scenes[0].Positions![0].Curve2X.ShouldBe(0.3),
            () => result.Scenes[0].Positions![0].Curve2Y.ShouldBe(0.4),
            () => result.Scenes[0].Positions![0].Movement1X.ShouldBe(0.5),
            () => result.Scenes[0].Positions![0].Movement1Y.ShouldBe(0.6),
            () => result.Scenes[0].Positions![0].Movement2X.ShouldBe(0.7),
            () => result.Scenes[0].Positions![0].Movement2Y.ShouldBe(0.8),
            () => result.Scenes[0].Variations.ShouldNotBeNull(),
            () => result.Scenes[0].Variations!.Count.ShouldBe(1),
            () => result.Scenes[0].Variations![0].Count.ShouldBe(1),
            () => result.Scenes[0].Variations![0][0].Name.ShouldBe("Variation Scene"),
            () => result.Scenes[0].CurrentVariation.ShouldNotBeNull(),
            () => result.Scenes[0].CurrentVariation!.Count.ShouldBe(1),
            () => result.Scenes[0].CurrentVariation![0].Name.ShouldBe("Current Variation"),
            () => result.Scenes[1].Positions.ShouldBeNull()
        );
    }

    private static Choreography BuildJsonChoreography()
    {
        var roleLead = new Role
        {
            ZIndex = 1,
            Name = "Lead",
            Color = Colors.Red
        };

        var roleFollow = new Role
        {
            ZIndex = 2,
            Name = "Follow",
            Color = Colors.Purple
        };

        var dancerA = new Dancer
        {
            DancerId = (DancerId)1,
            Role = roleLead,
            Name = "Alice",
            Shortcut = "A",
            Color = Colors.Orange,
            Icon = "icon-a"
        };

        var dancerB = new Dancer
        {
            DancerId = (DancerId)2,
            Role = roleFollow,
            Name = "Bob",
            Shortcut = "B",
            Color = Colors.Teal,
            Icon = "icon-b"
        };

        var scene1 = BuildJsonScene(dancerA, dancerB);
        var scene2 = BuildJsonScene(dancerA, dancerB);
        scene2.SceneId = (SceneId)11;
        scene2.Name = "Scene 2";
        scene2.Text = null;
        scene2.FixedPositions = false;
        scene2.Timestamp = null;
        scene2.VariationDepth = 0;
        scene2.Color = Colors.Transparent;
        scene2.Positions = null;
        scene2.Variations = null;
        scene2.CurrentVariation = null;

        return new Choreography
        {
            Comment = "comment",
            Name = "Choreo",
            Subtitle = "Subtitle",
            Date = "2026-01-10",
            Variation = "Variation",
            Author = "Author",
            Description = "Description",
            LastSaveDate = new DateTimeOffset(2026, 1, 2, 12, 0, 0, TimeSpan.Zero),
            Settings = new Settings
            {
                AnimationMilliseconds = 250,
                FrontPosition = FrontPosition.Left,
                DancerPosition = FrontPosition.Right,
                Resolution = 12,
                Transparency = 0.75m,
                PositionsAtSide = true,
                GridLines = true,
                FloorColor = Colors.Blue,
                DancerSize = 0.9m,
                ShowTimestamps = false,
                MusicPathAbsolute = "C:\\music\\track.mp3",
                MusicPathRelative = "track.mp3"
            },
            Floor = new Floor
            {
                SizeFront = 10,
                SizeBack = 11,
                SizeLeft = 12,
                SizeRight = 13
            },
            Roles = [roleLead, roleFollow],
            Dancers = [dancerA, dancerB],
            Scenes = [scene1, scene2]
        };
    }

    private static Scene BuildJsonScene(Dancer dancerA, Dancer dancerB)
    {
        var scene = new Scene
        {
            SceneId = (SceneId)10,
            Name = "Scene 1",
            Text = "Text",
            FixedPositions = true,
            Timestamp = TimeSpan.FromSeconds(12),
            VariationDepth = 1,
            Color = Colors.Green,
            Positions =
            [
                new Position
                {
                    Dancer = dancerA,
                    Orientation = 90.0,
                    X = 1.25,
                    Y = 2.5,
                    Curve1X = 0.1,
                    Curve1Y = 0.2,
                    Curve2X = 0.3,
                    Curve2Y = 0.4,
                    Movement1X = 0.5,
                    Movement1Y = 0.6,
                    Movement2X = 0.7,
                    Movement2Y = 0.8
                },
                new Position
                {
                    Dancer = dancerB,
                    X = 4.0,
                    Y = 5.0
                }
            ],
            Variations =
            [
                [
                    new Scene
                    {
                        SceneId = (SceneId)20,
                        Name = "Variation Scene",
                        VariationDepth = 2,
                        Color = Colors.Gold
                    }
                ]
            ],
            CurrentVariation =
            [
                new Scene
                {
                    SceneId = (SceneId)30,
                    Name = "Current Variation",
                    VariationDepth = 3,
                    Color = Colors.Cyan
                }
            ]
        };

        return scene;
    }

    private static ChoreographyModel BuildModelChoreography()
    {
        var roleLead = new RoleModel
        {
            ZIndex = 1,
            Name = "Lead",
            Color = Colors.Red
        };

        var roleFollow = new RoleModel
        {
            ZIndex = 2,
            Name = "Follow",
            Color = Colors.Purple
        };

        var dancerA = new DancerModel
        {
            DancerId = (DancerId)1,
            Role = roleLead,
            Name = "Alice",
            Shortcut = "A",
            Color = Colors.Orange,
            Icon = "icon-a"
        };

        var dancerB = new DancerModel
        {
            DancerId = (DancerId)2,
            Role = roleFollow,
            Name = "Bob",
            Shortcut = "B",
            Color = Colors.Teal,
            Icon = "icon-b"
        };

        var scene1 = BuildModelScene(dancerA, dancerB);
        var scene2 = BuildModelScene(dancerA, dancerB);
        scene2.SceneId = (SceneId)11;
        scene2.Name = "Scene 2";
        scene2.Text = null;
        scene2.FixedPositions = false;
        scene2.Timestamp = null;
        scene2.VariationDepth = 0;
        scene2.Color = Colors.Transparent;
        scene2.Positions.Clear();
        scene2.Variations.Clear();
        scene2.CurrentVariation.Clear();

        var model = new ChoreographyModel
        {
            Comment = "comment",
            Name = "Choreo",
            Subtitle = "Subtitle",
            Date = "2026-01-10",
            Variation = "Variation",
            Author = "Author",
            Description = "Description",
            LastSaveDate = new DateTimeOffset(2026, 1, 2, 12, 0, 0, TimeSpan.Zero),
            Settings = new SettingsModel
            {
                AnimationMilliseconds = 250,
                FrontPosition = FrontPosition.Left,
                DancerPosition = FrontPosition.Right,
                Resolution = 12,
                Transparency = 0.75m,
                PositionsAtSide = true,
                GridLines = true,
                FloorColor = Colors.Blue,
                DancerSize = 0.9m,
                ShowTimestamps = false,
                MusicPathAbsolute = "C:\\music\\track.mp3",
                MusicPathRelative = "track.mp3"
            },
            Floor = new FloorModel
            {
                SizeFront = 10,
                SizeBack = 11,
                SizeLeft = 12,
                SizeRight = 13
            }
        };

        model.Roles.Add(roleLead);
        model.Roles.Add(roleFollow);
        model.Dancers.Add(dancerA);
        model.Dancers.Add(dancerB);
        model.Scenes.Add(scene1);
        model.Scenes.Add(scene2);

        return model;
    }

    private static SceneModel BuildModelScene(DancerModel dancerA, DancerModel dancerB)
    {
        var scene = new SceneModel
        {
            SceneId = (SceneId)10,
            Name = "Scene 1",
            Text = "Text",
            FixedPositions = true,
            Timestamp = TimeSpan.FromSeconds(12),
            VariationDepth = 1,
            Color = Colors.Green
        };

        scene.Positions.Add(new PositionModel
        {
            Dancer = dancerA,
            Orientation = 90.0,
            X = 1.25,
            Y = 2.5,
            Curve1X = 0.1,
            Curve1Y = 0.2,
            Curve2X = 0.3,
            Curve2Y = 0.4,
            Movement1X = 0.5,
            Movement1Y = 0.6,
            Movement2X = 0.7,
            Movement2Y = 0.8
        });

        scene.Positions.Add(new PositionModel
        {
            Dancer = dancerB,
            X = 4.0,
            Y = 5.0
        });

        var variationList = new ObservableCollectionExtended<SceneModel>
        {
            new SceneModel
            {
                SceneId = (SceneId)20,
                Name = "Variation Scene",
                VariationDepth = 2,
                Color = Colors.Gold
            }
        };

        scene.Variations.Add(variationList);

        scene.CurrentVariation.Add(new SceneModel
        {
            SceneId = (SceneId)30,
            Name = "Current Variation",
            VariationDepth = 3,
            Color = Colors.Cyan
        });

        return scene;
    }
}
