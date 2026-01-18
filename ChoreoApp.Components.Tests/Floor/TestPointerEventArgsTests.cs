using Shouldly;
using Xunit;

namespace ChoreoApp.Components.Tests.Floor;

public sealed class TestPointerEventArgsTests
{
    [Fact(DisplayName = "Should expose position and button")]
    public void ShouldExposePositionAndButton()
    {
        // Arrange
        var expectedPoint = new Point(12, 34);
        var expectedButton = ButtonsMask.Primary;

        // Act
        var args = new TestPointerEventArgs(expectedPoint, expectedButton);

        // Assert
        args.GetPosition(null).ShouldBe(expectedPoint);
        args.Button.ShouldBe(expectedButton);
    }
}
