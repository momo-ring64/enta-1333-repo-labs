using System;

namespace Lab_6.Classes
{
    public class NormalRoom : Room
    {
        public override string RoomDescription()
        {
            string[] descs =
            {
                "A quiet empty chamber with stone walls.",
                "Dust covers the floor here.",
                "You see old markings on the wall.",
                "It’s eerily silent in this room."
            };
            Random rand = new Random();
            return descs[rand.Next(descs.Length)];
        }
    }
}
