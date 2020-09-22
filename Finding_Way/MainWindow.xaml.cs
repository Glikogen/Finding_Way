using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Finding_Way
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //задание - Агент должен собрать все цели, если возможно, и прийти в конечный пункт

        // препятствие - ■
        // агент - A
        // цели - ★
        // конечный пункт - ○

        // k - текущее поле из итогового списка полей (findingWay.Algorithm)
        int k = 0;
        int fieldSize;
        int obstaclesAmount;
        int targetsAmount;

        CreateElements createElements;
        FindingWay findingWay;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CB_FieldSize.SelectedItem == null || CB_ObstaclesAmount.SelectedItem == null || CB_TargetsAmount.SelectedItem == null)
            {
                MessageBox.Show("Выбери че-нибудь");
                return;
            }

            k = 0;
            // считываем размер поля, количество препятствий и целей
            fieldSize = int.Parse(((TextBlock)CB_FieldSize.SelectedItem).Text);
            obstaclesAmount = int.Parse(((TextBlock)CB_ObstaclesAmount.SelectedItem).Text);
            targetsAmount = int.Parse(((TextBlock)CB_TargetsAmount.SelectedItem).Text);

            createElements = new CreateElements(fieldSize, obstaclesAmount, targetsAmount);
            // удаляем прошлое поле (FieldGrid)
            MainGrid.Children.RemoveAt(0);
            // вставляем на его место новосозданное 
            MainGrid.Children.Insert(0, createElements.CreateNewField());

            //создаем экземпляр класса, ищущий путь и вызываем его метод FindAlgorithm
            findingWay = new FindingWay(createElements.GetField(), fieldSize, obstaclesAmount, targetsAmount);
            List<int[,]> result = findingWay.FindAlgorithm();

            //если не добраться до цели
            if (result == null)
            {
                MessageBox.Show("Не добраться");
                createElements = null;
            }

            //загоны русского языка
            string ResMls = " миллисекунд";
            if (findingWay.time == 11 || findingWay.time == 12 || findingWay.time == 13 || findingWay.time == 14)
            {
                Lb_time.Content = findingWay.time.ToString() + ResMls;
                return;
            }
            switch (findingWay.time % 10)
            {
                case 1:
                    ResMls += "а";
                    break;
                case 2:
                case 3:
                case 4:
                    ResMls += "ы";
                    break;
            }

            Lb_time.Content = findingWay.time.ToString() + ResMls;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (createElements == null) return;

            // удаляем прошлое поле (FieldGrid)
            MainGrid.Children.RemoveAt(0);
            // вставляем на его место следующее поле (переключаемся на "следующее состояние")
            MainGrid.Children.Insert(0, createElements.BuildField(findingWay.Algorithm[k]));
            if (k < findingWay.Algorithm.Count() - 1) k++;
        }

        private void CB_FieldSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // определяем размер поля из соответствуещего ComboBox
            ComboBox comboBox = (ComboBox)sender;
            fieldSize = int.Parse(((TextBlock)comboBox.SelectedItem).Text);
            var fieldIndex = comboBox.SelectedIndex;

            // создаем ComboBox определяющий кол-во препятсвий, зависещий от размера поля
            int maxObstacs = (int)Math.Round(fieldSize * 1.5);
            var obstacs = CB_ObstaclesAmount.SelectedIndex < fieldIndex ? CB_ObstaclesAmount.SelectedIndex : fieldIndex;
            CB_ObstaclesAmount.Items.Clear();
            for (int i = 4; i < maxObstacs + 1; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = i.ToString();
                CB_ObstaclesAmount.Items.Add(textBlock);
            }
            CB_ObstaclesAmount.SelectedIndex = obstacs;

            // создаем ComboBox определяющий кол-во целей, зависещий от размера поля
            var maxTargets = fieldSize + 3;
            var targets = CB_TargetsAmount.SelectedIndex < fieldIndex ? CB_TargetsAmount.SelectedIndex : fieldIndex;
            CB_TargetsAmount.Items.Clear();
            for (int i = 3; i < maxTargets; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = i.ToString();
                CB_TargetsAmount.Items.Add(textBlock);
            }
            CB_TargetsAmount.SelectedIndex = targets;
        }   
    }
}
