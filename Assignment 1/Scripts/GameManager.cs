using DiceGame;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace DiceGame
{
    internal class GameManager
    {
        private Player player;
        private ComputerPlayer computer;
        private Random rng = new Random();

        public void Play()
        {
            Console.WriteLine("*********************************");
            Console.WriteLine(" Welcome to the Dice Battle of DOOM ");
            Console.WriteLine("*********************************");
            Console.WriteLine("Made by Tyrone – 2025-10-01\n");

            // ask for player name once
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            bool keepPlaying = true;

            while (keepPlaying)
            {
                // setup dice for both players each new game
                Console.WriteLine($"\nHi {playerName}, let’s set up your dice!");
                Console.WriteLine("Enter dice sides separated by spaces (e.g., 6 8 12 20): ");
                string[] input = Console.ReadLine().Split(' ');

                List<int> diceSetup = new List<int>();
                foreach (string s in input)
                {
                    if (int.TryParse(s, out int sides))
                    {
                        diceSetup.Add(sides);
                    }
                }

                if (diceSetup.Count == 0)
                {
                    // fallback if user typed nothing valid
                    diceSetup.AddRange(new int[] { 6, 8, 12, 20 });
                }

                player = new Player(playerName, new List<int>(diceSetup));
                computer = new ComputerPlayer("Computer", new List<int>(diceSetup));

                Console.WriteLine($"\nAlright {player.Name}, here are your dice: {string.Join(", ", player.Dice)}");
                Console.WriteLine("The computer will use the same dice.\n");

                Console.WriteLine("Rules: Each round we both pick a die and roll it. Higher roll gets 1 point.\n");

                // decide who goes first
                bool playerFirst = rng.Next(2) == 0;  // 50/50 chance
                Console.WriteLine(playerFirst
                    ? $"{player.Name} will go first!\n"
                    : $"{computer.Name} will go first!\n");


                // the round loop
                int round = 1;
                while (player.Dice.Count > 0 && computer.Dice.Count > 0)
                {
                    Console.WriteLine($"--- Round {round} ---");

                    int chosenSides, playerRoll, compSides, compRoll;

                    if (playerFirst)
                    {
                        // if player chooses first
                        chosenSides = player.ChooseDie();
                        playerRoll = rng.Next(1, chosenSides + 1);

                        compSides = computer.ChooseDie();
                        compRoll = rng.Next(1, compSides + 1);
                    }
                    else
                    {
                        // if computer chooses first
                        compSides = computer.ChooseDie();
                        compRoll = rng.Next(1, compSides + 1);

                        chosenSides = player.ChooseDie();
                        playerRoll = rng.Next(1, chosenSides + 1);
                    }

                    Console.WriteLine($"{player.Name} rolled a d{chosenSides} and got {playerRoll}");
                    Console.WriteLine($"{computer.Name} rolled a d{compSides} and got {compRoll}");

                    if (playerRoll > compRoll)
                    {
                        player.Score++;
                        Console.WriteLine($"{player.Name} wins the round!\n");
                    }
                    else if (compRoll > playerRoll)
                    {
                        computer.Score++;
                        Console.WriteLine($"{computer.Name} wins the round!\n");
                    }
                    else
                    {
                        Console.WriteLine("It’s a tie! No points.\n");
                    }

                    round++;
                }


                // the summary of the game
                Console.WriteLine("=== Game Over ===");
                Console.WriteLine($"{player.Name} scored {player.Score}");
                Console.WriteLine($"{computer.Name} scored {computer.Score}");

                if (player.Score > computer.Score)
                    Console.WriteLine($"{player.Name} wins the game!");
                else if (computer.Score > player.Score)
                    Console.WriteLine($"{computer.Name} wins the game!");
                else
                    Console.WriteLine("It’s a tie overall!");

                // ask the player to play again
                Console.Write("\nDo you want to play again? (yes/no): ");
                string answer = Console.ReadLine().ToLower();

                if (answer != "yes" && answer != "y")
                {
                    keepPlaying = false;
                    Console.WriteLine("\nThanks for playing!");
                }
            }
        }
    }
}