using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD13_1333_Lab3_Tyrone.Scripts
{
    internal class GameManager
    {
        public void PlayGame()
        {
            //welcome message with name and date
            Console.WriteLine("Welcome to the Dice Game!");
            Console.WriteLine("Made by Tyrone 2025-09-17");

            //make the dieroller class and roll the dices
            DieRoller roller = new DieRoller();
            int total = roller.Roll();

            //show the total score. havent added any set score to get yet
            Console.WriteLine("Your total dice score is: " + total);

            //explain all the arithmetic thing
            Console.WriteLine("\nArithmetic Operators in C#");

            //adding is + duh
            Console.WriteLine("+: Adds numbers. Example: 6 + 7 = " + (5 + 7));
        
            //subtracting is - duh
            Console.WriteLine("-: Subtracts numbers. Example: 7 - 6 = " + (7 - 6));

            //multiplication is * 
            Console.WriteLine("*: Multiplies numbers. Example: 6 * 7 = " + (6 * 7));

            //division is the slash /
            Console.WriteLine("/: Divides numbers. Example: 7 / 6 = " + (7 / 6));

            //% = if you wanna give 6 cookies between 7 people. you cant because theres more people than cookies so you just keep all of them
            Console.WriteLine("%: Gives the remainder. Example: 6 % 7 = " + (6 % 7));

            //++ increases ONLY by 1(only...)
            int a = 6;
            a++;
            Console.WriteLine("++: Increases by 1. 6++ would be " + a);

            //-- decreases ONLY by 1(only...)
            int b = 7;
            b--;
            Console.WriteLine("--: Decreases by 1. 7-- would be " + b);

            //bye bye message
            Console.WriteLine("\nThank you for playing this game!");
        }
    }
   
}
