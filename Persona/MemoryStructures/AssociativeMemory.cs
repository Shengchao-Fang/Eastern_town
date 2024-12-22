// AssociativeMemory.cs
// ���ļ�������AssociativeMemory�࣬���ڹ����ɫ�Ĺ������䡣
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    // AssociativeMemory����չ��֧�ֹؼ���Ȩ�ص���
    public class AssociativeMemory
    {
        // �洢��������ĸ���ڵ㣬ÿ���ڵ��ʾһ��������Ŀ
        public Dictionary<string, ConceptNode> Nodes { get; private set; }
        // �洢�ؼ����������ǿ�ȵ�ӳ�䣬���ڿ��ټ�����ؼ���
        public Dictionary<string, double> KeywordStrength { get; private set; }

        // ���캯������ָ���ļ��м���Nodes��KeywordStrength
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

        // ����Nodes��KeywordStrength��ָ���ļ���
        public void Save(string outputFolderPath)
        {
            string nodesPath = Path.Combine(outputFolderPath, "nodes.json");
            File.WriteAllText(nodesPath, JsonConvert.SerializeObject(Nodes, Formatting.Indented));

            string keywordStrengthPath = Path.Combine(outputFolderPath, "kw_strength.json");
            File.WriteAllText(keywordStrengthPath, JsonConvert.SerializeObject(KeywordStrength, Formatting.Indented));
        }

        // �����ؼ��ʵ�Ȩ��
        public void AdjustKeywordStrength(string keyword, double adjustment)
        {
            if (KeywordStrength.ContainsKey(keyword))
            {
                KeywordStrength[keyword] += adjustment;
                if (KeywordStrength[keyword] < 0)
                {
                    KeywordStrength[keyword] = 0; // ȷ��Ȩ�ز�Ϊ��ֵ
                }
            }
            else
            {
                KeywordStrength[keyword] = adjustment;
            }
        }

        // ���ݹؼ��ʼ�����صĸ���ڵ�
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

    // ConceptNode���ʾһ����������Ľڵ�
    public class ConceptNode
    {
        public string NodeId { get; set; } // �ڵ�Ψһ��ʶ
        public string Type { get; set; } // �ڵ����ͣ����� thought / event / chat
        public DateTime Created { get; set; } // �ڵ㴴��ʱ��
        public DateTime? Expiration { get; set; } // �ڵ����ʱ�䣨��ѡ��
        public string Subject { get; set; } // ��������
        public string Predicate { get; set; } // ����ν��
        public string Object { get; set; } // �������
        public string Description { get; set; } // �ڵ�����
        public List<string> Keywords { get; set; } // �ڵ���صĹؼ���
    }
}
