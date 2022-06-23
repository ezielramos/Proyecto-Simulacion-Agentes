using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agentes
{
    public class Robot
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool HasChild { get; private set; }
        public bool HasChildAndCorral { get; private set; }

        public Robot(int x, int y, bool hasChild, bool hasChildAndCorral)
        {
            X = x;
            Y = y;
            HasChild = hasChild;
            HasChildAndCorral = hasChildAndCorral;
        }

        public Elements[,] RandomMove(Elements[,] map)
        {
            Random random = new Random();

            while (true)
            {
                int direction = random.Next(0, Utils.dx.Length);
                int next_x = X + Utils.dx[direction];
                int next_y = Y + Utils.dy[direction];

                if (Utils.IsValid(next_x, next_y, map))
                {
                    switch (map[next_x, next_y])
                    {
                        case Elements.Obstacles:
                            break;
                        case Elements.Dirt:
                            if (HasChildAndCorral)
                            {
                                // tener en cuenta que recogi una basura y que solte al chico en un corral
                                map[X, Y] = Elements.CorralAndChild;
                                map[next_x, next_y] = Elements.Robot;
                            }
                            else if (HasChild)
                            {
                                // tener en cuenta que recogi una basura
                                map[X, Y] = Elements.None;
                                map[next_x, next_y] = Elements.RobotAndChild;
                            }
                            else
                            {
                                // tener en cuenta que recogi una basura
                                map[X, Y] = Elements.None;
                                map[next_x, next_y] = Elements.Robot;
                            }
                            break;
                        case Elements.Child:
                            if (HasChildAndCorral)
                            {
                                // tener en cuenta que cargue un chico y que solte a otro en un corral
                                map[X, Y] = Elements.CorralAndChild;
                                map[next_x, next_y] = Elements.RobotAndChild;
                            }
                            if (!HasChild)
                            {
                                // tener en cuenta que cargue un chico
                                map[X, Y] = Elements.None;
                                map[next_x, next_y] = Elements.RobotAndChild;
                            }
                            break;
                        case Elements.Corral:
                            if (HasChild)
                            {
                                // tener en cuenta que meti el robot con un chico dentro de un corral
                                map[X, Y] = Elements.None;
                                map[next_x, next_y] = Elements.RobotAndChildAndCorral;
                            }
                            break;
                        case Elements.CorralAndChild:
                            break;
                        case Elements.None:
                            if (HasChildAndCorral)
                            {
                                // tener en cuenta que solte a un chico en un corral
                                map[X, Y] = Elements.CorralAndChild;
                                map[next_x, next_y] = Elements.Robot;
                            }
                            else if (HasChild)
                            {
                                map[X, Y] = Elements.None;
                                map[next_x, next_y] = Elements.RobotAndChild;
                            }
                            else
                            {
                                map[X, Y] = Elements.None;
                                map[next_x, next_y] = Elements.Robot;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }

            return (Elements[,])map.Clone();
        }

        public Elements[,] TrashMove(Elements[,] map)
        {
            Random random = new Random();
            Tuple<bool, int> trash = IsTrash(map);

            if (trash.Item1)
            {
                // se mueve a alguna basura de su alrededor
                int next_x = X + Utils.dx[trash.Item2];
                int next_y = Y + Utils.dy[trash.Item2];

                map[X, Y] = Elements.None;
                map[next_x, next_y] = Elements.Robot;
            }
            else
            {
                // se mueve random
                while (true)
                {
                    int direction = random.Next(0, Utils.dx.Length);
                    int next_x = X + Utils.dx[direction];
                    int next_y = Y + Utils.dy[direction];

                    if (Utils.IsValid(next_x, next_y, map) && map[next_x, next_y] == Elements.None)
                    {
                        map[X, Y] = Elements.None;
                        map[next_x, next_y] = Elements.Robot;
                        break;
                    }

                }
            }

            return (Elements[,])map.Clone();
        }

        private Tuple<bool, int> IsTrash(Elements[,] map)
        {
            for (int i = 0; i < Utils.dx.Length; i++)
            {
                int next_x = X + Utils.dx[i];
                int next_y = Y + Utils.dy[i];

                if (Utils.IsValid(next_x, next_y, map) && map[next_x, next_y] == Elements.Dirt)
                    return new Tuple<bool, int>(true, i);
            }
            return new Tuple<bool, int>(false, -1);
        }
    }
}
