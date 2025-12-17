using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using ChoreoApp.Algorithms.Hungarian;
using ChoreoApp.Algorithms.MinCostMaxFlow;
using Shouldly;
using Xunit;

namespace ChoreoApp.Algorithms.Tests;

public sealed class ThreeSceneAssignmentTests
{
    [Fact(DisplayName = "Should choose non-crossing mapping when midpoints are swapped")]
    public void ShouldChooseNonCrossingMappingWhenMidpointsAreSwapped()
    {
        // Arrange
        Vector2[] sceneA =
        [
            new Vector2(0.0f, 0.0f),
            new Vector2(2.0f, 0.0f)
        ];

        Vector2[] sceneB =
        [
            new Vector2(2.0f, 1.0f), // nearer to A1
            new Vector2(0.0f, 1.0f)  // nearer to A0
        ];

        Vector2[] sceneC =
        [
            new Vector2(0.0f, 2.0f),
            new Vector2(2.0f, 2.0f)
        ];

        // Act
        int[] hungarian = ThreeSceneTransitionPlanner.ComputeMidSceneAssignment(sceneA, sceneB, sceneC);
        int[] minCost = ThreeSceneAssignmentViaMinCostFlow.Solve(sceneA, sceneB, sceneC);

        // Assert
        hungarian.ShouldBe(new[] { 1, 0 });
        minCost.ShouldBe(new[] { 1, 0 });
    }

    [Fact(DisplayName = "Should fail when scenes differ in size")]
    public void ShouldFailWhenScenesDifferInSize()
    {
        // Arrange
        Vector2[] sceneA = [ new Vector2(0.0f, 0.0f) ];
        Vector2[] sceneB = [ new Vector2(1.0f, 0.0f), new Vector2(2.0f, 0.0f) ];
        Vector2[] sceneC = [ new Vector2(0.0f, 1.0f) ];

        // Act & Assert
        Should.Throw<ArgumentException>(() => ThreeSceneTransitionPlanner.ComputeMidSceneAssignment(sceneA, sceneB, sceneC));
        Should.Throw<ArgumentException>(() => ThreeSceneAssignmentViaMinCostFlow.Solve(sceneA, sceneB, sceneC));
    }

    [Fact(DisplayName = "Should keep dancer identities on real choreography sample")]
    public void ShouldKeepDancerIdentitiesOnRealChoreographySample()
    {
        // Arrange
        IReadOnlyList<Vector2> sceneA;
        IReadOnlyList<Vector2> sceneB;
        IReadOnlyList<Vector2> sceneC;
        (sceneA, sceneB, sceneC) = LoadFirstThreeScenes();

        // Act
        int[] hungarian = ThreeSceneTransitionPlanner.ComputeMidSceneAssignment(sceneA, sceneB, sceneC);
        int[] minCost = ThreeSceneAssignmentViaMinCostFlow.Solve(sceneA, sceneB, sceneC);

        // Assert
        for (int index = 0; index < sceneA.Count; index++)
        {
            hungarian[index].ShouldBe(index);
            minCost[index].ShouldBe(index);
        }

        hungarian.ShouldBe(minCost);
    }

    private static (IReadOnlyList<Vector2> sceneA, IReadOnlyList<Vector2> sceneB, IReadOnlyList<Vector2> sceneC) LoadFirstThreeScenes()
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "Test.choreo");
        string json = File.ReadAllText(filePath);

        using JsonDocument doc = JsonDocument.Parse(json);
        JsonElement scenes = doc.RootElement.GetProperty("Scenes");

        List<(int dancerId, Vector2 position)>[] collected = new List<(int dancerId, Vector2 position)>[3]
        {
            new List<(int dancerId, Vector2 position)>(),
            new List<(int dancerId, Vector2 position)>(),
            new List<(int dancerId, Vector2 position)>()
        };

        for (int sceneIndex = 0; sceneIndex < 3; sceneIndex++)
        {
            JsonElement scene = scenes[sceneIndex];
            JsonElement positions = scene.GetProperty("Positions");

            foreach (JsonElement position in positions.EnumerateArray())
            {
                int dancerId = int.Parse(position.GetProperty("Dancer").GetProperty("$ref").GetString()!, CultureInfo.InvariantCulture);
                float x = position.GetProperty("X").GetSingle();
                float y = position.GetProperty("Y").GetSingle();
                collected[sceneIndex].Add((dancerId, new Vector2(x, y)));
            }
        }

        for (int i = 0; i < collected.Length; i++)
        {
            collected[i] = collected[i]
                .OrderBy(pair => pair.dancerId)
                .ToList();
        }

        IReadOnlyList<Vector2> sceneA = collected[0].Select(pair => pair.position).ToList();
        IReadOnlyList<Vector2> sceneB = collected[1].Select(pair => pair.position).ToList();
        IReadOnlyList<Vector2> sceneC = collected[2].Select(pair => pair.position).ToList();

        return (sceneA, sceneB, sceneC);
    }
}
