using System;

namespace GD13_1333_Lab4_Tyrone.Scripts
{
    internal class Die
    {
        private int sides;
        private Random random = new Random();

        public Die(int sides)
        {
            this.sides = sides;
        }

        public int Roll()
        {
            return random.Next(1, sides + 1);
        }
    }
}
