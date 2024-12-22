// Maze.cs
// 该文件定义了Maze类，管理迷宫的初始化、元数据加载、事件管理和瓦片操作。
using System;
using System.Collections.Generic;

namespace Modules.Environment
{
    // Maze类负责管理迷宫的网格布局和瓦片操作
    public class Maze
    {
        // 瓦片网格的二维数组
        private Tile[,] grid;

        // 迷宫的宽度和高度
        public int Width { get; private set; }
        public int Height { get; private set; }

        // 构造函数，初始化迷宫
        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new Tile[width, height];

            // 初始化每个瓦片
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Tile(x, y);
                }
            }
        }

        // 获取指定坐标的瓦片
        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return grid[x, y];
            }
            throw new ArgumentOutOfRangeException("坐标超出迷宫范围。");
        }

        // 获取指定半径范围内的瓦片
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

        // 添加事件到指定瓦片
        public void AddEventToTile(int x, int y, string eventDescription)
        {
            Tile tile = GetTile(x, y);
            tile.AddEvent(eventDescription);
        }

        // 移除指定瓦片的事件
        public void RemoveEventFromTile(int x, int y)
        {
            Tile tile = GetTile(x, y);
            tile.RemoveEvent();
        }

        // 打印迷宫结构（用于调试）
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

    // Tile类表示迷宫中的单个瓦片
    public class Tile
    {
        public int X { get; private set; } // 瓦片的X坐标
        public int Y { get; private set; } // 瓦片的Y坐标
        public string Symbol { get; set; } // 瓦片的符号（例如用于调试）
        public bool HasEvent { get; private set; } // 是否有事件
        public string EventDescription { get; private set; } // 事件描述

        // 构造函数
        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            Symbol = "."; // 默认符号为"."
            HasEvent = false;
            EventDescription = null;
        }

        // 添加事件到当前瓦片
        public void AddEvent(string eventDescription)
        {
            HasEvent = true;
            EventDescription = eventDescription;
            Symbol = "E"; // 用"E"表示有事件的瓦片
        }

        // 移除当前瓦片的事件
        public void RemoveEvent()
        {
            HasEvent = false;
            EventDescription = null;
            Symbol = "."; // 恢复默认符号
        }
    }
}
