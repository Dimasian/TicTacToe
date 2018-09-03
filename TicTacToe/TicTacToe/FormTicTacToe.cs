using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Epam_Team4;

namespace TicTacToe
{
	public delegate void DrawBoard(int rows, int columns);
	public delegate void DrawFigure(State figure, int row, int col);
	public delegate void GameOverMessage(string message);
	public delegate void UpdateTimeLimit(Player player);
	public delegate void DrawBFLimits(Cell TLcell, Cell BRcell);


	public partial class FormTicTacToe : Form
	{
		Graphics graphics;
		GameManager gameManager;
		int cellSizeInPixels;
		int panelSize;
		int winFormSize;


		public FormTicTacToe()
		{
			InitializeComponent();
		}

		private void FormTicTacToe_Load(object sender, EventArgs e)
		{
			DefaultSettings();

			graphics = Board.CreateGraphics();

		}

		private void DefaultSettings()
		{
			tbBoardSize.Text = 5.ToString();// Board size (always square)
			tbStripeLengthToWin.Text = 4.ToString(); // (stripe length to win in a row)
			cmbPlayer1.Items.Add("Human");
			cmbPlayer1.Items.Add("Computer");
			cmbPlayer1.SelectedIndex = 0;// 0- human, 1-AIPlayer
			cmbPlayer2.Items.Add("Human");
			cmbPlayer2.Items.Add("Computer");
			cmbPlayer2.SelectedIndex = 1;// 0- human, 1-AIPlayer
			tbTimeLimit.Text = 120.ToString() ;// time limit for a game for each player in seconds
			cellSizeInPixels = 50; // cell size on the board for visual representation
		}

		private void btnStartGame_Click(object sender, EventArgs e)
		{
			if (gameManager!=null)
			{
				if (gameManager.gameOn)
				{
					gameManager.GameOver();
					btnStartGame.Text = "START GAME";
					return;
				}
				
			}
			
			// Перед каждой новой игрой считиываем настройки и пересоздаем класс GameManager
				//settings[0] - Board size
				//settings[1] - stripe length
				//settings[2] - index 0/1 (human/computer)
				//settings[3] - index 0/1 (human/computer)
				string[] settings = new string[5];

				if (SetParameters(ref settings))
				{
					panelSize = int.Parse(settings[0]) * cellSizeInPixels;
					winFormSize = (int)(panelSize * 1);
					Board.Size = new Size(panelSize, panelSize);
					this.Size = new Size(winFormSize + 350, winFormSize + 100);
					gameManager = new GameManager(settings, DrawBoard, DrawFigure, GameOverMessage, UpdateTimerLabel, DrawBFLimits);
					gameManager.StartGame();

					lblTimer1Descr.Text = string.Format("Time limit for player {0}", gameManager.player1.name);
					lblTimer2Descr.Text = string.Format("Time limit for player {0}", gameManager.player2.name);
					btnStartGame.Text = "STOP GAME";
				}
			

		}

