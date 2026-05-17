using System;
using Microsoft.Maui.Controls;

namespace XOArena
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void modePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modePicker.SelectedIndex == 1) // Bilgisayar
            {
                playerOEntry.Text = "Bilgisayar";
                playerOEntry.IsEnabled = false;
                playerOFrame.Opacity = 0.6;
                infoLabel.Text = "Yapay zekaya karşı!";
            }
            else
            {
                playerOEntry.Text = "";
                playerOEntry.IsEnabled = true;
                playerOFrame.Opacity = 1;
                infoLabel.Text = "Arkadaşınla kapış!";
            }
        }

        private async void StartGame_Clicked(object sender, EventArgs e)
        {
            string pX = string.IsNullOrWhiteSpace(playerXEntry.Text) ? "X" : playerXEntry.Text;
            string pO = string.IsNullOrWhiteSpace(playerOEntry.Text) ? "O" : playerOEntry.Text;
            bool vsAi = modePicker.SelectedIndex == 1;

            await Navigation.PushAsync(new GamePage(vsAi, pX, pO));
        }
    }
}