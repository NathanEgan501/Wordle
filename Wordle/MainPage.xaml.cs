// g00435730 Nathan Egan
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Wordle
{
    public partial class MainPage : ContentPage
    {
        private string secretWord;
        private int attempts;
        private List<string> wordList = new List<string>();
        private int currentAttempt;

        public MainPage()
        {
            InitializeComponent();
            LoadWordList();
        }

        private async void LoadWordList()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string wordData = await client.GetStringAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
                wordList = new List<string>(wordData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                StartNewGame();
            }
            catch (Exception ex)
            {
                MessageLabel.Text = "Error loading word list: " + ex.Message;
            }
        }

        private void StartNewGame()
        {
            
        }

    }

}
