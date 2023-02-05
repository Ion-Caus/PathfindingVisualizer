using PathfindingVisualizer.Models;

namespace PathfindingVisualizer.Algorithms;

public interface IPathfinder
{
    Queue<Node> Steps { get; }
    
    Stack<Node> FindPath(Node start, Node end);
}