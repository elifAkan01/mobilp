using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp
{
    public partial class TaskEditPage : ContentPage
    {
        private readonly Database _database;
        public ToDo? Item { get; set; }

        public TaskEditPage(Database database)
        {
            InitializeComponent();
            _database = database;
        }

        public void SetItem(ToDo item)
        {
            Item = item;

            TitleEntry.Text = item.Title;
            CategoryPicker.SelectedItem = item.Category;
            DueDatePicker.Date = item.DueDate == default ? DateTime.Now : item.DueDate;
            CompletedCheckBox.IsChecked = item.IsCompleted;

            // Sadece kayýtlý görevlerde Sil butonu görünsün
            DeleteButton.IsVisible = item.Id != 0;
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            if (Item is null)
                return;

            Item.Title = TitleEntry.Text;
            Item.Category = CategoryPicker.SelectedItem?.ToString();
            Item.DueDate = DueDatePicker.Date;
            Item.IsCompleted = CompletedCheckBox.IsChecked;

            await _database.SaveItemAsync(Item);
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (Item is null || Item.Id == 0)
                return;

            bool confirm = await DisplayAlert(
                "Sil",
                $"'{Item.Title}' görevini silmek istediðine emin misin?",
                "Evet",
                "Hayýr");

            if (!confirm)
                return;

            await _database.DeleteItemAsync(Item);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}