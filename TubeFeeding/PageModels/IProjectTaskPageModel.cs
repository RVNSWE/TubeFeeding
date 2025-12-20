using CommunityToolkit.Mvvm.Input;
using TubeFeeding.Models;

namespace TubeFeeding.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}