using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam_Team4
{
	// Player move. Abstract coordinates on the Board [m,n]
	// TODO: CONVERT CELL TO STUCT !?
	public class Cell
	{
		public int row { get; set; }
		public int col { get; set; }
		public State value { get; set; }

		public Cell(int row, int col, State value=State.Empty)
		{
			this.row = row;
			this.col = col;
			this.value = value;
		}
	}
}
