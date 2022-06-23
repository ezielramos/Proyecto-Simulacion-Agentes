using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agentes
{
    public enum Elements
    {
        Obstacles,
        Dirt,
        Child,
        Corral,
        CorralAndChild,
        Robot,
        RobotAndChild,
        None,
        RobotAndChildAndCorral
    }

    public static class Utils
    {
        public static int[] dx = { 1, 0, -1, 0, -1, 1, -1, 1 };
        public static int[] dy = { 0, 1, 0, -1, -1, 1, 1, -1 };

        public static bool IsValid(int row, int column, Elements[,] map)
        {
            return row >= 0 && column >= 0 && row < map.GetLength(0) && column < map.GetLength(1);
        }

        public static int CountChildrenAround(int row, int column, Elements[,] map)
        {
            int countChildren = 0;
            if (map[row, column] == Elements.Child)
                countChildren++;

            for (int i = 0; i < dx.Length; i++)
            {
                int next_row = row + dx[i];
                int next_column = column + dy[i];

                if (IsValid(next_row, next_column, map) && map[next_row, next_column] == Elements.Child)
                    countChildren++;
            }

            return countChildren;
        }

        public static void PrintEnviroment(Elements[,] enviroment)
        {
            Console.WriteLine("AMBIENTE");

            string none = ".";
            string dirt = "x";
            string obstacle = "-";
            string robot = "r";
            string child = "c";
            string corral = "m";
            string corral_child = "M";
            string robot_child = "R";
            string robot_child_corral = "Q";

            for (int i = 0; i < enviroment.GetLength(0); i++)
            {
                for (int j = 0; j < enviroment.GetLength(1); j++)
                {
                    switch (enviroment[i, j])
                    {
                        case Elements.None:
                            Console.Write("{0} ", none);
                            break;
                        case Elements.Dirt:
                            Console.Write("{0} ", dirt);
                            break;
                        case Elements.Obstacles:
                            Console.Write("{0} ", obstacle);
                            break;
                        case Elements.Robot:
                            Console.Write("{0} ", robot);
                            break;
                        case Elements.Child:
                            Console.Write("{0} ", child);
                            break;
                        case Elements.Corral:
                            Console.Write("{0} ", corral);
                            break;
                        case Elements.CorralAndChild:
                            Console.Write("{0} ", corral_child);
                            break;
                        case Elements.RobotAndChild:
                            Console.Write("{0} ", robot_child);
                            break;
                        case Elements.RobotAndChildAndCorral:
                            Console.Write("{0} ", robot_child_corral);
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
