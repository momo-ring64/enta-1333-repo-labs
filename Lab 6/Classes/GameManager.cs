using System;

namespace Lab_6.Classes
{
    public class GameManager
    {
        private Room[,] map;
        private Player player;
        private int currentX;
        private int currentY;
        private Random rand = new Random();

        public void StartGame()
        {
            Console.WriteLine("=== Dice Adventure ===");
            Console.Write("Enter your hero's name: ");
            string name = Console.ReadLine();
            player = new Player(name);

            Console.WriteLine($"\nWelcome, {player.Name}!");
            Console.WriteLine("You start your adventure with a sword, dagger, and a health potion.\n");

            InitializeMap();

            currentX = 1;
            currentY = 1;
            GameLoop();
        }

        private void InitializeMap()
        {
            map = new Room[3, 3];

            // Fill all rooms as empty by default
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    map[i, j] = new Room();
                }
            }

            // Place one TreasureRoom and one EncounterRoom randomly
            int treasureX = rand.Next(0, 3);
            int treasureY = rand.Next(0, 3);
            map[treasureX, treasureY] = new TreasureRoom();

            int encounterX, encounterY;
            do
            {
                encounterX = rand.Next(0, 3);
                encounterY = rand.Next(0, 3);
            } while (encounterX == treasureX && encounterY == treasureY);

            map[encounterX, encounterY] = new EncounterRoom();
        }

        private void GameLoop()
        {
            while (true)
            {
                Room currentRoom = map[currentX, currentY];
                Console.WriteLine($"\n?? You are in a room at position [{currentX}, {currentY}]");
                Console.WriteLine(currentRoom.RoomDescription());
                currentRoom.OnRoomEntered(player);

                // Check for encounters
                if (currentRoom is EncounterRoom)
                {
                    StartEncounter();
                }

                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Move (N/S/E/W)");
                Console.WriteLine("2. Search room");
                Console.WriteLine("3. View stats");
                Console.WriteLine("4. Use potion");
                Console.WriteLine("5. Quit");

                Console.Write("Choice: ");
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "1":
                        MovePlayer();
                        break;
                    case "2":
                        currentRoom.OnRoomSearched(player);
                        break;
                    case "3":
                        player.ShowStats();
                        break;
                    case "4":
                        player.UsePotion();
                        break;
                    case "5":
                        Console.WriteLine("Thanks for playing!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void MovePlayer()
        {
            Console.Write("Enter direction (N/S/E/W): ");
            string dir = Console.ReadLine().ToLower();

            int newX = currentX;
            int newY = currentY;

            switch (dir)
            {
                case "n": newX--; break;
                case "s": newX++; break;
                case "e": newY++; break;
                case "w": newY--; break;
                default:
                    Console.WriteLine("Invalid direction!");
                    return;
            }

            if (newX >= 0 && newX < 3 && newY >= 0 && newY < 3)
            {
                Console.WriteLine("You leave the room...");
                currentX = newX;
                currentY = newY;
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }

        private void StartEncounter()
        {
            Enemy enemy = new Enemy("Goblin", 10);

            Console.WriteLine($"A wild {enemy.Name} appears!\n");

            while (player.HitPoints > 0 && enemy.HitPoints > 0)
            {
                Console.WriteLine($"Your HP: {player.HitPoints} | {enemy.Name}'s HP: {enemy.HitPoints}");
                Console.WriteLine("Press Enter to roll your attack...");
                Console.ReadLine();

                int playerRoll = player.RollAttack();
                int enemyRoll = enemy.RollAttack();

                Console.WriteLine($"You rolled {playerRoll}. {enemy.Name} rolled {enemyRoll}.");

                if (playerRoll > enemyRoll)
                {
                    int dmg = playerRoll - enemyRoll;
                    enemy.HitPoints -= dmg;
                    Console.WriteLine($"You hit the {enemy.Name} for {dmg} damage!");
                }
                else if (enemyRoll > playerRoll)
                {
                    int dmg = enemyRoll - playerRoll;
                    player.HitPoints -= dmg;
                    Console.WriteLine($"The {enemy.Name} strikes you for {dmg} damage!");
                }
                else
                {
                    Console.WriteLine("Both of you parry each other's attacks!");
                }
            }

            if (player.HitPoints <= 0)
            {
                Console.WriteLine("You have been defeated...");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"You defeated the {enemy.Name}!");
            }
        }
    }
}
