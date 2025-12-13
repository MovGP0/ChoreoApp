using Shouldly;

namespace ChoreoMasterMobile.Json.Tests;

public class ImportTests
{
    [Fact]
    public void Import_LalaLand_File_Succeeds()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "TestData", "Test.choreo");
        File.Exists(path).ShouldBeTrue($"the file '{path}' was not found");

        Choreography choreography = Util.ImportFromFile(path);

        choreography
            .ShouldNotBeNull()
            .ShouldSatisfyAllConditions(
                () => choreography.Name.ShouldBe("ChoreoName"),
                () => choreography.Settings.ShouldNotBeNull(),
                () => choreography.Floor.ShouldNotBeNull(),
                () => choreography.Roles.ShouldNotBeEmpty(),
                () => choreography.Dancers.ShouldNotBeEmpty(),
                () => choreography.Scenes.ShouldNotBeEmpty());
    }
}
