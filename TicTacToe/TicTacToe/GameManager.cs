using System;
using System.Linq;
using Epam_Team4;

namespace TicTacToe
{
    // Game manager: create board, create players, check win condition, check time limit.
    public class GameManager
    {
        Board board;// board to play on
        TimeSpan timeLimit;// time limit for a game for each player
        Random rnd;
        DrawBoard deDrawBoard;// draw grid
        DrawFigure deDrawFigure;// draw figures
        GameOverMessage deGameOver;// update main form when game is over
        UpdateTimeLimit deUpdateTimeLimit;// update time limit on the form's labels
        DrawBFLimits deDrawBFLimits;// draw BattleField limits
        public bool gameOn;

        public int boardSize { get; set; }
        public int stripeLengthToWin { get; set; }
        PlayerType player1Type;// human/AI
        State player1Figure;// X or O
        PlayerType player2Type;// human/AI
        State player2Figure;// X or O
        public Player player1;
        public Player player2;
        public Player activePlayer;
        Epam_Team4.AIPlayer Team4AI;


        // Ctor.
        public GameManager(string[] settings, DrawBoard drawBoardDelegate, DrawFigure drawFigureDelegate, GameOverMessage gameOverMessageDelegate, UpdateTimeLimit updateTimeLimitDelegate, DrawBFLimits DrawBFLimitsDelegate)
        {
            // Set variables
            boardSize = int.Parse(settings[0]);
            stripeLengthToWin = int.Parse(settings[1]);
            player1Type = (PlayerType)(int.Parse(settings[2]));
            player2Type = (PlayerType)(int.Parse(settings[3]));
            deDrawBoard = drawBoardDelegate;
            deDrawFigure = drawFigureDelegate;
            deGameOver = gameOverMessageDelegate;
            deUpdateTimeLimit = updateTimeLimitDelegate;
            deDrawBFLimits = DrawBFLimitsDelegate;
            timeLimit = new TimeSpan(0, 0, int.Parse(settings[4]));

        }

        public void StartGame()
        {
            gameOn = true;
            // Generate abstract Board as array: int[x,y]
            board = new Board(boardSize, boardSize, stripeLengthToWin)
            {
                gameStatus = GameStatus.inProgress
            };
            RedrawBoard();
            InitializePlayers();
        }

