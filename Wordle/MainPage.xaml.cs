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
            currentAttempt = 0;
            attempts = 6;
            secretWord = GetRandomWord();
            MessageLabel.Text = "Guess the word!";
            ClearGuessGrid();
        }

        private string GetRandomWord()
        {
            Random random = new Random();
            int index = random.Next(wordList.Count);
            return wordList[index].ToLower();
        }

        private void ClearGuessGrid() 
        {
            for(int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var label = (Label)GuessGrid.Children[i * 5 j];
                    label.Text = string.Empty;
                }
            }
        }

        

    }

}
