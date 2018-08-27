using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
            if (Grid1.Children.Any())
            {
                AddNode.Visibility = Visibility.Collapsed;
            }
        }

        private void AddNodeOk_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 100,
            };
            rectangle.Fill=new SolidColorBrush()
            Grid grid = new Grid
            {
                Height = 100,
                Width = 100,
            };

        }
    }
}
