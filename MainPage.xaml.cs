using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Plugin.Maui.KeyListener;

namespace WordleApp;


public partial class MainPage : ContentPage
{
	string currentGuess="";
	string currentWord="";
	int totalGuesses = 6;
	int guessNumber=0;
	int charGuessNumber=0;
	bool gameComplete = false;

	public MainPage()
	{
		InitializeComponent();
		ListenForGuess();
	}

	private async void GenerateWord()
	{
		currentWord = await WordGenerator.Generate();
		// this.WordOfTheDay.Text=currentWord.ToUpper();
	}

	private void CheckWordGenerated()
	{
		if(currentWord=="")
		{
			GenerateWord();
		}
	}

	private void SelectLetter(object sender, EventArgs e)
	{
		Button letter = (Button)sender;
		char selectedLetter;
		if(char.TryParse(letter.Text, out selectedLetter))
		{
			SelectLetter(selectedLetter);
		}
		else 
		{
			DisplayAlert("Error", "That didn't work");
		}
	}	

	private void SelectLetter(char letter)
	{
		if(!gameComplete)
		{
			if (charGuessNumber<1)
			{
				CheckWordGenerated();
			}

				// update the display text for the word on the current guess line
				if(charGuessNumber<5){
					string labelName="letter"+guessNumber+charGuessNumber;
					Label label = (Label)FindByName(labelName);
					label.Text=letter.ToString();
					currentGuess+=letter.ToString();
					charGuessNumber++;

				} 
				
				checkButtons();
		}
	}

	private void Backspace(object sender, EventArgs e)
	{
		Backspace();
	}

	private void Backspace()
	{
		if(charGuessNumber>=1)
		{
			string labelName="letter"+guessNumber+(charGuessNumber-1);
			Label label = (Label)FindByName(labelName);
			label.Text="";
			currentGuess = currentGuess.Substring(0,currentGuess.Length-1);
			charGuessNumber--;
		}
		checkButtons();
	}

	private void Submit(object sender, EventArgs e)
	{
		Submit();
	}

	private async void Submit()
	{
		if(await GuessIsWord())
		{
			// allow user to submit the guess. Check if the word is correct
			bool correctAnswer = CheckGuess();
			if(correctAnswer)
			{
				DisplayAlert("Correct!!", "Congratulations");
				gameComplete = true;
				checkButtons(true);
			} 
			else 
			{
				// animation to wobble current guess line
				Grid grid = (Grid)FindByName("GuessLine"+guessNumber);
				await grid.TranslateTo(-10, 0 , 50);
				await grid.TranslateTo(10, 0 , 50);
				await grid.TranslateTo(-10, 0 , 50);
				await grid.TranslateTo(0, 0 , 50);

				guessNumber++;
				charGuessNumber = 0;
				if(guessNumber<totalGuesses)
				{
					// reset counters and move to next guess
					checkButtons();
					currentGuess = "";
				} 
				else
				{
					DisplayAlert("Incorrect", "The word was " + currentWord + ". Better luck next time");
					gameComplete = true;
					checkButtons(true);
				}
			}
		}
		else
		{
			DisplayAlert(currentGuess + " is not a valid word", "Please enter a valid word.");
		}
	}

	private void checkButtons(bool end=false)
	{
		Button backspaceButton = (Button)FindByName("backspaceButton");
		backspaceButton.IsEnabled = !end && charGuessNumber >= 1;

		Button submitButton = (Button)FindByName("submitButton");
		submitButton.IsEnabled = !end && charGuessNumber == 5;
	}

	private async Task<bool> GuessIsWord()
	{
		bool isWord = await WordGenerator.checkWord(currentGuess);
		return isWord;
	}

