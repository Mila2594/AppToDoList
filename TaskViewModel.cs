using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppToDoList
{
    public class TaskViewModel
    {
        public ObservableCollection<TaskItem> Tasks { get; set; } = new ObservableCollection<TaskItem>();

        public async Task HandleTaskCompletionAsync(TaskItem task)
        {
            var currentPage = Shell.Current.CurrentPage; // O usa Navigation para obtener la página actual
            string pageName = currentPage.GetType().Name;

            if (pageName == nameof(MainPage))
            {
                bool deleteTask = await Application.Current.MainPage.DisplayAlert(
                    "Tarea Completada",
                    "¿Quieres eliminar la tarea?",
                    "Sí", "No");

                // Verificar si la tarea está en la lista antes de intentar eliminarla
                var taskContainer = TaskContainer.Instance;

                if (taskContainer.Tasks.Contains(task))
                {
                    if (deleteTask)
                    {
                        taskContainer.Tasks.Remove(task); // Eliminar de la colección
                    }
                    else
                    {
                        task.IsCompleted = true;
                        task.PriorityColor = "LightGray";
                    }
                }
               
            }
            else if (pageName == nameof(EditListPage))
            {
                bool action = await Application.Current.MainPage.DisplayAlert(
                    "Tarea Completada",
                    "Acción no permitida. Ir a home para completar la tarea",
                    "Ok", "Ir a Home");

                task.IsCompleted = false;

                if (!action)
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
        }
    }
}
