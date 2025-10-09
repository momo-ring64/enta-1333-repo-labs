using System;
using System.Collections.Generic;

namespace Lab_6.Classes
{
    public class Player
    {
        public string Name { get; private set; }
        public int HitPoints { get; set; } = 20;
        public List<string> Inventory { get; private set; } = new List<string>();
        private Random rand = new Random();
        private int potions = 1;

        public Player(string name)
        {
            Name = name;
            Inventory.Add("Sword");
            Inventory.Add("Dagger");
            Inventory.Add("Health Potion");
        }

        public int RollAttack()
        {
            // Simple D&D-style attack: random between 1–12
            return rand.Next(1, 13);
        }

        public void UsePotion()
        {
            if (potions > 0)
            {
                HitPoints += 10;
                potions--;
                Console.WriteLine($"{Name} drinks a potion and restores 10 HP! You now have {HitPoints} HP.");
            }
            else
            {
                Console.WriteLine("You have no potions left!");
            }
        }

        public void AddItem(string item)
        {
            Inventory.Add(item);
            Console.WriteLine($"{item} added to your inventory!");
        }

        public void ShowStats()
        {
            Console.WriteLine($"\n{Name}'s Stats:");
            Console.WriteLine($"HP: {HitPoints}");
            Console.WriteLine("Inventory:");
            foreach (var item in Inventory)
                Console.WriteLine($" - {item}");
        }
    }
}
