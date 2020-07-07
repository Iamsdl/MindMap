using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SDL.MindMap.NodeLibrary
{
    public class NodeWPF : NodeAbstract
    {

        public NodeWPF() : base()
        {
        }

        protected override string Name
        {
            get { return base.Name; }
            set { base.Name = value; TitleLabel.Name = value; }
        }
        // internal override double X
        // {
        //     get { return base.X; }
        //     set
        //     {
        //         base.X = value;
        //         Canvas.SetValue(Canvas.LeftProperty, value);
        //         Line.X2 = value + 50;
        //         var angle = Math.Atan2(Y, value);
        //         (Line.Stroke as LinearGradientBrush).EndPoint = new Point(Math.Cos(angle) / 4 + 0.5, Math.Sin(angle) / 4 + 0.5);
        //     }
        // }
        // internal override double Y
        // {
        //     get { return base.Y; }
        //     set
        //     {
        //         base.Y = value;
        //         Canvas.SetValue(Canvas.TopProperty, value);
        //         Line.Y2 = value + 50;
        //         var angle = Math.Atan2(value, X);
        //         (Line.Stroke as LinearGradientBrush).EndPoint = new Point(Math.Cos(angle) / 4 + 0.5, Math.Sin(angle) / 4 + 0.5);
        //     }
        // }
        // internal override double RadiusX
        // {
        //     get { return base.RadiusX; }
        //     set
        //     {
        //         base.RadiusX = value;
        //         Rectangle.RadiusX = value;
        //     }
        // }
        // internal override double RadiusY
        // {
        //     get { return base.RadiusY; }
        //     set
        //     {
        //         base.RadiusY = value;
        //         Rectangle.RadiusY = value;
        //     }
        // }
        // internal override double Height
        // {
        //     get { return base.Height; }
        //     set
        //     {
        //         base.Height = value;
        //         Rectangle.Height = value;
        //     }
        // }
        // internal override double Width
        // {
        //     get { return base.Width; }
        //     set
        //     {
        //         base.Width = value;
        //         Rectangle.Width = value;
        //     }
        // }
        // internal override string Title
        // {
        //     get { return base.Title; }
        //     set
        //     {
        //         base.Title = value;
        //         TitleLabel.Content = value;
        //     }
        // }
        // internal override double FontSize
        // {
        //     get { return base.FontSize; }
        //     set
        //     {
        //         base.FontSize = value;
        //         TitleLabel.FontSize = value;
        //     }
        // }
        // internal override Color Color
        // {
        //     get { return base.Color; }
        //     set
        //     {
        //         base.Color = value;
        //         (Rectangle.Fill as SolidColorBrush).Color = value;
        //         Color labelColor = Color.Subtract(Color.FromRgb(255, 255, 255), value);
        //         labelColor.A = 255;
        //         TitleLabel.Foreground = new SolidColorBrush(labelColor);
        //     }
        // }
        // internal override BaseNode Parent { get => base.Parent; set => base.Parent = value; }
        // internal override List<BaseNode> Children { get => base.Children; set => base.Children = value; }

        private Canvas Canvas { get; set; }
        private Rectangle Rectangle { get; set; }
        private Label TitleLabel { get; set; }
        private ContextMenu Menu { get; set; }
        private Line Line { get; set; }

        public override void AddChild(BaseNode child)
        {
            if (child is WPFNode)
            {
                base.AddChild(child);
                WPFNode wpfChild = child as WPFNode;
                Canvas.Children.Add(wpfChild.Canvas);
                if (Name != "a")
                {
                    Canvas.Children.Add(wpfChild.Line);
                }
            }

        }

        public override void Delete(bool deleteChildren)
        {
            if (Parent is WPFNode)
            {
                WPFNode wpfParent = Parent as WPFNode;
                wpfParent.Canvas.Children.Remove(Canvas);
                wpfParent.Canvas.Children.Remove(Line);
                base.Delete(deleteChildren);
            }

        }

        public override void MakeChildOf(BaseNode parent)
        {
            base.MakeChildOf(parent);
        }

        public override void MoveBy(double dx, double dy)
        {
            base.MoveBy(dx, dy);
        }

        public override void MoveTo(double x, double y)
        {
            base.MoveTo(x, y);
        }

        public override void Rename()
        {
            base.Rename();
        }
    }
}
