using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] initial  = new int[,] { { 3, 8, 1 }, { 6, 2, 5 }, { 0, 4, 7 } };
            int[,] goal     = new int[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };
            int limit = 5000;

            Console.WriteLine("Solve the 8-Puzzle within " + limit + " search(es).");
            if ( new EightPuzzle(initial, goal, limit).Solve() )
            {
                Console.WriteLine("Solved!");
            }
            else
            {
                Console.WriteLine("Failed!");
            }
        }
    }
}
