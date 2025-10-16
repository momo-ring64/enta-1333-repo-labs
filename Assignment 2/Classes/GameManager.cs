using System;
using System.Linq;

namespace Assignment_2.Classes
{
    // game manager: main flow, map, movement and encounters
    public class GameManager
    {
        private Room[,] map;
        private Player player;
        private int currentX;
        private int currentY;
        private Random rand = new Random();

        // start the game
        public void StartGame()
        {
            Console.WriteLine("Welcome to");
            Console.WriteLine("    _        _   _               ____                      ");
            Console.WriteLine("   / \\   ___| |_(_) ___  _ __   / ___| _   _ _ __ __ _  ___ ");
            Console.WriteLine("  / _ \\ / __| __| |/ _ \\| '_ \\  \\___ \\| | | | '__/ _` |/ _ \\");
            Console.WriteLine(" / ___ \\ (__| |_| | (_) | | | |  ___) | |_| | | | (_| |  __/");
            Console.WriteLine("/_/   \\_\\___|\\__|_|\\___/|_| |_| |____/ \\__,_|_|  \\__, |\\___|");
            Console.WriteLine("                                                 |___/      ");
            Console.WriteLine("--The Dungeon Crawling Adventure Game!--");

            Console.Write("\nenter your hero's name: ");
            string name = Console.ReadLine();
            player = new Player(name);

            Console.WriteLine($"\nwelcome, {player.Name}!");
            Console.WriteLine("you start with a sword, dagger and a health potion.\n");

            InitializeMap();

            currentX = 1;
            currentY = 1;
            GameLoop();
        }

        // build a 3x3 map with one treasure, one encounter, one normal, rest empty
        private void InitializeMap()
        {
            int rows = 3, cols = 3;
            map = new Room[rows, cols];

            // fill with empty rooms
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    map[x, y] = new EmptyRoom();

            // make a list of coordinates and shuffle
            var coords = new (int x, int y)[rows * cols];
            int idx = 0;
            for (int x = 0; x < rows; x++)
                for (int y = 0; y < cols; y++)
                    coords[idx++] = (x, y);

            // fisher-yates shuffle
            for (int i = coords.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                var tmp = coords[i];
                coords[i] = coords[j];
                coords[j] = tmp;
            }

            // assign special rooms
            var t = coords[0];
            map[t.x, t.y] = new TreasureRoom();
            var e = coords[1];
            map[e.x, e.y] = new EncounterRoom();
            var n = coords[2];
            map[n.x, n.y] = new NormalRoom();

            // player starts in center (1,1) regardless of special placements
        }

        // main input loop
        private void GameLoop()
        {
            bool running = true;

            while (running)
            {
                Room current = map[currentX, currentY];
                Console.WriteLine($"\nyou are in room [{currentX},{currentY}]");
                Console.WriteLine(current.RoomDescription());
                current.OnRoomEntered(player);

                // if encounter room - start battle
                if (current is EncounterRoom)
                {
                    StartEncounter();
                    // if player died in encounter, break
                    if (player.HitPoints <= 0) return;
                }

                Console.WriteLine("\nwhat would you like to do?");
                Console.WriteLine("1. move (n/s/e/w)");
                Console.WriteLine("2. search");
                Console.WriteLine("3. inventory");
                Console.WriteLine("4. stats");
                Console.WriteLine("5. use potion");
                Console.WriteLine("6. quit");
                Console.Write("choice: ");
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
                        var potions = player.InventoryItems.OfType<Consumable>().ToArray();
                        if (potions.Length == 0)
                        {
                            Console.WriteLine("you have no potions.");
                        }
                        else
                        {
                            Console.WriteLine("choose a potion to use:");
                            for (int i = 0; i < potions.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {potions[i].Name} ({potions[i].DiceCount}d{potions[i].DiceSides} heal)");
                            }

                            Console.Write("potion number: ");
                            string sel = Console.ReadLine().Trim();
                            if (int.TryParse(sel, out int pi) && pi >= 1 && pi <= potions.Length)
                            {
                                var potion = potions[pi - 1];
                                int healed = potion.RollHeal();
                                player.ReceiveHeal(healed);
                                player.DropItem(potion);
                            }
                            else
                            {
                                Console.WriteLine("invalid selection.");
                            }
                        }
                        break;

                    case "6":
                    case "quit":
                        Console.WriteLine("thanks for playing!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("invalid choice.");
                        break;
                }
            }
        }

        // movement with bounds checking
        private void MovePlayer()
        {
            Console.Write("direction (n/s/e/w): ");
            string d = Console.ReadLine().Trim().ToLower();

            int nx = currentX, ny = currentY;
            switch (d)
            {
                case "n": nx--; break;
                case "s": nx++; break;
                case "e": ny++; break;
                case "w": ny--; break;
                default:
                    Console.WriteLine("invalid direction.");
                    return;
            }

            if (nx < 0 || ny < 0 || nx >= map.GetLength(0) || ny >= map.GetLength(1))
            {
                Console.WriteLine("you can't go that way.");
                return;
            }

            // call exit hook on current room
            map[currentX, currentY].OnRoomExit();

            currentX = nx;
            currentY = ny;
        }

