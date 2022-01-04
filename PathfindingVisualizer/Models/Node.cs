using System;

namespace PathfindingVisualizer.Models
{
    public class Node
    {
        public (int X, int Y) Position { get; set; }
        
        public Action<NodeState> ObservedState;

        private NodeState _state;
        public NodeState State
        {
            get => _state;
            set
            {
                _state = value;
                ObservedState?.Invoke(value);
            }
        }

        public bool IsWalkable => State is NodeState.Empty or NodeState.End or NodeState.Path;
        
        public int DistanceToTarget { get; set; }
        public int Cost { get; set; }
        public int Weight { get; set; } = 1;
        
        public int F
        {
            get
            {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                
                return -1;
            }
        }
        public Node Parent { set; get; }

        public bool SetStart()
        {
            switch (State)
            {
                case NodeState.Empty:
                case NodeState.Wall:
                    State = NodeState.Start;
                    return true;
                
                case NodeState.Start:
                    return false;
                case NodeState.End:
                    return false;
                default:
                    return false;
            }
        }
        
        public bool SetEnd()
        {
            switch (State)
            {
                case NodeState.Empty:
                case NodeState.Wall:
                    State = NodeState.End;
                    return true;
                
                case NodeState.Start:
                    return false;
                case NodeState.End:
                    return false;
                default:
                    return false;
            }
        }
        public void Toggle()
        {
            State = State switch
            {
                NodeState.Empty => NodeState.Wall,
                NodeState.Wall => NodeState.Empty,
                NodeState.Start => NodeState.Empty,
                NodeState.End => NodeState.Empty,
                _ => State
            };
        }
    }
}