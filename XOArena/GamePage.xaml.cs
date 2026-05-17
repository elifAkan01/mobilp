using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace XOArena
{
    public partial class GamePage : ContentPage
    {
        private string currentPlayer = "X";
        private bool gameOver = false;
        private bool isVsComputer;
        private string playerXName, playerOName;
        private int xScore = 0, oScore = 0, drawScore = 0;
        private Button[] buttons;

        public GamePage(bool isVsComputer, string pX, string pO)
        {
            InitializeComponent();
            this.isVsComputer = isVsComputer;
            this.playerXName = pX;
            this.playerOName = pO;
            buttons = new Button[] { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8 };
            UpdateUI();
        }

        private async void Cell_Clicked(object sender, EventArgs e)
        {
            if (gameOver) return;
            var btn = (Button)sender;
            if (!string.IsNullOrEmpty(btn.Text)) return;

            btn.Text = currentPlayer;
            btn.TextColor = currentPlayer == "X" ? Color.FromArgb("#2563EB") : Color.FromArgb("#F43F5E");
            btn.BorderColor = btn.TextColor;

            if (await CheckStatus()) return;

            // SIRA DEÐÝÞTÝRME
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
            UpdateUI();

            if (isVsComputer && currentPlayer == "O")
            {
                await Task.Delay(600);
                MakeBestMove();
            }
        }

        private void MakeBestMove()
        {
            if (gameOver) return;
            // Akýllý Zeka Mantýðý
            int move = GetSmartIndex("O");
            if (move == -1) move = GetSmartIndex("X");
            if (move == -1 && string.IsNullOrEmpty(buttons[4].Text)) move = 4;
            if (move == -1)
            {
                var empty = buttons.Select((b, i) => new { b, i }).Where(x => string.IsNullOrEmpty(x.b.Text)).ToList();
                if (empty.Any()) move = empty[new Random().Next(empty.Count)].i;
            }

            if (move != -1)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    buttons[move].Text = "O";
                    buttons[move].TextColor = Color.FromArgb("#F43F5E");
                    buttons[move].BorderColor = buttons[move].TextColor;

                    if (await CheckStatus()) return;
                    currentPlayer = "X";
                    UpdateUI();
                });
            }
        }

        private int GetSmartIndex(string p)
        {
            int[][] wins = { new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 }, new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 }, new[] { 0, 4, 8 }, new[] { 2, 4, 6 } };
            foreach (var w in wins)
            {
                int count = 0; int empty = -1;
                for (int i = 0; i < 3; i++)
                {
                    if (buttons[w[i]].Text == p) count++;
                    else if (string.IsNullOrEmpty(buttons[w[i]].Text)) empty = w[i];
                }
                if (count == 2 && empty != -1) return empty;
            }
            return -1;
        }

        private async Task<bool> CheckStatus()
        {
            int[][] wins = { new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 }, new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 }, new[] { 0, 4, 8 }, new[] { 2, 4, 6 } };
            bool hasWin = wins.Any(w => !string.IsNullOrEmpty(buttons[w[0]].Text) && buttons[w[0]].Text == buttons[w[1]].Text && buttons[w[1]].Text == buttons[w[2]].Text);

            if (hasWin)
            {
                gameOver = true;
                if (currentPlayer == "X") xScore++; else oScore++;
                UpdateUI();
                await DisplayAlert("Oyun Bitti", $"{(currentPlayer == "X" ? playerXName : playerOName)} Kazandý!", "Harika");
                return true;
            }
            if (buttons.All(b => !string.IsNullOrEmpty(b.Text)))
            {
                gameOver = true; drawScore++; UpdateUI();
                await DisplayAlert(  "Tamam","Berabere","Kimse kazanamadý!");
                return true;
            }
            return false;
        }

        private void UpdateUI()
        {
            string name = (currentPlayer == "X") ? playerXName : playerOName;
            turnLabel.Text = $"SIRA: {name.ToUpper()}";
            xScoreLabel.Text = xScore.ToString();
            oScoreLabel.Text = oScore.ToString();
            drawScoreLabel.Text = drawScore.ToString();
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            foreach (var b in buttons) { b.Text = ""; b.BorderColor = Color.FromArgb("#CBD5E1"); }
            currentPlayer = "X"; gameOver = false; UpdateUI();
        }
    }
}