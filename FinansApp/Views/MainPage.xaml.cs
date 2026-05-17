using FinansApp.Model;
using FinansApp.Services;

namespace FinansApp.Views;

public partial class MainPage : ContentPage
{
    private readonly FinanceDatabase _database;
    private readonly IServiceProvider _serviceProvider;

    private List<Transaction> _allTransactions = new();

    public MainPage(FinanceDatabase database, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _database = database;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        _allTransactions = await _database.GetTransactionsAsync();
        ApplyFilter(SearchBox.Text);

        double totalIncome = _allTransactions.Where(x => x.IsIncome).Sum(x => x.Amount);
        double totalExpense = _allTransactions.Where(x => !x.IsIncome).Sum(x => x.Amount);
        double balance = totalIncome - totalExpense;

        IncomeLabel.Text = totalIncome.ToString("C2");
        ExpenseLabel.Text = totalExpense.ToString("C2");
        BalanceLabel.Text = balance.ToString("C2");

        if (_allTransactions.Count == 0)
        {
            BalanceStatusLabel.Text = "Henüz işlem yok";
            BalanceLabel.TextColor = Colors.White;
        }
        else if (balance < 0)
        {
            BalanceStatusLabel.Text = "Dikkat! Harcamalar gelirden fazla";
            BalanceLabel.TextColor = Colors.White;
        }
        else if (balance == 0)
        {
            BalanceStatusLabel.Text = "Gelir ve gider eşit";
            BalanceLabel.TextColor = Colors.White;
        }
        else
        {
            BalanceStatusLabel.Text = "Durum iyi, bakiye pozitif";
            BalanceLabel.TextColor = Colors.White;
        }
    }

    private void ApplyFilter(string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            FinanceList.ItemsSource = _allTransactions;
            return;
        }

        var filtered = _allTransactions.Where(x =>
            (!string.IsNullOrWhiteSpace(x.Description) &&
             x.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            ||
            (!string.IsNullOrWhiteSpace(x.Category) &&
             x.Category.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        FinanceList.ItemsSource = filtered;
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var item = swipeItem?.CommandParameter as Transaction;

        if (item == null)
            return;

        bool confirm = await DisplayAlert(
            "Silme Onayı",
            $"'{item.Description}' işlemini silmek istiyor musun?",
            "Evet",
            "Hayır");

        if (confirm)
        {
            await _database.DeleteAsync(item);
            await LoadData();
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var page = _serviceProvider.GetService<TransactionEditPage>();
        if (page != null)
        {
            page.SetTransaction(new Transaction
            {
                Date = DateTime.Now,
                Category = "Diğer"
            });

            await Shell.Current.Navigation.PushAsync(page);
        }
    }

    private async Task OnEditTransaction(Transaction item)
    {
        var page = _serviceProvider.GetService<TransactionEditPage>();
        if (page != null)
        {
            page.SetTransaction(new Transaction
            {
                Id = item.Id,
                Description = item.Description,
                Amount = item.Amount,
                Date = item.Date,
                Category = item.Category,
                IsIncome = item.IsIncome
            });

            await Shell.Current.Navigation.PushAsync(page);
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var current = e.CurrentSelection.FirstOrDefault() as Transaction;

        if (current != null)
        {
            await OnEditTransaction(current);
        }

        ((CollectionView)sender).SelectedItem = null;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }
}