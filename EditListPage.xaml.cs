using System.Collections.ObjectModel;
using AppToDoList;

namespace AppToDoList;

public partial class EditListPage : ContentPage
{
    private TaskViewModel _viewModel = new TaskViewModel(); // Instancia del ViewModel

    // Constructor de la clase
    public EditListPage()
    {
        InitializeComponent();
        var taskContainer = TaskContainer.Instance;
        taskList_ListView.ItemsSource = taskContainer.Tasks;

        // Suscribirse al evento de cambio de la lista de tareas
        taskContainer.Tasks.CollectionChanged += TaskList_CollectionChanged;
        UpdateStatusLabel();
    }

    //metodo para actualizar el Label de estado según el número de tareas
    private void UpdateStatusLabel()
    {
        int taskCount = TaskContainer.Instance.Tasks.Count;

        if (taskCount > 0)
        {
            string taskWord = taskCount > 1 ? "tareas" : "tarea";
            statusList_Lavel.Text = $"La lista tiene {taskCount} {taskWord}";
        }
        else
        {
            statusList_Lavel.Text = "La lista está vacía";
        }
    }

    //evento al presionar el bóton Agregar Tarea
    private async void OnAddTaskClicked (object sender, EventArgs e)
    {
        //obtener el nombre de la tarea
        string taskname = taskNameEntry.Text;
        if (string.IsNullOrWhiteSpace(taskname)) 
        {
            await DisplayAlert("Error", "Por favor ingresa un nombre de tarea", "Ok");
            return;
        }

        //determinar prioridad según los checkbox
        int priority = 0;
        if (priorityCheckBox1.IsChecked)
        {
            priority = 1;
        }
        else if (priorityCheckBox2.IsChecked)
        {
            priority = 2;
        }
        else if (priorityCheckBox3.IsChecked)
        {
            priority = 3;
        }
        else
        {
            await DisplayAlert("Error", "Por favor selecciona una prioridad", "Ok");
            return;
        }

        //crea una tarea nueva y se agrega a la lista de tareas 
        var newTask = new AppToDoList.TaskItem(taskname, GetColorForPriority(priority));
        TaskContainer.Instance.Tasks.Add(newTask);

        //limpiar entrada y checkbox
        taskNameEntry.Text = string.Empty;
        priorityCheckBox1.IsChecked = false;
        priorityCheckBox2.IsChecked = false;
        priorityCheckBox3.IsChecked = false;

        //Actualizar el Label de estado
        UpdateStatusLabel();

    }

    private void TaskList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateStatusLabel();
    }

    //evento al presionar el botón Cerrar
    private async void OnCloseClicked(object sender, EventArgs e)
    {
        // Volver a MainPage
        await Shell.Current.GoToAsync("//MainPage");
    }


    //metodo para establer color según prioridad
    private string GetColorForPriority(int priority)
    {
        //devolver un codgio de color segun la prioridad
        return priority switch
        {
            1 => "#BF360C", //alta prioridad - rojo
            2 => "#00838F", //media prioridad - amarillo
            3 => "#BA68C8",  //baja prioridad - verde
            _ => "#808080" //por defecto
        };
    }

    //control del evento CheckedChanged para el CheckBox
    private async void OnCheckBoxCheckedChanged_Edit (object sender, CheckedChangedEventArgs e)
    {

        //si sender es de tipo CheckBox entonces se convierte a CheckBox y se asigna a la variable checkBox
        //y se verifica si la propiedad BindingContext del checkBox es de tipo TaskItem, se asigna el valor de BindingContext a la variable task de tipo TaskItem
        if (sender is CheckBox checkbox && e.Value)
        {
            var task = checkbox.BindingContext as TaskItem;
            if (task == null)
                return;

            // Asegúrate de que el argumento sea "EditListPage"
            await _viewModel.HandleTaskCompletionAsync(task);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ResetEditFrame(); // Llama al método para reiniciar el Frame de edición
    }



    // Método para reiniciar el Frame de edición
    private void ResetEditFrame()
    {
        taskNameEntry.Text = string.Empty;
        priorityCheckBox1.IsChecked = false;
        priorityCheckBox2.IsChecked = false;
        priorityCheckBox3.IsChecked = false;
    }

    //control checkbox según tap del label
    private void OnPriorityLabelTapped(object sender, TappedEventArgs e) 
    {
        if (e.Parameter is CheckBox selectedCheckBox)
        {
            // Cambiar el estado del CheckBox al seleccionado
            selectedCheckBox.IsChecked = true;

            // Llama al método para desmarcar los demás CheckBoxes
            UpdatePriorityCheckBoxes(selectedCheckBox);
        }

    }

    //control del checkbox
    private void OnPriorityCheckBoxChanged(object sender, CheckedChangedEventArgs e) 
    {
        if (sender is CheckBox selectedCheckbox && e.Value)
        {
            // Solo desmarcar los demás si este CheckBox está siendo marcado
            UpdatePriorityCheckBoxes(selectedCheckbox);
        }
    }

    private void UpdatePriorityCheckBoxes(CheckBox selectedCheckBox)
    {
        // Desmarcar los demás CheckBoxes
        if (selectedCheckBox != priorityCheckBox1)
        {
            priorityCheckBox1.IsChecked = false;
        }
        if (selectedCheckBox != priorityCheckBox2)
        {
            priorityCheckBox2.IsChecked = false;
        }
        if (selectedCheckBox != priorityCheckBox3)
        {
            priorityCheckBox3.IsChecked = false;
        }
    }

}