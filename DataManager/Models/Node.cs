using System.Collections.Generic;

namespace DataManager.Models
{
    public class Node
    {
        public Node()
        {
            Contents = new List<Node>();
        }

        public string Title { get; set; }
        public List<Node> Contents { get; set; }
    }
}
