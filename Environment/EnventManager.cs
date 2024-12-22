// EventManager.cs
// ���ļ�������EventManager�࣬���ڹ����Թ��еĶ�̬�¼���
using System;
using System.Collections.Generic;
using Modules.Environment;

namespace Modules.Events
{
	// EventManager�ฺ������Թ���Ƭ�ϵĶ�̬�¼�
	public class EventManager
	{
		private readonly Maze maze;

		// ���캯������ʼ���¼�������
		public EventManager(Maze maze)
		{
			this.maze = maze;
		}

		/// <summary>
		/// Ϊָ����Ƭ����¼���
		/// </summary>
		/// <param name="x">��Ƭ��X���ꡣ</param>
		/// <param name="y">��Ƭ��Y���ꡣ</param>
		/// <param name="eventDescription">�¼�������</param>
		public void AddEvent(int x, int y, string eventDescription)
		{
			var tile = maze.GetTile(x, y);
			tile.AddEvent(eventDescription);
		}

		/// <summary>
		/// �Ƴ�ָ����Ƭ�ϵ��¼���
		/// </summary>
		/// <param name="x">��Ƭ��X���ꡣ</param>
		/// <param name="y">��Ƭ��Y���ꡣ</param>
		public void RemoveEvent(int x, int y)
		{
			var tile = maze.GetTile(x, y);
			tile.RemoveEvent();
		}

		/// <summary>
		/// ��ȡָ����Χ�ڵ������¼���
		/// </summary>
		/// <param name="centerX">������Ƭ��X���ꡣ</param>
		/// <param name="centerY">������Ƭ��Y���ꡣ</param>
		/// <param name="radius">�����뾶��</param>
		/// <returns>�����¼��������б�</returns>
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
		/// ���Ϊ�Թ��е���Ƭ����¼���
		/// </summary>
		/// <param name="eventDescriptions">���ܵ��¼������б�</param>
		/// <param name="eventCount">��ӵ��¼�������</param>
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
