using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;

namespace AssociativeNetwork.Models
{
    public class WeightedGraph
    {
        public Dictionary<Edge<string>, bool> Answers;
        public Dictionary<Edge<string>, double> Costs;
        public AdjacencyGraph<string, Edge<string>> Graph;

        public WeightedGraph()
        {
            Graph = new AdjacencyGraph<string, Edge<string>>();
            Costs = new Dictionary<Edge<string>, double>();
            Answers = new Dictionary<Edge<string>, bool>();
        }

        public void AddEdgeWithCosts(string source, string target, double cost, bool answer)
        {
            var edge1 = new Edge<string>(source, target);
            var edge2 = new Edge<string>(target, source);
            Graph.AddVerticesAndEdge(edge1);
            Graph.AddVerticesAndEdge(edge2);
            Costs.Add(edge1, cost);
            Costs.Add(edge2, cost);
            Answers.Add(edge1, answer);
            Answers.Add(edge2, answer);
        }

        public static WeightedGraph FromTestResult(TestResult processedResult)
        {
            var graph = new WeightedGraph();
            var questions = processedResult.Result
                .Questions
                .OrderBy(q => q.AnswerTime);
            foreach (var question in questions)
                if(question.Dispersion != null && question.Dispersion/question.AnswerTime.Value.Milliseconds <= 1.5d)
                graph.AddIfBetterPath(question.FirstNode, question.SecondNode,
                    question.AnswerTime.Value.TotalMilliseconds, question.Answer);
            return graph;
        }

        private void AddIfBetterPath(string from, string to, double cost, bool answer)
        {
            TryFunc<string, IEnumerable<Edge<string>>> tryGetPath;
            try
            {
                tryGetPath = Graph.ShortestPathsDijkstra(edge => Costs[edge], from);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine($"No path found from {to} to {from}.");
                AddEdgeWithCosts(from, to, cost, answer);
                return;
            }

            IEnumerable<Edge<string>> path;
            if (tryGetPath(to, out path))
            {
                var pathCost = path.Select(edge => Costs[edge]).Sum();
                Console.WriteLine($"pathcost {pathCost} edgecost {cost}");
                if (pathCost > cost * (answer ? 1:4))
                    AddEdgeWithCosts(from, to, cost, answer);
            }
            else
            {
                AddEdgeWithCosts(from, to, cost, answer);
                Console.WriteLine($"No path found from {from} to {to}.");
            }
        }
    }
}