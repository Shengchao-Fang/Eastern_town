using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    public class Scratch
    {
        public int VisionRadius { get; set; } = 4;
        public int AttentionBandwidth { get; set; } = 3;
        public DateTime? CurrentTime { get; set; }
        public (int x, int y)? CurrentTile { get; set; }

        public List<string> DailyPlan { get; set; } = new List<string>();

        public Scratch(string savedFilePath)
        {
            if (File.Exists(savedFilePath))
            {
                var data = JsonConvert.DeserializeObject<Scratch>(File.ReadAllText(savedFilePath));
                VisionRadius = data.VisionRadius;
                AttentionBandwidth = data.AttentionBandwidth;
                CurrentTime = data.CurrentTime;
                CurrentTile = data.CurrentTile;
                DailyPlan = data.DailyPlan;
            }
        }

        public void Save(string outputPath)
        {
            File.WriteAllText(outputPath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}