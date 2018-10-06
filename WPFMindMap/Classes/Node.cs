using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Documents;

namespace WPFMindMap.Classes
{
    class Node
    {
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
                labelColor.A = 255;
                TitleLabel.Foreground = new SolidColorBrush(labelColor);
            }
        }
        public double Top
        {
            get
            {
                return Convert.ToDouble(Canvas.GetValue(Canvas.TopProperty));
            }
            set
            {
                Canvas.SetValue(Canvas.TopProperty, value);
                Line.Y2 = value + 50;
                var angle = Math.Atan2(value, Left);
                (Line.Stroke as LinearGradientBrush).EndPoint = new Point(Math.Cos(angle) / 4 + 0.5, Math.Sin(angle) / 4 + 0.5);
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
                var angle = Math.Atan2(Top, value);
                (Line.Stroke as LinearGradientBrush).EndPoint = new Point(Math.Cos(angle) / 4 + 0.5, Math.Sin(angle) / 4 + 0.5);
            }
        }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }

        private Canvas Canvas { get; set; }
        private Rectangle Rectangle { get; set; }
        private Label TitleLabel { get; set; }
        private ContextMenu Menu { get; set; }
        private Line Line { get; set; }
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

        public Node(string title, string description, Color color, ContextMenu menu, Canvas mainCanvas) : this(title, description, color, menu)
        {
            mainCanvas.Children.Add(Canvas);
            Canvas.SetLeft(Canvas, 0);
            Canvas.SetTop(Canvas, 0);
        }
        public Node(string title, string description, Color color, ContextMenu menu, double top = 0, double left = 0, string name = "a", bool rootNode=false)
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
                VerticalContentAlignment = VerticalAlignment.Center,
                size
            };
            if (rootNode)
            {
                Menu = new ContextMenu();
                MenuItem editNode = new MenuItem()
                {
                    Name = "EditNode"
                };
                editNode.Click += EditNode_Click;
                MenuItem addChildNode = new MenuItem()
                {
                    Name = "AddChildNode"
                };
                addChildNode.Click += AddChildNode_Click; ;
                MenuItem deleteNode = new MenuItem()
                {
                    Name = "DeleteNode"
                };
                editNode.Click += EditNode_Click;
                Menu.Items.Add()
            }
            Canvas = new Canvas()
            {
                Height = 100,
                Width = 100
            };
            Line = new Line()
            {
                StrokeThickness = 10,
                Stroke = new LinearGradientBrush(color, Color.FromArgb(0, 0, 0, 0), new Point(0.5, 0.5), new Point(1, 1))
            };
            TitleLabel.ContextMenu = menu;
            TitleLabel.PreviewMouseMove += TitleLabel_PreviewMouseMove;
            Canvas.Children.Add(Rectangle);
            Canvas.Children.Add(TitleLabel);
            Title = title;
            Description = description;
            Color = color;
            Top = top;
            Left = left;
            Name = name ?? "a";

            Children = new List<Node>();
        }

        private void AddChildNode_Click(object sender, RoutedEventArgs e)
        {
            Label titleLabel = null;
            if (sender is MenuItem menuItem)
            {
                if (menuItem.CommandParameter is ContextMenu contextMenu)
                {
                    titleLabel = contextMenu.PlacementTarget as Label;
                }
            }
            if (titleLabel == null)
                return;

            Node parent = Find(titleLabel.Name);

            NodeDialog addChildDialog = new NodeDialog();
            addChildDialog.ShowDialog();

            if (addChildDialog.DialogResult == true)
            {
                string title = !string.IsNullOrWhiteSpace(addChildDialog.TitleTextBox.Text) ? addChildDialog.TitleTextBox.Text : "node";

                TextRange textRange = new TextRange(addChildDialog.Description.Document.ContentStart, addChildDialog.Description.Document.ContentEnd);
                string richText = textRange.Text;
                string description = !string.IsNullOrWhiteSpace(richText) ? richText : "node description";

                var red = Convert.ToByte(!string.IsNullOrWhiteSpace(addChildDialog.Red.Text) ? addChildDialog.Red.Text : "0");
                var green = Convert.ToByte(!string.IsNullOrWhiteSpace(addChildDialog.Red.Text) ? addChildDialog.Green.Text : "0");
                var blue = Convert.ToByte(!string.IsNullOrWhiteSpace(addChildDialog.Red.Text) ? addChildDialog.Blue.Text : "0");
                Color color = Color.FromRgb(red, green, blue);

                Node child = new Node(title, description, color, nodeContext);
                parent.AddChild(child);
            }
        }

        private void EditNode_Click(object sender, RoutedEventArgs e)
        {
            var test = this;
            Label title = null;
            if (sender is MenuItem menuItem)
            {
                if (menuItem.CommandParameter is ContextMenu contextMenu)
                {
                    title = contextMenu.PlacementTarget as Label;
                }
            }
            if (title == null)
                return;

            Node nodeToEdit = Find(title.Name);

            NodeDialog editNodeDialog = new NodeDialog();
            editNodeDialog.TitleTextBox.Text = nodeToEdit.Title;
            editNodeDialog.Description.Document.Blocks.Clear();
            editNodeDialog.Description.Document.Blocks.Add(new Paragraph(new Run(nodeToEdit.Description)));

            editNodeDialog.Red.Text = nodeToEdit.Color.R.ToString();
            editNodeDialog.Green.Text = nodeToEdit.Color.G.ToString();
            editNodeDialog.Blue.Text = nodeToEdit.Color.B.ToString();
            editNodeDialog.ShowDialog();

            if (editNodeDialog.DialogResult == true)
            {
                nodeToEdit.Title = !string.IsNullOrWhiteSpace(editNodeDialog.TitleTextBox.Text) ? editNodeDialog.TitleTextBox.Text : "node";

                TextRange textRange = new TextRange(editNodeDialog.Description.Document.ContentStart, editNodeDialog.Description.Document.ContentEnd);
                string richText = textRange.Text;
                nodeToEdit.Description = !string.IsNullOrWhiteSpace(richText) ? richText : "node description";

                var red = Convert.ToByte(!string.IsNullOrWhiteSpace(editNodeDialog.Red.Text) ? editNodeDialog.Red.Text : "0");
                var green = Convert.ToByte(!string.IsNullOrWhiteSpace(editNodeDialog.Red.Text) ? editNodeDialog.Green.Text : "0");
                var blue = Convert.ToByte(!string.IsNullOrWhiteSpace(editNodeDialog.Red.Text) ? editNodeDialog.Blue.Text : "0");
                nodeToEdit.Color = Color.FromRgb(red, green, blue);
            }
        }

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

            this.Canvas.Children.Add(child.Canvas);
            this.Canvas.Children.Add(child.Line);

            child.Line.X1 = 50;
            child.Line.Y1 = 50;
            child.Line.X2 = child.Left + 50;
            child.Line.Y2 = child.Top + 50;

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

        public void Rename(int index)
        {
            if (Parent != null)
            {
                Name = String.Format(Parent.Name + "{0,2:00}", index);
            }
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rename(i);
            }
        }
        public NodeJson ToNodeJson()
        {
            NodeJson nodeJson = new NodeJson
            {
                Name = Name,
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

        private void TitleLabel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Label titleLabel && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(titleLabel, new Point(Left, Top), DragDropEffects.Move);
            }
        }
    }
}
