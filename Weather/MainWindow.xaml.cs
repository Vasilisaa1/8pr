using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Weather.Classes;
using Weather.Element;
using Weather.Models;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataResponse _weatherData;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadWeatherAsync();
        }

        private async Task LoadWeatherAsync(string city = "Пермь")
        {
            try
            {
                ShowStatus("Загрузка...");

                // ВАЖНО: тут теперь один вызов в наш сервис с БД и лимитами
                _weatherData = await WeatherS.GetWeatherCached(city);

                UpdateUI();
            }
            catch (Exception ex)
            {
                ShowStatus($"Ошибка: {ex.Message}");
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                HideStatus();
            }
        }

        private void UpdateUI()
        {
            if (_weatherData?.forecasts == null) return;

            DaysComboBox.Items.Clear();
            foreach (var forecast in _weatherData.forecasts)
            {
                DaysComboBox.Items.Add(forecast.date.ToString("dd.MM.yyyy"));
            }

            if (DaysComboBox.Items.Count > 0)
                DaysComboBox.SelectedIndex = 0;
        }

        private void ShowForecast(int dayIndex)
        {
            WeatherPanel.Children.Clear();

            if (_weatherData?.forecasts == null ||
                dayIndex < 0 || dayIndex >= _weatherData.forecasts.Count)
                return;

            var forecast = _weatherData.forecasts[dayIndex];
            foreach (var hour in forecast.hours)
            {
                WeatherPanel.Children.Add(new Item(hour));
            }
        }

        private void ShowStatus(string message)
        {
            StatusText.Text = message;
            StatusText.Visibility = Visibility.Visible;
            WeatherPanel.Children.Clear();
        }

        private void HideStatus()
        {
            StatusText.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async void GetWeatherClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                await LoadWeatherAsync(CityTextBox.Text.Trim());
            }
        }

        private void DaySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DaysComboBox.SelectedIndex >= 0)
            {
                ShowForecast(DaysComboBox.SelectedIndex);
            }
        }
    }
}