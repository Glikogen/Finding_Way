using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finding_Way
{
    class FindingWay
    {
        // список двумерных массивов - полей
        public List<int[,]> Algorithm;

        int[,] field;
        int agentCoordinateX = 0;
        int agentCoordinateY = 0;
        int fieldSize;
        int obstaclesAmount;
        int targetsAmount;
        List<int[]> obstaclesCoordinates = new List<int[]>();
        List<int[]> targetsCoordinates = new List<int[]>();

        public long time { set; get; }

        public FindingWay(int[,] field, int fSize, int obsAmount, int TargAmount)
        {
            this.field = field;
            fieldSize = fSize;
            obstaclesAmount = obsAmount;
            targetsAmount = TargAmount;

            // определяем координаты объектов на поле
            Algorithm = new List<int[,]>();
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == 2)
                    {
                        obstaclesCoordinates.Add(new int[] { j, i });
                    }

                    if (field[i, j] == 3)
                    {
                        targetsCoordinates.Add(new int[] { j, i });
                    }

                    if (field[i, j] == 4)
                    {
                        agentCoordinateX = j;
                        agentCoordinateY = i;
                    }
                }
            }
        }

        // метод поиска всего итогово пути (конечного списка полей)
        public List<int[,]> FindAlgorithm()
        {
            // запускаем таймер
            var watch = System.Diagnostics.Stopwatch.StartNew();

            FindingOneTarget oneTarget;
            // пока целей не 0 выполнять поиск до ближайшей звезды
            while (targetsAmount != 0)
            {
                // "пересоздаем" объект класса, ищущий одну цель, указывая в конструкторе новое поле
                oneTarget = new FindingOneTarget(field, fieldSize);
                oneTarget.FindingTarget();
                int stepsCount = oneTarget.AlgorithmForOneTarget.Count;

                // если до цели не добраться
                if (stepsCount == 0)
                {
                    watch.Stop();
                    time = watch.ElapsedMilliseconds;

                    return null;
                }

                // добавляем путь до одной цели в результирующий список путей 
                Algorithm.AddRange(oneTarget.AlgorithmForOneTarget);
                // уменьшаем кол-во целей
                targetsAmount--;
                // переопределяем поле (так как агент "переместился" на место найденной цели)
                for (int i = 0; i < fieldSize; i++)
                {
                    for (int j = 0; j < fieldSize; j++)
                    {
                        field[i, j] = oneTarget.AlgorithmForOneTarget[stepsCount - 1][i, j];
                    }
                }
            }

            // когда нашли все цели, ставим вместо конечной точки цель, и повторяем для нее алгоритм еще один раз
            field[0, fieldSize - 1] = 3;
            oneTarget = new FindingOneTarget(field, fieldSize);
            oneTarget.FindingTarget();

            // если до цели не добраться
            if (oneTarget.AlgorithmForOneTarget.Count == 0)
            {
                watch.Stop();
                time = watch.ElapsedMilliseconds;

                return null;
            }

            Algorithm.AddRange(oneTarget.AlgorithmForOneTarget);

            //останавливаем таймер
            watch.Stop();
            time = watch.ElapsedMilliseconds;

            return Algorithm;
        }
    }
}
