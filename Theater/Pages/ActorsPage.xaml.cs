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
using Theater.Windows;

namespace Theater.Pages
{
    /// <summary>
    /// Логика взаимодействия для ActorsPage.xaml
    /// </summary>
    public partial class ActorsPage : Page
    {



        public ActorsPage()
        {
            InitializeComponent();

            DG.ItemsSource = Core.DB.Actors.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddWindow("Actor", DG).Show();
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
