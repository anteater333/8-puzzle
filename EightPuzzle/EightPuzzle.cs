using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue; // Ref. https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp | MIT License 

namespace EightPuzzle
{
    class EightPuzzle
    {
        private SimplePriorityQueue<EPNode> _OPEN;
        private List<EPNode> _CLOSED;

        private int[,] _initial, _goal;
        private int[] _blank;
        
        private int _LIMIT;

        /// <summary>
        /// 8 퍼즐을 해결하는데 필요한 정보를 담고 있는 객체를 생성합니다.
        /// </summary>
        /// <param name="initial">초기 상태</param>
        /// <param name="goal">목표 상태</param>
        /// <param name="limit">탐색 한도</param>
        public EightPuzzle(int[,] initial, int[,] goal, int limit)
        {
            this._initial = initial; this._goal = goal;
            this._OPEN = new SimplePriorityQueue<EPNode>();
            this._CLOSED = new List<EPNode>();
            this._LIMIT = limit;

            Console.WriteLine("Checking initial puzzle state...");
            this._blank = CheckBlank(initial);
            if (this._blank[0] == -1)
                Console.WriteLine("ERR :: Puzzle should have only one blank!!");
            else
                Console.WriteLine("OK");
            Console.WriteLine("Checking goal puzzle state...");
            if (CheckBlank(goal)[0] == -1)
                Console.WriteLine("ERR :: Puzzle should have only one blank!!");
            else
                Console.WriteLine("OK");
            Console.WriteLine("========================================");
        }

        /// <summary>
        /// A* 알고리즘을 사용한 8 퍼즐 문제 풀이를 시작합니다.
        /// </summary>
        /// <returns></returns>
        public bool Solve()
        {
            int count = 0;

            EPNode rootNode = new EPNode(_initial, _blank[0], _blank[1], 0, null).Estimate(_goal);
            EPNode goalNode = new EPNode(_goal, 0, 0, 0, null); // for checking

            _OPEN.Enqueue(rootNode, rootNode.Distance + rootNode.Heuristic);

            while (_OPEN.Count > 0 && count < _LIMIT)
            {
                EPNode bestNode = _OPEN.Dequeue();

                Console.WriteLine("Node #" + ++count);
                bestNode.Print();

                if (bestNode.Equals(goalNode)) // Reached the goal!
                {
                    return true;
                }

                EPNode[] moved = new EPNode[4];
                moved[0] = bestNode.MoveUp();
                moved[1] = bestNode.MoveDown();
                moved[2] = bestNode.MoveLeft();
                moved[3] = bestNode.MoveRight();
                for (int i = 0; i < 4; i++)
                {
                    if (moved[i] == null) continue; // Hit the wall!

                    _OPEN.Enqueue(moved[i].Estimate(_goal), moved[i].Distance + moved[i].Heuristic);
                }
            }

            return false;
        }

        /// <summary>
        /// 퍼즐의 공백칸의 위치를 파악합니다.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>길이가 2인 정수 배열 입니다. [0] : x좌표, [1] : y좌표</returns>
        private int[] CheckBlank(int[,] matrix)
        {
            int x = -1, y = -1;
            bool hasABlank = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == 0 && !hasABlank)
                    {
                        x = i; y = j;
                        hasABlank = true;
                    }
                    else if (matrix[i, j] == 0 && hasABlank)
                    {
                        hasABlank = false;
                        break;
                    }
                }
            }
            if (!hasABlank)
            {
                return new int[] { -1, -1 };
            }
            else
            {
                return new int[] { x, y };
            }
        }
    }
}
