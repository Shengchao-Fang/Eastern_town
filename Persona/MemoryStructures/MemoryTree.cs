// MemoryTree.cs
// ���ļ�������MemoryTree�࣬�����ɫ�Ŀռ���䡣
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    // MemoryTree����չ��֧�ּ������ȼ��͹��ڴ���
    public class MemoryTree
    {
        // ���νṹ�洢��������
        public Dictionary<string, object> Tree { get; private set; }

        // ���캯�������ر���ļ������ļ����ʼ������
        public MemoryTree(string savedFilePath)
        {
            Tree = File.Exists(savedFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(savedFilePath))
                : new Dictionary<string, object>();
        }

        // �����������ָ���ļ�·��
        public void Save(string outputPath)
        {
            File.WriteAllText(outputPath, JsonConvert.SerializeObject(Tree, Formatting.Indented));
        }

        // ��Ӽ�����Ŀ���������ȼ��͹���ʱ��
        public void AddMemory(string key, object value, int priority = 1, DateTime? expiration = null)
        {
            var memory = new Dictionary<string, object>
            {
                { "value", value },
                { "priority", priority },
                { "expiration", expiration }
            };
            Tree[key] = memory;
        }

        // �Ƴ������ѹ��ڵļ�����Ŀ
        public void RemoveExpiredMemories()
        {
            foreach (var key in new List<string>(Tree.Keys))
            {
                if (Tree[key] is Dictionary<string, object> memory && memory.ContainsKey("expiration"))
                {
                    DateTime? expiration = memory["expiration"] as DateTime?;
                    if (expiration.HasValue && expiration.Value < DateTime.Now)
                    {
                        Tree.Remove(key);
                    }
                }
            }
        }

        // ��ȡ���ȼ���ߵļ�����Ŀ
        public object GetHighestPriorityMemory()
        {
            object highestPriorityMemory = null;
            int highestPriority = int.MinValue;

            foreach (var key in Tree.Keys)
            {
                if (Tree[key] is Dictionary<string, object> memory && memory.ContainsKey("priority"))
                {
                    int priority = (int)memory["priority"];
                    if (priority > highestPriority)
                    {
                        highestPriority = priority;
                        highestPriorityMemory = memory["value"];
                    }
                }
            }

            return highestPriorityMemory;
        }

        // ��ӡ�����������Ľṹ
        public void PrintTree()
        {
            PrintTreeRecursive(Tree, 0);
        }

        // �ݹ��ӡ������
        // ���룺
        //   - tree: ��ǰ�ļ���������
        //   - depth: ��ǰ��������ȣ�����������ʾ��
        private void PrintTreeRecursive(Dictionary<string, object> tree, int depth)
        {
            // ʹ�����ۺű�ʾ���Ĳ�νṹ
            string dash = new string('-', depth * 2);
            foreach (var entry in tree)
            {
                // ��ӡ��ǰ��
                Console.WriteLine($"{dash} {entry.Key}");

                // ���ֵ����������ݹ��ӡ
                if (entry.Value is Dictionary<string, object> subTree)
                {
                    PrintTreeRecursive(subTree, depth + 1);
                }
                // ���ֵ���б���ֱ�Ӵ�ӡ����
                else if (entry.Value is List<object> list)
                {
                    Console.WriteLine($"{dash} {string.Join(", ", list)}");
                }
                // ���ֵ���������ͣ���ֱ�Ӵ�ӡֵ
                else
                {
                    Console.WriteLine($"{dash} {entry.Value}");
                }
            }
        }
    }
}
