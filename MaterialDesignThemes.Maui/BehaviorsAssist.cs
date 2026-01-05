namespace MaterialDesignThemes.Maui;

public static class BehaviorsAssist
{
    private static readonly BindableProperty OriginalBehaviorProperty = BindableProperty.CreateAttached(
        "OriginalBehavior",
        typeof(Behavior),
        typeof(BehaviorsAssist),
        null);

    private static void SetOriginalBehavior(BindableObject obj, Behavior? value) =>
        obj.SetValue(OriginalBehaviorProperty, value);

    private static Behavior? GetOriginalBehavior(BindableObject obj) =>
        (Behavior?)obj.GetValue(OriginalBehaviorProperty);

    public static readonly BindableProperty BehaviorsProperty = BindableProperty.CreateAttached(
        "Behaviors",
        typeof(BehaviorCollection),
        typeof(BehaviorsAssist),
        null,
        propertyChanged: OnPropertyChanged);

    public static void SetBehaviors(BindableObject element, BehaviorCollection? value) =>
        element.SetValue(BehaviorsProperty, value);

    public static BehaviorCollection? GetBehaviors(BindableObject element) =>
        (BehaviorCollection?)element.GetValue(BehaviorsProperty);

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not VisualElement visualElement)
        {
            return;
        }

        var newBehaviors = newValue as BehaviorCollection;
        var oldBehaviors = oldValue as BehaviorCollection;
        if (newBehaviors == oldBehaviors)
        {
            return;
        }

        var elementBehaviors = visualElement.Behaviors;
        if (oldBehaviors is not null)
        {
            foreach (var behavior in oldBehaviors)
            {
                var index = GetIndexOf(elementBehaviors, behavior);
                if (index >= 0)
                {
                    elementBehaviors.RemoveAt(index);
                }
            }
        }

        if (newBehaviors is not null)
        {
            foreach (var behavior in newBehaviors)
            {
                var index = GetIndexOf(elementBehaviors, behavior);
                if (index >= 0)
                {
                    continue;
                }

                var clone = TryCloneBehavior(behavior) ?? behavior;
                SetOriginalBehavior(clone, behavior);
                elementBehaviors.Add(clone);
            }
        }
    }

    private static int GetIndexOf(IList<Behavior> behaviors, Behavior behavior)
    {
        var originalBehavior = GetOriginalBehavior(behavior);

        for (var i = 0; i < behaviors.Count; i++)
        {
            var currentBehavior = behaviors[i];
            if (ReferenceEquals(currentBehavior, behavior) || ReferenceEquals(currentBehavior, originalBehavior))
            {
                return i;
            }

            var currentOriginalBehavior = GetOriginalBehavior(currentBehavior);
            if (ReferenceEquals(currentOriginalBehavior, behavior) || ReferenceEquals(currentOriginalBehavior, originalBehavior))
            {
                return i;
            }
        }

        return -1;
    }

    private static Behavior? TryCloneBehavior(Behavior behavior)
    {
        if (behavior is ICloneable cloneable && cloneable.Clone() is Behavior clonedBehavior)
        {
            return clonedBehavior;
        }

        var type = behavior.GetType();
        var ctor = type.GetConstructor(Type.EmptyTypes);
        if (ctor is not null)
        {
            return Activator.CreateInstance(type) as Behavior;
        }

        return null;
    }
}
