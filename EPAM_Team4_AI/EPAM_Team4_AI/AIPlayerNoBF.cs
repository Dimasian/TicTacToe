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

	public class AIPlayerNoBF
	{
		#region VARIABLES
		// Fields, Properties 

		// Algorithms references
		MinMaxAlg minMaxAlg;
		#endregion

		// Default Ctor 
		public AIPlayerNoBF(State myFigure, State opponentFigure, int stripeLengthToWin)
		{
			minMaxAlg = new MinMaxAlg(myFigure, opponentFigure, stripeLengthToWin);
		}

		// To match my version of TicTacToe (testing, playing AI vs AI, Human vs AI)
		public Cell MakeMove(Board board, int depthRecursion, CancellationToken token = default(CancellationToken))
		{
			Cell aiMove;

			// If CancellationToken is not null, pass it to the MinMax.NextMove()
			if (token != CancellationToken.None)
			{
				aiMove = minMaxAlg.NextMove(board, depthRecursion, token);
			}
			else// no CancellationToken, Player can't stop AI from calculating the move.
			{
				aiMove = minMaxAlg.NextMove(board, depthRecursion);
			}


			return aiMove;
		}
	

	}
}
