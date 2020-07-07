using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace SDL.MindMap.NodeLibrary
{
    public abstract class NodeAbstract
    {
        protected Guid ID { get; set; }

        #region Title
        protected virtual string Title { get; set; }
        protected virtual double TitleSize { get; set; }
        protected virtual Color TitleColor { get; set; }
        #endregion

        protected virtual string Description { get; set; }

        #region Position
        protected virtual double X { get; set; }
        protected virtual double Y { get; set; }
        #endregion

        #region Roundness
        protected virtual double RadiusX { get; set; }
        protected virtual double RadiusY { get; set; }
        #endregion

        #region Rectangle
        protected virtual double Width { get; set; }
        protected virtual double Height { get; set; }
        protected virtual Color Color { get; set; }
        #endregion

        #region Relationship
        protected NodeAbstract Parent { get; set; }
        protected List<NodeAbstract> Children { get; set; }
        #endregion

        protected NodeAbstract()
        {
            ID = Guid.NewGuid();
            Children = new List<NodeAbstract>();
        }

        protected virtual void Add(NodeAbstract child)
        {
            child.Parent = this;

            Children.Add(child);
        }

        protected virtual void Add(List<NodeAbstract> children)
        {
            foreach (var child in children)
            {
                Add(child);
            }
        }

        protected virtual void Delete(bool withChildren)
        {
            if (withChildren)
            {
                foreach (var child in Children)
                {
                    child.Delete(withChildren);
                }
                Children.RemoveAll(x => true);
                Children = null;
            }
            else
            {
                AddChildrenToParent();
                Delete(withChildren);
            }
        }

        protected virtual void AddChildrenToParent()
        {
            Parent.Add(Children);
        }
        
        protected virtual void MoveBy(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }

        protected virtual void MoveTo(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}