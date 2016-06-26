using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficOptimizer.Graph
{
    using Model;

    public class GraphRatioCollection : IRatioProvider
    {
        public Graph BaseGraph
        {
            get;
            private set;
        }
        private Dictionary<Edge, float> _factors = new Dictionary<Edge, float>();

        public GraphRatioCollection(Graph graph)
        {
            BaseGraph = graph;
        }

        public float GetRatio(Edge edge)
        {
            if (_factors.ContainsKey(edge))
                return _factors[edge];
            return 1.0f;
        }
        public void SetRatio(Edge edge, float ratio)
        {
            if (_factors.ContainsKey(edge))
            {
                if (ratio == 1.0f)
                    _factors.Remove(edge);
                else
                    _factors[edge] = ratio;
            }
            else if (ratio != 1.0f)
                _factors.Add(edge, ratio);
        }

        public GraphRatioCollection MultipleRatio(GraphRatioCollection r_graph)
        {
            if (r_graph.BaseGraph == BaseGraph)
            {
                GraphRatioCollection res = new GraphRatioCollection(BaseGraph);
                foreach (var e in _factors)
                {
                    res.SetRatio(e.Key, e.Value * r_graph.GetRatio(e.Key));
                }
                return res;
            }
            else throw new ArgumentException("Must be for same graph");
        }
    }
}
