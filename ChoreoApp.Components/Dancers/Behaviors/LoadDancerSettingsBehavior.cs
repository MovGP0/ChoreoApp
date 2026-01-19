using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.Models;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class LoadDancerSettingsBehavior(GlobalStateModel globalState, ILogger<DancerSettingsViewModel> logger)
    : IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(LoadDancerSettingsBehavior), nameof(DancerSettingsViewModel));
        globalState
            .WhenAnyValue(state => state.Choreography)
            .Subscribe(choreography => LoadFromChoreography(viewModel, choreography))
            .DisposeWith(disposables);
    }

    private static void LoadFromChoreography(DancerSettingsViewModel viewModel, ChoreographyModel choreography)
    {
        viewModel.Roles.Clear();
        viewModel.Dancers.Clear();

        var roleMap = new Dictionary<RoleModel, RoleModel>();
        foreach (var role in choreography.Roles)
        {
            var copy = new RoleModel
            {
                Name = role.Name,
                Color = role.Color,
                ZIndex = role.ZIndex
            };
            viewModel.Roles.Add(copy);
            roleMap[role] = copy;
        }

        EnsureDefaultRoles(viewModel.Roles);

        foreach (var dancer in choreography.Dancers)
        {
            var role = dancer.Role is not null && roleMap.TryGetValue(dancer.Role, out var mapped)
                ? mapped
                : viewModel.Roles.FirstOrDefault(r => string.Equals(r.Name, dancer.Role?.Name, StringComparison.OrdinalIgnoreCase))
                  ?? viewModel.Roles.First();

            var copy = new DancerModel
            {
                DancerId = dancer.DancerId,
                Role = role,
                Name = dancer.Name,
                Shortcut = dancer.Shortcut,
                Color = dancer.Color,
                Icon = dancer.Icon
            };

            viewModel.Dancers.Add(copy);
        }

        viewModel.SelectedDancer = viewModel.Dancers.FirstOrDefault();
    }

    private static void EnsureDefaultRoles(ICollection<RoleModel> roles)
    {
        if (roles.Count > 0)
        {
            return;
        }

        roles.Add(new RoleModel { Name = "Dame" });
        roles.Add(new RoleModel { Name = "Herr" });
    }
}
