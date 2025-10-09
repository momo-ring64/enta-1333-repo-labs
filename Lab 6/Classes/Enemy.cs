using System;

namespace Lab_6.Classes
{
    public class Enemy
    {
        public string Name { get; private set; }
        public int HitPoints { get; set; }
        private Random rand = new Random();

        public Enemy(string name, int hp)
        {
            Name = name;
            HitPoints = hp;
        }

        public int RollAttack()
        {
            // Random attack roll, weaker than player’s range
            return rand.Next(1, 10);
        }
    }
}
