// AssociativeMemory.cs
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    public class AssociativeMemory
    {
        public Dictionary<string, ConceptNode> Nodes { get; private set; }
        public Dictionary<string, double> KeywordStrength { get; private set; }

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

        public void Save(string outputFolderPath)
        {
            string nodesPath = Path.Combine(outputFolderPath, "nodes.json");
            File.WriteAllText(nodesPath, JsonConvert.SerializeObject(Nodes, Formatting.Indented));

            string keywordStrengthPath = Path.Combine(outputFolderPath, "kw_strength.json");
            File.WriteAllText(keywordStrengthPath, JsonConvert.SerializeObject(KeywordStrength, Formatting.Indented));
        }
    }

    public class ConceptNode
    {
        public string NodeId { get; set; }
        public string Type { get; set; } // thought / event / chat
        public DateTime Created { get; set; }
        public DateTime? Expiration { get; set; }
        public string Subject { get; set; }
        public string Predicate { get; set; }
        public string Object { get; set; }
        public string Description { get; set; }
        public List<string> Keywords { get; set; }
    }
}