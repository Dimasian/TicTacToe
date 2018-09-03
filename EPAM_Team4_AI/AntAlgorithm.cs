using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

// при разработе использовался код из книги Тим Джонс. Исскуственный интеллект в приложениях.

// класс "Тип Муравей" не тестировался на графе из titactoe, для тестирования работы класса генерируется случайный граф узлов (nodes), на котором муравьи решают задачу комивояжора
// задача комивояжора: поиск кратчайшего пути по всем вершинам

namespace Epam_Team4
{
    // структура с определением констант
    //(макс кол-во узлов, макс расстояние, макс тур, макс кол-во муравеьв, коэф: ALPHA, BETA, RHO, QVAL, макс кол-во туров, начальное кол-во ферромона)
    public static class Constants
    {
        // макс количество узловов
        public const int MAX_NODES = 20;
        // макс расстояние
        public const int MAX_DISTANCE = 100;
        // макс тур (путь)
        public const int MAX_TOUR = (MAX_NODES * MAX_DISTANCE);
        // макс кол-во муравьев 
        public const int MAX_ANTS = 10;

        // альфа (это параметре из основной формулы этого алгор
        public const double ALPHA = 1.0;
        // бетта ?приоритет расстояния над количеством фермента?
        public const double BETA = 5.0;

        // RHO интесивность
        public static double RHO = 0.5;
        // QVAL испарение
        public static int QVAL = 100;

        // макс кол-во туров
        public const int MAX_TOURS = 20;

        // макс время
        public const int MAX_TIME = (MAX_TOURS * MAX_NODES);

        // начальное количество ферромонов
        public const double INIT_PHEROMONE = (1.0 / MAX_NODES);

    }
    // структура "Тип Узел"
    struct NodeType
    {
        public int x;
        public int y;
    }

    // класс "Тип Муравей"
    // может стоит сделать public static 
    class AntType
    {
        // текущий узел
        public int currentNode;
        // следующий узел
        public int nextNode;
        // список табу
        public int[] tabu;
        // индекс пути
        public int pathIndex;
        // путь
        public int[] path;
        // длина пути
        public double tourLength;
        // конструктор для инициализации полей класса муравей
        public AntType()
        {
            currentNode = 0;
            nextNode = 0;
            pathIndex = 0;
            tabu = new int[Constants.MAX_NODES];
            path = new int[Constants.MAX_NODES];
            tourLength = 0;
        }
    }

    // класс для генератора случайных числе
    public static class RandomProvider
    {
        private static int seed = Environment.TickCount;

        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
            new Random(Interlocked.Increment(ref seed))
        );

