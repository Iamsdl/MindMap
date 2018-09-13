using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFMindMap.Classes;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;

namespace WPFMindMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Path = "./tree.txt";
        private Node tree;
        private ContextMenu nodeContext;
        private double initialX = 0;
        private double initialY = 0;
        private double Scale
        {
            get
            {
                return ScaleTrans.ScaleX;
            }
            set
            {
                if (value > 0.1)
                {
                    ScaleTrans.ScaleX = value;
                    ScaleTrans.ScaleY = value;
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            nodeContext = (ContextMenu)FindResource("NodeContext");
            ScaleTrans.CenterX = ActualWidth / 2;
            ScaleTrans.CenterY = ActualHeight / 2;

            if (File.Exists(Path))
            {
                string json = File.ReadAllText(Path);
                NodeJson nodeJson = JsonConvert.DeserializeObject<NodeJson>(json);
                tree = nodeJson.ToNode(nodeContext);
                MainCanvas.Children.Add(tree.Canvas);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            NodeJson nodeJson = tree.ToNodeJson();
            string json = JsonConvert.SerializeObject(nodeJson);
            File.WriteAllText(Path, json);
            base.OnClosed(e);
        }

        private void EmptyAddNode_Click(object sender, RoutedEventArgs e)
        {
            if (tree == null)
            {
                CreateNode createNode = new CreateNode();
                createNode.ShowDialog();

                if (createNode.DialogResult == true)
                {
                    string title = !string.IsNullOrWhiteSpace(createNode.TitleTextBox.Text) ? createNode.TitleTextBox.Text : "node";

                    TextRange textRange = new TextRange(createNode.Description.Document.ContentStart, createNode.Description.Document.ContentEnd);
                    string richText = textRange.Text;
                    string description = !string.IsNullOrWhiteSpace(richText) ? richText : "node description";

                    var red = Convert.ToByte(!string.IsNullOrWhiteSpace(createNode.Red.Text) ? createNode.Red.Text : "0");
                    var green = Convert.ToByte(!string.IsNullOrWhiteSpace(createNode.Red.Text) ? createNode.Green.Text : "0");
                    var blue = Convert.ToByte(!string.IsNullOrWhiteSpace(createNode.Red.Text) ? createNode.Blue.Text : "0");
                    Color rectangleColor = Color.FromRgb(red, green, blue);

                    tree = new Node(title, description, rectangleColor, nodeContext, MainCanvas);
                }
            }
            else
            {
                MessageBox.Show("Root node already exists");
            }
        }
        private void EditNode_Click(object sender, RoutedEventArgs e)
        {
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

            Node nodeToEdit = tree.Find(title.Name);

            CreateNode editNode = new CreateNode();
            editNode.TitleTextBox.Text = nodeToEdit.Title;
            editNode.Description.Document.Blocks.Clear();
            editNode.Description.Document.Blocks.Add(new Paragraph(new Run(nodeToEdit.Description)));

            editNode.Red.Text = nodeToEdit.Color.R.ToString();
            editNode.Green.Text = nodeToEdit.Color.G.ToString();
            editNode.Blue.Text = nodeToEdit.Color.B.ToString();
            editNode.ShowDialog();

            if (editNode.DialogResult == true)
            {
                nodeToEdit.Title = !string.IsNullOrWhiteSpace(editNode.TitleTextBox.Text) ? editNode.TitleTextBox.Text : "node";

                TextRange textRange = new TextRange(editNode.Description.Document.ContentStart, editNode.Description.Document.ContentEnd);
                string richText = textRange.Text;
                nodeToEdit.Description = !string.IsNullOrWhiteSpace(richText) ? richText : "node description";

                var red = Convert.ToByte(!string.IsNullOrWhiteSpace(editNode.Red.Text) ? editNode.Red.Text : "0");
                var green = Convert.ToByte(!string.IsNullOrWhiteSpace(editNode.Red.Text) ? editNode.Green.Text : "0");
                var blue = Convert.ToByte(!string.IsNullOrWhiteSpace(editNode.Red.Text) ? editNode.Blue.Text : "0");
                nodeToEdit.Color = Color.FromRgb(red, green, blue);
            }
        }
        private void AddChild_Click(object sender, RoutedEventArgs e)
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

            Node parent = tree.Find(titleLabel.Name);

            CreateNode editNode = new CreateNode();
            editNode.ShowDialog();

            if (editNode.DialogResult == true)
            {
                string title = !string.IsNullOrWhiteSpace(editNode.TitleTextBox.Text) ? editNode.TitleTextBox.Text : "node";

                TextRange textRange = new TextRange(editNode.Description.Document.ContentStart, editNode.Description.Document.ContentEnd);
                string richText = textRange.Text;
                string description = !string.IsNullOrWhiteSpace(richText) ? richText : "node description";

                var red = Convert.ToByte(!string.IsNullOrWhiteSpace(editNode.Red.Text) ? editNode.Red.Text : "0");
                var green = Convert.ToByte(!string.IsNullOrWhiteSpace(editNode.Red.Text) ? editNode.Green.Text : "0");
                var blue = Convert.ToByte(!string.IsNullOrWhiteSpace(editNode.Red.Text) ? editNode.Blue.Text : "0");
                Color color = Color.FromRgb(red, green, blue);

                Node child = new Node(title, description, color, nodeContext);
                parent.AddChild(child);
            }
        }

        private void MainWindow_DragOver(object sender, DragEventArgs e)
        {
            if (e.Source is Label labelSource)
            {
                Node movingNode = tree.Find(labelSource.Name);
                if (movingNode == tree)
                {
                    movingNode.Move(ZeroLabel, e);
                }
                else
                {
                    movingNode.Move(e);
                }
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (initialX != 0)
                {
                    var deltaX = e.GetPosition(ZeroLabel).X - initialX;
                    var deltaY = e.GetPosition(ZeroLabel).Y - initialY;
                    tree.Top += deltaY;
                    tree.Left += deltaX;
                }
                initialX = e.GetPosition(ZeroLabel).X;
                initialY = e.GetPosition(ZeroLabel).Y;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            initialX = 0;
            initialY = 0;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                Scale += 0.1;
            }
            else
            {
                Scale -= 0.1;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScaleTrans.CenterX = ActualWidth / 2;
            ScaleTrans.CenterY = ActualHeight / 2;
        }

        private void DeleteNode_Click(object sender, RoutedEventArgs e)
        {
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

            Node nodeToDelete=tree.Find(title.Name);
            nodeToDelete.Delete();

        }
    }
}