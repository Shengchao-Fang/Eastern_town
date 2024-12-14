// MemoryTree.cs
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    public class MemoryTree
    {
        public Dictionary<string, object> Tree { get; private set; }

        public MemoryTree(string savedFilePath)
        {
            Tree = File.Exists(savedFilePath) 
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(savedFilePath))
                : new Dictionary<string, object>();
        }

        public void Save(string outputPath)
        {
            File.WriteAllText(outputPath, JsonConvert.SerializeObject(Tree, Formatting.Indented));
        }

        public void PrintTree()
        {
            PrintTreeRecursive(Tree, 0);
        }

        private void PrintTreeRecursive(Dictionary<string, object> tree, int depth)
        {
            string dash = new string('-', depth * 2);
            foreach (var entry in tree)
            {
                Console.WriteLine($"{dash} {entry.Key}");
                if (entry.Value is Dictionary<string, object> subTree)
                {
                    PrintTreeRecursive(subTree, depth + 1);
                }
                else if (entry.Value is List<object> list)
                {
                    Console.WriteLine($"{dash} {string.Join(", ", list)}");
                }
            }
        }
    }
}