        public static float GetThreadRandom()
        {
            return randomWrapper.Value.Next(0, int.MaxValue);
        }
    }

    class Program
    {
        // узела
        static NodeType[] nodes = new NodeType[Constants.MAX_NODES];
        // муравьи
        static AntType[] ants = new AntType[Constants.MAX_ANTS];

        // двумерные массивы растояний между i и j узломом
        static double[,] distance = new double[Constants.MAX_NODES, Constants.MAX_NODES];
        // двумерные массивы феромонов между i и j узломом
        static double[,] pheromone = new double[Constants.MAX_NODES, Constants.MAX_NODES];
        // лучший тур
        static double best = (double)(Constants.MAX_TOUR);
        // лучший индекс
        static int bestIndex;

        // генерируем случ числа специально для алгоритмы на основе класса RandomProvider
        static float getSRand()
        {

            return (((float)RandomProvider.GetThreadRandom()) / (float)(int.MaxValue));
        }

        // генератор случ числа от max 
        static int getRand(int x)
        {
            return (int)(x * getSRand());
        }

        public static void init()
        {

            int from, to, ant;
            // создание узловов и их локаций
            for (from = 0; from < Constants.MAX_NODES; from++)
            {
                // случайное расположение узлов на карте
                nodes[from].x = getRand(Constants.MAX_DISTANCE);
                nodes[from].y = getRand(Constants.MAX_DISTANCE);

                // getRand(Constants.MAX_DISTANCE);
                for (to = 0; to < Constants.MAX_NODES; to++)
                {
                    Program.distance[from, to] = 0.0;
                    Program.pheromone[from, to] = Constants.INIT_PHEROMONE;
                }
            }

            // вычисление расстояний для каждого узла на карте
            for (from = 0; from < Constants.MAX_NODES; from++)
            {
                for (to = 0; to < Constants.MAX_NODES; to++)
                {

                    if ((to != from) && (distance[from, to] == 0.0))
                    {
                        int xd = Math.Abs(nodes[from].x - nodes[to].x);
                        int yd = Math.Abs(nodes[from].y - nodes[to].y);

                        Program.distance[from, to] = Math.Sqrt((xd * xd) + (yd * yd));
                        Program.distance[to, from] = Program.distance[from, to];
                    }

                }

            }

            // инициализация муравьев
            to = 0;
            for (ant = 0; ant < Constants.MAX_ANTS; ant++)
            {
                Program.ants[ant] = new AntType();
                // распределение муравьев по кажому узлу равномерно
                if (to == Constants.MAX_NODES) to = 0;
                Program.ants[ant].currentNode = to++;

                for (from = 0; from < Constants.MAX_NODES; from++)
                {
                    Program.ants[ant].tabu[from] = 0;
                    Program.ants[ant].path[from] = -1;
                }
                Program.ants[ant].pathIndex = 1;
                Program.ants[ant].path[0] = Program.ants[ant].currentNode;
                Program.ants[ant].nextNode = -1;
                Program.ants[ant].tourLength = 0.0;

                // загрузка текущих узлов муравья у табу
                Program.ants[ant].tabu[Program.ants[ant].currentNode] = 1;

            }
        }

        //реинециализация популяции муравьев для старта в другом туре по графу
        static void restartAnts()
        {
            int ant, i, to = 0;

            for (ant = 0; ant < Constants.MAX_ANTS; ant++)
            {

                if (ants[ant].tourLength < best)
                {
                    best = ants[ant].tourLength;
                    bestIndex = ant;
                }

                ants[ant].nextNode = -1;
                ants[ant].tourLength = 0.0;

                for (i = 0; i < Constants.MAX_NODES; i++)
                {
                    ants[ant].tabu[i] = 0;
                    ants[ant].path[i] = -1;
                }

                if (to == Constants.MAX_NODES) to = 0;
                ants[ant].currentNode = to++;

                ants[ant].pathIndex = 1;
                ants[ant].path[0] = ants[ant].currentNode;

                ants[ant].tabu[ants[ant].currentNode] = 1;

            }

        }

        //Вычисление знаменателя для урванения вероятностного пути
        //(концентрация ферромонов текущего пути на сумму всех концентраций доступных путей)

        static double antProduct(int from, int to)
        {
            return Math.Pow(Program.pheromone[from, to], Constants.ALPHA) * Math.Pow((1.0 / Program.distance[from, to]), Constants.BETA);
        }

        //Используя алгоритм выбора вероятного пути и текущий уровень феромонов графа, выбор следущего узла для путешествия муравья
        static int selectnextNode(int ant)
        {
            int from, to;
            double denom = 0.0;

            // выбор следующего узла для посещения
            from = ants[ant].currentNode;

            // вычсиление знаменателя
            for (to = 0; to < Constants.MAX_NODES; to++)
            {
                if (ants[ant].tabu[to] == 0)
                {
                    denom += antProduct(from, to);
                }
            }

            Debug.Assert(denom != 0.0);

            do
            {
                double p;

                to++;
                if (to >= Constants.MAX_NODES) to = 0;

                if (ants[ant].tabu[to] == 0)
                {

                    p = antProduct(from, to) / denom;

                    if (getSRand() < p) break;

                }

            } while (true);

            return to;
        }

        //Моделирование единичного шага для каждого муравья популяции. Функция будет возвращать 0 только когда все муравьи окончат их туры
        static int simulateAnts()
        {

            int k;
            int moving = 0;

            for (k = 0; k < Constants.MAX_ANTS; k++)
            {
                // убедимся в том, что муравей до сих пор имеет узлы для посещения
                if (ants[k].pathIndex < Constants.MAX_NODES)
                {
                    ants[k].nextNode = selectnextNode(k);

                    ants[k].tabu[ants[k].nextNode] = 1;

                    ants[k].path[ants[k].pathIndex++] = ants[k].nextNode;

                    ants[k].tourLength += Program.distance[ants[k].currentNode, ants[k].nextNode];

                    // обработка финального сулчая (последний узел для первого)
                    if (ants[k].pathIndex == Constants.MAX_NODES)
                    {
                        ants[k].tourLength +=
                          Program.distance[ants[k].path[Constants.MAX_NODES - 1], ants[k].path[0]];
                    }


                    ants[k].currentNode = ants[k].nextNode;

                    moving++;
                }

            }

            return moving;
        }

        // обновление троп (следов)
        // обновление уровня феромонов на каждом реьре на основе количества муравьев,
        // которые путешестовали по нему, включая испарение существующего феромона

        static void updateTrails()
        {
            int from, to, i, ant;
            // испарение феромона
            for (from = 0; from < Constants.MAX_NODES; from++)
            {

                for (to = 0; to < Constants.MAX_NODES; to++)
                {

                    if (from != to)
                    {

                        Program.pheromone[from, to] *= (1.0 - Constants.RHO);

                        if (Program.pheromone[from, to] < 0.0) Program.pheromone[from, to] = Constants.INIT_PHEROMONE;

                    }

                }

            }

            // добавление нового феромона к следам
            // смотрим туры каждого муравья
            for (ant = 0; ant < Constants.MAX_ANTS; ant++)
            {
                // обновление каждого этапа тура, заданного длинной тура
                for (i = 0; i < Constants.MAX_NODES; i++)
                {

                    if (i < Constants.MAX_NODES - 1)
                    {
                        from = ants[ant].path[i];
                        to = ants[ant].path[i + 1];
                    }
                    else
                    {
                        from = ants[ant].path[i];
                        to = ants[ant].path[0];
                    }

                    Program.pheromone[from, to] += (Constants.QVAL / ants[ant].tourLength);
                    Program.pheromone[to, from] = Program.pheromone[from, to];

                }

            }

            for (from = 0; from < Constants.MAX_NODES; from++)
            {
                for (to = 0; to < Constants.MAX_NODES; to++)
                {
                    Program.pheromone[from, to] *= Constants.RHO;
                }
            }

        }

        // для каждого муравья с лучшего тура (кратчайший путь на гафе), генерируются вместе два файла 
        static void GenerateDataFile(int ant)
        {
            int node;
            StreamWriter fp;

            fp = new StreamWriter("nodes.dat");
            for (node = 0; node < Constants.MAX_NODES; node++)
            {
                fp.WriteLine("узел {0} X: {1} Y: {2} \n", node, nodes[node].x, nodes[node].y);
            }
            fp.Close();

            fp = new StreamWriter("solution.dat");
            for (node = 0; node < Constants.MAX_NODES; node++)
            {
                fp.WriteLine(" муравей ants[ant].path[node]: {0} X: {1} Y: {2}\n", ants[ant].path[node], nodes[ants[ant].path[node]].x, nodes[ants[ant].path[node]].y);
            }
            fp.WriteLine(" path_0 X: {0} Y: {1}\n", nodes[ants[ant].path[0]].x, nodes[ants[ant].path[0]].y);

            fp.Close();
        }

        static void emitTable()
        {
            int from, to;

            for (from = 0; from < Constants.MAX_NODES; from++)
            {
                for (to = 0; to < Constants.MAX_NODES; to++)
                {
                    Console.WriteLine("{0} ", Program.pheromone[from, to]);
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n");
        }

        // метод Main
        static void Main(string[] args)
        {
            int curTime = 0;

            //srand(time(0));
            //var dt = DateTime.Now();

            init();

            while (curTime++ < Constants.MAX_TIME)
            {

                if (simulateAnts() == 0)
                {

                    updateTrails();

                    if (curTime != Constants.MAX_TIME)
                        restartAnts();

                    Console.WriteLine("Time is {0} ({1})\n", curTime, best);
                }
            }

            Console.WriteLine("best tour {0}\n", best);
            Console.WriteLine("\n\n");
            Console.ReadKey();

            GenerateDataFile(bestIndex);

        }
    }
}
