using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTree
{
    public class GameTree
    {
        const int length = 3;
        const int high = 3;

        private int[,] Map = {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 }
         };

        int n = 0;
        List<int> AIStep = new List<int>();
        List<int> EnemyStep = new List<int>();

        Tuple<int, int> NextStep = new Tuple<int, int>(0, 0);

        public int[,] GetMap()
        {
            return Map;
        }

        public Tuple<int, int> GetNextStep()
        {
            return NextStep;
        }

        //极大极小值搜索
        public int MinMax(int depth, bool isAI)
        {
            n = 0;
            if (isAI)
            {
                return Max(depth);
            }
            else
            {
                return Min(depth);
            }
        }

        //极大值搜索
        private int Max(int depth)
        {
            int best = int.MinValue;
            if (depth <= 0 || GameOver() > 0)
            {
                return Evaluate();
            }
            List<Tuple<int, int>> nextPositionList = NextMovePosition();
            for(int i=0; i<nextPositionList.Count; i++)
            {
                MakeNextMove(nextPositionList[i], true);
                int value = Min(depth - 1);
                UnmakeMove(nextPositionList[i]);
                if (value > best)
                {
                    best = value;
                    NextStep = nextPositionList[i];
                }
            }
            n = n+1;
            return best;

        }

        //极小值搜索
        private int Min(int depth)
        {
            int best = int.MaxValue;
            if (depth <= 0 || GameOver() > 0)
            {
                return Evaluate();
            }
            List<Tuple<int, int>> nextPositionList = NextMovePosition();
            for (int i = 0; i < nextPositionList.Count; i++)
            {
                MakeNextMove(nextPositionList[i], false);
                int value = Max(depth - 1);
                UnmakeMove(nextPositionList[i]);
                if (value < best)
                {
                    best = value;
                    NextStep = nextPositionList[i];
                }
            }
            return best;
        }

        //评估函数
        private int Evaluate()
        {
            int k = 0;

            switch(GameOver())
            {
                case 0:
                    k = 0;
                    break;
                case 1:
                    k = 999;
                    break;
                case 2:
                    k = -999;
                    break;
                case 3:
                    k = 50;
                    break;

            }
            return k;
        }

        //进行下一步行动
        public void MakeNextMove(Tuple<int, int> position, bool isAI)
        {
            if (isAI)
            {
                Map[position.Item1, position.Item2] = 1;
            }
            else
            {
                Map[position.Item1, position.Item2] = 2;
            }
            
        }

        //撤回行动
        private void UnmakeMove(Tuple<int, int> position)
        {
            Map[position.Item1, position.Item2] = 0;
        }

        //下一步的所有走法
        private List<Tuple<int, int>> NextMovePosition()
        {
            List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
            for(int i=0; i<high; i++)
            {
                for(int j=0; j<length; j++)
                {
                    if (Map[i,j] == 0)
                    {
                        positions.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return positions;
        }

        //判断游戏是否结束;
        // 0:未结束; 1:A胜利; 2:B胜利; 3:平局;
        public int GameOver()
        {
            for(int i=0; i<high; i++)
            {
                if (Map[i,0]==1 && Map[i,1]==1 && Map[i, 2] == 1)
                {
                    return 1;
                }
                if (Map[i, 0] == 2 && Map[i, 1] == 2 && Map[i, 2] == 2)
                {
                    return 2;
                }
            }
            for (int i = 0; i < length; i++)
            {
                if (Map[0, i] == 1 && Map[1, i] == 1 && Map[2, i] == 1)
                {
                    return 1;
                }
                if (Map[0, i] == 2 && Map[1, i] == 2 && Map[2, i] == 2)
                {
                    return 2;
                }
            }
            if (Map[0, 0] == 1 && Map[1, 1] == 1 && Map[2, 2] == 1)
            {
                return 1;
            }
            if (Map[0, 0] == 2 && Map[1, 1] == 2 && Map[2, 2] == 2)
            {
                return 2;
            }
            if (Map[2, 0] == 1 && Map[1, 1] == 1 && Map[0, 2] == 1)
            {
                return 1;
            }
            if (Map[2, 0] == 2 && Map[1, 1] == 2 && Map[0, 2] == 2)
            {
                return 2;
            }
            int k = 999;
            for(int i=0;i<high;i++)
            {
                for(int j=0;j<length;j++)
                {
                    if (Map[i, j] < k)
                    {
                        k = Map[i, j];
                    }
                }
            }
            if(k > 0)
            {
                return 3;
            }
            return 0;
        }

        public void PrintMap()
        {
            for (int i = 0; i < high; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write("{0} ", Map[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
