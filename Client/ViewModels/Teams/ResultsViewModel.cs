using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Client.Service;

namespace Client.ViewModels.Teams
{
    public class ResultsViewModel : ViewModelBase
    {
        // private readonly IApiClient _apiClient;
        // public ObservableCollection<> Tasks { get; }
        //
        // public async Task FetchTasks()
        // {
        //     var response = await _apiClient.PostAsyncAuth<List<>>("/meeting/results",
        //         new  { DateTime = DateTime.UtcNow.AddDays(-7) });
        //     Tasks.Clear();
        //     if (response.IsOk)
        //     {
        //         foreach (var r in response.Value)
        //         {
        //             Tasks.Add(r);
        //         }
        //     }
        // }
        //
        // public ResultsViewModel()
        // {
        //     _apiClient = Services.GetService<IApiClient>();
        //     Tasks = new ObservableCollection<>();
        // }
    }
}
