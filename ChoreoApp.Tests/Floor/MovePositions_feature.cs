using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace ChoreoApp.Tests.Floor;

[FeatureDescription(
    @"In order to edit choreography positions
As a user
I want to select and move positions on the floor")]
public partial class MovePositions_feature
{
    [Scenario(DisplayName = "Should move all selected positions by drag delta")]
    public void MoveSelectedPositionsByDragDelta()
    {
        Runner.RunScenario(
            Given_dependency_injection_is_configured,
            Given_a_choreography_with_positions_is_loaded,
            Given_move_mode_is_active,
            When_the_user_selects_positions_with_rectangle,
            When_the_user_drags_a_selected_position_by_delta,
            Then_all_selected_positions_should_move_by_delta,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should clear selection when clicking outside")]
    public void ClearSelectionWhenClickingOutside()
    {
        Runner.RunScenario(
            Given_dependency_injection_is_configured,
            Given_a_choreography_with_positions_is_loaded,
            Given_move_mode_is_active,
            When_the_user_selects_positions_with_rectangle,
            When_the_user_clicks_outside_of_positions,
            Then_the_selection_should_be_cleared,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should move a single position when dragging")]
    public void MoveSinglePositionWhenDragging()
    {
        Runner.RunScenario(
            Given_dependency_injection_is_configured,
            Given_a_choreography_with_positions_is_loaded,
            Given_move_mode_is_active,
            When_the_user_drags_a_single_position_by_delta,
            Then_only_that_position_should_move,
            Then_cleanup_resources);
    }
}
