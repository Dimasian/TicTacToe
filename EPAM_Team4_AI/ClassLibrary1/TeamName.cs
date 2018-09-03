using System;

namespace EPAM.TicTacToe
{
    public class TeamName : Attribute
    {
        public TeamName(string team)
        {
            Team = team;
        }
        public string Team { get; }
    }
}
