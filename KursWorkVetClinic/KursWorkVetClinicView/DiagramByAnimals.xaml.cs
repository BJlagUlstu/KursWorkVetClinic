using KursWorkVetClinicBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System.Windows.Media;

namespace KursWorkVetClinicView
{
    public partial class DiagramByAnimals : Window
    {
        private readonly StatisticsLogic statisticsLogic;
        public DiagramByAnimals(StatisticsLogic _statisticsLogic)
        {
            InitializeComponent();
            statisticsLogic = _statisticsLogic;
        }
        private void ChartCreation(List<StatisticsByAnimalsViewModel> statistics)
        {
            SeriesCollection series = new SeriesCollection();
            ChartValues<int> numberOfVisits = new ChartValues<int>();
            List<string> animalsName = new List<string>();

            foreach (var item in statistics)
            {
                numberOfVisits.Add(item.NumberOfVisits);
                animalsName.Add(item.AnimalName);
            }

            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#000000");

            cartesianChart.AxisX.Clear();
            cartesianChart.AxisX.Add(new Axis()
            {
                Title = "\nЖивотные",
                FontSize = 20,
                Foreground = brush,
                Labels = animalsName
            });

            LineSeries animalLine = new LineSeries
            {
                Title = "Кол-во посещений: ",
                FontSize = 20,
                Values = numberOfVisits
            };

            series.Add(animalLine);
            cartesianChart.Series = series;
            cartesianChart.LegendLocation = LegendLocation.Top;
            cartesianChart.Visibility = Visibility.Visible;
        }
        private void Diagram_Loaded(object sender, RoutedEventArgs e)
        {
            cartesianChart.Visibility = Visibility.Hidden;
        }
        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            dataGridView.ItemsSource = null;
            if (dataFromDataPicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату начала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dataToDataPicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dataFromDataPicker.SelectedDate >= dataToDataPicker.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var statistics = statisticsLogic.GetAnimals(new StatisticsBindingModel
                {
                    DateFrom = dataFromDataPicker.SelectedDate,
                    DateTo = dataToDataPicker.SelectedDate
                });
                if (statistics != null)
                {
                    dataGridView.Items.Clear();
                    dataGridView.ItemsSource = statistics;
                }

                ChartCreation(statistics);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}