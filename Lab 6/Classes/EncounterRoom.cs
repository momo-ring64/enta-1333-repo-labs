using System;

namespace Lab_6.Classes
{
    public class EncounterRoom : Room
    {
        public override string RoomDescription()
        {
            return "A shadow moves... an enemy lurks nearby!";
        }

        public override void OnRoomEntered(Player player)
        {
            base.OnRoomEntered(player);
            // Battle will be handled in GameManager when this room type is detected
        }
    }
}
