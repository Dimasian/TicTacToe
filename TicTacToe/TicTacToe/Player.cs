using System;
using System.Timers;
using Epam_Team4;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace TicTacToe
{
	//public delegate void PlayerMadeMove(byte[] lastMoveByte);
	public delegate void PlayerMadeMove(Cell lastMoveByte);

	public delegate void TimesUp(Player player);
	public delegate void UpdateLabelTimeLimit(Player player);
	public delegate void PlayerUpdateBFLimits(Cell TLcell, Cell BRcell);

	public class Player
	{
		public event PlayerMadeMove playerMadeMoveEvent;
		public event TimesUp timesUpEvent;
		public event UpdateLabelTimeLimit updateTimeLimitEvent;
		public event PlayerUpdateBFLimits updateBFLimits;


		public String name { get; set; }
		public PlayerType MyPlayerType { get; set; }
		public PlayerNumber MyNumber { get; set; }
		public State MyFigure { get; set; }// player goes with "X" or "O".
		public bool IsActive { get; set; }
		AIPlayer Team4AI;
		public System.Timers.Timer timer;
		public TimeSpan timeLimit { get; set; }


		// Ctor.
		public Player()
		{		}
		// ctor for Human player
		public Player(HumanPlayer humanPlayer, TimeSpan timeLimit)
		{
			MyPlayerType = PlayerType.human;
			MyFigure = humanPlayer.MyFigure;
			this.timeLimit = timeLimit;
			name = humanPlayer.Name;
			SetTimer();
		}

		// Ctor to bring together Team4.dll class AIPlayer and TicTacToe class Player
		public Player(Epam_Team4.AIPlayer AIPlayer, TimeSpan timeLimit)
		{
			Team4AI = AIPlayer;
			MyPlayerType = PlayerType.computer;
			MyFigure = AIPlayer.MyFigure;
			this.timeLimit = timeLimit;
			name = AIPlayer.Name;
			SetTimer();
		}

		private void SetTimer()
		{
			timer = new System.Timers.Timer
			{
				Interval = 100
			};
			timer.Elapsed += timer_Tick;
		}


		// Async/Await - (separate UI thread from AIPlayer thread)
		// raise event handler in GameManager after player has make his move.
		// Этот метод вызывается только при ходе AIPlayer
		

		// Make move on the current board.
		public async void MakeMove(Board board)
		{
			timer.Start();// start timer for this player

			Cell move;
			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;
			// Запускаем вычисление следующего хода в отдельном задании асинхронно от потока UI,
			// в котором находится данный класс.
			Task<Cell> makeMoveTask = Task.Run<Cell>(() => Team4AI.MakeMove(board));
			await makeMoveTask;
			move = makeMoveTask.Result;

			timer.Stop();// stop AIplayer's timer

			// Raise event if it has any subscribers.
			updateBFLimits?.Invoke(Team4AI.aiWithBF.BFTLCell, Team4AI.aiWithBF.BFBRCell);
			playerMadeMoveEvent?.Invoke(move);
		}


		// Every time Timer ticks - update label with time for each player
		// If time is up - call appropriate event handler
		private void timer_Tick(object sender, EventArgs e)
		{
			if (timeLimit.TotalMilliseconds > 0)// if player still has time
			{
				timeLimit=timeLimit.Subtract(TimeSpan.FromMilliseconds(100));// decrement player time limit for 100ms
				updateTimeLimitEvent?.Invoke(this);
			}
			else // Time's up!
			{
				// Call event handler
				timesUpEvent?.Invoke(this);
			}
		}

	}
}
