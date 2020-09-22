using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finding_Way
{
    class FindingOneTarget
    {
        // список полей для достижения ближайшей цели
        public List<int[,]> AlgorithmForOneTarget;
        // список "путей" (каждый путь хранит List<Sides> steps - список сторон, т. е. последовательный набор возможных "движений" агента)
        public List<Path> Paths;

        int[,] field;
        int[,] additionalField;
        int agentCoordinateX = 0;
        int agentCoordinateY = 0;
        int fieldSize;
        // переменная указывающая, найдена ли цель
        bool findedTarget = false;

        public FindingOneTarget(int[,] field, int fSize)
        {
            this.field = field;
            fieldSize = fSize;
            additionalField = new int[fieldSize, fieldSize];
            AlgorithmForOneTarget = new List<int[,]>();
            Paths = new List<Path>();

            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    additionalField[i, j] = field[i, j];

                    if (field[i, j] == 4)
                    {
                        agentCoordinateX = j;
                        agentCoordinateY = i;
                    }
                }
            }
        }

        // метод, ищущий стороны, в которые можно пойти
        private List<Sides> FindingAllowingSides(Path path)
        {
            List<Sides> allowingSides = new List<Sides>() { Sides.up, Sides.right, Sides.down, Sides.left };
            // если агент рядом с границей то убираем соответствующие стороны 
            if (path.agentCoordinateX == 0) allowingSides.Remove(Sides.left);
            if (path.agentCoordinateY == 0) allowingSides.Remove(Sides.up);
            if (path.agentCoordinateX == fieldSize - 1) allowingSides.Remove(Sides.right);
            if (path.agentCoordinateY == fieldSize - 1) allowingSides.Remove(Sides.down);
            //создаём список запрещенных сторон (если рядом препятствие)
            //но если рядом цель, то можно сразу пойти в эту сторону и закончить поиск
            List<Sides> ForbiddenSides = new List<Sides>();
            foreach (var side in allowingSides)
            {
                switch (side)
                {
                    case Sides.up:
                        {
                            if (field[path.agentCoordinateY - 1, path.agentCoordinateX] == 2)
                                ForbiddenSides.Add(Sides.up);
                            if (field[path.agentCoordinateY - 1, path.agentCoordinateX] == 3)
                                findedTarget = true;
                        }
                        break;
                    case Sides.right:
                        {
                            if (field[path.agentCoordinateY, path.agentCoordinateX + 1] == 2)
                                ForbiddenSides.Add(Sides.right);
                            if (field[path.agentCoordinateY, path.agentCoordinateX + 1] == 3)
                                findedTarget = true;
                        }
                        break;
                    case Sides.down:
                        {
                            if (field[path.agentCoordinateY + 1, path.agentCoordinateX] == 2)
                                ForbiddenSides.Add(Sides.down);
                            if (field[path.agentCoordinateY + 1, path.agentCoordinateX] == 3)
                                findedTarget = true;
                        }
                        break;
                    case Sides.left:
                        {
                            if (field[path.agentCoordinateY, path.agentCoordinateX - 1] == 2)
                                ForbiddenSides.Add(Sides.left);
                            if (field[path.agentCoordinateY, path.agentCoordinateX - 1] == 3)
                                findedTarget = true;
                        }
                        break;
                }

                if (findedTarget) return new List<Sides>() { side };
            }

            //удалаем из списка разрешенных сторон стороны из запрещенного списка
            if (ForbiddenSides.Contains(Sides.up)) allowingSides.Remove(Sides.up);
            if (ForbiddenSides.Contains(Sides.left)) allowingSides.Remove(Sides.left);
            if (ForbiddenSides.Contains(Sides.down)) allowingSides.Remove(Sides.down);
            if (ForbiddenSides.Contains(Sides.right)) allowingSides.Remove(Sides.right);

            return allowingSides;
        }

        //главный метод = поиск пути до ближайшей цели
        public void FindingTarget()
        {
            //1)для начального положения определяем список разрешеннных сторон
            List<Sides> sides = FindingAllowingSides(new Path(agentCoordinateX, agentCoordinateY));
            // если до цели не добраться
            if (sides.Count == 0) return;
            // если цель найдена
            if (findedTarget)
            {
                Paths.Add(new Path(agentCoordinateX, agentCoordinateY, sides[0]));
                ConvertStepsInAlgorithm();
                return;
            }
            //2)"проходим" в каждую разрешенную сторону
            foreach (var side in sides)
            {
                Paths.Add(new Path(agentCoordinateX, agentCoordinateY, side));
            }
            field[agentCoordinateY, agentCoordinateX] = 2;
            //3)для каждого нового положения снова определяем разрешенные стороны
            // i - переменная, хранящая индекс текущего рассматриваемого пути
            int i = 0;
            while (Paths.Count > i)
            {
                //1)...
                List<Sides> currentPathSides = FindingAllowingSides(Paths[i]);
                if (currentPathSides.Count == 0)
                {
                    Paths.RemoveAt(i);
                    continue;
                }

                if (findedTarget)
                {
                    Paths[i].AddingStep(currentPathSides[0]);
                    Paths.Add(new Path(Paths[i]));
                    ConvertStepsInAlgorithm();
                    return;
                }

                //2)...
                foreach (var side in currentPathSides)
                {
                    Path newPath = new Path(Paths[i]);
                    Sides currentStep = side;
                    field[newPath.agentCoordinateY, newPath.agentCoordinateX] = 2;
                    if (side == Sides.up)
                    {
                        if (field[newPath.agentCoordinateY - 1, newPath.agentCoordinateX] == 0)
                        {
                            currentStep = Sides.up;
                        }
                    }
                    if (side == Sides.right)
                    {
                        if (field[newPath.agentCoordinateY, newPath.agentCoordinateX + 1] == 0)
                        {
                            currentStep = Sides.right;
                        }
                    }
                    if (side == Sides.down)
                    {
                        if (field[newPath.agentCoordinateY + 1, newPath.agentCoordinateX] == 0)
                        {
                            currentStep = Sides.down;
                        }
                    }
                    if (side == Sides.left)
                    {
                        if (field[newPath.agentCoordinateY, newPath.agentCoordinateX - 1] == 0)
                        {
                            currentStep = Sides.left;
                        }
                    }

                    //добавляем сторону в текущий объект Path
                    newPath.AddingStep(currentStep);
                    //добавляем объект Path в список Path-ов
                    Paths.Add(newPath);

                    if (findedTarget) break;
                }

                if (findedTarget)
                {
                    ConvertStepsInAlgorithm();
                    return;
                }

                //если до цели не добраться (даже за 1000000 вариантов различных путей)
                if (Paths.Count > 1000000)
                {
                    Paths.Clear();
                    break;
                }

                i++;
            }


        }

        // метод преобразующий список шагов итогово пути в список полей
        private void ConvertStepsInAlgorithm()
        {
            foreach (var step in Paths[Paths.Count - 1].steps)
            {
                switch (step)
                {
                    case Sides.up:
                        {
                            additionalField[agentCoordinateY, agentCoordinateX] = 0;
                            agentCoordinateY--;
                            additionalField[agentCoordinateY, agentCoordinateX] = 4;
                        }
                        break;
                    case Sides.down:
                        {
                            additionalField[agentCoordinateY, agentCoordinateX] = 0;
                            agentCoordinateY++;
                            additionalField[agentCoordinateY, agentCoordinateX] = 4;
                        }
                        break;
                    case Sides.left:
                        {
                            additionalField[agentCoordinateY, agentCoordinateX] = 0;
                            agentCoordinateX--;
                            additionalField[agentCoordinateY, agentCoordinateX] = 4;
                        }
                        break;
                    case Sides.right:
                        {
                            additionalField[agentCoordinateY, agentCoordinateX] = 0;
                            agentCoordinateX++;
                            additionalField[agentCoordinateY, agentCoordinateX] = 4;
                        }
                        break;
                }

                int[,] newField = new int[fieldSize, fieldSize];
                for (int i = 0; i < fieldSize; i++)
                {
                    for (int j = 0; j < fieldSize; j++)
                    {
                        newField[i, j] = additionalField[i, j];
                    }
                }
                AlgorithmForOneTarget.Add(newField);
            }
        }
    }
}
