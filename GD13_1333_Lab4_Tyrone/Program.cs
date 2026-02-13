using GD13_1333_Lab4_Tyrone.Scripts;
using System;

namespace GD13_1333_Lab3_Tyrone
{
    //program class literally only starts the game
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager manager = new GameManager();
            manager.PlayGame();
        }
    }
}
