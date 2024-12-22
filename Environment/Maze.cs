// Maze.cs
// ���ļ�������Maze�࣬�����Թ��ĳ�ʼ����Ԫ���ݼ��ء��¼��������Ƭ������
using System;
using System.Collections.Generic;

namespace Modules.Environment
{
    // Maze�ฺ������Թ������񲼾ֺ���Ƭ����
    public class Maze
    {
        // ��Ƭ����Ķ�ά����
        private Tile[,] grid;

        // �Թ��Ŀ�Ⱥ͸߶�
        public int Width { get; private set; }
        public int Height { get; private set; }

        // ���캯������ʼ���Թ�
        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new Tile[width, height];

            // ��ʼ��ÿ����Ƭ
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Tile(x, y);
                }
            }
        }

        // ��ȡָ���������Ƭ
        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return grid[x, y];
            }
            throw new ArgumentOutOfRangeException("���곬���Թ���Χ��");
        }

        // ��ȡָ���뾶��Χ�ڵ���Ƭ
        public List<Tile> GetTilesInRadius(int centerX, int centerY, int radius)
        {
            var tilesInRadius = new List<Tile>();

            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (x >= 0 && x < Width && y >= 0 && y < Height)
                    {
                        int distance = Math.Abs(centerX - x) + Math.Abs(centerY - y);
                        if (distance <= radius)
                        {
                            tilesInRadius.Add(grid[x, y]);
                        }
                    }
                }
            }

            return tilesInRadius;
        }

        // ����¼���ָ����Ƭ
        public void AddEventToTile(int x, int y, string eventDescription)
        {
            Tile tile = GetTile(x, y);
            tile.AddEvent(eventDescription);
        }

        // �Ƴ�ָ����Ƭ���¼�
        public void RemoveEventFromTile(int x, int y)
        {
            Tile tile = GetTile(x, y);
            tile.RemoveEvent();
        }

        // ��ӡ�Թ��ṹ�����ڵ��ԣ�
        public void PrintMaze()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(grid[x, y].Symbol + " ");
                }
                Console.WriteLine();
            }
        }
    }

    // Tile���ʾ�Թ��еĵ�����Ƭ
    public class Tile
    {
        public int X { get; private set; } // ��Ƭ��X����
        public int Y { get; private set; } // ��Ƭ��Y����
        public string Symbol { get; set; } // ��Ƭ�ķ��ţ��������ڵ��ԣ�
        public bool HasEvent { get; private set; } // �Ƿ����¼�
        public string EventDescription { get; private set; } // �¼�����

        // ���캯��
        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            Symbol = "."; // Ĭ�Ϸ���Ϊ"."
            HasEvent = false;
            EventDescription = null;
        }

        // ����¼�����ǰ��Ƭ
        public void AddEvent(string eventDescription)
        {
            HasEvent = true;
            EventDescription = eventDescription;
            Symbol = "E"; // ��"E"��ʾ���¼�����Ƭ
        }

        // �Ƴ���ǰ��Ƭ���¼�
        public void RemoveEvent()
        {
            HasEvent = false;
            EventDescription = null;
            Symbol = "."; // �ָ�Ĭ�Ϸ���
        }
    }
}
