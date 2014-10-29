using System;
using System.Timers;
using BlockPartyShared;

namespace BlockPartyServer
{
	public class Game
	{
		enum GameState
		{
			Lobby,
			Game,
			Results,
		}

		GameState state;

		TimeSpan lobbyElapsed;
		TimeSpan lobbyDuration = TimeSpan.FromSeconds(10);

		TimeSpan gameElapsed;
		TimeSpan gameDuration = TimeSpan.FromSeconds(10);

		TimeSpan resultsElapsed;
		TimeSpan resultsDuration = TimeSpan.FromSeconds(10);

		Timer updateTimer;
		const int updatesPerSecond = 1;

		GameTime gameTime = new GameTime();

		NetworkingManager networkingManager = new NetworkingManager();

		public Game()
		{
			networkingManager.MessageReceived += networkingManager_MessageReceived;

			updateTimer = new Timer(1000.0f / updatesPerSecond);
			updateTimer.Elapsed += Update;
			updateTimer.Start();

			SetGameState(GameState.Lobby);

			while(true) { }
		}

		void networkingManager_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			switch(e.Message.Type)
			{
			case NetworkMessage.MessageType.ClientResults:
				//GameResults.Add(e.Sender, (int)e.Message.Content);
				break;
			}
		}

		void SetGameState(GameState state)
		{
			Console.WriteLine("Setting game state to " + state.ToString());
			this.state = state;

			NetworkMessage message = new NetworkMessage(NetworkMessage.MessageType.ServerGameState, state.ToString());
			networkingManager.Broadcast(message);

			switch(state)
			{
			case GameState.Game:
				gameElapsed = TimeSpan.Zero;
				break;

			case GameState.Results:
				resultsElapsed = TimeSpan.Zero;
				break;

			case GameState.Lobby:
				lobbyElapsed = TimeSpan.Zero;
				break;
			}
		}

		void Update(object sender, ElapsedEventArgs e)
		{
			gameTime.Update();

			switch(state)
			{
			case GameState.Lobby:
				lobbyElapsed += gameTime.Elapsed;
				
				if(lobbyElapsed >= lobbyDuration)
				{
					SetGameState(GameState.Game);
				}
				break;

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
					SetGameState(GameState.Lobby);
				}
				break;
			}
		}
	}
}