		private bool SetParameters(ref string[] settings)
		{
			settings[0] = tbBoardSize.Text;// rows
			settings[1] = tbStripeLengthToWin.Text; // stripe length
			settings[2] = cmbPlayer1.SelectedIndex.ToString();// 0 - human, 1 - computer
			settings[3] = cmbPlayer2.SelectedIndex.ToString();// 0 - human, 1 - computer
			settings[4] = tbTimeLimit.ToString();// time limit in minutes for a whole game for each player

			// Check that all settings are positive integers
			Regex regex = new Regex(@"^[0-9]*$");
			for (int i = 0; i < settings.Length; i++)
			{
				if (!regex.IsMatch(settings[i]))
				{
					MessageBox.Show("Неправильно заданы стартовые параметры!", "Error in parameters:", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}
			}

			// Field size cannot be less than 3
			for (int i = 0; i < 2; i++)
			{
				if (int.Parse(settings[i])<3)
				{
					MessageBox.Show("Неправильно заданы стартовые параметры!", "Error in parameters:", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}
			}

			return true;
		}

		private void DrawBoard(int rows, int columns)
		{
			graphics = Board.CreateGraphics();
			graphics.Clear(Color.White);
			// now draw the vertical lines

			Pen pen = new Pen(Brushes.Black)
			{
				Width = 5
			};

			for (int i = 0; i < rows - 1; i++)
			{
				graphics.DrawLine(pen, 
					cellSizeInPixels * (i + 1), 0, 
					cellSizeInPixels * (i + 1), Board.Height);
			}

			// now draw the horizontal lines

			for (int j = 0; j < columns - 1; j++)
			{
				graphics.DrawLine(pen, 
					0, cellSizeInPixels * (j + 1), 
					Board.Width, cellSizeInPixels * (j + 1));

			}
		}

		private void DrawFigure(State figure, int row, int col)
		{
			// Calculate where to draw "X" or "O" on the panel
			PointF point = PointFromCoords(row, col);

			// Font size should be dependant on the pixelPerRow parameter
			Font f = new Font("Arial", 20);
			if (figure==State.Empty)
			{
				return;
			}
			else if (figure==State.X)
			{
				graphics.DrawString("X", f, Brushes.Blue, point);

			}
			else if (figure == State.O)
			{
				graphics.DrawString("O", f, Brushes.Red, point);

			}

		}
		
		// Draw BattleField limits using TopLef and BottomRight cells
		private void DrawBFLimits(Cell TL, Cell BR)
		{
			// Redraw board to clear previous BF Limits
			gameManager.RedrawBoard();
			// 
			Pen pen = new Pen(Brushes.Red);
			PointF TLpointf = PointFromCoordsBFLimits(TL.row, TL.col, true);
			PointF BRpointf = PointFromCoordsBFLimits(BR.row, BR.col, false);

			graphics.DrawRectangle(pen, TLpointf.X, TLpointf.Y, BRpointf.X, BRpointf.Y);
		}

		// Get point on panel to draw figure - from board coordinates (x,y)
		private PointF PointFromCoordsBFLimits(int row, int col, bool BFTLCell)
		{
			float x, y;
			if (BFTLCell)
			{
				x = cellSizeInPixels * (col);// + half the width of the cell
				y = cellSizeInPixels * (row);// + half the heigth of the cell
			}
			else
			{
				x = cellSizeInPixels * (col+1);// + half the width of the cell
				y = cellSizeInPixels * (row+1);// + half the heigth of the cell
			}

			return new PointF(x, y);
		}

		// Get point on panel to draw figure - from board coordinates (x,y)
		private PointF PointFromCoords(int row, int col)
		{
			float x = cellSizeInPixels * (col+0.22f);// + half the width of the cell
			float y = cellSizeInPixels * (row+0.2f);// + half the heigth of the cell

			return new PointF(x, y);
		}

		private Cell CoordsFromPointF(PointF clickedPoint)
		{
			int r = (int)clickedPoint.Y / cellSizeInPixels;
			int c = (int)clickedPoint.X / cellSizeInPixels;
			return new Cell(r, c, gameManager.activePlayer.MyFigure);
		}

		// Player clicks on the board -> MAKES MOVE
		private void Board_MouseClick(object sender, MouseEventArgs e)
		{
			if (gameManager!=null)
			{
				if (gameManager.gameOn && gameManager.activePlayer.MyPlayerType == PlayerType.human)
				{
					// get cell abstract coords from point on screen
					Cell cellClicked = CoordsFromPointF(new PointF(e.X, e.Y));
					// call gameManager to validate this move
					gameManager.OnPlayerMadeMove(cellClicked);
				}
			}

		}

		private void GameOverMessage(string message)
		{
			if (btnStartGame.InvokeRequired)
			{
				btnStartGame.BeginInvoke((MethodInvoker)delegate () { btnStartGame.Text = "START GAME"; });
			}
			else
			{
				btnStartGame.Text = "START GAME";
			}
			MessageBox.Show(message, "Game Over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void FormTicTacToe_ResizeEnd(object sender, EventArgs e)
		{
			if (gameManager!=null)
			{
				if (gameManager.gameOn)
				{
					gameManager.RedrawBoard();
				}
			}
		}

		private void UpdateTimerLabel(Player player)
		{
			string timeLimit = string.Format("{0:mm\\:ss}:{1}", player.timeLimit, player.timeLimit.Milliseconds/100);

			if (player.MyNumber==PlayerNumber.player1)
			{
				if (lblTimer1Value.InvokeRequired)
				{
					lblTimer1Value.BeginInvoke((MethodInvoker)delegate () { lblTimer1Value.Text = timeLimit; });
				}
				else
				{
					lblTimer1Value.Text = timeLimit ;
				}
			}
			else
			{
				if (lblTimer2Value.InvokeRequired)
				{
					lblTimer2Value.BeginInvoke((MethodInvoker)delegate () { lblTimer2Value.Text = timeLimit; });
				}
				else
				{
					lblTimer2Value.Text = timeLimit;
				}
			}
		}
	}
}
