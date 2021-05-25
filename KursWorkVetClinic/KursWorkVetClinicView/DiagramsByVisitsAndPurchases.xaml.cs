using KursWorkVetClinicBusinessLogic.BusinessLogic;
using System;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using KursWorkVetClinicBusinessLogic.BindingModels;
using System.Windows.Media;
using KursWorkVetClinicBusinessLogic.ViewModels;

namespace KursWorkVetClinicView
{
    public partial class DiagramsByVisitsAndPurchases : Window
    {
        private readonly StatisticsLogic statisticsLogic;
        public DiagramsByVisitsAndPurchases(StatisticsLogic _statisticsLogic)
        {
            InitializeComponent();
            statisticsLogic = _statisticsLogic;
        }
        private void Diagram_Loaded(object sender, RoutedEventArgs e)
        {
            cartesianChart.Visibility = Visibility.Hidden;
        }
        private void ChartCreation(List<StatisticsByVisitsAndPurchasesViewModel> statistics, string title)
        {
            SeriesCollection series = new SeriesCollection();
            ChartValues<int> amountPerDay = new ChartValues<int>();
            List<string> dates = new List<string>();

            foreach (var item in statistics)
            {
                amountPerDay.Add(item.AmountPerDay);
                dates.Add(item.Data);
            }

            var converter = new BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#000000");

            cartesianChart.AxisX.Clear();
            cartesianChart.AxisX.Add(new Axis()
            {
                Title = "\nДаты",
                FontSize = 20,
                Foreground = brush,
                Labels = dates
            });

            LineSeries eventLine = new LineSeries
            {
                Title = title,
                FontSize = 20,
                Values = amountPerDay
            };

            series.Add(eventLine);
            cartesianChart.Series = series;
            cartesianChart.LegendLocation = LegendLocation.Top;
            cartesianChart.Visibility = Visibility.Visible;
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
            if (SelectedDiagramComboBox.SelectedItem == null)
            {
                MessageBox.Show("Укажите диаграмму", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (SelectedDiagramComboBox.Text == "Покупки")
                {
                    var statistics = statisticsLogic.GetPurchases(new StatisticsBindingModel
                    {
                        DateFrom = dataFromDataPicker.SelectedDate,
                        DateTo = dataToDataPicker.SelectedDate
                    });
                    if (statistics != null)
                    {
                        dataGridView.Items.Clear();
                        dataGridView.ItemsSource = statistics;
                    }
                    AmountPerDayDataGridTextColumn.Header = "Выручка за день";
                    ChartCreation(statistics, "Выручка");
                }
                else
                {
                    var statistics = statisticsLogic.GetVisits(new StatisticsBindingModel
                    {
                        DateFrom = dataFromDataPicker.SelectedDate,
                        DateTo = dataToDataPicker.SelectedDate
                    });
                    if (statistics != null)
                    {
                        dataGridView.Items.Clear();
                        dataGridView.ItemsSource = statistics;
                    }
                    AmountPerDayDataGridTextColumn.Header = "Кол-во посещений за день";
                    ChartCreation(statistics, "Кол-во посещений");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}