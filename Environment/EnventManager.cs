// EventManager.cs
// 该文件定义了EventManager类，用于管理迷宫中的动态事件。
using System;
using System.Collections.Generic;
using Modules.Environment;

namespace Modules.Events
{
	// EventManager类负责管理迷宫瓦片上的动态事件
	public class EventManager
	{
		private readonly Maze maze;

		// 构造函数，初始化事件管理器
		public EventManager(Maze maze)
		{
			this.maze = maze;
		}

		/// <summary>
		/// 为指定瓦片添加事件。
		/// </summary>
		/// <param name="x">瓦片的X坐标。</param>
		/// <param name="y">瓦片的Y坐标。</param>
		/// <param name="eventDescription">事件描述。</param>
		public void AddEvent(int x, int y, string eventDescription)
		{
			var tile = maze.GetTile(x, y);
			tile.AddEvent(eventDescription);
		}

		/// <summary>
		/// 移除指定瓦片上的事件。
		/// </summary>
		/// <param name="x">瓦片的X坐标。</param>
		/// <param name="y">瓦片的Y坐标。</param>
		public void RemoveEvent(int x, int y)
		{
			var tile = maze.GetTile(x, y);
			tile.RemoveEvent();
		}

		/// <summary>
		/// 获取指定范围内的所有事件。
		/// </summary>
		/// <param name="centerX">中心瓦片的X坐标。</param>
		/// <param name="centerY">中心瓦片的Y坐标。</param>
		/// <param name="radius">搜索半径。</param>
		/// <returns>包含事件描述的列表。</returns>
		public List<string> GetEventsInRadius(int centerX, int centerY, int radius)
		{
			var events = new List<string>();
			var tiles = maze.GetTilesInRadius(centerX, centerY, radius);

			foreach (var tile in tiles)
			{
				if (tile.HasEvent)
				{
					events.Add(tile.EventDescription);
				}
			}

			return events;
		}

		/// <summary>
		/// 随机为迷宫中的瓦片添加事件。
		/// </summary>
		/// <param name="eventDescriptions">可能的事件描述列表。</param>
		/// <param name="eventCount">添加的事件数量。</param>
		public void GenerateRandomEvents(List<string> eventDescriptions, int eventCount)
		{
			var random = new Random();
			int width = maze.Width;
			int height = maze.Height;

			for (int i = 0; i < eventCount; i++)
			{
				int x = random.Next(0, width);
				int y = random.Next(0, height);

				if (!maze.GetTile(x, y).HasEvent)
				{
					string eventDescription = eventDescriptions[random.Next(eventDescriptions.Count)];
					AddEvent(x, y, eventDescription);
				}
			}
		}
	}
}
