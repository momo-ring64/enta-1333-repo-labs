using System;

namespace Lab_6.Classes
{
    public class TreasureRoom : Room
    {
        private bool searched = false;

        public override string RoomDescription()
        {
            return "You step into a glittering treasure room filled with shining objects.";
        }

        public override void OnRoomSearched(Player player)
        {
            if (!searched)
            {
                string[] items = { "Health Potion", "Magic Ring", "Iron Shield", "Steel Sword" };
                Random rand = new Random();
                string found = items[rand.Next(items.Length)];
                player.AddItem(found);
                searched = true;
            }
            else
            {
                Console.WriteLine("You already searched this room. Nothing left.");
            }
        }
    }
}
