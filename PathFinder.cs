// PathFinder.cs
// ���ļ�������PathFinder�࣬����ʵ�ֽ�ɫ���Թ��е�·���滮��
using System;
using System.Collections.Generic;
using Modules.Environment;

namespace Modules.PathFinding
{
    // PathFinder�ฺ�����Թ��н���·������
    public static class PathFinder
    {
        /// <summary>
        /// A*�㷨ʵ�֣����ڲ��Ҵ���㵽�յ�����·����
        /// </summary>
        /// <param name="maze">��ǰ���Թ�����</param>
        /// <param name="start">�����Ƭ���� (x, y)��</param>
        /// <param name="end">�յ���Ƭ���� (x, y)��</param>
        /// <returns>һ����Ƭ�б���ʾ����㵽�յ��·����</returns>
        public static List<Tile> FindPath(Maze maze, (int x, int y) start, (int x, int y) end)
        {
            var openSet = new PriorityQueue<TileNode>();
            var closedSet = new HashSet<(int x, int y)>();

            // ��ʼ�����ڵ�
            var startNode = new TileNode(maze.GetTile(start.x, start.y), null, 0, Heuristic(start, end));
            openSet.Enqueue(startNode);

            while (openSet.Count > 0)
            {
                // ��ȡ��ǰ������С�Ľڵ�
                var current = openSet.Dequeue();

                // ��������յ㣬�ؽ�·��������
                if (current.Tile.X == end.x && current.Tile.Y == end.y)
                {
                    return ReconstructPath(current);
                }

                // ����ǰ�ڵ���Ϊ�ѷ���
                closedSet.Add((current.Tile.X, current.Tile.Y));

                // �������ڽڵ�
                foreach (var neighbor in GetNeighbors(maze, current.Tile))
                {
                    if (closedSet.Contains((neighbor.X, neighbor.Y)))
                        continue; // �����ѷ��ʽڵ�

                    int tentativeG = current.G + 1; // ����ÿ���ƶ��Ĵ���Ϊ1

                    var neighborNode = new TileNode(neighbor, current, tentativeG, Heuristic((neighbor.X, neighbor.Y), end));

                    // ����ھӲ��ڿ��ż��������
                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Enqueue(neighborNode);
                    }
                }
            }

            // ����Ҳ���·�������ؿ��б�
            return new List<Tile>();
        }

        /// <summary>
        /// ��ȡָ����Ƭ��������Ƭ��
        /// </summary>
        /// <param name="maze">�Թ�����</param>
        /// <param name="tile">��ǰ��Ƭ��</param>
        /// <returns>������Ƭ���б�</returns>
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
        /// ��������ʽ����ֵ��ʹ�������پ��룩��
        /// </summary>
        /// <param name="current">��ǰ�ڵ�����ꡣ</param>
        /// <param name="goal">Ŀ��ڵ�����ꡣ</param>
        /// <returns>��ǰ�ڵ㵽Ŀ��ڵ�Ĺ�����롣</returns>
        private static int Heuristic((int x, int y) current, (int x, int y) goal)
        {
            return Math.Abs(current.x - goal.x) + Math.Abs(current.y - goal.y);
        }

        /// <summary>
        /// �ؽ�·�������յ���ݵ���㡣
        /// </summary>
        /// <param name="node">�յ�ڵ㡣</param>
        /// <returns>����·����Ƭ���б�</returns>
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

    // TileNode�࣬����A*�㷨�еĽڵ��ʾ
    internal class TileNode : IComparable<TileNode>
    {
        public Tile Tile { get; } // ��ǰ�ڵ��Ӧ����Ƭ
        public TileNode Parent { get; } // ���ڵ�
        public int G { get; } // ����㵽��ǰ�ڵ�Ĵ���
        public int F => G + H; // �ܴ��ۣ�G + H��
        public int H { get; } // ����ʽ����

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
