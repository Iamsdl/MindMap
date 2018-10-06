using System.Collections.Generic;
using System.Windows.Media;

namespace SDL.MindMap.NodeLibrary
{
    public abstract class AbstractNode
    {
        internal abstract string Name { get; set; }
        internal abstract double X { get; set; }
        internal abstract double Y { get; set; }
        internal abstract double RadiusX { get; set; }
        internal abstract double RadiusY { get; set; }
        internal abstract double Height { get; set; }
        internal abstract double Width { get; set; }
        internal abstract string Title { get; set; }
        internal abstract string Description { get; set; }
        internal abstract double FontSize { get; set; }
        internal abstract Color Color { get; set; }
        internal abstract AbstractNode Parent { get; set; }
        internal abstract List<AbstractNode> Children { get; set; }

        public abstract void AddChild(AbstractNode child);
        public abstract void Delete(bool deleteChildren);
        public abstract void Rename();
        public abstract void MakeChildOf(AbstractNode parent);
        public abstract void MoveBy(double dx, double dy);
        public abstract void MoveTo(double x, double y);
    }
}