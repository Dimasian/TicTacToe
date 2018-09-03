using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam_Team4
{
	// ICloneable - чтобы передавать объект по значению, а не по ссылке
	[Obsolete]
	public class HeavyBoard : Board, ICloneable
	{
		public Dictionary<string, Cell> emptyCells = new Dictionary<string, Cell>();

		// ctor
		public HeavyBoard(int rows, int columns, int stripeLengthToWin):base(rows,columns,stripeLengthToWin)
		{
			this.rows = rows;
			this.columns = columns;
			cells = new int[rows, columns];
			this.StripeLengthToWin = stripeLengthToWin;
			gameStatus = GameStatus.inProgress;
			DefineEmptyCells();
		}

		public new object Clone()
		{
			HeavyBoard board = new HeavyBoard(rows, columns, StripeLengthToWin);
			board.lastMove = new Cell(lastMove.row, lastMove.col, lastMove.value);
			Array.Copy(cells, board.cells, cells.Length);
			//var copy2d = orig2d.Select(a => a.ToArray()).ToArray();- in case Array.Copy doesn't work (creatin shallow copy instead of deep copy) use LINQ to deep copy array.
			board.emptyCells = new Dictionary<string, Cell>(this.emptyCells);
			board.gameStatus = this.gameStatus;
			return board;
		}

		public void DefineEmptyCells()
		{
			// First turn on empty board
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					if (this[i, j] == (int)State.Empty)
					{
						emptyCells.Add(string.Format("[{0},{1}]", i, j), new Cell(i, j, State.Empty));
					}
				}
			}
		}

		public new void UpdateBoard(Cell move)
		{
			if (this[move.row, move.col] == (int)State.Empty)
			{
				this[move.row, move.col] = (int)move.value;
				emptyCells.Remove(string.Format("[{0},{1}]", move.row, move.col));
				this.lastMove = new Cell(move.row, move.col, move.value);
			}
		}

		// Update game status basing on current board and last made move
		public void UpdateBoardStatus()
		{
			// if no more free cells to make move
			if (this.emptyCells.Count < 1)
			{
				if (!CheckWinner(this))// no winner detected on the last move
				{
					this.gameStatus = GameStatus.draw;
				}
				else
				{
					this.gameStatus = GameStatus.win;// we have a winner
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
