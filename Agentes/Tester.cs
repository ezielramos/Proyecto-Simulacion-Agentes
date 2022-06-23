using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agentes
{
    class Tester
    {
        // Leyenda
        // casilla vacia (.)
        // basura (x)
        // obstaculo (-)
        // robot (r)
        // child (c)
        // corral (m)
        // corral_child (M)
        // robot_child (R)
        // robot_child_corral (Q)

        public static void SimularRobotMoveRandom()
        {
            Enviroment enviroment = new Enviroment(10, 10, 5, 5, 4);
            Utils.PrintEnviroment(enviroment.Map);
            enviroment.GetInfo();
            Console.WriteLine("----------------------------------");

            int i = 0;

            while (i < 100)
            {
                i++;
                Elements[,] map = enviroment.Map;
                Tuple<int, int, bool, bool> datos_robot = enviroment.GetRobot();
                Robot robot = new Robot(datos_robot.Item1, datos_robot.Item2,
                    datos_robot.Item3, datos_robot.Item4);
                map = robot.RandomMove(map);

                List<Tuple<int, int>> children = enviroment.GetChildren();
                for (int j = 0; j < children.Count; j++)
                {
                    Child child = new Child(children[j].Item1, children[j].Item2);
                    map = child.Move(map);
                }

                enviroment.SetMap(map);

                if (i % 10 == 0)
                    enviroment.Change();

                Utils.PrintEnviroment(enviroment.Map);
                enviroment.GetInfo();
                Console.WriteLine("Turno: {0}", i);
                Console.WriteLine("----------------------------------");

                if (enviroment.DischargeRobot())
                {
                    Console.WriteLine("CASA SUCIA!!! EL ROBOT FUE DESPEDIDO");
                    break;
                }
                else if (enviroment.CleanHome())
                {
                    Console.WriteLine("CASA LIMPIA!!! FIN DE LA SIMULACION");
                    break;
                }
            }
        }

        public static void SimularRobotMoveTrash()
        {
            Enviroment enviroment = new Enviroment(10, 10, 5, 5, 4);
            Utils.PrintEnviroment(enviroment.Map);
            enviroment.GetInfo();
            Console.WriteLine("----------------------------------");

            int i = 0;

            while (i < 100)
            {
                i++;
                Elements[,] map = enviroment.Map;
                Tuple<int, int, bool, bool> datos_robot = enviroment.GetRobot();
                Robot robot = new Robot(datos_robot.Item1, datos_robot.Item2,
                    datos_robot.Item3, datos_robot.Item4);
                map = robot.TrashMove(map);

                List<Tuple<int, int>> children = enviroment.GetChildren();
                for (int j = 0; j < children.Count; j++)
                {
                    Child child = new Child(children[j].Item1, children[j].Item2);
                    map = child.Move(map);
                }

                enviroment.SetMap(map);

                if (i % 10 == 0)
                    enviroment.Change();

                Utils.PrintEnviroment(enviroment.Map);
                enviroment.GetInfo();
                Console.WriteLine("Turno: {0}", i);
                Console.WriteLine("----------------------------------");

                if (enviroment.DischargeRobot())
                {
                    Console.WriteLine("CASA SUCIA!!! EL ROBOT FUE DESPEDIDO");
                    break;
                }
                else if (enviroment.CleanHome())
                {
                    Console.WriteLine("CASA LIMPIA!!! FIN DE LA SIMULACION");
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            //SimularRobotMoveRandom();

            SimularRobotMoveTrash();
        }
    }
}
