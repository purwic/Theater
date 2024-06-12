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
using Theater.Entities;
using Theater.Windows;

namespace Theater.Pages
{
    /// <summary>
    /// Логика взаимодействия для PerformancesPage.xaml
    /// </summary>
    public partial class PerformancesPage : Page
    {
        public PerformancesPage()
        {
            InitializeComponent();

            Update();
        }

        private void Update()
        {
            DG.ItemsSource = Core.DB.Performances.ToList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddWindow("Performance", DG).Show();
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new ActorsPage());
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new EnploymentPage());
        }


        private void DG_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DG.SelectedItem.GetType();
                        Core.DB.Performances.Remove((Performance)DG.SelectedItem);
                        Core.DB.SaveChanges();
                    }
                    catch { }
                }

                Update();
            }
        }

        private void DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                Performance Selected = (Performance)DG.SelectedItem;
                var cell = (TextBox)e.EditingElement;
                string Value = cell.Text;

                if (Selected.Title != null && Selected.Year != null && Selected.Budget != null && Value != "")
                {
                    if ($"{e.Column.Header}" == "Title")
                        Selected.Title = Value;
                    else if ($"{e.Column.Header}" == "Year")
                        Selected.Year = Value;
                    else if ($"{e.Column.Header}" == "Budget")
                        Selected.Budget = Value;

                    Core.DB.SaveChanges();
                }

            }
            catch { }


            Update();
        }
    }
}
