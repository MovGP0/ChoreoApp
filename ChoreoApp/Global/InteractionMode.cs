namespace ChoreoApp.Global;

public enum InteractionMode
{
    /// <summary>
    /// View the choreography in a read-only mode.
    /// </summary>
    View,

    /// <summary>
    /// Select and move the position of the selected dancer(s).
    /// <br/>
    /// When the user hovers the mouse over a given position, the position is highlighted.
    /// <br/>
    /// This is done by press&amp;hold (e.g. left mouse button or finger) on a position
    /// and then drag to a new position.
    /// The position will be snapped to the grid on release.
    /// <br/>
    /// To move multiple dancers, we first need a selection.
    /// This is done by pressing down away from the position and then drag a rectangular selection.
    /// The positions within the rectangle will be added to the selection.
    /// When a dancer of the selection is moved, the whole selection moves.
    /// </summary>
    Move,

    /// <summary>
    /// Rotate the selected positions around the center point of the positions within a selection.
    /// </summary>
    RotateAroundCenter,

    /// <summary>
    /// Rotate the selected positions around a selected dancer.
    /// </summary>
    RotateAroundDancer,

    /// <summary>
    /// Scale the position distances around the center of the positions.
    /// </summary>
    Scale,

    /// <summary>
    /// Modify the view direction of the dancer.
    /// </summary>
    LineOfSight
}
