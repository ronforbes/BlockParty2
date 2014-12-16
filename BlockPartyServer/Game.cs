using System;
using System.Timers;
using BlockPartyShared;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
		UserManager userManager = new UserManager();

		Dictionary<string, int> gameResults = new Dictionary<string, int>();

		public Game()
		{
			networkingManager.MessageReceived += networkingManager_MessageReceived;

			updateTimer = new Timer(1000.0f / updatesPerSecond);
			updateTimer.Elapsed += Update;
			updateTimer.Start();

			SetGameState(GameState.Lobby);
		}

		void networkingManager_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			switch(e.Message.Type)
			{
			case NetworkMessage.MessageType.ClientFacebookId:
				userManager.Users.Add((string)e.Message.Content, new User((string)e.Message.Content, e.Sender));
				break;
			case NetworkMessage.MessageType.ClientResults:
				gameResults.Add(userManager.GetUserByTcpClient(e.Sender).Name, (int)e.Message.Content);
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
				gameResults.Clear();
				break;

			case GameState.Results:
				resultsElapsed = TimeSpan.Zero;
				break;

			case GameState.Lobby:
				lobbyElapsed = TimeSpan.Zero;

				Console.WriteLine("gameResults.Count=" + gameResults.Count);
				List<KeyValuePair<string, int>> sortedGameResults = gameResults.ToList();
				sortedGameResults.Sort((firstPair, nextPair) => { return firstPair.Value.CompareTo(nextPair) * -1; });
				Console.WriteLine("sortedGameResults.Count=" + sortedGameResults.Count);
				if(sortedGameResults.Count > 0)
				{
					Console.WriteLine("Game winner is " + sortedGameResults[0].Key + " with score " + sortedGameResults[0].Value);
					
					networkingManager.Broadcast(new NetworkMessage(NetworkMessage.MessageType.ServerLeaderboard, sortedGameResults));
				}

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

		public void Run()
		{
			while(true) { }
		}
	}
}