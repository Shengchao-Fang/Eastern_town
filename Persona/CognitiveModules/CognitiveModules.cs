// CognitiveModules.cs
// 该文件定义了CognitiveModules类，用于实现角色的认知模块逻辑，包括感知、检索、计划、执行和反思功能。
using System;
using System.Collections.Generic;
using Modules.Persona;
using Modules.MemoryStructures;
using Modules.Environment;

namespace Modules.CognitiveModules
{
    public static class CognitiveModules
    {
        /// <summary>
        /// Perceive方法负责处理角色对周围环境的感知。
        /// </summary>
        /// <param name="persona">当前角色对象。</param>
        /// <param name="maze">角色所在的迷宫对象。</param>
        /// <returns>返回角色感知到的概念节点列表。</returns>
        public static List<ConceptNode> Perceive(Persona persona, Maze maze)
        {
            List<ConceptNode> perceivedNodes = new List<ConceptNode>();

            // 遍历角色视野范围内的瓦片
            foreach (var tile in maze.GetTilesInRadius(
                persona.ScratchMemory.CurrentTile.x,
                persona.ScratchMemory.CurrentTile.y,
                persona.ScratchMemory.VisionRadius))
            {
                // 如果瓦片上存在事件，添加到感知节点
                if (tile.HasEvent)
                {
                    perceivedNodes.Add(new ConceptNode
                    {
                        NodeId = Guid.NewGuid().ToString(),
                        Type = "event",
                        Subject = tile.EventDescription,
                        Predicate = "located_at",
                        Object = tile.X + "," + tile.Y,
                        Description = tile.EventDescription,
                        Keywords = new List<string> { tile.EventDescription },
                        Created = DateTime.Now
                    });
                }
            }

            // 根据角色注意力带宽筛选感知节点
            int attentionBandwidth = persona.ScratchMemory.AttentionBandwidth;
            if (perceivedNodes.Count > attentionBandwidth)
            {
                perceivedNodes = perceivedNodes.GetRange(0, attentionBandwidth);
            }

            return perceivedNodes;
        }

        /// <summary>
        /// Retrieve方法根据感知到的事件检索角色的关联记忆。
        /// </summary>
        /// <param name="persona">当前角色对象。</param>
        /// <param name="perceived">感知到的概念节点列表。</param>
        /// <returns>返回一个包含相关事件和想法的字典。</returns>
        public static Dictionary<string, Dictionary<string, object>> Retrieve(Persona persona, List<ConceptNode> perceived)
        {
            Dictionary<string, Dictionary<string, object>> retrieved = new Dictionary<string, Dictionary<string, object>>();

            // 遍历每个感知的事件节点
            foreach (var eventNode in perceived)
            {
                var eventDetails = new Dictionary<string, object>();

                // 检索相关事件
                var relevantEvents = persona.AssociativeMemory.GetRelevantNodesByKeyword(eventNode.Description);
                eventDetails["events"] = relevantEvents;

                // 检索相关想法（示例）
                var relevantThoughts = persona.AssociativeMemory.GetRelevantNodesByKeyword("thought");
                eventDetails["thoughts"] = relevantThoughts;

                // 将当前事件节点信息添加到字典
                eventDetails["curr_event"] = eventNode;

                // 将该事件的所有相关信息存储在检索结果中
                retrieved[eventNode.Description] = eventDetails;
            }

            return retrieved;
        }

        /// <summary>
        /// Plan方法负责根据检索到的记忆和当前情况生成角色的行动计划。
        /// </summary>
        /// <param name="persona">当前角色对象。</param>
        /// <param name="maze">当前角色所在的迷宫对象。</param>
        /// <param name="retrieved">从记忆中检索到的相关事件和想法。</param>
        /// <returns>返回一个对象，表示生成的计划。</returns>
        public static object Plan(Persona persona, Maze maze, Dictionary<string, Dictionary<string, object>> retrieved)
        {
            var plan = new
            {
                Actions = new List<string>(),
                Reasoning = "Generated based on retrieved events and thoughts."
            };

            // 遍历检索到的事件和想法，根据内容生成行动
            foreach (var entry in retrieved)
            {
                var eventDescription = entry.Key;
                var eventDetails = entry.Value;

                // 基于事件生成简单行动计划（示例）
                plan.Actions.Add($"Investigate: {eventDescription}");
            }

            return plan;
        }

        /// <summary>
        /// Execute方法负责执行角色的行动计划。
        /// </summary>
        /// <param name="persona">当前角色对象。</param>
        /// <param name="plan">角色生成的行动计划。</param>
        /// <returns>返回执行结果，通常是一个描述执行情况的字符串。</returns>
        public static string Execute(Persona persona, object plan)
        {
            if (plan == null || !(plan is dynamic))
            {
                return "Invalid plan provided.";
            }

            // 示例执行逻辑
            var actions = plan.Actions as List<string>;
            if (actions == null || actions.Count == 0)
            {
                return "No actions to execute.";
            }

            string executionResult = "";
            foreach (var action in actions)
            {
                executionResult += $"Executed action: {action}\n";
            }

            return executionResult.Trim();
        }

        /// <summary>
        /// Reflect方法负责角色对一天的总结和反思。
        /// </summary>
        /// <param name="persona">当前角色对象。</param>
        /// <returns>返回一个字符串，表示反思内容。</returns>
        public static string Reflect(Persona persona)
        {
            string reflection = "Reflection for the day:\n";

            // 遍历记忆节点，提取当天的主要事件和想法
            foreach (var node in persona.AssociativeMemory.Nodes.Values)
            {
                if (node.Created.Date == DateTime.Now.Date)
                {
                    reflection += $"- {node.Description}\n";
                }
            }

            reflection += "\nEnd of day reflection.";
            return reflection;
        }
    }
}