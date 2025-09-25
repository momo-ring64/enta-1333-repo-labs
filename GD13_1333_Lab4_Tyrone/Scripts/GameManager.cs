using System;

namespace GD13_1333_Lab4_Tyrone.Scripts
{
    internal class GameManager
    {
        private Player player;
        private Player computer;
        private Random random = new Random();

        public void PlayGame()
        {
            // the intro lines.
            // /n means new line. a space after that
            Console.WriteLine("Welcome to the Dice Battle!");
            Console.WriteLine("Made by Tyrone 2025-09-24\n");

            // dsk the players name
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            player = new Player(name);
            computer = new Player("Computer");

            // decide who goes first with using random for coin flip
            int coinFlip = random.Next(2); // 0 or 1
            Console.WriteLine("\nFlipping a coin to see who goes first...");
            bool playerFirst = (coinFlip == 0);

            if (playerFirst)
            {
                Console.WriteLine($"{player.Name} goes first!");
                PlayerTurn(player);
                ComputerTurn();
            }
            else
            {
                Console.WriteLine("Computer goes first!");
                ComputerTurn();
                PlayerTurn(player);
            }

            // show what was rolled
            Console.WriteLine($"\n{player.Name} rolled {player.CurrentRoll}");
            Console.WriteLine($"Computer rolled {computer.CurrentRoll}");

            // Pick winner
            if (player.CurrentRoll > computer.CurrentRoll)
            {
                Console.WriteLine($"{player.Name} wins this round!");
                player.Score++;
            }
            else if (computer.CurrentRoll > player.CurrentRoll)
            {
                Console.WriteLine("Computer wins this round!");
                computer.Score++;
            }
            else
            {
                Console.WriteLine("It’s a tie!");
            }

            // aftermath of round 
            Console.WriteLine("\n--- Round Summary ---");
            Console.WriteLine($"{player.Name}: {player.Score} points");
            Console.WriteLine($"Computer: {computer.Score} points");

            // bye bye message
            Console.WriteLine("\nThanks for playing!");
        }

        private void PlayerTurn(Player p)
        {
            Console.WriteLine($"\n{p.Name}, choose a die to roll (d6, d8, d12, d20):");
            string choice = Console.ReadLine().ToLower(); //ToLower means just uncapitalize it to fit the readings

            int sides;
            if (choice == "d6") sides = 6;
            else if (choice == "d8") sides = 8;
            else if (choice == "d12") sides = 12;
            else if (choice == "d20") sides = 20;
            else sides = 6; // just roll this if player is dumb and doesnt type any other ones

            Die die = new Die(sides);
            p.CurrentRoll = die.Roll();
            Console.WriteLine($"{p.Name} rolled {p.CurrentRoll} on a d{sides}");
        }


        //computers turn to roll
        private void ComputerTurn()
        {
            Console.WriteLine("\nComputer’s turn...");
            int[] diceOptions = { 6, 8, 12, 20 };
            int sides = diceOptions[random.Next(diceOptions.Length)];

            Die die = new Die(sides);
            computer.CurrentRoll = die.Roll();
            Console.WriteLine($"Computer rolled {computer.CurrentRoll} on a d{sides}");
        }
    }
}
