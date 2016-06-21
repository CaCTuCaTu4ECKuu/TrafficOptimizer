using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizer.Test.GraphTest
{
    using Graph;
    using Graph.Model;

    [TestClass]
    public class PathTest
    {
        private Graph g;
        private RatioCollection ratio;
        private List<Node> nodes;

        [TestMethod]
        public void PathTest_PathIsSolid()
        {
            Path p = g.FindPath(ratio, nodes[0], nodes[2]);

            Assert.IsTrue(p.IsSolid);
        }

        public PathTest()
        {
            nodes = new List<Node>();
            g = new Graph();
            ratio = new RatioCollection(g);
            nodes.Add(g.MakeNode());
            nodes.Add(g.MakeNode());
            nodes.Add(g.MakeNode());
            g.Unite(nodes[0], nodes[1], 1);
            g.Unite(nodes[1], nodes[2], 1);
        }
    }
}
