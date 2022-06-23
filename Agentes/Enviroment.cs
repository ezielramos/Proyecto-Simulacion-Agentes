using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agentes
{
    public class Enviroment
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int CountDirtyCells { get; private set; }
        public int CountObstacles { get; private set; }
        public int CountChildren { get; private set; }
        public int CountChildrenInCorral { get; private set; }
        public Elements[,] Map { get; private set; }

        public Enviroment(int rows, int columns, int dirtyCells, 
            int obstacles, int children)
        {
            Rows = rows;
            Columns = columns;

            int totalCells = rows * columns;
            CountDirtyCells = totalCells * dirtyCells / 100;
            CountObstacles = totalCells * obstacles / 100;
            CountChildren = children;
            CountChildrenInCorral = 0;

            InitializeMap();
            GenerateCorral();
            GenerateDirty();
            GenerateObstacles();
            GenerateChildren();
            GenerateRobot();
        }

        private void InitializeMap()
        {
            Map = new Elements[Rows, Columns];

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j] = Elements.None;
                }
            }
        }

        private void GenerateDirty()
        {
            Random random = new Random();
            int countDirtyCells = CountDirtyCells;

            while (countDirtyCells > 0)
            {
                int row = random.Next(0, Rows);
                int column = random.Next(0, Columns);

                if (Map[row, column] == Elements.None)
                {
                    Map[row, column] = Elements.Dirt;
                    countDirtyCells--;
                }
            }
        }

        private void GenerateObstacles()
        {
            Random random = new Random();
            int countObstacles = CountObstacles;

            while (countObstacles > 0)
            {
                int row = random.Next(0, Rows);
                int column = random.Next(0, Columns);

                if (Map[row, column] == Elements.None)
                {
                    Map[row, column] = Elements.Obstacles;
                    countObstacles--;
                }
            }
        }

        private void GenerateCorral()
        {
            Random random = new Random();
            int countChildren = CountChildren + CountChildrenInCorral;

            while (true)
            {
                int row = random.Next(0, Rows);
                int column = random.Next(0, Columns);

                if (CanHorizontal(row, column, countChildren) || 
                        CanVertical(row, column, countChildren) || 
                            CanDiagonal(row, column, countChildren))
                    break;
            }

            int aux = CountChildrenInCorral;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (aux > 0 && Map[i, j] == Elements.Corral)
                    {
                        Map[i, j] = Elements.CorralAndChild;
                        aux--;
                    }
                }
            }
        }

        private bool CanHorizontal(int pos_x, int pos_y, int countCorral)
        {
            for (int i = 0; i < countCorral; i++)
            {
                int y = pos_y + i;
                if (y >= Columns || Map[pos_x, y] != Elements.None)
                    return false;
            }

            for (int i = pos_y; i < pos_y + countCorral; i++)
            {
                Map[pos_x, i] = Elements.Corral;
            }

            return true;
        }

        private bool CanVertical(int pos_x, int pos_y, int countCorral)
        {
            for (int i = 0; i < countCorral; i++)
            {
                int x = pos_x + i;
                if (x >= Rows || Map[x, pos_y] != Elements.None)
                    return false;
            }

            for (int i = pos_x; i < pos_x + countCorral; i++)
            {
                Map[i, pos_y] = Elements.Corral;
            }

            return true;
        }

        private bool CanDiagonal(int pos_x, int pos_y, int coutCorral)
        {
            for (int i = 0; i < coutCorral; i++)
            {
                int x = pos_x + i;
                int y = pos_y + i;
                if (x >= Rows || y >= Columns || Map[x, y] != Elements.None)
                    return false;
            }

            for (int i = 0; i < coutCorral; i++)
            {
                int x = pos_x + i;
                int y = pos_y + i;
                Map[x, y] = Elements.Corral;
            }

            return true;
        }

        private void GenerateChildren()
        {
            Random random = new Random();
            int countChildren = CountChildren;

            while (countChildren > 0)
            {
                int row = random.Next(0, Rows);
                int column = random.Next(0, Columns);

                if (Map[row, column] == Elements.None)
                {
                    Map[row, column] = Elements.Child;
                    countChildren--;
                }
            }
        }

        private void GenerateRobot()
        {
            Random random = new Random();

            while (true)
            {
                int row = random.Next(0, Rows);
                int column = random.Next(0, Columns);

                if (Map[row, column] == Elements.None)
                {
                    Map[row, column] = Elements.Robot;
                    break;
                }
            }
        }

        public Tuple<int, int, bool, bool> GetRobot()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Map[i, j] == Elements.Robot)
                        return new Tuple<int, int, bool, bool>(i, j, false, false);
                    else if (Map[i, j] == Elements.RobotAndChild)
                        return new Tuple<int, int, bool, bool>(i, j, true, false);
                    else if (Map[i, j] == Elements.RobotAndChildAndCorral)
                        return new Tuple<int, int, bool, bool>(i, j, true, true);
                }
            }
            return new Tuple<int, int, bool, bool>(-1, -1, false, false);
        }

        public List<Tuple<int, int>> GetChildren()
        {
            List<Tuple<int, int>> children = new List<Tuple<int, int>>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Map[i, j] == Elements.Child)
                        children.Add(new Tuple<int, int>(i, j));
                }
            }
            return children;
        }

        public void SetMap(Elements[,] map)
        {
            Map = (Elements[,])map.Clone();
            Rows = map.GetLength(0);
            Columns = map.GetLength(1);
            CountChildren = 0;
            CountDirtyCells = 0;
            CountObstacles = 0;
            CountChildrenInCorral = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == Elements.Child || map[i, j] == Elements.RobotAndChild)
                        CountChildren++;
                    else if (map[i, j] == Elements.Dirt)
                        CountDirtyCells++;
                    else if (map[i, j] == Elements.Obstacles)
                        CountObstacles++;
                    else if (map[i, j] == Elements.CorralAndChild)
                        CountChildrenInCorral++;
                }
            }
        }

        public void Change()
        {
            InitializeMap();
            GenerateCorral();
            GenerateDirty();
            GenerateObstacles();
            GenerateChildren();
            GenerateRobot();
        }

        public void GetInfo()
        {
            Console.WriteLine("Cantidad de filas: {0}", Rows);
            Console.WriteLine("Cantidad de columnas: {0}", Columns);
            Console.WriteLine("Cantidad de basuras: {0}", CountDirtyCells);
            Console.WriteLine("Cantidad de obstaculos: {0}", CountObstacles);
            Console.WriteLine("Cantidad de chicos fueran del corral: {0}", CountChildren);
            Console.WriteLine("Cantidad de chicos dentro del corral: {0}", CountChildrenInCorral);
        }

        public bool CleanHome()
        {
            return CountChildrenInCorral == CountObstacles && CountDirtyCells == 0;
        }

        public bool DischargeRobot()
        {
            int por_ciento = 60 * Rows * Columns / 100;
            if (CountDirtyCells >= por_ciento)
                return true;
            else
                return false;
        }
    }
}
