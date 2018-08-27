using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Classes
{
    public class Node
    {
        public Node(string name, string description, Color color)
        {
            Name = name;
            Description = description;
            Color = color;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Color Color { get; set; }
        public List<Node> Children { get; set; }
    }
}
