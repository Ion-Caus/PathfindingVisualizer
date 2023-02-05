namespace PathfindingVisualizer.Models;

public class Node
{
    private const int DefaultWeight = 1;
    private const int DefaultCost = 1;
    private const int DefaultDistanceToTarget = -1;
    
    public (int Row, int Col) Position { get; }
    public NodeState State { get; set; } = NodeState.Empty;

    public Node? Parent { set; get; }
    public int DistanceToTarget { get; set; } = DefaultDistanceToTarget;
    public int Cost { get; set; } = DefaultCost;
    public int CostDistance => DistanceToTarget + Cost;
    public int Weight { get; set; } = DefaultWeight;

    public bool IsWalkable => State is NodeState.Empty or NodeState.End;

    public Node(int row, int col)
    {
        Position = (row, col);
    }

    public Node Clone(NodeState? state = null) =>
        new(Position.Row, Position.Col)
        {
            State = state ?? State,
            Parent = Parent,
            DistanceToTarget = DistanceToTarget,
            Cost = Cost,
            Weight = Weight
        };

    public void Reset()
    {
        Cost = DefaultCost;
        DistanceToTarget = DefaultDistanceToTarget;
        Weight = DefaultWeight;
        Parent = null;
    }
}