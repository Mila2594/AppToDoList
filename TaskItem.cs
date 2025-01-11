using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppToDoList
{
    public class TaskItem : INotifyPropertyChanged
    {
        private string _taskText;
        private bool _isCompleted;
        private string _priorityColor;
        public string SourceView { get; set; } = string.Empty; // Nueva propiedad

        public string TaskText
        {
            get => _taskText;
            set
            {
                if (_taskText != value)
                {
                    _taskText = value;
                    OnPropertyChanged(nameof(TaskText));
                }
            }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                    OnPropertyChanged(nameof(PriorityColor)); // Notifica el cambio de color
                }
            }
        }

        public string PriorityColor
        {
            get => _priorityColor;
            set
            {
                if (_priorityColor != value)
                {
                    _priorityColor = value;
                    OnPropertyChanged(nameof(PriorityColor));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; // Aceptar null para compatibilidad

        public TaskItem(string taskText, string priorityColor)
        {
            _taskText = taskText ?? throw new ArgumentNullException(nameof(taskText)); // Asegúrate de que no sea null
            _priorityColor = priorityColor ?? throw new ArgumentNullException(nameof(priorityColor)); // Asegúrate de que no sea null
            IsCompleted = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
