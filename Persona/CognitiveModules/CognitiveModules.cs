// CognitiveModules.cs
// ���ļ�������CognitiveModules�࣬����ʵ�ֽ�ɫ����֪ģ���߼���������֪���������ƻ���ִ�кͷ�˼���ܡ�
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
        /// Perceive�����������ɫ����Χ�����ĸ�֪��
        /// </summary>
        /// <param name="persona">��ǰ��ɫ����</param>
        /// <param name="maze">��ɫ���ڵ��Թ�����</param>
        /// <returns>���ؽ�ɫ��֪���ĸ���ڵ��б�</returns>
        public static List<ConceptNode> Perceive(Persona persona, Maze maze)
        {
            List<ConceptNode> perceivedNodes = new List<ConceptNode>();

            // ������ɫ��Ұ��Χ�ڵ���Ƭ
            foreach (var tile in maze.GetTilesInRadius(
                persona.ScratchMemory.CurrentTile.x,
                persona.ScratchMemory.CurrentTile.y,
                persona.ScratchMemory.VisionRadius))
            {
                // �����Ƭ�ϴ����¼�����ӵ���֪�ڵ�
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

            // ���ݽ�ɫע��������ɸѡ��֪�ڵ�
            int attentionBandwidth = persona.ScratchMemory.AttentionBandwidth;
            if (perceivedNodes.Count > attentionBandwidth)
            {
                perceivedNodes = perceivedNodes.GetRange(0, attentionBandwidth);
            }

            return perceivedNodes;
        }

        /// <summary>
        /// Retrieve�������ݸ�֪�����¼�������ɫ�Ĺ������䡣
        /// </summary>
        /// <param name="persona">��ǰ��ɫ����</param>
        /// <param name="perceived">��֪���ĸ���ڵ��б�</param>
        /// <returns>����һ����������¼����뷨���ֵ䡣</returns>
        public static Dictionary<string, Dictionary<string, object>> Retrieve(Persona persona, List<ConceptNode> perceived)
        {
            Dictionary<string, Dictionary<string, object>> retrieved = new Dictionary<string, Dictionary<string, object>>();

            // ����ÿ����֪���¼��ڵ�
            foreach (var eventNode in perceived)
            {
                var eventDetails = new Dictionary<string, object>();

                // ��������¼�
                var relevantEvents = persona.AssociativeMemory.GetRelevantNodesByKeyword(eventNode.Description);
                eventDetails["events"] = relevantEvents;

                // ��������뷨��ʾ����
                var relevantThoughts = persona.AssociativeMemory.GetRelevantNodesByKeyword("thought");
                eventDetails["thoughts"] = relevantThoughts;

                // ����ǰ�¼��ڵ���Ϣ��ӵ��ֵ�
                eventDetails["curr_event"] = eventNode;

                // �����¼������������Ϣ�洢�ڼ��������
                retrieved[eventNode.Description] = eventDetails;
            }

            return retrieved;
        }

        /// <summary>
        /// Plan����������ݼ������ļ���͵�ǰ������ɽ�ɫ���ж��ƻ���
        /// </summary>
        /// <param name="persona">��ǰ��ɫ����</param>
        /// <param name="maze">��ǰ��ɫ���ڵ��Թ�����</param>
        /// <param name="retrieved">�Ӽ����м�����������¼����뷨��</param>
        /// <returns>����һ�����󣬱�ʾ���ɵļƻ���</returns>
        public static object Plan(Persona persona, Maze maze, Dictionary<string, Dictionary<string, object>> retrieved)
        {
            var plan = new
            {
                Actions = new List<string>(),
                Reasoning = "Generated based on retrieved events and thoughts."
            };

            // �������������¼����뷨���������������ж�
            foreach (var entry in retrieved)
            {
                var eventDescription = entry.Key;
                var eventDetails = entry.Value;

                // �����¼����ɼ��ж��ƻ���ʾ����
                plan.Actions.Add($"Investigate: {eventDescription}");
            }

            return plan;
        }

        /// <summary>
        /// Execute��������ִ�н�ɫ���ж��ƻ���
        /// </summary>
        /// <param name="persona">��ǰ��ɫ����</param>
        /// <param name="plan">��ɫ���ɵ��ж��ƻ���</param>
        /// <returns>����ִ�н����ͨ����һ������ִ��������ַ�����</returns>
        public static string Execute(Persona persona, object plan)
        {
            if (plan == null || !(plan is dynamic))
            {
                return "Invalid plan provided.";
            }

            // ʾ��ִ���߼�
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
        /// Reflect���������ɫ��һ����ܽ�ͷ�˼��
        /// </summary>
        /// <param name="persona">��ǰ��ɫ����</param>
        /// <returns>����һ���ַ�������ʾ��˼���ݡ�</returns>
        public static string Reflect(Persona persona)
        {
            string reflection = "Reflection for the day:\n";

            // ��������ڵ㣬��ȡ�������Ҫ�¼����뷨
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