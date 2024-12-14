// Persona.cs
using System;
using System.Collections.Generic;
using Modules.MemoryStructures;
using Modules.CognitiveModules;

namespace Modules.Persona
{
    public class Persona
    {
        // 属性：名称
        public string Name { get; set; }
        // 属性：空间记忆
        public MemoryTree SpatialMemory { get; set; }
        // 属性：联想记忆
        public AssociativeMemory AssociativeMemory { get; set; }
        // 属性：临时记忆
        public Scratch ScratchMemory { get; set; }

        // 构造函数：初始化Persona对象
        public Persona(string name, string memoryFolderPath = null)
        {
            Name = name;
            // 初始化空间记忆
            SpatialMemory = new MemoryTree($"{memoryFolderPath}/bootstrap_memory/spatial_memory.json");
            // 初始化联想记忆
            AssociativeMemory = new AssociativeMemory($"{memoryFolderPath}/bootstrap_memory/associative_memory");
            // 初始化临时记忆
            ScratchMemory = new Scratch($"{memoryFolderPath}/bootstrap_memory/scratch.json");
        }

        // 感知方法：感知迷宫
        public List<ConceptNode> Perceive(Maze maze)
        {
            return CognitiveModules.Perceive(this, maze);
        }

        // 检索方法：根据感知结果进行检索
        public Dictionary<string, object> Retrieve(List<ConceptNode> perceived)
        {
            return CognitiveModules.Retrieve(this, perceived);
        }

        // 计划方法：根据检索结果制定计划
        public object Plan(Maze maze, Dictionary<string, Persona> personas, string newDay, Dictionary<string, object> retrieved)
        {
            return CognitiveModules.Plan(this, maze, personas, newDay, retrieved);
        }

        // 执行方法：执行计划
        public object Execute(Maze maze, Dictionary<string, Persona> personas, object plan)
        {
            return CognitiveModules.Execute(this, maze, personas, plan);
        }

        // 反思方法：进行反思
        public void Reflect()
        {
            CognitiveModules.Reflect(this);
        }

        // 移动方法：在迷宫中移动
        public object Move(Maze maze, Dictionary<string, Persona> personas, (int x, int y) currentTile, DateTime currentTime)
        {
            // 更新当前所在的格子
            ScratchMemory.CurrentTile = currentTile;
            // 判断是否是新的一天
            bool isNewDay = ScratchMemory.CurrentTime.Date != currentTime.Date;

            // 更新当前时间
            ScratchMemory.CurrentTime = currentTime;

            // 如果是新的一天，进行反思
            if (isNewDay)
            {
                Reflect();
            }

            // 感知迷宫
            var perceived = Perceive(maze);
            // 根据感知结果进行检索
            var retrieved = Retrieve(perceived);
            // 根据检索结果制定计划
            var plan = Plan(maze, personas, isNewDay ? "New day" : null, retrieved);

            // 执行计划
            return Execute(maze, personas, plan);
        }
    }
}