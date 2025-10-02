
using System;
using System.Collections.Generic;

namespace DiceGame
{
    internal class ComputerPlayer : Player
    {
        private Random rng = new Random();

        public ComputerPlayer(string name, List<int> dice) : base(name, dice)
        {
        }

        // Computer picks a die at random
        public int ChooseDie()
        {
            int index = rng.Next(Dice.Count);
            int chosen = Dice[index];
            Dice.RemoveAt(index);  // remove used die
            return chosen;
        }
    }
}
