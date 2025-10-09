using Lab_6.Classes;

public class Room
{
    protected bool visited = false;
    protected bool searched = false;

    public virtual string RoomDescription()
    {
        if (!visited)
            return "Dust covers the floor here.";
        else
            return "You've been here before.";
    }

    public virtual void OnRoomEntered(Player player)
    {
        visited = true;
    }

    public virtual void OnRoomSearched(Player player)
    {
        if (!searched)
        {
            Console.WriteLine("You look around, but find nothing special.");
            searched = true;
        }
        else
        {
            Console.WriteLine("You've already searched this room.");
        }
    }

    // Add this to allow overriding in subclasses
    public virtual void OnRoomExit()
    {
        Console.WriteLine("You leave the room.");
    }
}
