using System.Drawing;
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

namespace WPFMindMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EmptyAddNode_Click(object sender, RoutedEventArgs e)
        {
            CreateNode createNode = new CreateNode();
            createNode.ShowDialog();
            if (createNode.DialogResult == true)
            {
                var red = Convert.ToByte(createNode.Red.Text);
                var green = Convert.ToByte(createNode.Green.Text);
                var blue = Convert.ToByte(createNode.Blue.Text);

                Color rectangleColor = Color.FromRgb(red, green, blue);
                Rectangle rectangle = new Rectangle()
                {
                    Height = 100,
                    Width = 100,
                    Fill = new SolidColorBrush(rectangleColor),
                    RadiusX = 13,
                    RadiusY = 13,
                    ContextMenu = FindResource("NodeContext") as ContextMenu
                };

                byte x = 255;

                Color labelColor = Color.FromRgb((byte)(x - red), (byte)(x - green), (byte)(x - blue));
                Label title = new Label()
                {
                    Content = createNode.Title.Text,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(labelColor)
                };

                TextRange textRange = new TextRange(createNode.Description.Document.ContentStart, createNode.Description.Document.ContentEnd);
                string richText = textRange.Text;

                Label description = new Label()
                {
                    Content = richText,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Visibility = Visibility.Collapsed
                };
                Grid grid = new Grid()
                {
                    Height = 100,
                    Width = 100
                };
                grid.Children.Add(rectangle);
                grid.Children.Add(title);
                grid.Children.Add(description);
                MainGrid.Children.Add(grid);
            }
        }

    }
}
