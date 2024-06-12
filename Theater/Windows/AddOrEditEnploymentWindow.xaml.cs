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
using System.Windows.Shapes;
using Theater.Entities;

namespace Theater.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditEnploymentWindow.xaml
    /// </summary>
    public partial class AddOrEditEnploymentWindow : Window
    {



        private static void SetHint(TextBox textBox, string content)
        {
            var cueBannerBrush = new VisualBrush
            {
                AlignmentX = AlignmentX.Center,
                AlignmentY = AlignmentY.Center,
                Stretch = Stretch.None,
                Visual = new Label
                {
                    Content = content,
                    Foreground = Brushes.LightGray,
                    FontSize = 24,
                }
            };

            textBox.Style = new Style
            {
                TargetType = typeof(TextBox),
                Resources =
                {
                    { "CueBannerBrush", cueBannerBrush }
                },
                Triggers =
                {
                    new Trigger
                    {
                        Property = TextBox.TextProperty,
                        Value = string.Empty,
                        Setters =
                        {
                            new Setter { Property = TextBox.BackgroundProperty, Value = cueBannerBrush }
                        }
                    },
                    new Trigger
                    {
                        Property = TextBox.TextProperty,
                        Value = null,
                        Setters =
                        {
                            new Setter { Property = TextBox.BackgroundProperty, Value = cueBannerBrush }
                        }
                    },
                    new Trigger
                    {
                        Property = TextBox.IsKeyboardFocusedProperty,
                        Value = true,
                        Setters =
                        {
                            new Setter { Property = TextBox.BackgroundProperty, Value = Brushes.White }
                        }
                    }
                }
            };
        }


        private static TextBox CreateTextBox(string content)
        {
            TextBox tb = new TextBox()
            {
                Name = content + "TextBox",
                Margin = new Thickness(0, 7, 0, 0),
                Height = 36,
                BorderThickness = new Thickness(0),
                FontSize = 18,
                TextAlignment = TextAlignment.Center,
            };

            SetHint(tb, content);

            return tb;
        }




        public AddOrEditEnploymentWindow(string mode, Enployment enployment, DataGrid DG)
        {
            InitializeComponent();
            List<string> values = new List<string> { "Role", "Cost" };
            List<TextBox> text_boxes = new List<TextBox>();

            values.ForEach((it) => { text_boxes.Add(CreateTextBox(it)); });
            text_boxes.ForEach((it) => { MainStackPanel.Children.Add(it); });

            List<Actor> ActorsList = Core.DB.Actors.ToList();
            List<Performance> PerformancesList = Core.DB.Performances.ToList();

            ActorCB.ItemsSource = ActorsList;
            PerformanceCB.ItemsSource = PerformancesList;
            DG.SelectedItem = null;

            if (mode == "edit")
            {
                ActorCB.SelectedItem = enployment.Actor;
                PerformanceCB.SelectedItem = enployment.Performance;
                text_boxes[0].Text = enployment.Role;
                text_boxes[1].Text = enployment.Cost;

                bool IsNull = false;
                MainStackPanel.PreviewKeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        text_boxes.ForEach((it) => { if (it.Text == null || it.Text == "") IsNull = true; });
                        if (ActorCB.SelectedItem == null || PerformanceCB.SelectedItem == null) IsNull = true;

                        if (!IsNull)
                        {

                            enployment.ActorID = (ActorCB.SelectedItem as Actor).ActorID;
                            enployment.PerformanceID = (PerformanceCB.SelectedItem as Performance).PerformanceID;
                            enployment.Role = text_boxes[0].Text;
                            enployment.Cost = text_boxes[1].Text;

                            Core.DB.SaveChanges();
                            DG.ItemsSource = Core.DB.Enployments.ToList();

                        }
                        IsNull = false;
                        Close();
                    }
                };
            }

            if(mode == "add")
            {

                bool IsNull = false;
                MainStackPanel.KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        text_boxes.ForEach((it) => { if (it.Text == null || it.Text == "") IsNull = true; });
                        if (ActorCB.SelectedItem == null || PerformanceCB.SelectedItem == null) IsNull = true;

                        if (!IsNull)
                        {

                            Core.DB.Enployments.Add(new Enployment()
                            {
                                ActorID = (ActorCB.SelectedItem as Actor).ActorID,
                                PerformanceID = (PerformanceCB.SelectedItem as Performance).PerformanceID,
                                Role = text_boxes[0].Text,
                                Cost = text_boxes[1].Text
                            });

                            Core.DB.SaveChanges();
                            DG.ItemsSource = Core.DB.Enployments.ToList();
                            text_boxes.ForEach((it) => { it.Text = ""; });
                            ActorCB.SelectedItem = null;
                            PerformanceCB.SelectedItem = null;
                        }
                        IsNull = false;
                    }
                };
            }
        }
    }
}
