using System;
using System.Collections.Generic;

namespace DiceGame
{
    internal class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public List<int> Dice { get; set; }

        public Player(string name, List<int> dice)
        {
            Name = name;
            Dice = dice;
            Score = 0;
        }

        // player picks die manually
        public int ChooseDie()
        {
            Console.WriteLine("Your dice: " + string.Join(", ", Dice));
            Console.Write("Pick a die by typing its number (like 6 for d6): ");

            int chosenSides;
            while (!int.TryParse(Console.ReadLine(), out chosenSides) || !Dice.Contains(chosenSides))
            {
                Console.Write("Invalid choice. Try again: ");
            }

            Dice.Remove(chosenSides);
            return chosenSides;
        }
    }
}