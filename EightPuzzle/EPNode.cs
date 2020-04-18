using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightPuzzle
{
    /// <summary>
    /// 8 퍼즐 상태에 대한 노드입니다.
    /// </summary>
    class EPNode
    {
        /// <summary>
        /// 부모 노드 입니다. Backtracking에 사용됩니다.
        /// </summary>
        public EPNode Parent { get; }
        /// <summary>
        /// 시작 노드로부터의 거리 입니다.
        /// </summary>
        public int Distance { get; }
        /// <summary>
        /// 목표 노드까지의 추정치 입니다.
        /// </summary>
        public int Heuristic { get; set; }

        private int[,] _matrix;
        private int _blankX, _blankY;

        /// <summary>
        /// 8 퍼즐 상태에 대한 노드를 생성합니다.
        /// </summary>
        /// <param name="matrix">8 퍼즐의 상태를 나타낸 2차원 배열([3][3]) 입니다.</param>
        /// <param name="x">빈 칸의 x 좌표 입니다.</param>
        /// <param name="y">빈 칸의 y 좌표 입니다.</param>
        /// <param name="level">탐색 트리에서 이 노드와 루트 노드 사이의 거리 입니다.</param>
        /// <param name="parent">부모 노드 입니다.</param>
        public EPNode(int[,] matrix, int x, int y, int level, EPNode parent)
        {
            this.Parent = parent;
            this.Distance = level;
            this.Heuristic = int.MaxValue;

            this._matrix = matrix;
            this._blankX = x; this._blankY = y;
        }

        /// <summary>
        /// 목표 노드에 대한 이 노드의 추정치를 계산합니다.
        /// </summary>
        /// <param name="goal">목표 노드 입니다.</param>
        public EPNode Estimate(int[,] goal)
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++) 
                {
                    if (this._matrix[i, j] != 0 && this._matrix[i, j] != goal[i, j]) count++;
                }
            }
            this.Heuristic = count;
            return this;
        }

        /// <summary>
        /// 한 칸 위로 빈 칸을 이동시킵니다.
        /// </summary>
        /// <returns>이동 후의 상태로 새로 생성된 노드를 반환합니다. 이동할 수 없는 경우 null을 반환합니다.</returns>
        public EPNode MoveUp()
        {
            if (this._blankX == 0)
            {
                return null;
            }
            else
            {
                int[,] newMat = this._matrix;
                int tmp = newMat[this._blankX, this._blankY];
                newMat[this._blankX, this._blankY] = newMat[this._blankX - 1, this._blankY];
                newMat[this._blankX - 1, this._blankY] = tmp;
                return new EPNode(newMat, this._blankX - 1, this._blankY, this.Distance + 1, this);
            }
        }

        /// <summary>
        /// 한 칸 아래로 빈 칸을 이동시킵니다.
        /// </summary>
        /// <returns>이동 후의 상태로 새로 생성된 노드를 반환합니다. 이동할 수 없는 경우 null을 반환합니다.</returns>
        public EPNode MoveDown()
        {
            if (this._blankX == 2)
            {
                return null;
            }
            else
            {
                int[,] newMat = this._matrix;
                int tmp = newMat[this._blankX, this._blankY];
                newMat[this._blankX, this._blankY] = newMat[this._blankX + 1, this._blankY];
                newMat[this._blankX + 1, this._blankY] = tmp;
                return new EPNode(newMat, this._blankX + 1, this._blankY, this.Distance + 1, this);
            }
        }

        /// <summary>
        /// 한 칸 왼쪽으로 빈 칸을 이동시킵니다.
        /// </summary>
        /// <returns>이동 후의 상태로 새로 생성된 노드를 반환합니다. 이동할 수 없는 경우 null을 반환합니다.</returns>
        public EPNode MoveLeft()
        {
            if (this._blankY == 0)
            {
                return null;
            }
            else
            {
                int[,] newMat = this._matrix;
                int tmp = newMat[this._blankX, this._blankY];
                newMat[this._blankX, this._blankY] = newMat[this._blankX, this._blankY - 1];
                newMat[this._blankX, this._blankY - 1] = tmp;
                return new EPNode(newMat, this._blankX, this._blankY - 1, this.Distance + 1, this);
            }
        }

        /// <summary>
        /// 한 칸 오른쪽으로 빈 칸을 이동시킵니다.
        /// </summary>
        /// <returns>이동 후의 상태로 새로 생성된 노드를 반환합니다. 이동할 수 없는 경우 null을 반환합니다.</returns>
        public EPNode MoveRight()
        {
            if (this._blankY == 2)
            {
                return null;
            }
            else
            {
                int[,] newMat = this._matrix;
                int tmp = newMat[this._blankX, this._blankY];
                newMat[this._blankX, this._blankY] = newMat[this._blankX, this._blankY + 1];
                newMat[this._blankX, this._blankY + 1] = tmp;
                return new EPNode(newMat, this._blankX, this._blankY + 1, this.Distance + 1, this);
            }
        }

        /// <summary>
        /// Console에 현재 노드가 나타내는 퍼즐 상태를 출력합니다.
        /// </summary>
        public EPNode Print()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(this._matrix[i, j] + " ");
                }
                if (i != 2) Console.WriteLine();
                else Console.WriteLine("f(n) = " + this.Distance + " + " + ((this.Heuristic == int.MaxValue) ? "INF" : this.Heuristic.ToString()));
            }
            return this;
        }
    }
}
