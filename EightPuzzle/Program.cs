namespace EightPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] initial  = new int[,] { { 2, 8, 3 }, { 1, 6, 4 }, { 7, 0, 5 } };
            int[,] goal     = new int[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };

            new EPNode(initial, 2, 1, 0, null).Estimate(goal).Print()
                .MoveUp().Estimate(goal).Print()
                .MoveUp().Estimate(goal).Print()
                .MoveLeft().Estimate(goal).Print()
                .MoveDown().Estimate(goal).Print()
                .MoveRight().Estimate(goal).Print();







            //if ( new Puzzle(initial, goal).Solve() )
            //{
            //    Console.WriteLine("Solved!");
            //}
            //else
            //{
            //    Console.WriteLine("Failed!");
            //}
        }
    }
}