        // encounter: turn-based use of weapons/consumables
        private void StartEncounter()
        {
            Enemy enemy = new Enemy("goblin", rand.Next(8, 14));
            Console.WriteLine($"\na wild {enemy.Name} appears! ({enemy.HitPoints} hp)");

            // loop until someone dies
            while (player.HitPoints > 0 && enemy.HitPoints > 0)
            {
                Console.WriteLine($"\nplayer hp: {player.HitPoints} | {enemy.Name} hp: {enemy.HitPoints}");
                Console.WriteLine("actions: 1.attack  2.use consumable  3.run");
                Console.Write("choice: ");
                string choice = Console.ReadLine().Trim();

                if (choice == "1" || choice.ToLower() == "attack")
                {
                    // list weapons
                    var weapons = player.InventoryItems.OfType<Weapon>().ToArray();
                    if (weapons.Length == 0)
                    {
                        // no weapon: basic attack (d6)
                        int dmg = RollDice(6);
                        Console.WriteLine($"you punch for {dmg} damage.");
                        enemy.HitPoints -= dmg;
                    }
                    else
                    {
                        Console.WriteLine("choose a weapon:");
                        for (int i = 0; i < weapons.Length; i++)
                            Console.WriteLine($"{i + 1}. {weapons[i].Name} ({weapons[i].DiceCount}d{weapons[i].DiceSides})");

                        Console.Write("weapon number: ");
                        string sel = Console.ReadLine().Trim();
                        if (int.TryParse(sel, out int wi) && wi >= 1 && wi <= weapons.Length)
                        {
                            var weapon = weapons[wi - 1];
                            int dmg = weapon.RollDamage();
                            Console.WriteLine($"you attack with {weapon.Name} for {dmg} damage.");
                            enemy.HitPoints -= dmg;
                        }
                        else
                        {
                            Console.WriteLine("invalid selection, you fumble and miss.");
                        }
                    }
                }
                else if (choice == "2" || choice.ToLower().Contains("use"))
                {
                    // list consumables
                    var cons = player.InventoryItems.OfType<Consumable>().ToArray();
                    if (cons.Length == 0)
                    {
                        Console.WriteLine("you have no consumables.");
                    }
                    else
                    {
                        Console.WriteLine("choose a consumable:");
                        for (int i = 0; i < cons.Length; i++)
                            Console.WriteLine($"{i + 1}. {cons[i].Name} ({cons[i].DiceCount}d{cons[i].DiceSides} heal)");

                        Console.Write("consumable number: ");
                        string sel = Console.ReadLine().Trim();
                        if (int.TryParse(sel, out int ci) && ci >= 1 && ci <= cons.Length)
                        {
                            var con = cons[ci - 1];
                            int healed = con.RollHeal();
                            player.ReceiveHeal(healed);
                            // consumable used -> remove from inventory
                            player.DropItem(con);
                        }
                        else
                        {
                            Console.WriteLine("invalid selection.");
                        }
                    }
                }
                else if (choice == "3" || choice.ToLower() == "run")
                {
                    // simple 50% run chance
                    if (rand.Next(2) == 0)
                    {
                        Console.WriteLine("you manage to run away!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("you fail to escape.");
                    }
                }
                else
                {
                    Console.WriteLine("invalid action.");
                }

                // enemy turn if still alive
                if (enemy.HitPoints > 0)
                {
                    int ed = enemy.RollAttack();
                    Console.WriteLine($"the {enemy.Name} attacks for {ed} damage.");
                    player.ReceiveDamage(ed);
                }
            }

            if (player.HitPoints > 0 && enemy.HitPoints <= 0)
            {
                Console.WriteLine($"you defeated the {enemy.Name}!");
                // small chance to drop an item
                if (rand.Next(100) < 40)
                {
                    Item loot = RandomLoot();
                    player.AddItem(loot);
                }
            }
            else if (player.HitPoints <= 0)
            {
                Console.WriteLine("you have been defeated...");
            }
        }

        // helper to roll dice
        private int RollDice(int sides)
        {
            int total = 0;
            for (int i = 0; i < 1; i++)
                total += rand.Next(1, sides + 1);
            return total;
        }

        // treasure room and enemy drop pool
        private Item RandomLoot()
        {
            // simple pool
            Item[] pool = new Item[]
            {
                new Weapon("short dagger", 1, 4),
                new Weapon("longsword", 1, 8),
                new Weapon("steel halberd", 1, 10),
                new Weapon("steel greatsword", 1, 10),
                new Consumable("small potion", 1, 4),
                new Consumable("large potion", 2, 6)
            };
            return pool[rand.Next(pool.Length)];
        }
    }
}
