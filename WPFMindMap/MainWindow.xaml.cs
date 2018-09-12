using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFMindMap.Classes;
using System.Windows.Input;
using System.Diagnostics;

namespace WPFMindMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Node node;
        private ContextMenu nodeContext;

        public MainWindow()
        {
            InitializeComponent();
            nodeContext = (ContextMenu)FindResource("NodeContext");
        }

        private void EmptyAddNode_Click(object sender, RoutedEventArgs e)
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

                node = new Node(title, description, rectangleColor, nodeContext, MainCanvas);
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

            Node nodeToEdit = Node.Find(node, title.Name);

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

            Node nodeToAddChild = Node.Find(node, titleLabel.Name);

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
                nodeToAddChild.AddChild(child);
            }
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            Debug.WriteLine(node.Top.ToString());
            Label labelSource = e.Source as Label;
            Node movingNode = Node.Find(node, labelSource.Name);
            movingNode.Top = e.GetPosition(this).Y - 50;
            movingNode.Left = e.GetPosition(this).X - 50;
        }
    }
}