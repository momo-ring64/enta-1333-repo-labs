using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD13_1333_Lab3_Tyrone.Scripts
{
    internal class DieRoller
    {
        //using the random instance for the dice
        private Random random = new Random();

        public int Roll()
        {
            // roll 4 different dice
            int d6 = random.Next(1, 7);   ///has to be one plus because it starts at 0  so it wouldnt be 1-6 for example
            int d8 = random.Next(1, 9);   
            int d12 = random.Next(1, 13);  
            int d20 = random.Next(1, 21);  

            // print all the randoms that were rolled
            Console.WriteLine("\nYou rolled:");
            Console.WriteLine("d6 = " + d6);
            Console.WriteLine("d8 = " + d8);
            Console.WriteLine("d12 = " + d12);
            Console.WriteLine("d20 = " + d20);

            // return the total dice 
            return d6 + d8 + d12 + d20;
        }
    }
}
