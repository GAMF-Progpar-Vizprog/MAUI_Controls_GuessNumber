using System.Collections.ObjectModel;

namespace MAUI_Controls_GuessNumber
{
	public partial class MainPage : ContentPage
	{
		int TargetNumber;
		readonly ObservableCollection<Result> GuessLog = new();

		public MainPage()
		{
			InitializeComponent();

			TargetNumber = GenerateRandomNumber();
			cvResults.ItemsSource = GuessLog;
		}

		/// <summary>
		/// Négyjegyű szám, amelynek minden számjegye különböző.
		/// </summary>
		private static int GenerateRandomNumber()
		{
			int n;
			do
			{
				n = Random.Shared.Next(1000, 10000);
			} while (n.ToString().Distinct().Count() != 4);
			return n;
		}

		private async void btOK_Clicked(object? sender, EventArgs e)
		{
			string userGuess = enNumber.Text?.Trim() ?? string.Empty;
			if (userGuess.Length != 4 || !userGuess.All(char.IsDigit) ||
				userGuess.Distinct().Count() != 4)
			{
				llResult.Text = "Inappropriate value! Enter four different digits.";
				return;
			}
			int correctDigits = 0;
			int correctPositions = 0;
			string tn = TargetNumber.ToString();
			for (int i = 0; i < 4; i++)
			{
				if (tn.Contains(userGuess[i]))
					correctDigits++;
				if (userGuess[i] == tn[i])
					correctPositions++;
			}
			llResult.Text = $"Of the number you entered, {correctDigits} digits were in the number I meant, and of that {correctPositions} digits were in place";
			GuessLog.Insert(0, new Result
			{
				Guess = userGuess,
				CorrectDigits = correctDigits,
				CorrectPositions = correctPositions
			});
			if (correctPositions == 4)
				await DisplayAlertAsync("Success!", "You guessed it!", "OK");
		}

		private void btNewGame_Clicked(object? sender, EventArgs e)
		{
			GuessLog.Clear();
			enNumber.Text = string.Empty;
			llResult.Text = string.Empty;
			TargetNumber = GenerateRandomNumber();
		}
	}

	public class Result
	{
		public string? Guess { get; set; }
		public int CorrectDigits { get; set; }
		public int CorrectPositions { get; set; }
	}
}
