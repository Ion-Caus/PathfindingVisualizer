namespace PathfindingVisualizer.Models
{
    public class Grid
    {
        public int Rows { get; set; } = 20;
        public int Columns { get; set; } = 35;
        
        public Node[][] Nodes { get; set; }
        
    }
}