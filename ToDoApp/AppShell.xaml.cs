using ToDoApp.Models;

namespace ToDoApp
{
    public partial class AppShell : Shell
    {
        public Command AddNewTaskCommand { get; }
        public Command GoToCompletedCommand { get; }
        public Command GoToOverdueCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(TaskEditPage), typeof(TaskEditPage));

            AddNewTaskCommand = new Command(async () => await OnAddNewTask());
            GoToCompletedCommand = new Command(async () => await OnGoToCompleted());
            GoToOverdueCommand = new Command(async () => await OnGoToOverdue());

            BindingContext = this;
        }

        private async Task OnAddNewTask()
        {
            var services = Handler?.MauiContext?.Services;
            var editPage = services?.GetService<TaskEditPage>();

            if (editPage != null)
            {
                editPage.SetItem(new ToDo { DueDate = DateTime.Now });
                await Navigation.PushAsync(editPage);
                FlyoutIsPresented = false;
            }
        }

        private async Task OnGoToCompleted()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}?filter=completed");
            FlyoutIsPresented = false;
        }

        private async Task OnGoToOverdue()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}?filter=overdue");
            FlyoutIsPresented = false;
        }
    }
}