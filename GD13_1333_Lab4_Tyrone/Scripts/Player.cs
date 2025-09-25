namespace GD13_1333_Lab4_Tyrone.Scripts
{
    internal class Player
    {
        public string Name;
        public int CurrentRoll;
        public int Score;

        public Player(string name)
        {
            //all the variables for the player
            Name = name;
            CurrentRoll = 0;
            Score = 0;
        }
    }
}
