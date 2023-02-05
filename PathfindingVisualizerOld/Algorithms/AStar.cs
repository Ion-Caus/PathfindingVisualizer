using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using PathfindingVisualizer.Models;

namespace PathfindingVisualizer.Algorithms
{
    public class AStar
    {
        public AStar(int delay, Node[][] grid)
        {
            Delay = delay;
            Grid = grid;
        }
        
        public AStar(Node[][] grid) : this(50, grid)
        { }

        public int Delay { get; init; }
        public Node[][] Grid { get; init; }
        
        private int GridRows => Grid?.First().Length ?? 0;
        private int GridCols => Grid?.Length ?? 0;
        

        public async Task<Stack<Node>> FindPathAsync(Node start, Node end)
        {
            var path = new Stack<Node>();
            var openList = new List<Node>();
            var closedList = new List<Node>();
            var current = start;

            // add start node to Open List
            openList.Add(start);

            while (openList.Any() &&
                   !closedList.Exists(node => node.Position == end.Position))
            {
                current = openList.First();
                openList.Remove(current);
                closedList.Add(current);
                ChangeNodeState(current, NodeState.Closed);
                
                var adjacencies = GetAdjacentNodes(current);
                
                foreach (var n in adjacencies)
                {
                    if (closedList.Contains(n) || !n.IsWalkable) continue;
                    
                    if (openList.Contains(n)) continue;
                    
                    n.Parent = current;
                    n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) +
                                         Math.Abs(n.Position.Y - end.Position.Y);
                    n.Cost = n.Weight + n.Parent.Cost;
                            
                    ChangeNodeState(n, NodeState.Open);
                    openList.Add(n);
                    //await Task.Delay(Delay);
                }

                openList = openList.OrderBy(node => node.F).ToList();
            }

            // construct path, if end was not closed return empty stack
            if (!closedList.Exists(node => node.Position == end.Position))
            {
                return new Stack<Node>();
            }

            // if all good, return path
            var temp = current;
            do
            {
                path.Push(temp);
                ChangeNodeState(temp, NodeState.Path);
                //await Task.Delay(Delay);
                temp = temp.Parent;
            } while (temp.Position != start.Position);
            
            return path;
        }

        private List<Node> GetAdjacentNodes(Node n)
        {
            var temp = new List<Node>();

            var row = n.Position.X;
            var col = n.Position.Y;

            if (row + 1 < GridRows)
            {
                temp.Add(Grid[col][row + 1]);
            }

            if (row - 1 >= 0)
            {
                temp.Add(Grid[col][row - 1]);
            }

            if (col - 1 >= 0)
            {
                temp.Add(Grid[col - 1][row]);
            }

            if (col + 1 < GridCols)
            {
                temp.Add(Grid[col + 1][row]);
            }

            return temp;
        }
        
        private void ChangeNodeState(Node n, NodeState newState) {
            if (n.State is NodeState.End or NodeState.Start) return;
            
            Grid[n.Position.Y][n.Position.X].State = newState;
        }
    }
}