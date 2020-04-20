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
            Console.WriteLine("====== 8-Puzzle ================================");

            int[,] initial = new int[,] { { 3, 8, 1 }, { 6, 2, 5 }, { 0, 4, 7 } };
            int[,] goal = new int[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };
            int limit = 10000;

            Console.WriteLine("Initial State : ");
            new EPNode(initial, 0, 0, 0, null).Print();
            Console.WriteLine("Goal State : ");
            new EPNode(goal, 0, 0, 0, null).Print();

            Console.WriteLine("Solve the 8-Puzzle within " + limit + " search(es).");
            Console.WriteLine("================================================");

            EightPuzzle puzzle = new EightPuzzle(initial, goal, limit)
            {
                DEBUG = false
            };
            EPNode result = puzzle.Solve();

            if ( result != null )
            {
                Console.WriteLine("Solved!");
                Console.WriteLine("================================================");
                Console.WriteLine("Path : ");
                EightPuzzle.PrintPath(result);
            }
            else
            {
                Console.WriteLine("Failed!");
            }
        }
    }
}
