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

	public class AIPlayerWithBF
	{
		#region VARIABLES
		// Fields, Properties 
		public Board BattleField { get; set; }
		// this cells define active battlefield, because board can be very big in regards to the moves already made, so no need to calculate possible moves for all empty cells, just the closest ones.
		public Cell TopLeftCell;
		public Cell BotRightCell;
		public Cell BFTLCell;
		public Cell BFBRCell;
		int padding;// padding to form a battlefield from the main board and made moves.
										// Algorithms references
		MinMaxAlg minMaxAlg;
		#endregion

		// Default Ctor 
		public AIPlayerWithBF(State myFigure, State opponentFigure, int stripeLengthToWin)
		{
			TopLeftCell = new Cell(0, 0);
			BotRightCell = new Cell(0, 0);
			BFTLCell = new Cell(0, 0);
			BFBRCell = new Cell(0, 0);
			minMaxAlg = new MinMaxAlg(myFigure, opponentFigure, stripeLengthToWin);// MyFigure, OppFigure - minMax can self define, stripesToWin - take from board.stripelenthtowin
			
		}

		// To match my version of TicTacToe (testing, playing AI vs AI, Human vs AI)
		public Cell MakeMove(Board board, int depthRecursion, CancellationToken token = default(CancellationToken))
		{
			Cell aiMove;

			if (board.rows * board.columns - board.remainingQtyMovesForGame == 0)// 1st move in game
			{
				// AIPlayer takes center, this option never happens.
			}
			else if(board.rows * board.columns - board.remainingQtyMovesForGame == 1)// 2nd move in game
			{
				// 1st move was made by opponent, so update with 'true'
				UpdateBattleFieldLimits(board, true);
			}
			else// 3rd and after move in game
			{
				UpdateBattleFieldLimits(board, false);
			}

			BattleField = MakeBattleField(board);

			// If CancellationToken is not null, pass it to the MinMax.NextMove()
			if (token != CancellationToken.None)
			{
				aiMove = minMaxAlg.NextMove(BattleField, depthRecursion, token);
			}
			else// no CancellationToken, Player can't stop AI from calculating the move.
			{
				aiMove = minMaxAlg.NextMove(BattleField, depthRecursion);
			}

			// Convert BattleField coords to board coords
			aiMove = CoordsBFtoMainBoard(aiMove);

			// Updating borders for BattleField
			UpdateBattleFieldLimits(board, false);

			return aiMove;
		}

		
		#region MakeBattleField
		//---------------------------------
		// Make BattleField from board - shortening the analysis field
		// Проблема в том, что даже BattleField быстро вырастает до размера 6 на 6 или более, с 
		// учетом того, что у нас еще отступ в две клетки.
		// Поэтому алгоритм должен быстро работать и без сокращения игрового поля на досках размерами до 10 на 10.
		//---------------------------------
		private Board MakeBattleField(Board board)
		{
			Board newBoard;

			// Every time make new Battlefield, because with increasting size all indexes of the cells will change as well.
			// BotRightCell.row + 1 because we convert cell index(starts from 0) into row/column count(starts from 1).
			int rows = BFBRCell.row + 1 - BFTLCell.row;
			int cols = BFBRCell.col + 1 - BFTLCell.col;

			// TODO: заполняется ли BF нулями по умолчанию?
			newBoard = new Board(rows, cols, board.StripeLengthToWin);

			for (int i = 0; i < newBoard.rows; i++)
			{
				for (int j = 0; j < newBoard.columns; j++)
				{
					// Если клетка поля не пустая (!=0) записываем ее значение в новую доску
					if (board[BFTLCell.row + i, BFTLCell.col + j] != (int)State.Empty)
					{
						Cell lastMove = new Cell(i, j, (State)board[BFTLCell.row + i, BFTLCell.col + j]);
						newBoard.UpdateBoard(lastMove);
					}
				}
			}
			newBoard.lastMove = new Cell(board.lastMove.row - BFTLCell.row, board.lastMove.col - BFTLCell.col, board.lastMove.value);
			// При искусственном ограничении размера поля количество в ряд не должно превышать размеры нового поля
			newBoard.StripeLengthToWin = newBoard.rows >= board.StripeLengthToWin ? board.StripeLengthToWin : newBoard.rows;
			return newBoard;
		}

		private Cell CoordsBFtoMainBoard(Cell battleFieldMove)
		{
			Cell newMove;
			newMove = new Cell(BFTLCell.row + battleFieldMove.row, BFTLCell.col + battleFieldMove.col, battleFieldMove.value);
			return newMove;
		}

		private void UpdateBattleFieldLimits(Board board, bool firstBFLimitsUpdate)
		{
			// if players made only 1 move
			if (firstBFLimitsUpdate && board.rows > 3)
			{
				padding = 1;
			}
			else
			{
				padding = 2;
			}

			UpdateTopLeftCell(board.lastMove, firstBFLimitsUpdate);
			UpdateBotRightCell(board.lastMove, firstBFLimitsUpdate);
			UpdateBFTopLeftCell(board);
			UpdateBFBotRightCell(board);
			
		}

		private void UpdateTopLeftCell(Cell lastMove, bool firstBFLimitsUpdate)
		{
			if (firstBFLimitsUpdate)
			{
				TopLeftCell.row = lastMove.row;
				TopLeftCell.col = lastMove.col;
			}
			else
			{
				if (lastMove.row < TopLeftCell.row)
				{
					TopLeftCell.row = lastMove.row;
				}
				if (lastMove.col < TopLeftCell.col)
				{
					TopLeftCell.col = lastMove.col;
				}
			}
			
		}

		private void UpdateBotRightCell(Cell lastMove, bool firstBFLimitsUpdate)
		{
			if (firstBFLimitsUpdate)
			{
				BotRightCell.row = lastMove.row;
				BotRightCell.col = lastMove.col;
			}
			else
			{
				if (lastMove.row > BotRightCell.row)
				{
					BotRightCell.row = lastMove.row;
				}
				if (lastMove.col > BotRightCell.col)
				{
					BotRightCell.col = lastMove.col;
				}
			}
			
		}

		// Update top left cell basing on last move
		private void UpdateBFTopLeftCell(Board board)
		{
				BFTLCell.col = TopLeftCell.col - padding < 0 ? 0 : TopLeftCell.col - padding;
				BFTLCell.row = TopLeftCell.row - padding < 0 ? 0 : TopLeftCell.row - padding;
			
		}

		// Update bot right cell basing on last move
		private void UpdateBFBotRightCell(Board board)
		{
				// keep values inside the board
			BFBRCell.col = BotRightCell.col + padding > board.columns - 1 ? board.columns - 1 : BotRightCell.col + padding;
			BFBRCell.row = BotRightCell.row + padding > board.rows - 1 ? board.rows - 1 : BotRightCell.row + padding;

		}

		#endregion

	}
}
