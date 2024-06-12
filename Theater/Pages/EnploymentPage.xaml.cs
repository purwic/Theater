using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

/*
<ComboBox x:Name="myComboBox" ItemsSource="{Binding actorsList}" Height="20" Width="100">
    <ComboBox.ItemTemplate>
        <DataTemplate>
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="{Binding Name}" />
                <TextBlock Text=" "></TextBlock>
                <TextBlock Text="{Binding Surname}" />
                <TextBlock Text=" "></TextBlock>
                <TextBlock Text="{Binding Patronymic}" />
            </StackPanel>
        </DataTemplate>
    </ComboBox.ItemTemplate>
</ComboBox>
 */


namespace Theater.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnploymentPage.xaml
    /// </summary>
    public partial class EnploymentPage : Page
    {


        public EnploymentPage()
        {
            InitializeComponent();

            Update();
        }

        private void Update()
        {
            DG.ItemsSource = Core.DB.Enployments.ToList();
        }


        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new ActorsPage());
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new PerformancesPage());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddOrEditEnploymentWindow("add", null, DG).Show();
        }

        private void DG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (Enployment)DG.SelectedItem;
                if(Core.DB.Enployments.Find(item.EnploymentID) != null)
                    new AddOrEditEnploymentWindow("edit", item, DG).Show();
            }
            catch { }
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
                        Core.DB.Enployments.Remove((Enployment)DG.SelectedItem);
                        Core.DB.SaveChanges();
                    }
                    catch { }
                }

                Update();
            }
        }

        private void DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Update();
        }
    }
}
