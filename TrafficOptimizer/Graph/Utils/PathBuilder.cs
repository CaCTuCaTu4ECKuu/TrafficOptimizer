using System;
using System.Collections.Generic;

namespace TrafficOptimizer.Graph.Utils
{
    using Model;

    public static class PathBuilder
    {
        /// <summary>
        /// Находит кратчайший маршрут между указанными узлами
        /// </summary>
        /// <param name="ratio">Коллекция коэффициентов для веса ребер графа, может иметь значение null</param>
        /// <param name="searchScope">Список узлов, которые учавствуют в маршруте</param>
        /// <param name="edges">Ребра, которые можно использовать для построения маршрута</param>
        /// <param name="task">Направление, которое необходимо найти</param>
        /// <returns>Маршрут</returns>
        public static Path FindShortestPath(GraphRatioCollection ratio, List<Node> searchScope, Dictionary<Direction, Edge> edges, Direction task)
        {
            if (ratio == null)
                throw new ArgumentException("Ratio value can't be null");

            Path res = null;

            List<Node> notVisited = new List<Node>(searchScope);
            var track = new Dictionary<Node, SearchData>();
            track[task.Source] = new SearchData(null, 0);
            
            while(true)
            {
                // Выбераем узел, с которым работаем
                Node recent = null;
                // определяем минимальную длинну маршрута
                float best = float.PositiveInfinity;
                // Для каждого узла из не-посещенных проверяем
                foreach (var n in notVisited)
                {
                    // Если от текущего узла к не посещенному есть маршрут и его вес меньше текущего
                    if (track.ContainsKey(n) && track[n].Price < best)
                    {
                        // Выбираем этот узел как текущий
                        recent = n;
                        // Отмечаем вес грани к этому узлу
                        best = track[n].Price;
                    }
                }
                // Если от этого узла не идет ниодной грани
                if (recent == null)
                {
                    // Мы закончили - возвращаем путь с одним узлом
                    res = new Path(task);
                    res.Nodes.Add(task.Source);
                    return res;
                }

                // Если мы дошли до узла назначения - мы нашли маршрут
                if (recent == task.Destination)
                    break;

                // Проверяем все прилежащие грани
                foreach (var e in recent.RelatedEdges)
                {
                    Direction td = new Direction(recent, e.Key);
                    // Не идти по путям, которые запрещены
                    if (edges.ContainsKey(td))
                    {
                        // Запоминаем вес путь до этого узла
                        var currPrice = track[recent].Price + edges[td].Weight * ratio.GetRatio(edges[td]);
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
