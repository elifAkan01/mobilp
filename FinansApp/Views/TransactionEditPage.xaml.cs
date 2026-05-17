using FinansApp.Model;
using FinansApp.Services;

namespace FinansApp.Views;

public partial class TransactionEditPage : ContentPage
{
    private readonly FinanceDatabase _database;

    public Transaction Item { get; set; } = new Transaction();

    public TransactionEditPage(FinanceDatabase database)
    {
        InitializeComponent();
        _database = database;
    }

    public void SetTransaction(Transaction transaction)
    {
        Item = transaction;

        DescEntry.Text = Item.Description;
        AmountEntry.Text = Item.Amount > 0 ? Item.Amount.ToString() : string.Empty;
        IncomeSwitch.IsToggled = Item.IsIncome;
        TransDate.Date = Item.Date == default ? DateTime.Now : Item.Date;

        var category = string.IsNullOrWhiteSpace(Item.Category) ? "Diðer" : Item.Category;

        if (CategoryPicker.Items.Contains(category))
            CategoryPicker.SelectedItem = category;
        else
            CategoryPicker.SelectedItem = "Diðer";
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DescEntry.Text))
        {
            await DisplayAlert("Hata", "Lütfen açýklama gir.", "Tamam");
            return;
        }

        if (!double.TryParse(AmountEntry.Text, out double amount) || amount <= 0)
        {
            await DisplayAlert("Hata", "Lütfen 0'dan büyük geçerli bir tutar gir.", "Tamam");
            return;
        }

        if (CategoryPicker.SelectedItem == null)
        {
            await DisplayAlert("Hata", "Lütfen kategori seç.", "Tamam");
            return;
        }

        Item.Description = DescEntry.Text.Trim();
        Item.Amount = amount;
        Item.IsIncome = IncomeSwitch.IsToggled;
        Item.Date = TransDate.Date;
        Item.Category = CategoryPicker.SelectedItem.ToString();

        await _database.SaveAsync(Item);
        await Shell.Current.Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}