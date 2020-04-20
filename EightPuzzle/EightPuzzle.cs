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
        public bool DEBUG { get; set; }

        private SimplePriorityQueue<EPNode> _OPEN;
        private List<EPNode> _CLOSED;

        private int[,] _initial, _goal;
        private int[] _blank;
        
        private int _LIMIT;

        private bool _solvability;

        /// <summary>
        /// 노드로부터 루트 까지의 경로를 콘솔 화면에 출력합니다.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static public int PrintPath(EPNode node)
        {
            if (node == null)
                return -1;

            int num = PrintPath(node.Parent) + 1;
            Console.WriteLine("Depth " + num);
            node.Print();
            return num;
        }

        /// <summary>
        /// 8 퍼즐을 해결하는데 필요한 정보를 담고 있는 객체를 생성합니다.
        /// </summary>
        /// <param name="initial">초기 상태</param>
        /// <param name="goal">목표 상태</param>
        /// <param name="limit">탐색 한도</param>
        public EightPuzzle(int[,] initial, int[,] goal, int limit)
        {
            this.DEBUG = false;

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
            Console.WriteLine("Checking the puzzle's solvability...");
            this._solvability = CheckSolvable();
            if (this._solvability)
                Console.WriteLine("OK");
            else
                Console.WriteLine("The goal state is unreachable!!");
            Console.WriteLine("================================================");
        }

        /// <summary>
        /// A* 알고리즘을 사용한 8 퍼즐 문제 풀이를 시작합니다.
        /// </summary>
        /// <returns></returns>
        public EPNode Solve()
        {
            if (!_solvability) return null;

            Console.WriteLine("Start searching.");

            int count = 0;

            EPNode rootNode = new EPNode(_initial, _blank[0], _blank[1], 0, null).Estimate(_goal);
            EPNode goalNode = new EPNode(_goal, 0, 0, 0, null); // for checking

            _OPEN.Enqueue(rootNode, rootNode.Distance + rootNode.Heuristic);

            while (_OPEN.Count > 0 && count < _LIMIT)
            {
                count++;
                // 예상 비용이 가장 낮은 노드 선택
                EPNode bestNode = _OPEN.Dequeue();

                // Console창 출력
                if (DEBUG)
                {
                    Console.WriteLine("Node #" + count);
                    bestNode.Print();
                }
                else
                {
                    Console.Write("Search count : " + count + " ");
                    for (int i = 0; i < (count / 200) % 10; i++)
                        Console.Write(".");
                }

                // CLOSED 큐에 추가
                _CLOSED.Add(bestNode);

                // 목표 도달
                if (bestNode.Equals(goalNode)) // Reached the goal!
                {
                    if (!DEBUG)
                        Console.WriteLine("...");
                    return bestNode;
                }

                // 자식 노드 생성 (상, 하, 좌, 우 이동)
                EPNode[] movedNode = new EPNode[4];
                movedNode[0] = bestNode.MoveUp();
                movedNode[1] = bestNode.MoveDown();
                movedNode[2] = bestNode.MoveLeft();
                movedNode[3] = bestNode.MoveRight();

                // 각 자식 노드들에 대해 다음 연산을 수행
                for (int i = 0; i < 4; i++)
                {
                    // 해당 방향으로는 이동이 불가능한 경우. 해당 노드 무시.
                    if (movedNode[i] == null) continue; // Hit the wall!

                    // OPEN, CLOSED 큐에서 해당 노드가 존재하는지 검색
                    EPNode nodeInOpen = Find(_OPEN, movedNode[i]);
                    EPNode nodeInClosed = Find(_CLOSED, movedNode[i]);

                    if (nodeInOpen == null && nodeInClosed == null)   // OPEN, CLOSED 모두에 해당 노드가 존재하지 않는 경우
                    {
                        // 자식 노드의 추정값 계산
                        movedNode[i].Estimate(_goal);
                        _OPEN.Enqueue(movedNode[i], movedNode[i].Distance + movedNode[i].Heuristic);
                    }
                    else if (nodeInOpen != null)                      // OPEN 큐에 해당 노드가 존재할 경우
                    {                                                    // 만약 기존 노드보다 새로 생성한 자식 노드가 더 효율적일 때,
                        if (nodeInOpen.Distance > movedNode[i].Distance) // ( == Distance가 더 작을 때)
                        {
                            // 큐에 넣기 전 자식 노드의 추정값 계산(추정값은 큐에 이미 존재하는 노드와 같음)
                            movedNode[i].Estimate(_goal);
                            nodeInOpen = movedNode[i];                   // 새로 생성한 자식 노드로 기존 노드를 대체
                        }
                        else                                             // 그렇지 않으면,
                            continue;                                    // 그냥 무시하고 진행
                    }
                    else if (nodeInClosed != null)                    // OPEN 큐에는 없지만 CLOSED 큐에 해당 노드가 존재할 경우
                    {                                                    // 더 좋은 path가 나올 가능성이 없으므로 패스
                        continue;
                    }
                }

                if (!DEBUG)
                    Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
            }

            if (!DEBUG)
                Console.WriteLine("...");

            return null;
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

        /// <summary>
        /// 퍼즐의 해결 가능 여부를 확인합니다.
        /// </summary>
        /// <returns></returns>
        private bool CheckSolvable()
        {
            int[] initial = new int[9];
            int[] goal = new int[9];

            int index = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    initial[index] = _initial[i, j];
                    goal[index] = _goal[i, j];
                    index++;
                }
            }

            int initialParity = 0; int goalParity = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = i + 1; j < 9; j++)
                {
                    if (initial[i] > initial[j] && initial[j] != 0) initialParity++;
                    if (goal[i] > goal[j] && goal[j] != 0) goalParity++;
                }
            }

            return (initialParity % 2) == (goalParity % 2);
        }

        /// <summary>
        /// 큐에서 해당 노드가 존재하는지 찾아 해당 노드를 반환합니다. 만약 존재하지 않는다면 -1을 반환합니다.
        /// </summary>
        /// <param name="queue">검색을 수행할 큐</param>
        /// <param name="node">찾으려는 노드</param>
        /// <returns></returns>
        private EPNode Find(IEnumerable<EPNode> queue, EPNode node)
        {
            foreach(EPNode item in queue)
            {
                if (item.Equals(node))
                    return item;
            }
            return null;
        }
    }
}
