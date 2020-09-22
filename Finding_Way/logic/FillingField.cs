using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finding_Way
{
    class FillingField
    {
        //0 - пустая клетка
        //1 - конечный пункт
        //2 - препятсвие
        //3 - цель для сбора
        //4 - агент

        Random random = new Random();
        int fieldSize;
        int obstaclesAmount;
        int targetsAmount;


        int[,] field;

        public FillingField(int fSize, int obsAmount, int TargAmount)
        {
            fieldSize = fSize;
            obstaclesAmount = obsAmount;
            targetsAmount = TargAmount;

            field = new int[fieldSize, fieldSize];

            // заполняем нулями двумерный массив - логическое представление поля
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    field[i, j] = 0;

                    if (i == 0 && j == fieldSize - 1)
                        field[i, j] = 1;
                }
            }
        }

        //метод рандомно заполняющий поле
        private void RandomingField()
        {
            int x = random.Next(fieldSize);
            int y = random.Next(fieldSize);

            for (int k = 0; k < obstaclesAmount; k++)
            {
                while (field[x, y] != 0)
                {
                    x = random.Next(fieldSize);
                    y = random.Next(fieldSize);
                }

                field[x, y] = 2;
            }

            for (int k = 0; k < targetsAmount; k++)
            {
                while (field[x, y] != 0)
                {
                    x = random.Next(fieldSize);
                    y = random.Next(fieldSize);
                }

                field[x, y] = 3;
            }

            while (field[x, y] != 0)
            {
                x = random.Next(fieldSize);
                y = random.Next(fieldSize);
            }

            field[x, y] = 4;
        }

        public int[,] GetRandomField()
        {
            RandomingField();
            return field;
        }
    }
}
