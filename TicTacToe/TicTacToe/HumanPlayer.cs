using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam_Team4;

namespace TicTacToe
{
	public class HumanPlayer
	{
		public string Name { get; set; }
		public State MyFigure { get; set; }

		public HumanPlayer(State playerFigure)
		{
			Name = "Human";
			MyFigure = playerFigure;
		}

		// TODO: нужнен ли в классе HumanPlayer метод MakeMove()?
		public byte[] MakeMove()
		{
			byte[] move=new byte[] { };
			return move;
		}
	}
}
