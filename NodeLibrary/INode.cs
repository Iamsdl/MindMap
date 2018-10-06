namespace SDL.MindMap.NodeLibrary
{
    public interface INode
    {
        void AddChild(INode child);
        void Delete(bool deleteChildren);
        void MakeChildOf(INode parent);
        void MoveBy(double dx, double dy);
        void MoveTo(double x, double y);
        void Rename();
    }
}