using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFMindMap.Classes
{
    class Node
    {
        public Node(string title, string description, Color color, Canvas mainCanvas) : this(title, description, color)
        {
            mainCanvas.Children.Add(Canvas);
        }
        public Node(string title, string description, Color color)
        {
            Rectangle = new Rectangle()
            {
                Height = 100,
                Width = 100,
                RadiusX = 13,
                RadiusY = 13,
            };
            TitleLabel = new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Canvas = new Canvas()
            {
                Height = 100,
                Width = 100
            };
            Canvas.Children.Add(Rectangle);
            Canvas.Children.Add(TitleLabel);

            Title = title;
            Description = description;
            Color = color;
            Top = 0;
            Left = 0;
            Name = "0";
            Children = new List<Node>();
        }

        public string Title
        {
            get
            {
                return TitleLabel.Content.ToString();
            }
            set
            {
                TitleLabel.Content = value;
            }
        }
        public string Description { get; set; }
        public Color Color
        {
            get
            {
                return (Rectangle.Fill as SolidColorBrush).Color;
            }
            set
            {
                Rectangle.Fill = new SolidColorBrush(value);
                Color labelColor = Color.Subtract(Color.FromRgb(255, 255, 255), value);
                labelColor.A = 100;
                TitleLabel.Foreground = new SolidColorBrush(labelColor);
            }
        }
        public double Top
        {
            get
            {
                return Canvas.GetTop(Canvas);
            }
            set
            {
                Canvas.SetTop(Canvas, Top);
            }
        }
        public double Left
        {
            get
            {
                return Canvas.GetLeft(Canvas);
            }
            set
            {
                Canvas.SetTop(Canvas, Left);
            }
        }

        private string Name { get; set; }
        private Canvas Canvas { get; set; }
        private Rectangle Rectangle { get; set; }
        private Label TitleLabel { get; set; }

        private Node Parent { get; set; }
        private List<Node> Children { get; set; }

        public void AddChild(Node node)
        {
            node.Parent = this;
            Children.Add(node);
            Canvas.Children.Add(node.Canvas);
        }
    }
}
