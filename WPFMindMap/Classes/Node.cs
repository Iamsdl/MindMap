using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

namespace WPFMindMap.Classes
{
    class Node
    {
        public Node(string title, string description, Color color, ContextMenu menu, Canvas mainCanvas) : this(title, description, color, menu)
        {
            mainCanvas.Children.Add(Canvas);
        }
        public Node(string title, string description, Color color, ContextMenu menu)
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
                Height = 100,
                Width = 100,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            TitleLabel.ContextMenu = menu;
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
            Name = "";
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

        private string Name
        {
            get
            {
                return TitleLabel.Name;
            }
            set
            {
                TitleLabel.Name = value;
            }
        }
        private Canvas Canvas { get; set; }
        private Rectangle Rectangle { get; set; }
        private Label TitleLabel { get; set; }

        private Node Parent { get; set; }
        private List<Node> Children { get; set; }

        public Node Find(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return this;
            }
            else
            {
                ushort index = Convert.ToUInt16(name.Substring(0, 2));
                Node child = Children[index];
                return child.Find(name.Substring(2, name.Length - 2));
            }

        }
        public void AddChild(Node child)
        {
            child.Name = Name + string.Format("{00:1}", Children.Count + 1);
            child.Parent = this;
            Children.Add(child);
            Canvas.Children.Add(child.Canvas);
        }
    }
}
