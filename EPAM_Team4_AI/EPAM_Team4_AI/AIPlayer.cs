using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EPAM.TicTacToe;// Mikhail namespace

namespace Epam_Team4
{
	//public delegate void UpdateBoardStatus(Board board);

	[TeamName("Fourth team")]
	public class AIPlayer : IPlayer
	{
		#region VARIABLES
		// Fields, Properties 
		public State MyFigure { get; set; }// player goes with "X" or "O".1-'O', 2-'X'.
		public State OpponentFigure { get; set; }// opponent goes with "X" or "O".
		public String Name { get; set; }
		public Board MainBoard { get; set; }
		// Algorithms references
		int depthRecursion;

		AIPlayerNoBF aiNoBF;
		public AIPlayerWithBF aiWithBF;
		#endregion

		// Default Ctor for Mikhail interaface

		public AIPlayer()
		{
		}

		// Ctor with parameters
		public AIPlayer(byte signForPlayer, byte qtySingsForWin, int maxLengthFieldOfBattlefield)
		{
			SetAIPlayerParameters(signForPlayer, qtySingsForWin, maxLengthFieldOfBattlefield);
		}

		private void SetAIPlayerParameters(byte signForPlayer, byte qtySingsForWin, int maxLengthFieldOfBattlefield)
		{

			MyFigure = (State)signForPlayer;
			OpponentFigure = (MyFigure == State.X) ? State.O : State.X;
			MainBoard = new Board(maxLengthFieldOfBattlefield, maxLengthFieldOfBattlefield, qtySingsForWin);
			Name = "Team 4 AI Player";

			SetDepthRecursion(maxLengthFieldOfBattlefield);

			aiNoBF = new AIPlayerNoBF(MyFigure, OpponentFigure, qtySingsForWin);
			aiWithBF = new AIPlayerWithBF(MyFigure, OpponentFigure, (int)qtySingsForWin);

		}

		private void SetDepthRecursion(int boardSize)
		{
			depthRecursion = 5;
			//if (boardSize<=5)
			//{
			//	depthRecursion = 4;
			//}
			//else if (boardSize>5 && boardSize<=10)
			//{
			//	depthRecursion = 4;
			//}
			//else
			//{
			//	depthRecursion = 4;
			//}
		}


		// ПОСЛЕДНЯЯ РЕАЛИЗАЦИЯ ИНТЕРФЕЙСА ОТ МИХАИЛА
		public CellCoordinates NextMove(CellState.cellState[,] currentState, byte qtyCellsForWin, bool isHuman, TimeSpan remainingTimeForGame, int remainingQtyMovesForGame)
		{
			CellCoordinates nextMove = new CellCoordinates();
			Cell aiMove;// move in Team4 naming 

			// Check empty field -> AIPlayer makes 1st move
			if (IsBoardEmpty(currentState))
			{
				// TODO: ПЕРВЫЕ ХОДЯТ ВСЕГДА КРЕСТИКИ
				SetAIPlayerParameters(1, qtyCellsForWin, currentState.GetLength(0));
				aiMove = TakeCenter(MainBoard);
			}
			else //not 1st move in game!
			{

				if (MainBoard == null)// if AIPlayer has not made a turn yet (second turn in game) - AI plays 'O'.
				{
					SetAIPlayerParameters(2, qtyCellsForWin, currentState.GetLength(0));
				}
				// Update MainBoard lastMove - not 1st or 2nd move in game
				MainBoard.UpdateBoard(DefineLastMove(currentState));
				aiMove = MakeMove(MainBoard);
			}

			nextMove.X = (byte)aiMove.row;// Y - это значение столбца, а X - строки.
			nextMove.Y = (byte)aiMove.col;
			return nextMove;
		}

		// To match my version of TicTacToe (testing, playing AI vs AI, Human vs AI)
		public Cell MakeMove(Board board, CancellationToken token = default(CancellationToken))
		{
			Cell aiMove;


			#region УБРАТЬ В ФИНАЛЬНОЙ ВЕРСИИ. ЭТО ДЛЯ ТЕСТИРОВАНИЯ
			// TODO: УБРАТЬ UpdateBoard из этого метода В ФИНАЛЬНОЙ Версии, т.к. дублирует код в функции NextMove
			if (board.rows * board.columns - board.remainingQtyMovesForGame == 0)
			{
				aiMove = TakeCenter(MainBoard);
				MainBoard.UpdateBoard(aiMove);
				return aiMove;
			}
			else
			{
				MainBoard.UpdateBoard(board.lastMove);
			}
			#endregion


			// if there are less than 25? free cells, don't use BattleField
			if (board.remainingQtyMovesForGame <= 25 && board.rows*board.columns- board.remainingQtyMovesForGame>2)
			{
				//aiMove = aiNoBF.MakeMove(board, depthRecursion);
				aiMove = RandomMove(board);

			}
			else// if there are more than 25? free cells,  use BattleField to shorten the three
			{
				//aiMove = aiWithBF.MakeMove(board, depthRecursion);
				aiMove = RandomMove(board);

			}

			MainBoard.UpdateBoard(aiMove);

			return aiMove;
		}

		public void RefreshUI(CellState.cellState[,] CurrentState)
		{
		}

		// Is the game board passed by the host empty?
		private bool IsBoardEmpty(CellState.cellState[,] board)
		{
			int count = 0;
			for (int i = 0; i < board.GetLength(0); i++)
			{
				for (int j = 0; j < board.GetLength(1); j++)
				{
					if (board[i, j] == CellState.cellState.Empty)
					{
						count++;
					}
				}
			}
			if (count == board.GetLength(0) * board.GetLength(1))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private Cell DefineLastMove(CellState.cellState[,] currentState)
		{
			Cell lastMove = null;
			for (int i = 0; i < MainBoard.rows; i++)
			{
				for (int j = 0; j < MainBoard.columns; j++)
				{
					if (MainBoard[i, j] != (int)currentState[i, j])
					{
						// т.к. класс Михаила 'CellState' не совпадает с нашей внутренней реализацией Enums.State
						// то сначала конвертируем в Int32, а затем в State.
						lastMove = new Cell(i, j, (State)((int)currentState[i, j]));
					}
				}
			}
			return lastMove;
		}

		// If board is empty - take center!
		private Cell TakeCenter(Board board)
		{
			double r, c = 0;
			r = board.rows / 2;
			c = board.columns / 2;
			return new Cell((int)Math.Floor(r), (int)Math.Floor(c), MyFigure);
		}

		// Just for testing purposes - most simple AI making random move every time (to empty cells only).
		private Cell RandomMove(Board board)
		{
			Cell aiMove = new Cell(0, 0, MyFigure);
			// Make random move for now from the list of available moves
			Random rnd = new Random();

			// Choose randomly one of the available cells
			do
			{
				aiMove.row = rnd.Next(0, board.rows);
				aiMove.col = rnd.Next(0, board.columns);
			} while (board[aiMove.row, aiMove.col] != 0);

			return aiMove;
		}

	}
}
