using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Modules.MemoryStructures
{
    public class Scratch
    {
        //属性：视野半径
        public int VisionRadius { get; set; } = 4;
        //属性：注意力带宽-表示对象可以同时处理多少个任务或信息的属性-可以决定角色在某一时刻可以关注多少个敌人或目标
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