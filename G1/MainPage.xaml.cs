namespace Game
{
    using System.ComponentModel;
    using System.Windows.Input;

    namespace GuessTheWordGame
    {
        public partial class MainPage : ContentPage, INotifyPropertyChanged
        {
            private string _currentWord;
            private string _currentGuess;
            private string _currentWordDisplay;
            private int _attemptsLeft;
            private string _message;

            public event PropertyChangedEventHandler PropertyChanged;

            public string CurrentWordDisplay
            {
                get => _currentWordDisplay;
                set
                {
                    _currentWordDisplay = value;
                    OnPropertyChanged(nameof(CurrentWordDisplay));
                }
            }

            public string CurrentGuess
            {
                get => _currentGuess;
                set
                {
                    _currentGuess = value;
                    OnPropertyChanged(nameof(CurrentGuess));
                }
            }

            public int AttemptsLeft
            {
                get => _attemptsLeft;
                set
                {
                    _attemptsLeft = value;
                    OnPropertyChanged(nameof(AttemptsLeft));
                }
            }

            public string Message
            {
                get => _message;
                set
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }

            public ICommand GuessCommand { get; }

            private readonly string[] _words = { "Star fruit", "banana", "cherry", "date", "elderberry" };

            public MainPage()
            {
                InitializeComponent();
                GuessCommand = new Command(OnGuess);
                StartNewGame();
                BindingContext = this;
            }

            private void StartNewGame()
            {
                var random = new Random();
                _currentWord = _words[random.Next(_words.Length)];
                CurrentWordDisplay = new string('_', _currentWord.Length);
                AttemptsLeft = 6; // Set the number of attempts
                Message = string.Empty;
            }

            private void OnGuess()
            {
                if (string.IsNullOrWhiteSpace(CurrentGuess) || CurrentGuess.Length != 1)
                {
                    Message = "Please enter a single letter.";
                    return;
                }

                char guessedLetter = CurrentGuess.ToLower()[0];
                CurrentGuess = string.Empty;

                if (_currentWord.Contains(guessedLetter))
                {
                    // Update the current word display
                    CurrentWordDisplay = new string(_currentWord.Select(c => _currentWordDisplay.Contains(c) ? c : '_').ToArray());
                    Message = "Good guess!";
                }
                else
                {
                    AttemptsLeft--;
                    Message = "Wrong guess!";
                }

                if (CurrentWordDisplay == _currentWord)
                {
                    Message = "Congratulations! You've guessed the word!";
                }
                else if (AttemptsLeft <= 0)
                {
                    Message = $"Game over! The word was '{_currentWord}'.";
                }
            }

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
