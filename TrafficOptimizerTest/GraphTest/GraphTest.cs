using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizerTest.GraphTest
{
    using TrafficOptimizer.Graph;
    using TrafficOptimizer.Graph.Model;

    [TestClass]
    public class GraphTest
    {
        private Graph getGraph(out Node[] nodes, out Edge[] edges)
        {
            Graph g = new Graph();
            Node n1 = g.MakeNode();
            Node n2 = g.MakeNode();
            Node n3 = g.MakeNode();
            Node n4 = g.MakeNode();
            Edge e1 = g.Unite(n1, n2, 1);
            Edge e2 = g.Unite(n2, n1, 1);
            Edge e3 = g.Unite(n2, n3, 1);
            Edge e4 = g.Unite(n3, n2, 1);
            Edge e5 = g.Unite(n3, n4, 1);
            Edge e6 = g.Unite(n4, n3, 1);

            nodes = new Node[] { n1, n2, n3, n4 };
            edges = new Edge[] { e1, e2, e3, e4, e5, e6 };

            return g;
        }

        [TestMethod]
        public void RemoveEdgeTest_NodeStayIfHaveRelatedEdges()
        {
            Node[] n;
            Edge[] e;
            Graph g = getGraph(out n, out e);

            g.Divide(n[0], n[1]);

            Assert.IsTrue(n[1].RelatedEdges.ContainsKey(n[0]));
        }
        [TestMethod]
        public void RemoveEdgeTest_NodeRemovedIfNoRelatedEdges()
        {
            Node[] n;
            Edge[] e;
            Graph g = getGraph(out n, out e);

            g.Divide(n[0], n[1]);
            g.Divide(n[1], n[0]);

            Assert.IsFalse(n[1].RelatedEdges.ContainsKey(n[0]));
        }
    }
}
