// MemoryTree.cs
// 该文件定义了MemoryTree类，管理角色的空间记忆。
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    // MemoryTree类扩展，支持记忆优先级和过期处理
    public class MemoryTree
    {
        // 树形结构存储记忆数据
        public Dictionary<string, object> Tree { get; private set; }

        // 构造函数，加载保存的记忆树文件或初始化空树
        public MemoryTree(string savedFilePath)
        {
            Tree = File.Exists(savedFilePath)
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(savedFilePath))
                : new Dictionary<string, object>();
        }

        // 保存记忆树到指定文件路径
        public void Save(string outputPath)
        {
            File.WriteAllText(outputPath, JsonConvert.SerializeObject(Tree, Formatting.Indented));
        }

        // 添加记忆条目，包括优先级和过期时间
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

        // 移除所有已过期的记忆条目
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

        // 获取优先级最高的记忆条目
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

        // 打印整个记忆树的结构
        public void PrintTree()
        {
            PrintTreeRecursive(Tree, 0);
        }

        // 递归打印记忆树
        // 输入：
        //   - tree: 当前的记忆子树。
        //   - depth: 当前子树的深度，用于缩进表示。
        private void PrintTreeRecursive(Dictionary<string, object> tree, int depth)
        {
            // 使用破折号表示树的层次结构
            string dash = new string('-', depth * 2);
            foreach (var entry in tree)
            {
                // 打印当前键
                Console.WriteLine($"{dash} {entry.Key}");

                // 如果值是子树，则递归打印
                if (entry.Value is Dictionary<string, object> subTree)
                {
                    PrintTreeRecursive(subTree, depth + 1);
                }
                // 如果值是列表，则直接打印内容
                else if (entry.Value is List<object> list)
                {
                    Console.WriteLine($"{dash} {string.Join(", ", list)}");
                }
                // 如果值是其他类型，则直接打印值
                else
                {
                    Console.WriteLine($"{dash} {entry.Value}");
                }
            }
        }
    }
}
