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
using Classes;

namespace WPFMindMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Node tree;

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
                #region rectangle
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
                #endregion

                #region label
                byte x = 255;
                Color labelColor = Color.FromRgb((byte)(x - red), (byte)(x - green), (byte)(x - blue));
                Label title = new Label()
                {
                    Content = createNode.Title.Text,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(labelColor),
                    ContextMenu = FindResource("NodeContext") as ContextMenu
                };
                #endregion

                #region description
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
                #endregion

                #region grid
                Grid grid = new Grid()
                {
                    Height = 100,
                    Width = 100
                };
                grid.Children.Add(rectangle);
                grid.Children.Add(title);
                grid.Children.Add(description);
                #endregion

                MainGrid.Children.Add(grid);
            }
        }

        private void EditNode_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = null;
            if (sender is MenuItem menuItem)
            {
                if (menuItem.CommandParameter is ContextMenu contextMenu)
                {
                    rectangle = contextMenu.PlacementTarget as Rectangle;
                }
            }
            if (rectangle == null)
                return;

            Grid grid = rectangle.Parent as Grid;
            Label title = grid.Children[1] as Label;
            Label description = grid.Children[2] as Label;

            CreateNode editNode = new CreateNode();
            editNode.ShowDialog();
            editNode.Title.Text = title.Content.ToString();
            editNode.Description.Text = title.Content.ToString();
            editNode.Red.Text = (rectangle.Fill as SolidColorBrush).Color.R.ToString();
            editNode.Green.Text = (rectangle.Fill as SolidColorBrush).Color.G.ToString();
            editNode.Blue.Text = (rectangle.Fill as SolidColorBrush).Color.B.ToString();

            if (editNode.DialogResult == true)
            {
                #region rectangle
                var red = Convert.ToByte(editNode.Red.Text);
                var green = Convert.ToByte(editNode.Green.Text);
                var blue = Convert.ToByte(editNode.Blue.Text);
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
                #endregion

                #region label
                byte x = 255;
                Color labelColor = Color.FromRgb((byte)(x - red), (byte)(x - green), (byte)(x - blue));
                Label title = new Label()
                {
                    Content = editNode.Title.Text,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(labelColor),
                };
                #endregion

                #region description
                TextRange textRange = new TextRange(editNode.Description.Document.ContentStart, editNode.Description.Document.ContentEnd);
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
                #endregion

                #region grid
                Grid grid = new Grid()
                {
                    Height = 100,
                    Width = 100
                };
                grid.Children.Add(rectangle);
                grid.Children.Add(title);
                grid.Children.Add(description);
                #endregion
            }
        }
    }
}
