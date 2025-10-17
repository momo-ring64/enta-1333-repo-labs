using System;
using System.Linq;

namespace Assignment_2.Classes
{
    public class GameManager
    {
        private Room[,] map;
        private Player player;
        private int currentX;
        private int currentY;
        private Random rand = new Random();
        private bool[,] visitedRooms;

        public void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Welcome to");
            Console.WriteLine("    _        _   _               ____                      ");
            Console.WriteLine("   / \\   ___| |_(_) ___  _ __   / ___| _   _ _ __ __ _  ___ ");
            Console.WriteLine("  / _ \\ / __| __| |/ _ \\| '_ \\  \\___ \\| | | | '__/ _` |/ _ \\");
            Console.WriteLine(" / ___ \\ (__| |_| | (_) | | | |  ___) | |_| | | | (_| |  __/");
            Console.WriteLine("/_/   \\_\\___|\\__|_|\\___/|_| |_| |____/ \\__,_|_|  \\__, |\\___|");
            Console.WriteLine("                                                 |___/      ");
            Console.WriteLine("-- The Dungeon Crawling Adventure Game --");
            Console.WriteLine("Traverse through the dungeon to fight goblins and loot for items!\n");

            Console.Write("Enter your hero's name: ");
            string name = Console.ReadLine();
            player = new Player(name);

            Console.WriteLine($"\nWelcome, {player.Name}!");
            Console.WriteLine("You start with a sword, dagger, and a health potion.\n");

            InitializeMap();

            currentX = 1;
            currentY = 1;
            GameLoop();
        }

        // Initialize 4x4 dungeon with random room types
        private void InitializeMap()
        {
            int rows = 4, cols = 4;
            map = new Room[rows, cols];
            visitedRooms = new bool[rows, cols];

            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    map[x, y] = new EmptyRoom();

            var coords = new (int x, int y)[rows * cols];
            int idx = 0;
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    coords[idx++] = (x, y);

            // Shuffle
            for (int i = coords.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (coords[i], coords[j]) = (coords[j], coords[i]);
            }

            // Assign 3 treasures and 3 encounters
            for (int i = 0; i < 3; i++)
            {
                var c = coords[i];
                map[c.x, c.y] = new TreasureRoom();
            }
            for (int i = 3; i < 6; i++)
            {
                var c = coords[i];
                map[c.x, c.y] = new EncounterRoom();
            }
        }

