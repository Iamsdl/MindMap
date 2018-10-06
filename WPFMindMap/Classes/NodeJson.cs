using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFMindMap.Classes
{
    class NodeJson
    {
        public string Name;
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
            Node node = new Node(Title, Description, Color.FromRgb(Red, Green, Blue), contextMenu, top: Top, left: Left, name: Name);
            foreach (NodeJson nodeJson in Children)
            {
                Node childNode = nodeJson.ToNode(contextMenu);
                node.AddChild(childNode);
            }
            return node;
        }
    }
}