// CognitiveModules.cs
// ���ļ�������CognitiveModules�࣬����ʵ�ֽ�ɫ����֪ģ���߼���������֪�������ȹ��ܡ�
using System;
using System.Collections.Generic;
using Modules.Persona;
using Modules.MemoryStructures;

namespace Modules.CognitiveModules
{
    public static class CognitiveModules
    {
        // Perceive�����������ɫ����Χ�����ĸ�֪��
        // ���룺
        //   - persona: ��ǰ��ɫ����
        //   - maze: ��ɫ���ڵ��Թ�����
        // �����
        //   - ���ؽ�ɫ��֪���ĸ���ڵ��б�
        public static List<ConceptNode> Perceive(Persona persona, Maze maze)
        {
            List<ConceptNode> perceivedNodes = new List<ConceptNode>();

            // ������ɫ��Ұ��Χ�ڵ���Ƭ
            foreach (var tile in maze.GetTilesInRadius(persona.ScratchMemory.CurrentTile, persona.ScratchMemory.VisionRadius))
            {
                // �����Ƭ�ϴ����¼�����ӵ���֪�ڵ�
                if (tile.HasEvent)
                {
                    perceivedNodes.Add(new ConceptNode
                    {
                        NodeId = Guid.NewGuid().ToString(),
                        Type = "event",
                        Subject = tile.Event.Subject,
                        Predicate = tile.Event.Predicate,
                        Object = tile.Event.Object,
                        Description = tile.Event.Description,
                        Keywords = tile.Event.Keywords,
                        Created = DateTime.Now
                    });
                }
            }

            // ���ݽ�ɫע��������ɸѡ��֪�ڵ㣬ֻ�ͽ�����
            int attentionBandwidth = persona.ScratchMemory.AttentionBandwidth;
            if (perceivedNodes.Count > attentionBandwidth)
            {
                perceivedNodes = perceivedNodes.GetRange(0, attentionBandwidth);
            }

            return perceivedNodes;
        }

        // Retrieve�������ݸ�֪�����¼�������ɫ�Ĺ������䡣
        // ���룺
        //   - persona: ��ǰ��ɫ����
        //   - perceived: ��֪���ĸ���ڵ��б�
        // �����
        //   - ����һ����������¼����뷨���ֵ䡣
        public static Dictionary<string, Dictionary<string, object>> Retrieve(Persona persona, List<ConceptNode> perceived)
        {
            Dictionary<string, Dictionary<string, object>> retrieved = new Dictionary<string, Dictionary<string, object>>();

            // ����ÿ����֪���¼��ڵ�
            foreach (var eventNode in perceived)
            {
                var eventDetails = new Dictionary<string, object>();

                // ��������¼�
                var relevantEvents = persona.AssociativeMemory.Nodes.Values;
                eventDetails["events"] = relevantEvents;

                // ��������뷨
                var relevantThoughts = persona.AssociativeMemory.Nodes.Values;
                eventDetails["thoughts"] = relevantThoughts;

                // ����ǰ�¼��ڵ���Ϣ��ӵ��ֵ�
                eventDetails["curr_event"] = eventNode;

                // �����¼������������Ϣ�洢�ڼ��������
                retrieved[eventNode.Description] = eventDetails;
            }

            return retrieved;
        }

        // Plan����������ݼ������ļ���͵�ǰ������ɽ�ɫ���ж��ƻ���
        // ���룺
        //   - persona: ��ǰ��ɫ����
        //   - maze: ��ǰ��ɫ���ڵ��Թ�����
        //   - retrieved: �Ӽ����м�����������¼����뷨��
        // �����
        //   - ����һ�����󣬱�ʾ���ɵļƻ���
        public static object Plan(Persona persona, Maze maze, Dictionary<string, Dictionary<string, object>> retrieved)
        {
            // ����һ��ʾ���ƻ�����
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

                // �����¼����뷨���ɼ��ж��ƻ���ʾ����
                plan.Actions.Add($"Investigate: {eventDescription}");
            }

            return plan;
        }

        // Execute��������ִ�н�ɫ���ж��ƻ���
        // ���룺
        //   - persona: ��ǰ��ɫ����
        //   - plan: ��ɫ���ɵ��ж��ƻ���
        // �����
        //   - ����ִ�н����ͨ����һ������ִ��������ַ�����
        public static string Execute(Persona persona, object plan)
        {
            if (plan == null || !(plan is dynamic))
            {
                return "Invalid plan provided.";
            }

            // ʾ��ִ���߼��������ƻ��е�ÿ���ж�
            var actions = plan.Actions as List<string>;
            if (actions == null || actions.Count == 0)
            {
                return "No actions to execute.";
            }

            string executionResult = "";
            foreach (var action in actions)
            {
                // ִ�ж���������¼ִ�н�����˴�Ϊģ��ִ�У�
                executionResult += $"Executed action: {action}\n";
            }

            return executionResult.Trim();
        }

        // Reflect���������ɫ��һ����ܽ�ͷ�˼��
        // ���룺
        //   - persona: ��ǰ��ɫ����
        // �����
        //   - ����һ���ַ�������ʾ��˼���ݡ�
        public static string Reflect(Persona persona)
        {
            // ������˼����
            string reflection = "Reflection for the day:\n";

            // ��������ڵ㣬��ȡ�������Ҫ�¼����뷨
            foreach (var node in persona.AssociativeMemory.Nodes.Values)
            {
                if (node.Created.Date == DateTime.Now.Date)
                {
                    reflection += $"- {node.Description}\n";
                }
            }

            // ����ܽ�
            reflection += "\nEnd of day reflection.";

            return reflection;
        }
    }
}
