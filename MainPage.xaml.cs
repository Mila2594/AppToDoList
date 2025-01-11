using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace AppToDoList
{
    public partial class MainPage : ContentPage
    {
        private TaskViewModel _viewModel = new TaskViewModel(); // Instancia del ViewModel

        public MainPage()
        {
            InitializeComponent();
            var taskContainer = TaskContainer.Instance;
            taskList_ListView.ItemsSource = taskContainer.Tasks;

            // Suscribirse al evento de cambio de la lista de tareas
            taskContainer.Tasks.CollectionChanged += TaskList_CollectionChanged;
            UpdateListStatus();
        }

        // Evento al presionar el botón de "Editar Lista"
        private async void OnEditListClicked(object sender, EventArgs e)
        {
            // Navegar a la página de edición de lista y pasar la lista de tareas
            await Shell.Current.GoToAsync("//EditListPage");
        }

        // Método para actualizar el estado de la lista y la interfaz
        private void UpdateListStatus ()
        {
            int taskCount = TaskContainer.Instance.Tasks.Count;

            if (taskCount > 0)
            {
                //si hay tareas, muestra el numero de tareas y la lista, y se oculta el icono de la app
                string taskWord = taskCount > 1 ? "tareas" : "tarea";
                statusList_Lavel.Text = $"La lista tiene {taskCount} {taskWord}";
                taskList_ListView.IsVisible = true ;
                emptyIcon.IsVisible = false ;
            }
            else
            {
                //si no hay tareas, muestra el mensaje de lista vacía y el icono de la app, y se oculta el elemento listView
                statusList_Lavel.Text = "La lista está vacía";
                taskList_ListView.IsVisible = false ;
                emptyIcon.IsVisible = true ;
            }
        }

        //evento para manejar los cambios en la lista de tareas
        private void TaskList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateListStatus();
        }

        // Evento cuando se cambia el estado del CheckBox
        private async void OnCheckBoxCheckedChanged (object? sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && e.Value)
            {
                var task = checkbox.BindingContext as TaskItem;
                if (task == null)
                    return;

                // Usar el ViewModel para manejar la lógica de la alerta
                await _viewModel.HandleTaskCompletionAsync(task);
            }

        }

        // Evento cuando se hace clic en el Label
        private void OnLabelTapped (object sender, TappedEventArgs e)
        {
            if (sender is Label label && e.Parameter is TaskItem task)
            {
                // Cambiar el estado del CheckBox asociado a esta tarea
                task.IsCompleted = !task.IsCompleted;

                // Forzar la actualización visual del ListView
                OnCheckBoxCheckedChanged(label.BindingContext as CheckBox, new CheckedChangedEventArgs(task.IsCompleted));
            }
        }

    }

}
