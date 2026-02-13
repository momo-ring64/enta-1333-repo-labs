using System;

namespace DiceGame
{
    internal class DiceRoller
    {
        private int sides;
        private Random random = new Random();

        public DiceRoller(int sides)
        {
            this.sides = sides;
        }

        public int Roll()
        {
            return random.Next(1, sides + 1);
        }
    }
}
