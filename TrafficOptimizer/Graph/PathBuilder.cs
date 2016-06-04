using System.Collections.Generic;

namespace TrafficOptimizer.Graph
{
    using Model;

    public static class PathBuilder
    {
        /// <summary>
        /// Находит кратчайший маршрут между указанными узлами
        /// </summary>
        /// <param name="searchScope">Список узлов, которые учавствуют в маршруте</param>
        /// <param name="edges">Ребра, которые можно использовать для построения маршрута</param>
        /// <param name="task">Направление, которое необходимо найти</param>
        /// <returns>Маршрут</returns>
        public static Path FindShortestPath(List<Node> searchScope, Dictionary<Direction, Edge> edges, Direction task)
        {
            Path res = null;

            List<Node> notVisited = new List<Node>(searchScope);
            var track = new Dictionary<Node, SearchData>();
            track[task.Source] = new SearchData(null, 0);
            
            while(true)
            {
                Node recent = null;
                float best = float.MaxValue;
                foreach (var n in notVisited)
                {
                    if (track.ContainsKey(n) && track[n].Price < best)
                    {
                        recent = n;
                        best = track[n].Price;
                    }
                }
                if (recent == null)
                {
                    res = new Path(task);
                    res.Nodes.Add(task.Source);
                    return res;
                }

                if (recent == task.Destination)
                    break;

                foreach (var e in recent.RelatedEdges)
                {
                    Direction td = new Direction(recent, e.Key);
                    // Не идти по путям, которые запрещены
                    if (edges.ContainsKey(td))
                    {
                        var currPrice = track[recent].Price + edges[td].Weight;
                        var nextNode = e.Key;
                        if (!track.ContainsKey(nextNode) || track[nextNode].Price > currPrice)
                        {
                            track[nextNode] = new SearchData(recent, currPrice);
                        }
                    }
                }
                notVisited.Remove(recent);
            }
            res = new Path(task);
            var t = task.Destination;
            while (t != null)
            {
                res.Nodes.Insert(0, t);
                t = track[t].Previvous;
            }
            return res;

        }
        private class SearchData
        {
            public float Price { get; set; }
            public Node Previvous { get; set; }
            public SearchData(Node prev, float price)
            {
                Price = price;
                Previvous = prev;
            }
        }
    }
}
