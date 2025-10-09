using System;

namespace Lab_6.Classes
{
    public class EmptyRoom : Room
    {
        public override string RoomDescription()
        {
            return "An empty, dusty room. Nothing of interest here.";
        }

        public override void OnRoomEntered(Player player)
        {
            Console.WriteLine("You enter an empty room. It’s eerily quiet...");
        }

        public override void OnRoomSearched(Player player)
        {
            Console.WriteLine("You search the room, but find nothing.");
        }

        public override void OnRoomExit()
        {
            Console.WriteLine("You leave the empty room behind.");
        }
    }
}
