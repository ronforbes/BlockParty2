using System;
using System.Timers;

namespace BlockPartyServer
{
	public class Game
	{
		enum GameState
		{
			Game,
			Results,
			Leaderboard,
		}

		GameState state;

		TimeSpan gameElapsed;
		TimeSpan gameDuration = TimeSpan.FromSeconds(10);

		TimeSpan resultsElapsed;
		TimeSpan resultsDuration = TimeSpan.FromSeconds(10);

		TimeSpan leaderboardElapsed;
		TimeSpan leaderboardDuration = TimeSpan.FromSeconds(10);

		Timer updateTimer;
		const int updatesPerSecond = 1;

		GameTime gameTime = new GameTime();

		public Game()
		{
			updateTimer = new Timer(1000.0f / updatesPerSecond);
			updateTimer.Elapsed += Update;
			updateTimer.Start();

			SetGameState(GameState.Game);

			while(true) { }
		}

		void SetGameState(GameState state)
		{
			Console.WriteLine("Setting game state to " + state.ToString());
			this.state = state;

			switch(state)
			{
			case GameState.Game:
				gameElapsed = TimeSpan.Zero;
				break;

			case GameState.Results:
				resultsElapsed = TimeSpan.Zero;
				break;

			case GameState.Leaderboard:
				leaderboardElapsed = TimeSpan.Zero;
				break;
			}
		}

		void Update(object sender, ElapsedEventArgs e)
		{
			gameTime.Update();

			switch(state)
			{
			case GameState.Game:
				gameElapsed += gameTime.Elapsed;
				
				if(gameElapsed >= gameDuration)
				{
					SetGameState(GameState.Results);
				}
				break;

			case GameState.Results:
				resultsElapsed += gameTime.Elapsed;

				if(resultsElapsed >= resultsDuration)
				{
					SetGameState(GameState.Leaderboard);
				}
				break;

			case GameState.Leaderboard:
				leaderboardElapsed += gameTime.Elapsed;

				if(leaderboardElapsed >= leaderboardDuration)
				{
					SetGameState(GameState.Game);
				}
				break;
			}
		}
	}
}