        // Redraw board and all figures "X" and "O"
        public void RedrawBoard()
        {
            // Draw board on screen
            deDrawBoard(boardSize, boardSize);

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    State state = (State)board[i, j];

                    deDrawFigure(state, i, j);
                }
            }
        }

        /// <summary>
        /// Initialize players and Determine which figure for which player, who goes first and subscribe for PlayerMadeMove event
        /// </summary>
        private void InitializePlayers()
        {
            // Determine which figure for which player: "X" go first, "O" go second.
            rnd = new Random();
            if (rnd.Next(0, 2) == 0)
            {
                player1Figure = State.X;
                player2Figure = State.O;
            }
            else
            {
                player1Figure = State.O;
                player2Figure = State.X;
            }
            // Initialize 1st player.
            if (player1Type == PlayerType.human)// from game settings
            {
                player1 = new Player(new HumanPlayer(player1Figure), timeLimit);
            }
            else
            {
                Team4AI = new Epam_Team4.AIPlayer((byte)player1Figure, (byte)stripeLengthToWin, boardSize);
                player1 = new Player(Team4AI, timeLimit);
            }
            player1.MyNumber = PlayerNumber.player1;


            // Initialize 2nd player. 
            if (player2Type == PlayerType.human)// from game settings
            {
                player2 = new Player(new HumanPlayer(player2Figure), timeLimit);
            }
            else
            {
                Team4AI = new Epam_Team4.AIPlayer((byte)player2Figure, (byte)stripeLengthToWin, boardSize);
                player2 = new Player(Team4AI, timeLimit);
            }
            player2.MyNumber = PlayerNumber.player2;

            // Subscribe to players time's up event.
            player1.updateBFLimits += OnPlayerUpdateBFLimits;
            player2.updateBFLimits += OnPlayerUpdateBFLimits;

            // Subscribe to players time's up event.
            player1.timesUpEvent += OnTimesUp;
            player2.timesUpEvent += OnTimesUp;

            // Subscribe to player update event
            player1.updateTimeLimitEvent += OnUpdateTimeLimit;
            player2.updateTimeLimitEvent += OnUpdateTimeLimit;

            // Update labels with time limit for each player.
            deUpdateTimeLimit(player1);
            deUpdateTimeLimit(player2);


            // Subscribe to players MadeMove events
            if (player1.MyPlayerType == PlayerType.computer)
            {
                player1.playerMadeMoveEvent += OnPlayerMadeMove;
            }
            if (player2.MyPlayerType == PlayerType.computer)
            {
                player2.playerMadeMoveEvent += OnPlayerMadeMove;
            }

            // Set activePlayer
            activePlayer = player1.MyFigure == State.X ? player1 : player2;

            // FIRST TURN
            // If first turn is up to computer, call his move.
            if (activePlayer.MyPlayerType == PlayerType.computer)
            {
                activePlayer.MakeMove((Board)board.Clone());// pass new board to the AIPlayer
            }
            else
            {
                activePlayer.timer.Start();// if human - start his timer
            }
        }

        private void OnUpdateTimeLimit(Player player)
        {
            deUpdateTimeLimit(player);
        }

        private void OnTimesUp(Player player)
        {
            string opponentName = player.MyNumber == player1.MyNumber ? player2.name : player1.name;
            // Generate message for Game Over function
            string message = string.Format("Time for player {0} is up! Winner is: {1}", activePlayer.name, opponentName);
            GameOver(message);
        }

        public void GameOver(string message = "")
        {
            gameOn = false;
            player1.timer.Enabled = false;
            player2.timer.Enabled = false;
            if (message != string.Empty)
            {
                deGameOver(message);
            }
        }

        // Event handler. 
        // После хода игрока сюда передается его последний ход.
        // Поле оценивается на предмет победы/ничьей и, в случае продолжения игры
        // меняется активный игрок и вызывается его метод "сделать ход".
        public void OnPlayerMadeMove(Cell lastMove)
        {
            if (gameOn == false) { return; }// Don't act on moving or clicking on the board if game is over.

            // Check current game status before last move
            switch (board.gameStatus)
            {
                case GameStatus.inProgress:
                    // if player has made a legal move - to the empty cell
                    if (board[lastMove.row, lastMove.col] == (int)State.Empty)
                    {
                        // stop timer for current active player
                        activePlayer.timer.Stop();
                        // Update board cell[,] values
                        board.UpdateBoard(lastMove);
                        // Update visual board - draw player figure
                        deDrawFigure(activePlayer.MyFigure, lastMove.row, lastMove.col);
                    }
                    else// if illegal move!
                    {
                        if (activePlayer.MyPlayerType == PlayerType.computer)
                        {
                            throw new Exception();// cell is occupied but AIPlayer try to rewrite it!
                        }
                        else
                        {
                            activePlayer.timer.Start();
                            return;// Human made move in the same cell - ignore this move
                                   // and don't stop his timer!
                        }
                    }


                    // Check Game Over after this move
                    board.UpdateBoardStatus();
                    // If after last move board.status has changed to win or draw, finish the game.
                    if (board.gameStatus != GameStatus.inProgress)
                    {
                        // рекурсивно вызвать эту же функцию,т.к. игра завершилась и нам нужно теперь попасть в другое 
                        // состояние Switch.gameStatus
                        OnPlayerMadeMove(board.lastMove);
                        break;
                    }

                    // If game is not yet finished.
                    // Switch players.
                    activePlayer = (activePlayer.MyFigure == player1.MyFigure) ? player2 : player1;

                    // if it's AIplayer turn now, set him as active player and wait for MakeMove event to trigger
                    if (activePlayer.MyPlayerType == PlayerType.computer)
                    {
                        activePlayer.MakeMove((Board)board.Clone());// pass new board to the AIPlayer
                    }
                    else
                    {
                        activePlayer.timer.Start();// for human player just start his timer
                    }
                    break;

                case GameStatus.win:
                    string message;
                    message = string.Format("Winner is:{0}", (activePlayer.name));
                    GameOver(message);
                    break;

                case GameStatus.draw:
                    message = string.Format("It's a draw!");
                    GameOver(message);
                    break;
            }

        }


        // Draw Rectangle, representing BattleField for the AIPlayer.
        public void OnPlayerUpdateBFLimits(Cell TLcell, Cell BRcell)
        {
            deDrawBFLimits(TLcell, BRcell);
        }



    }
}

