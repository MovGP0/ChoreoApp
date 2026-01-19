using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Models;
using ChoreoMasterMobile.Json;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Dancers.Behaviors;

public sealed class AddDancerBehavior(
    ILogger<DancerSettingsViewModel> logger):
    IBehavior<DancerSettingsViewModel>
{
    public void Activate(DancerSettingsViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(AddDancerBehavior), nameof(DancerSettingsViewModel));
        viewModel.AddDancerCommand
            .Subscribe(_ => AddDancer(viewModel))
            .DisposeWith(disposables);
    }

    private static void AddDancer(DancerSettingsViewModel viewModel)
    {
        EnsureDefaultRoles(viewModel.Roles);

        var nextId = GetNextDancerId(viewModel.Dancers);
        var role = viewModel.Roles.FirstOrDefault() ?? new RoleModel { Name = "Dame" };
        if (!viewModel.Roles.Contains(role))
        {
            viewModel.Roles.Add(role);
        }

        var dancer = new DancerModel
        {
            DancerId = nextId,
            Role = role,
            Name = string.Empty,
            Shortcut = string.Empty,
            Color = role.Color,
            Icon = null
        };

        viewModel.Dancers.Add(dancer);
        viewModel.SelectedDancer = dancer;
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

    private static DancerId GetNextDancerId(IReadOnlyCollection<DancerModel> dancers)
    {
        var max = 0;
        foreach (var dancer in dancers)
        {
            var value = (int)dancer.DancerId;
            if (value > max)
            {
                max = value;
            }
        }

        return (DancerId)(max + 1);
    }
}
