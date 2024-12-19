﻿// g00435730 Nathan Egan
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
                
                if (wordList.Count == 0)
                {
                    MessageLabel.Text = "The word list is empty. Please try again later.";
                    return;
                }

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
                    var label = (Label)GuessGrid.Children[i * 5 + j];
                    label.Text = string.Empty;
                    label.TextColor = Colors.Black;
                }
            }
        }

        private void OnSubmitGuessClicked(object sender, EventArgs e)
        {
            string guess = GuessEntry.Text?.ToLower();

            if (string.IsNullOrWhiteSpace(guess) || guess.Length != 5)
            {
                MessageLabel.Text = "Please enter a valid 5-letter word.";
                return;
            }

            if (currentAttempt < attempts)
            {
                MessageLabel.Text = "No more attempts left! The word was: " + secretWord;
                return;
            }

            DisplayGuess(guess);
            GuessEntry.Text = string.Empty;
            currentAttempt++;
        }

        private void DisplayGuess(string guess)
        {
            for (int i = 0; i < 5; i++)
            {
                var label = (Label)GuessGrid.Children[currentAttempt * 5 + i];
                label.Text = guess[i].ToString();

                if (guess[i] == secretWord[i])
                {
                    label.TextColor = Colors.Green;
                }
                else if (secretWord.Contains(guess[i]))
                {
                    label.TextColor= Colors.Yellow;
                }
                else
                {
                    label.TextColor= Colors.Red;
                }

            }

            if (guess == secretWord)
            {
                MessageLabel.Text = "Congratulations! You've guessed the word!";
            }
            else if (currentAttempt > attempts - 1)
            {
                MessageLabel.Text = "Game Over! The word was: " + secretWord;
            }
        }

    }

}
