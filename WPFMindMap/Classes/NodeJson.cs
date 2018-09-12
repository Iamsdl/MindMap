using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFMindMap.Classes
{
    class NodeJson
    {
        public string Title;
        public string Description;
        public byte Red;
        public byte Green;
        public byte Blue;
        public double Top;
        public double Left;
        public List<NodeJson> Children;

        internal Node ToNode(ContextMenu contextMenu)
        {
            Node node = new Node(Title, Description, Color.FromRgb(Red, Green, Blue), contextMenu, top: Top, left: Left);
            foreach (NodeJson nodeJson in Children)
            {
                Node childNode = nodeJson.ToNode(contextMenu);
                node.AddChild(childNode);
            }
            return node;
        }
    }
}