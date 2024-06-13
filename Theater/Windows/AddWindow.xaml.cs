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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {

        // сложни метод, делает подсказку с текстом который задаётся аргументом content
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
                Margin = new Thickness(0, 7, 0, 0),
                Height = 36,
                BorderThickness = new Thickness(0),
                FontSize = 24,
                TextAlignment = TextAlignment.Center,
            };

            SetHint(tb, content);

            return tb;
        }

        public AddWindow(string type, DataGrid DG)
        {
            InitializeComponent();

            // тут помещены названия столбцов, каких именно определяется в switch дальше
            List<string> values = new List<string> { };
            List<TextBox> text_boxes = new List<TextBox> { };
            switch (type)
            {
                case "Actor":

                    values = new List<string> { "Имя", "Фамилия", "Отчество", "Звание", "Стаж" };
                    break;

                case "Performance":

                    values = new List<string> { "Название", "Год постановки", "Бюджет" };
                    break;
            }

            values.ForEach((it) => { text_boxes.Add(CreateTextBox(it)); });
            text_boxes.ForEach((it) => { MainStackPanel.Children.Add(it); });

            bool IsNull = false;
            MainStackPanel.KeyDown += (sender, e) =>
            {
                if(e.Key == Key.Enter)
                {
                    text_boxes.ForEach((it) => { if (it.Text == null || it.Text == "") IsNull = true; });


                    if (!IsNull)
                    {
                        switch (type) 
                        {
                            case "Actor":
                                Core.DB.Actors.Add(new Actor() 
                                {
                                    Name = text_boxes[0].Text, 
                                    Surname = text_boxes[1].Text,
                                    Patronymic = text_boxes[2].Text,
                                    Rank = text_boxes[3].Text,
                                    Experience = text_boxes[4].Text,
                                }); break;

                            case "Performance":
                                Core.DB.Performances.Add(new Performance()
                                {
                                    Title = text_boxes[0].Text,
                                    Year = text_boxes[1].Text,
                                    Budget = text_boxes[2].Text,
                                }); break;


                        }

                        Core.DB.SaveChanges();
                        switch (type)
                        {
                            case "Actor": DG.ItemsSource = Core.DB.Actors.ToList(); break;

                            case "Performance": DG.ItemsSource = Core.DB.Performances.ToList(); break;
                        }
                        text_boxes.ForEach((it) => { it.Text = ""; });
                    }
                    IsNull = false;
                }
            };
        }
    }
}
