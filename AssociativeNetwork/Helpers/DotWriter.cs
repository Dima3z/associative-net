using System;
using System.Collections.Generic;
using System.Text;
using AssociativeNetwork.Models;

namespace AssociativeNetwork.Helpers
{
    public static class DotWriter
    {
        public static string ToDot(this WeightedGraph g)
        {
            var builder = new StringBuilder("graph G {\r\n");
            var visitedPairs = new List<(string, string)>();
            foreach (var edge in g.Graph.Edges)
            {
                if (visitedPairs.Contains((edge.Source, edge.Target)) ||
                    visitedPairs.Contains((edge.Target, edge.Source)))
                    continue;
                visitedPairs.Add((edge.Source, edge.Target));

                builder.AppendLine(
                    $"\"{edge.Source}\" -- \"{edge.Target}\" [label=\"{TimeSpan.FromMilliseconds(g.Costs[edge])}\" {(g.Answers[edge] ? "" : ",color = red")}];");
            }

            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}