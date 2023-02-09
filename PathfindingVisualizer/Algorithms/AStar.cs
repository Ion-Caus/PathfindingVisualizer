using PathfindingVisualizer.Models;

namespace PathfindingVisualizer.Algorithms;

public class AStar : IPathfinder
{
    private Grid Grid { get; }
    public bool IncludePathIntoSteps { get; set; } = true;
    public Queue<Node> Steps { get; }   // Used for visualizing the steps of the algorithm 
    public AStar(Grid grid)
    {
        Grid = grid.Clone();
        Steps = new Queue<Node>();
    }
    
    //TODO: improve the algorithm with a priority queue
    public Stack<Node> FindPath(Node start, Node end)
    {
        var openList = new List<Node>();
        var closedList = new List<Node>();

        var current = start;
        
        openList.Add(current);
        Steps.Enqueue(current.Clone(NodeState.Open));

        while (openList.Any() && closedList.All(n => n.Position != end.Position))
        {
            current = openList.First();

            openList.Remove(current);
            closedList.Add(current);
            Steps.Enqueue(current.Clone(NodeState.Closed)); 

            var adjacentNodes = GetAdjacentNodes(current);

            foreach (var n in adjacentNodes)
            {
                if (!n.IsWalkable) continue;
                
                if (closedList.Contains(n)
                    || openList.Contains(n)) continue;

                n.Parent = current;
                n.DistanceToTarget = Math.Abs(n.Position.Row - end.Position.Row) +
                                     Math.Abs(n.Position.Col - end.Position.Col);
                n.Cost = n.Weight + n.Parent.Cost;
                
                openList.Add(n);
                Steps.Enqueue(n.Clone(NodeState.Open));
            }

            openList = openList.OrderBy(n => n.CostDistance).ToList();
        }

        var path = new Stack<Node>();
        if (closedList.All(n => n.Position != end.Position)) return path;

        var temp = current;
        do
        {
            path.Push(temp);
            if (IncludePathIntoSteps) Steps.Enqueue(temp.Clone(NodeState.Path));
            
            temp = temp.Parent;
        } while (temp is not null && temp.Position != start.Position);

        return path;
    }

    private IEnumerable<Node> GetAdjacentNodes(Node node)
    {
        var row = node.Position.Row;
        var col = node.Position.Col;

        var adjacentNodes = new List<Node>();

        if (row > 0) adjacentNodes.Add(Grid[row - 1][col]);
        if (col > 0) adjacentNodes.Add(Grid[row][col - 1]);
        
        if (row + 1 < Grid.RowSize) adjacentNodes.Add(Grid[row + 1][col]);
        if (col + 1 < Grid.ColumnSize) adjacentNodes.Add(Grid[row][col + 1]);

        return adjacentNodes;
    }
}