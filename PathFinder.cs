// PathFinder.cs
// 该文件定义了PathFinder类，用于实现角色在迷宫中的路径规划。
using System;
using System.Collections.Generic;
using Modules.Environment;

namespace Modules.PathFinding
{
    // PathFinder类负责在迷宫中进行路径搜索
    public static class PathFinder
    {
        /// <summary>
        /// A*算法实现，用于查找从起点到终点的最短路径。
        /// </summary>
        /// <param name="maze">当前的迷宫对象。</param>
        /// <param name="start">起点瓦片坐标 (x, y)。</param>
        /// <param name="end">终点瓦片坐标 (x, y)。</param>
        /// <returns>一个瓦片列表，表示从起点到终点的路径。</returns>
        public static List<Tile> FindPath(Maze maze, (int x, int y) start, (int x, int y) end)
        {
            var openSet = new PriorityQueue<TileNode>();
            var closedSet = new HashSet<(int x, int y)>();

            // 初始化起点节点
            var startNode = new TileNode(maze.GetTile(start.x, start.y), null, 0, Heuristic(start, end));
            openSet.Enqueue(startNode);

            while (openSet.Count > 0)
            {
                // 获取当前代价最小的节点
                var current = openSet.Dequeue();

                // 如果到达终点，重建路径并返回
                if (current.Tile.X == end.x && current.Tile.Y == end.y)
                {
                    return ReconstructPath(current);
                }

                // 将当前节点标记为已访问
                closedSet.Add((current.Tile.X, current.Tile.Y));

                // 遍历相邻节点
                foreach (var neighbor in GetNeighbors(maze, current.Tile))
                {
                    if (closedSet.Contains((neighbor.X, neighbor.Y)))
                        continue; // 跳过已访问节点

                    int tentativeG = current.G + 1; // 假设每次移动的代价为1

                    var neighborNode = new TileNode(neighbor, current, tentativeG, Heuristic((neighbor.X, neighbor.Y), end));

                    // 如果邻居不在开放集，则添加
                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Enqueue(neighborNode);
                    }
                }
            }

            // 如果找不到路径，返回空列表
            return new List<Tile>();
        }

        /// <summary>
        /// 获取指定瓦片的相邻瓦片。
        /// </summary>
        /// <param name="maze">迷宫对象。</param>
        /// <param name="tile">当前瓦片。</param>
        /// <returns>相邻瓦片的列表。</returns>
        private static List<Tile> GetNeighbors(Maze maze, Tile tile)
        {
            var neighbors = new List<Tile>();
            var directions = new (int dx, int dy)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            foreach (var dir in directions)
            {
                int newX = tile.X + dir.dx;
                int newY = tile.Y + dir.dy;

                if (newX >= 0 && newX < maze.Width && newY >= 0 && newY < maze.Height)
                {
                    neighbors.Add(maze.GetTile(newX, newY));
                }
            }

            return neighbors;
        }

        /// <summary>
        /// 计算启发式函数值（使用曼哈顿距离）。
        /// </summary>
        /// <param name="current">当前节点的坐标。</param>
        /// <param name="goal">目标节点的坐标。</param>
        /// <returns>当前节点到目标节点的估算距离。</returns>
        private static int Heuristic((int x, int y) current, (int x, int y) goal)
        {
            return Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y);
        }

        /// <summary>
        /// 重建路径，从终点回溯到起点。
        /// </summary>
        /// <param name="node">终点节点。</param>
        /// <returns>包含路径瓦片的列表。</returns>
        private static List<Tile> ReconstructPath(TileNode node)
        {
            var path = new List<Tile>();

            while (node != null)
            {
                path.Add(node.Tile);
                node = node.Parent;
            }

            path.Reverse();
            return path;
        }
    }

    // TileNode类，用于A*算法中的节点表示
    internal class TileNode : IComparable<TileNode>
    {
        public Tile Tile { get; } // 当前节点对应的瓦片
        public TileNode Parent { get; } // 父节点
        public int G { get; } // 从起点到当前节点的代价
        public int F => G + H; // 总代价（G + H）
        public int H { get; } // 启发式代价

        public TileNode(Tile tile, TileNode parent, int g, int h)
        {
            Tile = tile;
            Parent = parent;
            G = g;
            H = h;
        }

        public int CompareTo(TileNode other)
        {
            return F.CompareTo(other.F);
        }

        public override bool Equals(object obj)
        {
            return obj is TileNode other && Tile.X == other.Tile.X && Tile.Y == other.Tile.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tile.X, Tile.Y);
        }
    }
}
