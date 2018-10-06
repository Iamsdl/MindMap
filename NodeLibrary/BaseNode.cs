using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace SDL.MindMap.NodeLibrary
{
    public class BaseNode
    {
        internal virtual string Name { get; set; }
        internal virtual double X { get; set; }
        internal virtual double Y { get; set; }
        internal virtual double RadiusX { get; set; }
        internal virtual double RadiusY { get; set; }
        internal virtual double Height { get; set; }
        internal virtual double Width { get; set; }
        internal virtual string Title { get; set; }
        internal virtual string Description { get; set; }
        internal virtual double FontSize { get; set; }
        internal virtual Color Color { get; set; }
        internal virtual BaseNode Parent { get; set; }
        internal virtual List<BaseNode> Children { get; set; }

        public BaseNode()
        {
            Children = new List<BaseNode>();
        }

        public virtual void AddChild(BaseNode child)
        {
            child.X = X + Width + 10;
            child.Name = String.Format(Name + "{0,2:00}", Children.Count);
            child.Parent = this;
            Children.Add(child);
        }
        public virtual void Delete(bool deleteChildren)
        {
            if (deleteChildren)
            {
                Parent.Children.Remove(this);
                Parent.Rename();
            }
            else
            {
                Parent.Children.Remove(this);
                while (Children.Any())
                {
                    BaseNode child = Children[0];
                    child.MakeChildOf(Parent);
                    child.Rename();
                }
            }
            Parent = null;
        }
        public virtual void Rename()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Name = String.Format(Name + "{0,2:00}", i);
                Children[i].Rename();
            }
        }
        public virtual void MakeChildOf(BaseNode parent)
        {
            Parent.Children.Remove(this);
            parent.AddChild(this);
        }
        public virtual void MoveTo(double x, double y)
        {
            X = x;
            Y = y;
        }
        public virtual void MoveBy(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }
        internal virtual BaseNode Find(string name)
        {
            if (name == "a")
            {
                return this;
            }
            else
            {
                int index = Convert.ToInt32(name.Substring(1, 2));
                return Children[index].Find("a" + name.Substring(3));

            }
        }
    }
}
