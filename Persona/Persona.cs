// Persona.cs
// 该文件定义了Persona类，包含角色的核心逻辑与属性。
using System;
using System.Collections.Generic;
using Modules.MemoryStructures;
using Modules.CognitiveModules;

namespace Modules.Persona
{
    // Persona类表示一个角色，包含感知、记忆和行为的完整逻辑。
    public class Persona
    {
        public string Name { get; set; } // 角色名称

        // 角色的空间记忆模块
        public MemoryTree SpatialMemory { get; set; }

        // 角色的关联记忆模块
        public AssociativeMemory AssociativeMemory { get; set; }

        // 角色的短期记忆模块
        public Scratch ScratchMemory { get; set; }

        // 构造函数，初始化角色并加载记忆模块
        public Persona(string name, string memoryFolderPath)
        {
            Name = name;
            SpatialMemory = new MemoryTree($"{memoryFolderPath}/spatial_memory.json");
            AssociativeMemory = new AssociativeMemory($"{memoryFolderPath}/associative_memory");
            ScratchMemory = new Scratch($"{memoryFolderPath}/scratch_memory.json");
        }

        // Move方法是角色行为的主循环
        // 输入：
        //   - maze: 当前迷宫对象。
        //   - currentTime: 当前时间。
        // 输出：
        //   - 返回角色的执行结果。
        public string Move(Maze maze, DateTime currentTime)
        {
            // 检查是否是新的一天，触发反思
            if (ScratchMemory.CurrentTime.Date != currentTime.Date)
            {
                string reflection = CognitiveModules.Reflect(this);
                Console.WriteLine(reflection);
            }

            ScratchMemory.CurrentTime = currentTime;

            // 感知环境
            List<ConceptNode> perceived = CognitiveModules.Perceive(this, maze);

            // 检索记忆
            Dictionary<string, Dictionary<string, object>> retrieved = CognitiveModules.Retrieve(this, perceived);

            // 生成计划
            object plan = CognitiveModules.Plan(this, maze, retrieved);

            // 执行计划
            string executionResult = CognitiveModules.Execute(this, plan);

            return executionResult;
        }
    }
}
