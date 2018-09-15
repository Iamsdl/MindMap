using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Diagnostics;

namespace WPFMindMap.Classes
{
    class Node
    {
        public Node(string title, string description, Color color, ContextMenu menu, Canvas mainCanvas) : this(title, description, color, menu)
        {
            mainCanvas.Children.Add(Canvas);
            Canvas.SetLeft(Canvas, 0);
            Canvas.SetTop(Canvas, 0);
        }
        public Node(string title, string description, Color color, ContextMenu menu, double top = 0, double left = 0)
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
            TitleLabel.PreviewMouseMove += TitleLabel_PreviewMouseMove;
            Canvas = new Canvas()
            {
                Height = 100,
                Width = 100
            };
            Line = new Line()
            {
                StrokeThickness = 10,
                Stroke = new LinearGradientBrush(Color.FromArgb(255, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), new Point(0.5, 0.5), new Point(1, 1))
            };
            Canvas.Children.Add(Rectangle);
            Canvas.Children.Add(TitleLabel);
            Title = title;
            Description = description;
            Color = color;
            Top = top;
            Left = left;
            Name = "a";

            Children = new List<Node>();
        }

        internal void AddToWindow(Canvas mainCanvas)
        {
            mainCanvas.Children.Add(this.Canvas);
        }

        private void TitleLabel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Label titleLabel && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(titleLabel, new Point(Left, Top), DragDropEffects.Move);
            }
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
                return Canvas.GetTop(this.Canvas);
            }
            set
            {
                Canvas.SetValue(Canvas.TopProperty, value);
                Line.Y2 = value + 50;
                var angle = Math.Atan2(value, Left);
                Debug.WriteLine(angle * 180 / Math.PI);
                ((LinearGradientBrush)Line.Stroke).EndPoint = new Point(Math.Cos(angle) / 4 + 0.5, Math.Sin(angle) / 4 + 0.5);
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
                Canvas.SetValue(Canvas.LeftProperty, value);
                Line.X2 = value + 50;
            }
        }

        public Node Parent { get; set; }
        public List<Node> Children { get; set; }

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
        public Canvas Canvas { get; set; }
        private Rectangle Rectangle { get; set; }
        private Label TitleLabel { get; set; }
        private Line Line { get; set; }


        public Node Find(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return null;
            }
            if (name == "a")
            {
                return this;
            }
            else
            {
                int index = Convert.ToInt32(name.Substring(1, 2));
                Node child = this.Children[index];
                return child.Find("a" + name.Substring(3, name.Length - 3));
            }

        }

        public void Move(DragEventArgs e)
        {
            Top = e.GetPosition(Parent.TitleLabel).Y - 50;
            Left = e.GetPosition(Parent.TitleLabel).X - 50;
        }

        internal void Move(Label zeroLabel, DragEventArgs e)
        {
            Top = e.GetPosition(zeroLabel).Y - 50;
            Left = e.GetPosition(zeroLabel).X - 50;
        }

        public void AddChild(Node child)
        {
            child.Name = this.Name + string.Format("{0,2:00}", this.Children.Count);
            child.Parent = this;

            child.Line.X1 = 50;
            child.Line.Y1 = 50;
            child.Line.X2 = child.Left + 50;
            child.Line.Y2 = child.Top + 50;
            this.Canvas.Children.Add(child.Canvas);
            this.Canvas.Children.Add(child.Line);
            this.Children.Add(child);


            child.Line.SetValue(Canvas.ZIndexProperty, -999);
        }

        internal void Delete()
        {
            if (Parent != null)
            {
                Parent.Canvas.Children.Remove(Canvas);
                Parent.Canvas.Children.Remove(Line);

                Parent.Children.Remove(this);
                for (int i = 0; i < Parent.Children.Count; i++)
                {
                    if (Parent.Children[i].Name == String.Format(Parent.Name + "{0,2:00}", i))
                    {
                        continue;
                    }
                    else
                    {
                        Parent.Children[i].Rename(i);
                    }
                }
            }

        }

        private void Rename(int index)
        {
            Name = String.Format(Parent.Name + "{0,2:00}", index);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rename(i);
            }
        }

        public NodeJson ToNodeJson()
        {
            NodeJson nodeJson = new NodeJson
            {
                Title = Title,
                Description = Description,
                Top = Top,
                Left = Left,
                Red = Color.R,
                Green = Color.G,
                Blue = Color.B,
                Children = new List<NodeJson>()
            };
            foreach (Node node in Children)
            {
                NodeJson childNodeJson = node.ToNodeJson();
                nodeJson.Children.Add(childNodeJson);
            }
            return nodeJson;
        }
    }
}
