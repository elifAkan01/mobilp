using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp;

[QueryProperty(nameof(Filter), "filter")]
public partial class MainPage : ContentPage
{
    private readonly Database _database;
    private readonly IServiceProvider _serviceProvider;

    public string Filter { get; set; } = "all";

    public MainPage(Database database, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _database = database;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTasks();
    }

    private async Task LoadTasks()
    {
        List<ToDo> tasks;

        if (Filter == "completed")
        {
            Title = "Tamamlanan Görevler";
            tasks = await _database.GetCompletedItemsAsync();
        }
        else if (Filter == "overdue")
        {
            Title = "Süresi Geçmiş Görevler";
            tasks = await _database.GetOverdueItemsAsync();
        }
        else
        {
            Title = "Görev Listesi";
            tasks = await _database.GetItemsAsync();
        }

        TasksCollection.ItemsSource = tasks;
        EmptyStateLayout.IsVisible = tasks.Count == 0;
    }

    private async void OnEditInvoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipe || swipe.CommandParameter is not ToDo item)
            return;

        var editPage = _serviceProvider.GetService<TaskEditPage>();
        if (editPage is null)
            return;

        editPage.SetItem(item);
        await Navigation.PushAsync(editPage);
    }

    private async void OnDeleteInvoked(object sender, EventArgs e)
    {
        if (sender is not SwipeItem swipe || swipe.CommandParameter is not ToDo item)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Sil",
            $"'{item.Title}' görevini silmek istediğine emin misin?",
            "Evet",
            "Hayır");

        if (!confirm)
            return;

        await _database.DeleteItemAsync(item);
        await LoadTasks();
    }

    private bool _isUpdating;

    private async void OnTaskCompletedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (_isUpdating)
            return;

        if (sender is not CheckBox checkbox || checkbox.BindingContext is not ToDo item)
            return;

        try
        {
            _isUpdating = true;

            item.IsCompleted = e.Value;
            await _database.SaveItemAsync(item);

            // Eğer tamamlananlar ekranındaysan ve işareti kaldırdıysan
            // görev listeden düşmesi normal
            await LoadTasks();
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        var editPage = _serviceProvider.GetService<TaskEditPage>();
        if (editPage is null)
            return;

        editPage.SetItem(new ToDo { DueDate = DateTime.Now });
        await Navigation.PushAsync(editPage);
    }
}