	private bool CheckGuess()
	{
		// allow user to submit the guess. Check if the word is correct
		bool correctGuess = true;
		// array to keep track of characters that are in correct position
		List<char> greenChars = new List<char> {};

		// strt for loop at last character so we can ensure the orange colours appear on left most letter
		for(int i = currentWord.Length - 1; i >=0; i--)
		{
			// by default change the label to dark grey. Below if statements updates it if letter is correct
			string labelName="letter"+guessNumber+i;
			Label label = (Label)FindByName(labelName);
			label.BackgroundColor = Colors.DarkGray;

			string buttonName = "letter"+currentGuess[i];
			Button button = (Button)FindByName(buttonName);
			button.BackgroundColor = Colors.DarkGray;

			if(currentGuess[i]==currentWord[i])
			{
				// change colour of character label that is correct to green and the letters the user can select
				label.BackgroundColor = Colors.Green;
				button.BackgroundColor = Colors.Green;
				greenChars.Add(currentGuess[i]);

			} 
			else
			{
				correctGuess=false;
				// first check if letter is in the answer. if not dont apply a colour change
				if(currentWord.Contains(currentGuess[i]))
				{
					var charWordCount = currentWord.ToCharArray().Count(x => x==currentGuess[i]);
					var charGuessCount = currentGuess.ToCharArray().Count(x => x==currentGuess[i]);
					// take one away to ignore the ith element so we get true pre i check
					int charGuessCountPreI = currentGuess.Substring(i, currentWord.Length - i).Count(x => x==currentGuess[i]) - 1;
					// below includes the current check
					int charGuessesToCheck = charGuessCount-charGuessCountPreI;
					
					// as we work from right to left for the guesses characters the below will ignore making letters orange if we have more of the char in the guess than in the word
					if(charWordCount>=charGuessesToCheck)
					{
						// now need to account for any previous green chars to the right of current one being checked
						// if there is we need to adujst word count down and then make current char orange if the guesses to check or less than or equal to adjust word count
						int greenOccurences = countOccurences(currentGuess[i], greenChars);
						int charWordCountLessGreen = charWordCount - greenOccurences;
						if(charWordCountLessGreen>=charGuessesToCheck)
						{
							label.BackgroundColor = Colors.Orange;
							button.BackgroundColor = Colors.Orange;
						}

					} 
				} 
			}
		}
		return correctGuess;
	}

	private int countOccurences(char letter, List<char> greenChars)
	{
		int count = 0;
		foreach ( char x in greenChars)
		{
			if(letter==x)
				count++;
		}
		return count;
	}

	private async void DisplayAlert(string type, string message)
	{
		await DisplayAlert(type, message, "OK");
	}

	private void ResetGame(object sender, EventArgs e)
	{
		// reset all global variables
		currentGuess="";
		currentWord="";
		guessNumber=0;
		charGuessNumber=0;
		gameComplete = false;

		for(int i = 0; i < totalGuesses; i++)
		{
			for(int j = 0; j < 5; j++)
			{
				Label label = (Label)FindByName("letter"+i+j);
				label.Text="";
				label.BackgroundColor = Colors.Transparent;
			}	
		}

		for(int i = 0; i < 3; i++)
		{
			Grid letterGrid = (Grid)FindByName("letterGrid"+i);
			foreach(Button button in letterGrid.Children.Cast<Button>())
			{
				string hexCode = "#512BD4";
				string text = button.Text;
				if(text=="del" || text=="⮐")
					button.IsEnabled=false;
				else 
					button.BackgroundColor=Color.FromArgb(hexCode);
			}
		}
	}

    void ListenForGuess()
    {
        var kb = new KeyboardBehavior();
		
        this.Behaviors.Add(kb);
        kb.KeyDown += (s, e) =>
        {
            char letter;
            if (char.TryParse(e.Keys.ToString().ToUpper(), out letter))
                SelectLetter(letter);
			else if(e.Keys.ToString() == "Backspace")
				Backspace();
            else if(e.Keys.ToString()=="Enter")
				if (charGuessNumber==5)
					Submit();
					
        };
    }
}

