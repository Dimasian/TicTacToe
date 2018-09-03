namespace TicTacToe
{
	partial class FormTicTacToe
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.smParameters = new System.Windows.Forms.ToolStripMenuItem();
			this.BoardSize = new System.Windows.Forms.ToolStripMenuItem();
			this.tbBoardSize = new System.Windows.Forms.ToolStripTextBox();
			this.StripeLengthToWin = new System.Windows.Forms.ToolStripMenuItem();
			this.tbStripeLengthToWin = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmbPlayer1 = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmbPlayer2 = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbTimeLimit = new System.Windows.Forms.ToolStripTextBox();
			this.btnStartGame = new System.Windows.Forms.Button();
			this.Board = new System.Windows.Forms.Panel();
			this.lblTimer1Descr = new System.Windows.Forms.Label();
			this.lblInformation = new System.Windows.Forms.Label();
			this.lblTimer1Value = new System.Windows.Forms.Label();
			this.lblTimer2Value = new System.Windows.Forms.Label();
			this.lblTimer2Descr = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smParameters});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(533, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// smParameters
			// 
			this.smParameters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BoardSize,
            this.StripeLengthToWin,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
			this.smParameters.Name = "smParameters";
			this.smParameters.Size = new System.Drawing.Size(78, 20);
			this.smParameters.Text = "Parameters";
			// 
			// BoardSize
			// 
			this.BoardSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbBoardSize});
			this.BoardSize.Name = "BoardSize";
			this.BoardSize.Size = new System.Drawing.Size(228, 22);
			this.BoardSize.Text = "Board size (Rows = Columns)";
			// 
			// tbBoardSize
			// 
			this.tbBoardSize.Name = "tbBoardSize";
			this.tbBoardSize.Size = new System.Drawing.Size(100, 23);
			// 
			// StripeLengthToWin
			// 
			this.StripeLengthToWin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbStripeLengthToWin});
			this.StripeLengthToWin.Name = "StripeLengthToWin";
			this.StripeLengthToWin.Size = new System.Drawing.Size(228, 22);
			this.StripeLengthToWin.Text = "Stripe Length To Win";
			// 
			// tbStripeLengthToWin
			// 
			this.tbStripeLengthToWin.Name = "tbStripeLengthToWin";
			this.tbStripeLengthToWin.Size = new System.Drawing.Size(100, 23);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbPlayer1});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(228, 22);
			this.toolStripMenuItem1.Text = "Player 1";
			// 
			// cmbPlayer1
			// 
			this.cmbPlayer1.Name = "cmbPlayer1";
			this.cmbPlayer1.Size = new System.Drawing.Size(121, 23);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbPlayer2});
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(228, 22);
			this.toolStripMenuItem2.Text = "Player 2";
			// 
			// cmbPlayer2
			// 
			this.cmbPlayer2.Name = "cmbPlayer2";
			this.cmbPlayer2.Size = new System.Drawing.Size(121, 23);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbTimeLimit});
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(228, 22);
			this.toolStripMenuItem3.Text = "Time Limit (sec)";
			// 
			// tbTimeLimit
			// 
			this.tbTimeLimit.Name = "tbTimeLimit";
			this.tbTimeLimit.Size = new System.Drawing.Size(100, 23);
			// 
			// btnStartGame
			// 
			this.btnStartGame.Location = new System.Drawing.Point(122, -1);
			this.btnStartGame.Name = "btnStartGame";
			this.btnStartGame.Size = new System.Drawing.Size(91, 23);
			this.btnStartGame.TabIndex = 1;
			this.btnStartGame.Text = "START GAME";
			this.btnStartGame.UseVisualStyleBackColor = true;
			this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
			// 
			// Board
			// 
			this.Board.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Board.Location = new System.Drawing.Point(13, 28);
			this.Board.Name = "Board";
			this.Board.Size = new System.Drawing.Size(200, 200);
			this.Board.TabIndex = 2;
			this.Board.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Board_MouseClick);
			// 
			// lblTimer1Descr
			// 
			this.lblTimer1Descr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimer1Descr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTimer1Descr.Location = new System.Drawing.Point(234, 111);
			this.lblTimer1Descr.Name = "lblTimer1Descr";
			this.lblTimer1Descr.Size = new System.Drawing.Size(214, 23);
			this.lblTimer1Descr.TabIndex = 3;
			this.lblTimer1Descr.Text = "Таймер игрока 1:";
			this.lblTimer1Descr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblInformation
			// 
			this.lblInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblInformation.Location = new System.Drawing.Point(234, 28);
			this.lblInformation.Name = "lblInformation";
			this.lblInformation.Size = new System.Drawing.Size(287, 70);
			this.lblInformation.TabIndex = 4;
			this.lblInformation.Text = "Информационное табло";
			this.lblInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblTimer1Value
			// 
			this.lblTimer1Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimer1Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTimer1Value.Location = new System.Drawing.Point(454, 111);
			this.lblTimer1Value.Name = "lblTimer1Value";
			this.lblTimer1Value.Size = new System.Drawing.Size(67, 23);
			this.lblTimer1Value.TabIndex = 5;
			this.lblTimer1Value.Text = "--";
			this.lblTimer1Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblTimer2Value
			// 
			this.lblTimer2Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimer2Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTimer2Value.Location = new System.Drawing.Point(454, 143);
			this.lblTimer2Value.Name = "lblTimer2Value";
			this.lblTimer2Value.Size = new System.Drawing.Size(67, 23);
			this.lblTimer2Value.TabIndex = 7;
			this.lblTimer2Value.Text = "--";
			this.lblTimer2Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblTimer2Descr
			// 
			this.lblTimer2Descr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTimer2Descr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTimer2Descr.Location = new System.Drawing.Point(234, 143);
			this.lblTimer2Descr.Name = "lblTimer2Descr";
			this.lblTimer2Descr.Size = new System.Drawing.Size(214, 23);
			this.lblTimer2Descr.TabIndex = 6;
			this.lblTimer2Descr.Text = "Таймер игрока 2:";
			this.lblTimer2Descr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// FormTicTacToe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(533, 271);
			this.Controls.Add(this.lblTimer2Value);
			this.Controls.Add(this.lblTimer2Descr);
			this.Controls.Add(this.lblTimer1Value);
			this.Controls.Add(this.lblInformation);
			this.Controls.Add(this.lblTimer1Descr);
			this.Controls.Add(this.Board);
			this.Controls.Add(this.btnStartGame);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FormTicTacToe";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tic Tac Toe";
			this.Load += new System.EventHandler(this.FormTicTacToe_Load);
			this.ResizeEnd += new System.EventHandler(this.FormTicTacToe_ResizeEnd);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem smParameters;
		private System.Windows.Forms.Button btnStartGame;
		private System.Windows.Forms.ToolStripMenuItem BoardSize;
		private System.Windows.Forms.ToolStripTextBox tbBoardSize;
		private System.Windows.Forms.ToolStripMenuItem StripeLengthToWin;
		private System.Windows.Forms.ToolStripTextBox tbStripeLengthToWin;
		private System.Windows.Forms.Panel Board;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripComboBox cmbPlayer1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripComboBox cmbPlayer2;
		public System.Windows.Forms.Label lblTimer1Descr;
		public System.Windows.Forms.Label lblInformation;
		public System.Windows.Forms.Label lblTimer1Value;
		public System.Windows.Forms.Label lblTimer2Value;
		public System.Windows.Forms.Label lblTimer2Descr;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripTextBox tbTimeLimit;
	}
}