using System;
using System.Collections.Generic;

namespace Epam_Team4
{
	// ICloneable - чтобы передавать объект по значению, а не по ссылке
	public class Board : ICloneable
	{
		public int rows;
		public int columns;
		public int[,] cells;// 0 - 'Empty', 1 - 'X' 2 - 'O'
		public Cell lastMove;
		public GameStatus gameStatus;
		public int score;
		public int StripeLengthToWin;
		public int remainingQtyMovesForGame;
		public List<Board> children;


		// ctor
		public Board(int rows, int columns, int stripeLengthToWin)
		{
			this.rows = rows;
			this.columns = columns;
			cells = new int[rows, columns];
			this.StripeLengthToWin = stripeLengthToWin;
			gameStatus = GameStatus.inProgress;
			remainingQtyMovesForGame = rows * columns;
			lastMove = null;
			score = 0;
			children = new List<Board>();
		}

		// Indexer to get Board element - cell at coords (x,y)
		public int this[int i, int j]
		{
			get { return cells[i, j]; }
			set { cells[i, j] = value; }
		}

		public object Clone()
		{
			Board board = new Board(rows, columns, StripeLengthToWin);
			try
			{
				board.lastMove = new Cell(lastMove.row, lastMove.col, lastMove.value);
			}
			catch
			{
				board.lastMove = null;
			}
			Array.Copy(cells, board.cells, cells.Length);
			//var copy2d = orig2d.Select(a => a.ToArray()).ToArray();- in case Array.Copy doesn't work (creatin shallow copy instead of deep copy) use LINQ to deep copy array.
			board.gameStatus = GameStatus.inProgress;
			board.remainingQtyMovesForGame = this.remainingQtyMovesForGame;
			board.score = 0;
			return board;
		}

		// Update board with last move of the player
		public void UpdateBoard(Cell move)
		{
			if (this[move.row, move.col] == 0)// if cell is empty
			{
				this[move.row, move.col] = (int)move.value;// player made move - X|O 
				this.lastMove = new Cell(move.row, move.col, move.value);// update LastMove field of this board
				this.remainingQtyMovesForGame--;// one less free cell to play
			}
		}


		// Update game status basing on current board and last made move
		public void UpdateBoardStatus()
		{
			// if no more free cells to make move
			if (remainingQtyMovesForGame < 1)
			{
				if (CheckWinner(this))// no winner detected on the last move
				{
					this.gameStatus = GameStatus.win;
				}
				else
				{
					this.gameStatus = GameStatus.draw;// we have a winner
				}
			}
			else // if there are free cells to make move, check for winner
			{
				if (CheckWinner(this))
				{
					this.gameStatus = GameStatus.win;// we have a winner
				}
				else // no winner yet
				{
					// game still in progress...
				}
			}
		}

		// Check if there's a win or draw situation after player makes his turn.
		private bool CheckWinner(Board board)
		{
			if (CheckDiagonal1(board))
			{
				return true;
			}
			if (CheckDiagonal2(board))
			{
				return true;
			}
			if (CheckVertical(board))
			{
				return true;
			}
			if (CheckHorizontal(board))
			{
				return true;
			}

			return false;
		}

		// Check Diagonal1
		private bool CheckDiagonal1(Board board)
		{
			int value, row, col, counter = 0;
			row = board.lastMove.row;
			col = board.lastMove.col;
			// while inside board borers increment row, col
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					row++;
					col--;
				}
				else
				{
					break;
				}
			}

			// check opposite diagonal side
			row = board.lastMove.row;
			col = board.lastMove.col;
			row--;
			col++;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					row--;
					col++;
				}
				else
				{
					break;
				}
			}

			return false;
		}

		// Check Diagonal2
		private bool CheckDiagonal2(Board board)
		{
			int value, row, col, counter = 0;
			row = board.lastMove.row;
			col = board.lastMove.col;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					row++;
					col++;
				}
				else
				{
					break;
				}
			}

			// check opposite diagonal side
			row = board.lastMove.row;
			col = board.lastMove.col;
			row--;
			col--;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					row--;
					col--;
				}
				else
				{
					break;
				}
			}

			return false;
		}

		// Check Vertical
		private bool CheckVertical(Board board)
		{
			int value, row, col, counter = 0;
			row = board.lastMove.row;
			col = board.lastMove.col;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == (int)board.lastMove.value)
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					row++;
				}
				else
				{
					break;
				}
			}

			// check opposite diagonal side
			row = board.lastMove.row;
			col = board.lastMove.col;
			row--;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					row--;
				}
				else
				{
					break;
				}
			}

			return false;
		}

		// Check Horizontal
		private bool CheckHorizontal(Board board)
		{
			int value, row, col, counter = 0;
			row = board.lastMove.row;
			col = board.lastMove.col;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					col++;
				}
				else
				{
					break;
				}
			}

			// check opposite diagonal side
			row = board.lastMove.row;
			col = board.lastMove.col;
			col--;
			while (row < board.rows && row >= 0 && col < board.columns && col >= 0)
			{
				value = board[row, col];
				if (value == board[board.lastMove.row, board.lastMove.col])
				{
					counter++;
					if (counter >= StripeLengthToWin)
					{
						return true;
					}
					col--;
				}
				else
				{
					break;
				}
			}

			return false;
		}

		
	}
}
