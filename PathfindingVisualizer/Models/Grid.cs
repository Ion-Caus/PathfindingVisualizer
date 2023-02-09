namespace PathfindingVisualizer.Models;

public class Grid
{
    public readonly int RowSize;
    public readonly int ColumnSize;

    public IEnumerable<IEnumerable<Node>> Rows => _grid;
    
    private readonly Node[][] _grid;
    
    private Grid(Node[][] grid)
    {
        _grid = grid;
        RowSize = grid.Length;
        ColumnSize = grid.FirstOrDefault()?.Length ?? 0;
    }
    
    public static Grid CreateGrid(int rowSize, int columnSize)
    {
        var grid = new Node[rowSize][];
        for (var row = 0; row < rowSize; row++)
        {
            grid[row] = new Node[columnSize];
            for (var col = 0; col < columnSize; col++)
            {
                grid[row][col] = new Node(row, col);
            }
        }
        return new Grid(grid);
    }
    
    public Node[] this[int index] => _grid[index];
    
    public void Clear()
    {
        foreach (var row in _grid)
        {
            foreach (var node in row)
            {
                node.Reset();

                if (node.State is NodeState.Start or NodeState.End) continue;

                node.State = NodeState.Empty;
            }
        }
    }

    public Grid Clone()
    {
        var newGrid = new Node[RowSize][];
        for (var row = 0; row < RowSize; row++)
        {
            newGrid[row] = new Node[ColumnSize];
            for (var col = 0; col < ColumnSize; col++)
            {
                newGrid[row][col] = _grid[row][col].Clone();
            }
        }

        return new Grid(newGrid);
    }
    
    public void UpdateNode((int Row, int Col) position, Node newNode)
    {
        var (row, col) = position;
        var node = _grid[row][col];
        
        node.DistanceToTarget = newNode.DistanceToTarget;
        node.Cost = newNode.Cost;
        node.Parent = node.Parent;

        if (node.State is NodeState.Wall or NodeState.Start or NodeState.End) return;
        
        node.State = newNode.State;
    }
}