using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Epam_Team4
{
	public class MinMaxAlg
	{
		State MyFigure;
		State OpponentFigure;
		State activeFigure;
		int StripeLengthToWin;
		int maxDepthRecursion;

		//Ctor
		public MinMaxAlg(State MyFigure, State OpponentFigure, int StripeLengthToWin)
		{
			this.MyFigure = MyFigure;
			this.OpponentFigure = OpponentFigure;
			this.StripeLengthToWin = StripeLengthToWin;
		}

		// Make NextMove basing on the board.lastMove
		// TODO: кто задает глубину рекурсии - сам алгоритм, или вызывающая программа, которая
		// следит за размером поля и выбирает алгоритмы?
		public Cell NextMove(Board board, int depthRecursion, CancellationToken token = default(CancellationToken))
		{
			maxDepthRecursion = depthRecursion;
			Cell aiMove;

			// When NextMove is called, set activeFigure to MyFigure
				activeFigure = MyFigure;

			//ConcurrentBag<Board> children = new ConcurrentBag<Board>();
			List<Board> children = new List<Board>();

			// Generate children using board.Clone()
			children = GenerateChildren(board, activeFigure);

			// Evaluate each child board. Update CHILD BOARD SCORE!
			// TODO: Вроде параллелизм работает, но комп подвисает...Сделать лог?
			Parallel.ForEach(children, child =>
			{

				// Evaluate each child board
				if (token != CancellationToken.None)
				{
					child.score += EvaluateBoard(child, activeFigure, depthRecursion, token);

				}
				else
				{
					child.score += EvaluateBoard(child, activeFigure, depthRecursion);
				}
			});

			//Однопоточная реализация
			//foreach (Board child in children)
			//{
			//	//Evaluate each child board
			//	child.score += EvaluateBoard(child, activeFigure, depthRecursion);
			//}


			// Sort all children of the 1st breed by max score value and select max.
			Board highestPriorityChild = children.OrderByDescending(item => item.score).First();
			aiMove = highestPriorityChild.lastMove;
			 return aiMove;
		}

		// RECURSIVE: Check game status for the board, repeat if gameStatus==gameStatus.inProgress
		private int EvaluateBoard(Board board, State activeFigure, int depthRecursion, CancellationToken token = default(CancellationToken))
		{
			if (token != CancellationToken.None)
			{
				// Check for cancellation
				token.ThrowIfCancellationRequested();
			}

			int score = 0;
			int multiplier = 10;
			// Multiplier gives more value more to closer win, rather than bigger possible score later.
			multiplier = (int)Math.Pow(multiplier, depthRecursion);

			board.UpdateBoardStatus();

			switch (board.gameStatus)
			{
				case GameStatus.win:
					if (activeFigure == MyFigure)
					{
						if (depthRecursion == maxDepthRecursion)
						{
							// AIPlayer wins with move on max recursion level  - return!
							score= 100 * multiplier;
							board.score = score;
							break;
						}
						else
						{
							score = 10 * multiplier;
							board.score = score;
						}

					}
					else
					{
						score = -10 * multiplier;
						board.score = score;
					}
					break;
				case GameStatus.draw:
					score = 2 * multiplier;
					board.score = score;
					break;
				case GameStatus.inProgress:
					// go next level of recursion
					if (depthRecursion > 0)
					{
						depthRecursion--;// decrement depth of recursion
						// switch active player by switching its figure
						activeFigure = activeFigure == MyFigure ? OpponentFigure : MyFigure;

						List<Board> children = GenerateChildren(board, activeFigure);
						
						// Evaluate each child board. Update PARENT BOARD SCORE!
						//Parallel.ForEach(children, child =>
						//{
						//	// Evaluate each child board
						//	score += EvaluateBoard(child, activeFigure, depth);
						//});
						if (token != CancellationToken.None)
						{
							foreach (Board child in children)
							{
								// Evaluate each child board. Update PARENT BOARD SCORE!
								board.score += EvaluateBoard(child, activeFigure, depthRecursion, token);
							}
						}
						else
						{
							foreach (Board child in children)
							{
								//board.children.Add(child);

								// Evaluate each child board. Update PARENT BOARD SCORE!
								board.score += EvaluateBoard(child, activeFigure, depthRecursion);
							}
						}

					}
					break;
			}
			return board.score;
		}


		// Make new Boards from the current one, makin moves only in empty cells
		private List<Board> GenerateChildren(Board board, State activeFigure)
		{
			List<Board> boards = new List<Board>();

			// For all empty cells on the board
			for (int i = 0; i < board.rows; i++)
			{
				for (int j = 0; j < board.columns; j++)
				{
					if (board[i, j] == 0)
					{
						// Copy parent board to all children boards (значение класса передается по ссылке, что не подходит, поэтому исп. Clone())
						boards.Add((Board)board.Clone());
						// Make random move for each empty cell with figure of current player.
						boards.Last().UpdateBoard(new Cell(i, j, activeFigure));
					}
				}
			}

			return boards;
		}

		// может ConcurrentBage не нужен для реалзицаии Parallel.ForEach? Во всяком случае с этой реализацией, алгоритм не работает как надо.
		private ConcurrentBag<Board> GenerateChildren(Board board, State activeFigure, bool concurrent)
		{
			ConcurrentBag<Board> boards = new ConcurrentBag<Board>();

			// For all empty cells on the board
			for (int i = 0; i < board.rows; i++)
			{
				for (int j = 0; j < board.columns; j++)
				{
					if (board[i, j] == 0)
					{
						// Copy parent board to all children boards (значение передается по ссылке или копируется?)
						boards.Add((Board)board.Clone());
						// Make random move for each empty cell with figure of current player.
						boards.Last().UpdateBoard(new Cell(i, j, activeFigure));
					}
				}
			}

			return boards;
		}




	}
}
