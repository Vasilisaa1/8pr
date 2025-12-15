using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Weather.Models;

namespace Weather.Element
{
    /// <summary>
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        public Item(Hour hour)
        {
            InitializeComponent();

            // Используем свойство Text вместо Content для TextBlock
            lHour.Text = hour.hour;
            lCondition.Text = hour.ToCondition();
            lHumidity.Text = hour.humidity + "%";
            lPrecType.Text = hour.ToPrecType();
            lTemp.Text = hour.temp.ToString() + "°C";
        }
    
    }
}
