using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using AppToDoList;

    public class TaskContainer
    {
        private static readonly Lazy<TaskContainer> _instance = new Lazy<TaskContainer>(() => new TaskContainer());
        public static TaskContainer Instance => _instance.Value;
        public ObservableCollection<TaskItem> Tasks { get; set; }

        private TaskContainer()
        {
            Tasks = new ObservableCollection<TaskItem>();
        }
    }




