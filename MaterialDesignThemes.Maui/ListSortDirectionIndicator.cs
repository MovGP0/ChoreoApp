using System.ComponentModel;

namespace MaterialDesignThemes.Maui;

public class ListSortDirectionIndicator : TemplatedView
{
    public const string DirectionGroupName = "Direction";
    public const string NoneStateName = "None";
    public const string AscendingStateName = "Ascending";
    public const string DescendingStateName = "Descending";

    public static readonly BindableProperty ListSortDirectionProperty = BindableProperty.Create(
        nameof(ListSortDirection),
        typeof(ListSortDirection?),
        typeof(ListSortDirectionIndicator),
        propertyChanged: OnListSortDirectionChanged);

    private static readonly BindablePropertyKey IsNeutralPropertyKey = BindableProperty.CreateReadOnly(
        nameof(IsNeutral),
        typeof(bool),
        typeof(ListSortDirectionIndicator),
        true);

    public static readonly BindableProperty IsNeutralProperty = IsNeutralPropertyKey.BindableProperty;

    public ListSortDirection? ListSortDirection
    {
        get => (ListSortDirection?)GetValue(ListSortDirectionProperty);
        set => SetValue(ListSortDirectionProperty, value);
    }

    public bool IsNeutral
    {
        get => (bool)GetValue(IsNeutralProperty);
        private set => SetValue(IsNeutralPropertyKey, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        GotoVisualState(ListSortDirection);
    }

    private static void OnListSortDirectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ListSortDirectionIndicator indicator)
        {
            indicator.GotoVisualState(indicator.ListSortDirection);
            indicator.IsNeutral = !indicator.ListSortDirection.HasValue;
        }
    }

    private void GotoVisualState(ListSortDirection? direction)
    {
        var stateName = direction.HasValue
            ? (direction.Value == System.ComponentModel.ListSortDirection.Ascending
                ? AscendingStateName
                : DescendingStateName)
            : NoneStateName;

        VisualStateManager.GoToState(this, stateName);
    }
}
