using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Client.Service;

namespace Client.ViewModels.Teams
{
    public class CreateViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private string _newTask;

        public string NewTask
        {
            get => _newTask;
            set
            {
                _newTask = value;
                OnPropertyChanged(nameof(NewTask));
            }
        }

        public ObservableCollection<string> Tasks { get; }

        public void AddTask()
        {
            if (NewTask.Length > 0 && !Tasks.Contains(NewTask))
            {
                Tasks.Add(NewTask);
                NewTask = "";
            }

        }

        public void DeleteTask(string taskName)
        {
            if (Tasks.Contains(taskName))
            {
                Tasks.Remove(taskName);
            }
        }

        public CreateViewModel()
        {
            _apiClient = Services.GetService<IApiClient>();
            Tasks = new ObservableCollection<string>();
        }
    }
}
