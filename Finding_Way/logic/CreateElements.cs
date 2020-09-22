using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Finding_Way
{
    // класс создающий поле и элементы на нем
    class CreateElements
    {
        FillingField fillingField;
        // поле хранится как двумерный массив
        int[,] field;
        int fieldSize;
        int obstaclesAmount;
        int targetsAmount;

        public CreateElements(int fSize, int obsAmount, int TargAmount)
        {
            fieldSize = fSize;
            obstaclesAmount = obsAmount;
            targetsAmount = TargAmount;
        }

        public Grid grid { private set; get; }

        public Grid CreateNewField()
        {
            fillingField = new FillingField(fieldSize, obstaclesAmount, targetsAmount);

            this.field = fillingField.GetRandomField();

            return BuildField(this.field);
        }

        // метод создающий препятсвия
        private List<Label> CreateObstacles(int[,] field)
        {
            List<int> obstaclesOrdinate = new List<int>();
            List<int> obstaclesAbscissa = new List<int>();

            // определяем координаты препятсвий (2 в двумерном массиве)
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == 2)
                    {
                        obstaclesOrdinate.Add(i);
                        obstaclesAbscissa.Add(j);
                    }
                }
            }

            List<Label> obstacles = new List<Label>();
            for (int i = 0; i < obstaclesAmount; i++)
            {
                Label obstacle = new Label();
                obstacle.Background = Brushes.Black;
                obstacle.SetValue(Grid.ColumnProperty, obstaclesAbscissa[i]);
                obstacle.SetValue(Grid.RowProperty, obstaclesOrdinate[i]);
                obstacles.Add(obstacle);
            }

            return obstacles;
        }

        // метод создающий цели
        private List<Label> CreateTargets(int[,] field)
        {
            List<Label> labels = new List<Label>();
            for (int i = 0; i < targetsAmount; i++)
            {
                Label target = new Label();
                labels.Add(target);
            }

            List<int> targetsOrdinate = new List<int>();
            List<int> targetslesAbscissa = new List<int>();
            // определяем координаты препятсвий (3 в двумерном массиве)
            int targetsAmount1 = 0;
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == 3)
                    {
                        targetsAmount1++;
                        targetsOrdinate.Add(i);
                        targetslesAbscissa.Add(j);
                    }
                }
            }

            for (int i = 0; i < targetsAmount1; i++)
            {
                labels[i].Content = "★";
                labels[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                labels[i].VerticalAlignment = VerticalAlignment.Top;
                labels[i].Foreground = Brushes.Blue;
                // шрифт зависит от размера поля
                switch (fieldSize)
                {
                    case 4:
                        labels[i].FontSize = 90;
                        break;
                    case 5:
                        labels[i].FontSize = 80;
                        break;
                    case 6:
                        labels[i].FontSize = 70;
                        break;
                    case 7:
                        labels[i].FontSize = 60;
                        break;
                    case 8:
                        labels[i].FontSize = 50;
                        break;
                    case 9:
                        labels[i].FontSize = 40;
                        break;
                    case 10:
                        labels[i].FontSize = 30;
                        break;
                }
                labels[i].SetValue(Grid.ColumnProperty, targetslesAbscissa[i]);
                labels[i].SetValue(Grid.RowProperty, targetsOrdinate[i]);

                if (field[0, fieldSize - 1] == 3)
                {
                    labels[i].Content = "○";
                    labels[i].Foreground = Brushes.Red;
                }
            }

            return labels;
        }

        // метод создающий агента
        private Label CreateAgent(int[,] field)
        {
            Label agent = new Label();

            int agentX = 0;
            int agentY = 0;

            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == 4)
                    {
                        agentX = j;
                        agentY = i;
                    }
                }
            }

            agent.Content = "A";
            agent.HorizontalContentAlignment = HorizontalAlignment.Center;
            agent.VerticalAlignment = VerticalAlignment.Top;
            agent.Foreground = Brushes.Green;
            switch (fieldSize)
            {
                case 4:
                    agent.FontSize = 90;
                    break;
                case 5:
                    agent.FontSize = 80;
                    break;
                case 6:
                    agent.FontSize = 70;
                    break;
                case 7:
                    agent.FontSize = 60;
                    break;
                case 8:
                    agent.FontSize = 50;
                    break;
                case 9:
                    agent.FontSize = 40;
                    break;
                case 10:
                    agent.FontSize = 30;
                    break;
            }
            agent.SetValue(Grid.ColumnProperty, agentX);
            agent.SetValue(Grid.RowProperty, agentY);

            return agent;
        }

        // // метод создающий итоговое поле
        public Grid BuildField(int[,] field)
        {
            //создаем новый пустой грид, и определяем все необходимые параметры и свойства
            Grid gridField = new Grid();
            for (int i = 0; i < fieldSize; i++)
            {
                gridField.ColumnDefinitions.Add(new ColumnDefinition());
                gridField.RowDefinitions.Add(new RowDefinition());
            }
            gridField.ShowGridLines = true;
            gridField.Height = 585;
            gridField.VerticalAlignment = VerticalAlignment.Top;
            gridField.SetValue(Grid.ColumnProperty, 1);

            Label endPoint = new Label();
            endPoint.Content = "○";
            endPoint.HorizontalContentAlignment = HorizontalAlignment.Center;
            endPoint.VerticalAlignment = VerticalAlignment.Top;
            endPoint.Foreground = Brushes.Red;
            switch (fieldSize)
            {
                case 4:
                    endPoint.FontSize = 90;
                    break;
                case 5:
                    endPoint.FontSize = 80;
                    break;
                case 6:
                    endPoint.FontSize = 70;
                    break;
                case 7:
                    endPoint.FontSize = 60;
                    break;
                case 8:
                    endPoint.FontSize = 50;
                    break;
                case 9:
                    endPoint.FontSize = 40;
                    break;
                case 10:
                    endPoint.FontSize = 30;
                    break;
            }
            endPoint.SetValue(Grid.ColumnProperty, fieldSize - 1);
            gridField.Children.Add(endPoint);

            // наполняем поле созданными элментами
            List<Label> ObstaclesLabels = CreateObstacles(field);
            for (int i = 0; i < obstaclesAmount; i++)
            {
                gridField.Children.Add(ObstaclesLabels[i]);
            }

            List<Label> targetLabels = CreateTargets(field);
            for (int i = 0; i < targetsAmount; i++)
            {
                gridField.Children.Add(targetLabels[i]);
            }

            gridField.Children.Add(CreateAgent(field));
            this.grid = gridField;

            return gridField;
        }

        public int[,] GetField()
        {
            return field;
        }
    }
}
