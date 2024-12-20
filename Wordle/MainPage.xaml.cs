using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.IO;
using System.Diagnostics;

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
            string fileName = "words.txt"; 
            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (File.Exists(localFilePath))
            {
                wordList = new List<string>(await File.ReadAllLinesAsync(localFilePath));
                Console.WriteLine("Loaded word list from local file.");
            }
            else
            {
                try
                {
                    using HttpClient client = new HttpClient();
                    string wordData = await client.GetStringAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");

                    Console.WriteLine("Raw word data: " + wordData);

                    if (string.IsNullOrWhiteSpace(wordData))
                    {
                        Console.WriteLine("Warning: No data received from the word list URL.");
                        return;
                    }

                    await File.WriteAllTextAsync(localFilePath, wordData);
                    wordList = new List<string>(wordData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                    Console.WriteLine("Downloaded and saved word list to local file.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading word list: {ex.Message}");
                    return;
                }
            }

            if (wordList.Count == 0)
            {
                Console.WriteLine("Error: Word list is empty. Cannot start the game.");
                return; 
            }

            StartNewGame();
        }

        private void StartNewGame()
        {
            currentAttempt = 0;
            attempts = 6;
            secretWord = GetRandomWord();
            MessageLabel.Text = "Guess the word!";
            ClearGuessGrid();
            GuessEntry.Text = string.Empty;
            Console.WriteLine("The secret word is: " + secretWord); 
        }

        private string GetRandomWord()
        {
            Random random = new Random();
            int index = random.Next(wordList.Count);
            string selectedWord = wordList[index].ToLower();
            Console.WriteLine("Secret word selected: " + selectedWord);
            return selectedWord;
        }

        private void ClearGuessGrid()
        {
            int totalChildren = GuessGrid.Children.Count;
            Console.WriteLine($"Total Children in GuessGrid: {totalChildren}");

            if (totalChildren != 30)
            {
                Console.WriteLine("Warning: Unexpected number of children in GuessGrid. Expected 30.");
                return; 
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var index = i * 5 + j; 

                    if (index < totalChildren)
                    {
                        var label = (Label)GuessGrid.Children[index];
                        label.Text = string.Empty; 
                        label.TextColor = Colors.Black; 
                    }
                    else
                    {
                        Console.WriteLine($"Index out of bounds: {index}");
                    }
                }
            }
        }

        private void OnSubmitGuessClicked(object sender, EventArgs e)
        {
            string guess = GuessEntry.Text?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(guess) || guess.Length != 5)
            {
                MessageLabel.Text = "Please enter a valid 5-letter word.";
                return;
            }

            if (currentAttempt >= attempts)
            {
                MessageLabel.Text = "No more attempts left! The word was: " + secretWord;
                return;
            }

            DisplayGuess(guess);
            GuessEntry.Text = string.Empty;
            currentAttempt++;

            if (guess == secretWord)
            {
                MessageLabel.Text = $"Well done! You've guessed the word in {currentAttempt} attempts!";
            }
            else if (currentAttempt >= attempts)
            {
                MessageLabel.Text = "Bad luck! The word was: " + secretWord;
            }
        }

        private void DisplayGuess(string guess)
        {
            Console.WriteLine($"Current Attempt: {currentAttempt}, Total Children: {GuessGrid.Children.Count}");

            if (currentAttempt < attempts)
            {
                for (int i = 0; i < 5; i++)
                {
                    int index = currentAttempt * 5 + i;

                    if (index < GuessGrid.Children.Count)
                    {
                        var label = (Label)GuessGrid.Children[index];
                        label.Text = guess[i].ToString();

                        if (guess[i] == secretWord[i])
                        {
                            label.TextColor = Colors.Green;
                        }
                        else if (secretWord.Contains(guess[i]))
                        {
                            label.TextColor = Colors.Orange;
                        }
                        else
                        {
                            label.TextColor = Colors.Red;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Index out of bounds: {index}");
                    }
                }
            }
        }

        private void OnNewGameClicked(object sender, EventArgs e)
        {
            StartNewGame(); 
        }
   
    
    
    
    
    
    
    
    
    
    
    }
}