using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agentes
{
    public class Child
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Child(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Elements[,] Move(Elements[,] Map)
        {
            Random random = new Random();

            if (random.Next(0, 2) == 0)
            {
                int direction = random.Next(0, Utils.dx.Length);
                int next_x = X + Utils.dx[direction];
                int next_y = Y + Utils.dy[direction];

                if (Utils.IsValid(next_x, next_y, Map) && Map[next_x, next_y] == Elements.None)
                {
                    Map[next_x, next_y] = Elements.Child;
                    Map[X, Y] = Elements.None;
                }
                else if (Utils.IsValid(next_x, next_y, Map) && Map[next_x, next_y] == Elements.Obstacles)
                {
                    List<Tuple<int, int>> obstacles = new List<Tuple<int, int>>();
                    obstacles.Add(new Tuple<int, int>(next_x, next_y));

                    while (true)
                    {
                        int next_xx = next_x + Utils.dx[direction];
                        int next_yy = next_y + Utils.dy[direction];

                        if (Utils.IsValid(next_xx, next_yy, Map) && 
                                            Map[next_xx, next_yy] == Elements.None)
                        {
                            //Map[next_xx, next_yy] = Map[obstacles[obstacles.Count - 1].Item1, obstacles[obstacles.Count - 1].Item2];
                            Map[next_xx, next_yy] = Elements.Obstacles;
                            for (int i = obstacles.Count - 1; i >= 1; i--)
                            {
                                Map[obstacles[i].Item1, obstacles[i].Item2] = Elements.Obstacles;
                                //Map[obstacles[i].Item1, obstacles[i].Item2] = Map[obstacles[i - 1].Item1, obstacles[i - 1].Item2];
                            }
                            Map[obstacles[0].Item1, obstacles[0].Item2] = Elements.Child;
                            Map[X, Y] = Elements.None;

                            break;
                        }
                        else if (Utils.IsValid(next_xx, next_yy, Map) && 
                                      Map[next_xx, next_yy] == Elements.Obstacles)
                        {
                            obstacles.Add(new Tuple<int, int>(next_xx, next_yy));
                            next_x = next_xx;
                            next_y = next_yy;
                        }
                        else
                            break;
                    }
                }
            }

            int countChildrenAround = Utils.CountChildrenAround(X, Y, Map);

            if (countChildrenAround == 1)
            {
                int direction = random.Next(0, Utils.dx.Length);
                int trash_x = X + Utils.dx[direction];
                int trash_y = Y + Utils.dy[direction];

                if (Utils.IsValid(trash_x, trash_y, Map) && Map[trash_x, trash_y] == Elements.None)
                    Map[trash_x, trash_y] = Elements.Dirt;
            }
            else if (countChildrenAround == 2)
            {
                for (int i = 1; i <= 3; i++)
                {
                    int direction = random.Next(0, Utils.dx.Length);
                    int trash_x = X + Utils.dx[direction];
                    int trash_y = Y + Utils.dy[direction];

                    if (Utils.IsValid(trash_x, trash_y, Map) && Map[trash_x, trash_y] == Elements.None)
                        Map[trash_x, trash_y] = Elements.Dirt;
                }
            }
            else
            {
                for (int i = 1; i <= 6; i++)
                {
                    int direction = random.Next(0, Utils.dx.Length);
                    int trash_x = X + Utils.dx[direction];
                    int trash_y = Y + Utils.dy[direction];

                    if (Utils.IsValid(trash_x, trash_y, Map) && Map[trash_x, trash_y] == Elements.None)
                        Map[trash_x, trash_y] = Elements.Dirt;
                }
            }

            return (Elements[,])Map.Clone();
        }
    }
}
