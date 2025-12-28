namespace ChoreoApp.ChoreographySettings;

public static class DependencyInjection
{
    public static IServiceCollection AddChoreographySettings(this IServiceCollection services)
    {
        services.AddTransient<IViewFor<ChoreographySettingsViewModel>, ChoreographySettingsView>();
        services.AddTransient<ChoreographySettingsView>();
        services.AddTransient<ChoreographySettingsViewModel>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.LoadChoreographySettingsBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateCommentBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateNameBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateSubtitleBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateDateBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateVariationBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateAuthorBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateDescriptionBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateFloorFrontBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateFloorBackBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateFloorLeftBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateFloorRightBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateGridResolutionBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateDrawPathFromBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateDrawPathToBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateGridLinesBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateFloorColorBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateShowTimestampsBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdatePositionsAtSideBehavior>();
        services.AddTransient<IBehavior<ChoreographySettingsViewModel>, Behaviors.UpdateTransparencyBehavior>();
        return services;
    }
}
