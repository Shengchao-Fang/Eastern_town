// AssociativeMemory.cs
// 该文件定义了AssociativeMemory类，用于管理角色的关联记忆。
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    // AssociativeMemory类扩展，支持关键词权重调整
    public class AssociativeMemory
    {
        // 存储关联记忆的概念节点，每个节点表示一个记忆条目
        public Dictionary<string, ConceptNode> Nodes { get; private set; }
        // 存储关键词与其关联强度的映射，用于快速检索相关记忆
        public Dictionary<string, double> KeywordStrength { get; private set; }

        // 构造函数，从指定文件夹加载Nodes和KeywordStrength
        public AssociativeMemory(string savedFolderPath)
        {
            string nodesPath = Path.Combine(savedFolderPath, "nodes.json");
            Nodes = File.Exists(nodesPath)
                ? JsonConvert.DeserializeObject<Dictionary<string, ConceptNode>>(File.ReadAllText(nodesPath))
                : new Dictionary<string, ConceptNode>();

            string keywordStrengthPath = Path.Combine(savedFolderPath, "kw_strength.json");
            KeywordStrength = File.Exists(keywordStrengthPath)
                ? JsonConvert.DeserializeObject<Dictionary<string, double>>(File.ReadAllText(keywordStrengthPath))
                : new Dictionary<string, double>();
        }

        // 保存Nodes和KeywordStrength到指定文件夹
        public void Save(string outputFolderPath)
        {
            string nodesPath = Path.Combine(outputFolderPath, "nodes.json");
            File.WriteAllText(nodesPath, JsonConvert.SerializeObject(Nodes, Formatting.Indented));

            string keywordStrengthPath = Path.Combine(outputFolderPath, "kw_strength.json");
            File.WriteAllText(keywordStrengthPath, JsonConvert.SerializeObject(KeywordStrength, Formatting.Indented));
        }

        // 调整关键词的权重
        public void AdjustKeywordStrength(string keyword, double adjustment)
        {
            if (KeywordStrength.ContainsKey(keyword))
            {
                KeywordStrength[keyword] += adjustment;
                if (KeywordStrength[keyword] < 0)
                {
                    KeywordStrength[keyword] = 0; // 确保权重不为负值
                }
            }
            else
            {
                KeywordStrength[keyword] = adjustment;
            }
        }

        // 根据关键词检索相关的概念节点
        public List<ConceptNode> GetRelevantNodesByKeyword(string keyword)
        {
            var relevantNodes = new List<ConceptNode>();

            foreach (var node in Nodes.Values)
            {
                if (node.Keywords.Contains(keyword))
                {
                    relevantNodes.Add(node);
                }
            }

            return relevantNodes;
        }
    }

    // ConceptNode类表示一个关联记忆的节点
    public class ConceptNode
    {
        public string NodeId { get; set; } // 节点唯一标识
        public string Type { get; set; } // 节点类型，例如 thought / event / chat
        public DateTime Created { get; set; } // 节点创建时间
        public DateTime? Expiration { get; set; } // 节点过期时间（可选）
        public string Subject { get; set; } // 记忆主体
        public string Predicate { get; set; } // 记忆谓语
        public string Object { get; set; } // 记忆宾语
        public string Description { get; set; } // 节点描述
        public List<string> Keywords { get; set; } // 节点相关的关键词
    }
}
