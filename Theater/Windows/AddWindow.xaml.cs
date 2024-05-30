﻿using System;
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

namespace Theater.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private string type;

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
                Name = content + "TextBox",
                Margin = new Thickness(0, 7, 0, 0),
                Height = 36,
                BorderThickness = new Thickness(0),
                FontSize = 24,
                TextAlignment = TextAlignment.Center,
            };

            SetHint(tb, content);

            tb.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    // Выполнить действие при нажатии Enter

                }
            };

            return tb;
        }

        public AddWindow(string type)
        {
            InitializeComponent();

            this.type = type;

            // тут помещены названия столбцов, каких именно определяется в switch дальше
            List<string> values = new List<string> { };
            List<TextBox> text_boxes = new List<TextBox> { };
            switch (type)
            {
                case "Actor":

                    values = new List<string> { "Name", "Surname", "Patronymic", "Rank", "Experience" };
                    break;


            }

            values.ForEach((it) => { text_boxes.Add(CreateTextBox(it)); });

            text_boxes.ForEach((it) => { MainStackPanel.Children.Add(it); });
        }
    }
}
