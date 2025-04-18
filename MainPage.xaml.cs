using System.Collections.ObjectModel;

namespace MAUIGuessNumber
{
	public partial class MainPage : ContentPage
	{
		Random rnd = new Random();
		int TargetNumber;
		ObservableCollection<Result> GuessLog;
		public MainPage()
		{
			InitializeComponent();

			TargetNumber = GenerateRandomNumber();
			GuessLog = new ObservableCollection<Result>();
			lvResults.ItemsSource = GuessLog;
		}

		private int GenerateRandomNumber()
		{
			int n;
			do
			{
				n = rnd.Next(1000, 10000);
			} while (n.ToString().Distinct().Count() != 4);
			return n;
		}

		async private void btOK_Clicked(object sender, EventArgs e)
		{
			string UserGuess = enNumber.Text;
			if (UserGuess.Distinct().Count() != 4 || !UserGuess.All(char.IsDigit))
			{
				llResult.Text = "Inappropriate value!";
				return;
			}
			int CorrectDigits = 0;
			int CorrectPositions = 0;
			string TN = TargetNumber.ToString();
			for (int i = 0; i < 4; i++)
			{
				if (TN.Contains(UserGuess[i]))
					CorrectDigits++;
				if (UserGuess[i] == TN[i])
					CorrectPositions++;
			}
			llResult.Text = $"Of the number you entered, {CorrectDigits} digits were in the number I meant, and of that {CorrectPositions} digits were in place"; 
			var nr = new Result
			{
				Guess = UserGuess,
				CorrectDigits = CorrectDigits,
				CorrectPositions = CorrectPositions
			};
			GuessLog.Insert(0, nr);
			if (CorrectPositions == 4)
				await DisplayAlert("Success!", "You guessed it!","OK");
		}

		private void btNewGame_Clicked(object sender, EventArgs e)
		{
			GuessLog.Clear();
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
