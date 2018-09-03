using System;
namespace EPAM.TicTacToe
{
    public interface IPlayer
    {
        CellCoordinates NextMove(CellState.cellState[,] CurrentState, byte qtyCellsForWin, bool isHuman, TimeSpan remainingTimeForGame, int remainingQtyMovesForGame);
    }
}
