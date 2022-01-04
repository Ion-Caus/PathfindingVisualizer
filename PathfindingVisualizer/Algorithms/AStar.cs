using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PathfindingVisualizer.Models;

namespace PathfindingVisualizer.Algorithms
{
    public class AStar
    {
        public Node[][] Grid { get; set; }
        public int GridRows => Grid[0].Length;

        public int GridCols => Grid.Length;
        

        public async Task<Stack<Node>> FindPathAsync(Node start, Node end)
        {
            Stack<Node> path = new Stack<Node>();
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            List<Node> adjacencies;
            Node current = start;

            // add start node to Open List
            openList.Add(start);

            while (openList.Count != 0 &&
                   !closedList.Exists(node => node.Position == end.Position))
            {
                current = openList[0];
                openList.Remove(current);
                closedList.Add(current);
                ChangeNodeState(current, NodeState.Closed);
                
                adjacencies = GetAdjacentNodes(current);


                foreach (Node n in adjacencies)
                {

                    if (!closedList.Contains(n) && n.IsWalkable)
                    {
                        if (!openList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) +
                                                 Math.Abs(n.Position.Y - end.Position.Y);
                            n.Cost = n.Weight + n.Parent.Cost;
                            
                            ChangeNodeState(n, NodeState.Open);
                            openList.Add(n);
                            await Task.Delay(50);
                        }
                    }
                }

                openList = openList.OrderBy(node => node.F).ToList();
            }

            // construct path, if end was not closed return null
            if (!closedList.Exists(node => node.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Node temp = closedList[closedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                path.Push(temp);
                ChangeNodeState(temp, NodeState.Path);
                await Task.Delay(50);
                temp = temp.Parent;
            } while (temp != null && temp.Position != start.Position);
            
            return path;
        }

        private List<Node> GetAdjacentNodes(Node n)
        {
            List<Node> temp = new List<Node>();

            int row = n.Position.X;
            int col = n.Position.Y;

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