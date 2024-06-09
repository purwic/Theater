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
    /// Логика взаимодействия для ActorsPage.xaml
    /// </summary>
    public partial class ActorsPage : Page
    {

        public ActorsPage()
        {
            InitializeComponent();

            Update();
        }

        private void Update()
        {
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
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new PerformancesPage());
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
                        Core.DB.Actors.Remove((Actor) DG.SelectedItem);
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
                Actor Selected = (Actor) DG.SelectedItem;
                var cell = (TextBox) e.EditingElement;
                string Value = cell.Text;

                if (Selected.Name != null && Selected.Surname != null && Selected.Patronymic != null && Selected.Rank != null && Selected.Experience != null && Value != "")
                {
                    if ($"{e.Column.Header}" == "Name")
                        Selected.Name = Value;
                    else if ($"{e.Column.Header}" == "Surname")
                        Selected.Surname = Value;
                    else if ($"{e.Column.Header}" == "Patronymic")
                        Selected.Patronymic = Value;
                    else if ($"{e.Column.Header}" == "Rank")
                        Selected.Rank = Value;
                    else if ($"{e.Column.Header}" == "Experience")
                        Selected.Experience = Value;

                    Core.DB.SaveChanges();
                }

            } catch { }


            Update();
        }

    }
}