        private void GameLoop()
        {
            bool running = true;

            while (running)
            {
                Room current = map[currentX, currentY];

                Console.WriteLine($"\nYou are in room [{currentX},{currentY}]");
                Console.WriteLine(current.RoomDescription());

                if (!visitedRooms[currentX, currentY])
                {
                    visitedRooms[currentX, currentY] = true;
                    current.OnRoomEntered(player);

                    // start encounter only first time entering
                    if (current is EncounterRoom)
                    {
                        StartEncounter();
                        if (player.HitPoints <= 0)
                        {
                            running = false;
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You've already been here before.");
                }

                // win condition check
                if (AllRoomsVisited())
                {
                    Console.WriteLine("\nYou’ve explored every room in the dungeon!");
                    Console.WriteLine("Congratulations, brave adventurer — you’ve conquered the dungeon!");
                    running = false;
                    break;
                }

                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1. Move (n/s/e/w)");
                Console.WriteLine("2. Search");
                Console.WriteLine("3. Inventory");
                Console.WriteLine("4. Stats");
                Console.WriteLine("5. Use Potion");
                Console.WriteLine("6. Quit");
                Console.Write("Choice: ");
                string input = Console.ReadLine().Trim().ToLower();

                switch (input)
                {
                    case "1":
                    case "move":
                        MovePlayer();
                        break;
                    case "2":
                    case "search":
                        current.OnRoomSearched(player);
                        break;
                    case "3":
                    case "inventory":
                        player.ShowInventory();
                        break;
                    case "4":
                    case "stats":
                        player.ShowStats();
                        break;
                    case "5":
                    case "use potion":
                    case "potion":
                        UsePotionMenu();
                        break;
                    case "6":
                    case "quit":
                        Console.WriteLine("You decide to leave the dungeon for now...");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

            TryAgainMenu();
        }

        private bool AllRoomsVisited()
        {
            foreach (bool v in visitedRooms)
                if (!v) return false;
            return true;
        }

        //try again menu when dead, win, or quit
        private void TryAgainMenu()
        {
            Console.WriteLine("\nWould you like to play again? (y/n)");
            string again = Console.ReadLine().Trim().ToLower();
            if (again == "y" || again == "yes")
                StartGame();
            else
                Console.WriteLine("Thanks for playing! Goodbye!");
        }

        private void UsePotionMenu()
        {
            var potions = player.InventoryItems.OfType<Consumable>().ToArray();
            if (potions.Length == 0)
            {
                Console.WriteLine("You have no potions.");
                return;
            }

            Console.WriteLine("Choose a potion to use:");
            for (int i = 0; i < potions.Length; i++)
                Console.WriteLine($"{i + 1}. {potions[i].Name} ({potions[i].DiceCount}d{potions[i].DiceSides} heal)");

            Console.Write("Potion number: ");
            if (int.TryParse(Console.ReadLine(), out int pi) && pi >= 1 && pi <= potions.Length)
            {
                var potion = potions[pi - 1];
                int healed = potion.RollHeal();
                player.ReceiveHeal(healed);
                player.DropItem(potion);
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void MovePlayer()
        {
            Console.Write("Direction (n/s/e/w): ");
            string d = Console.ReadLine().Trim().ToLower();

            int nx = currentX, ny = currentY;
            switch (d)
            {
                case "n": nx--; break;
                case "s": nx++; break;
                case "e": ny++; break;
                case "w": ny--; break;
                default:
                    Console.WriteLine("Invalid direction.");
                    return;
            }

            if (nx < 0 || ny < 0 || nx >= map.GetLength(0) || ny >= map.GetLength(1))
            {
                Console.WriteLine("You can't go that way.");
                return;
            }

            map[currentX, currentY].OnRoomExit();

            currentX = nx;
            currentY = ny;
        }

        private void StartEncounter()
        {
            Enemy enemy = new Enemy("Goblin", rand.Next(8, 14));
            Console.WriteLine($"\nA wild {enemy.Name} appears! ({enemy.HitPoints} HP)");

            while (player.HitPoints > 0 && enemy.HitPoints > 0)
            {
                Console.WriteLine($"\nPlayer HP: {player.HitPoints} | {enemy.Name} HP: {enemy.HitPoints}");
                Console.WriteLine("Actions: 1.Attack  2.Use consumable  3.Run");
                Console.Write("Choice: ");
                string choice = Console.ReadLine().Trim().ToLower();

                if (choice == "1" || choice == "attack")
                {
                    PlayerAttack(enemy);
                }
                else if (choice == "2" || choice.Contains("use"))
                {
                    UsePotionMenu();
                }
                else if (choice == "3" || choice == "run")
                {
                    if (rand.Next(2) == 0)
                    {
                        Console.WriteLine("You manage to run away!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You fail to escape.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid action.");
                }

                if (enemy.HitPoints > 0)
                {
                    int dmg = enemy.RollAttack();
                    Console.WriteLine($"The {enemy.Name} attacks for {dmg} damage!");
                    player.ReceiveDamage(dmg);
                }
            }

            if (player.HitPoints > 0 && enemy.HitPoints <= 0)
            {
                Console.WriteLine($"You defeated the {enemy.Name}!");
                if (rand.Next(100) < 40)
                {
                    Item loot = RandomLoot();
                    player.AddItem(loot);
                }
            }
            else if (player.HitPoints <= 0)
            {
                Console.WriteLine("You have been defeated...");
            }
        }

        private void PlayerAttack(Enemy enemy)
        {
            var weapons = player.InventoryItems.OfType<Weapon>().ToArray();
            if (weapons.Length == 0)
            {
                int dmg = RollDice(6);
                Console.WriteLine($"You punch for {dmg} damage.");
                enemy.HitPoints -= dmg;
            }
            else
            {
                Console.WriteLine("Choose a weapon:");
                for (int i = 0; i < weapons.Length; i++)
                    Console.WriteLine($"{i + 1}. {weapons[i].Name} ({weapons[i].DiceCount}d{weapons[i].DiceSides})");

                Console.Write("Weapon number: ");
                if (int.TryParse(Console.ReadLine(), out int wi) && wi >= 1 && wi <= weapons.Length)
                {
                    var weapon = weapons[wi - 1];
                    int dmg = weapon.RollDamage();
                    Console.WriteLine($"You attack with {weapon.Name} for {dmg} damage.");
                    enemy.HitPoints -= dmg;
                }
                else
                {
                    Console.WriteLine("Invalid selection, you fumble and miss.");
                }
            }
        }

        private int RollDice(int sides) => rand.Next(1, sides + 1);

        private Item RandomLoot()
        {
            Item[] pool = new Item[]
            {
                new Weapon("Short Dagger", 1, 4),
                new Weapon("Longsword", 1, 8),
                new Weapon("Steel Halberd", 1, 10),
                new Weapon("Steel Greatsword", 1, 10),
                new Consumable("Small Potion", 1, 4),
                new Consumable("Large Potion", 2, 6)
            };
            return pool[rand.Next(pool.Length)];
        }
    }
}
