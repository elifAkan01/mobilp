using FinansApp.Model;
using FinansApp.Views;

namespace FinansApp;

public partial class AppShell : Shell
{
    public Command AddIncomeCommand { get; }
    public Command AddExpenseCommand { get; }

    public AppShell()
    {
        InitializeComponent();

        AddIncomeCommand = new Command(async () => await NavigateToEdit(true));
        AddExpenseCommand = new Command(async () => await NavigateToEdit(false));

        BindingContext = this;
    }

    private async Task NavigateToEdit(bool isIncome)
    {
        var services = Handler?.MauiContext?.Services;
        var editPage = services?.GetService<TransactionEditPage>();

        if (editPage != null)
        {
            editPage.SetTransaction(new Transaction
            {
                IsIncome = isIncome,
                Date = DateTime.Now,
                Category = "Diğer"
            });

            await Shell.Current.Navigation.PushAsync(editPage);
            FlyoutIsPresented = false;
        }
    }
}