using Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MindMap
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            AddNode.Visibility = Visibility.Visible;

            Windows.UI.Core.Preview.SystemNavigationManagerPreview.GetForCurrentView().CloseRequested +=
            (sender, args) =>
              {

              };
        }

        private void AddNodeOk_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string name = NewNodeName.Text;
            NewNodeDescription.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string description);
            Color color = Color.FromArgb(Convert.ToByte(alpha.Text), Convert.ToByte(red.Text), Convert.ToByte(green.Text), Convert.ToByte(blue.Text));

            Grid grid = new Grid
            {
                Height = 100,
                Width = 100,
            };
            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = new SolidColorBrush(color),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            TextBlock nameTextBlock = new TextBlock()
            {
                Text = name,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            TextBlock descriptionTextBlock = new TextBlock()
            {
                Text = description,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Collapsed
            };

            Grid1.Children.Add(grid);



            grid.Children.Add(rectangle);
            grid.Children.Add(nameTextBlock);
            grid.Children.Add(descriptionTextBlock);

            AddNode.Visibility = Visibility.Collapsed;
            EditNode.Hide();

            rectangle.DoubleTapped += (s, eargs) =>
              {
                  rectangle = s as Rectangle;
                  grid = rectangle.Parent as Grid;
                  nameTextBlock = grid.Children.Where(x => x is TextBlock).ElementAt(0) as TextBlock;
                  descriptionTextBlock = grid.Children.Where(x => x is TextBlock).ElementAt(1) as TextBlock;

                  EditNode.ShowAt(rectangle);
              };



        }

        private void AddNode_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditNode.ShowAt((Button)sender);
        }
    }
}
