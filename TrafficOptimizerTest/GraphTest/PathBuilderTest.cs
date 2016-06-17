using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizer.Test.GraphTest
{
    using Graph;
    using Graph.Model;

    [TestClass]
    public class PathBuilderTest
    {
        private Graph g;
        private List<Node> nodes;
        private List<Edge> edges;

        public PathBuilderTest()
        {
            // Строим граф, на котором будем тестировать
            nodes = new List<Node>();
            edges = new List<Edge>();

            g = new Graph();
            nodes.Add(g.MakeNode());
            nodes.Add(g.MakeNode());
            nodes.Add(g.MakeNode());
            nodes.Add(g.MakeNode());
            edges.Add(g.Unite(nodes[0], nodes[1], 25));
            edges.Add(g.Unite(nodes[0], nodes[2], 10));
            edges.Add(g.Unite(nodes[0], nodes[3], 50));
            edges.Add(g.Unite(nodes[1], nodes[3], 5));
            edges.Add(g.Unite(nodes[2], nodes[3], 42));
            edges.Add(g.Unite(nodes[2], nodes[1], 6));
        }

        /// <summary>
        /// Проверяет работу алгоритма
        /// </summary>
        [TestMethod]
        public void PathIsCorrectTest_SolidPathLengthCorrect()
        {
            var res = g.FindPath(nodes[0], nodes[3]);
            Assert.AreEqual(21D, res.Weight);
        }
        [TestMethod]
        public void PathIsCorrectTest_PathWithRestrickedEdgeCorrect()
        {
            // Запрещаем использовать одну из граней
            List<Edge> restricked = new List<Edge>();
            restricked.Add(edges[5]);
            var res = g.FindPath(nodes[0], nodes[3], restricked);
            Assert.AreEqual(30D, res.Weight);
        }
        [TestMethod]
        public void PathIsCorrectTest_PathThroughDefinedNodesCorrect()
        {
            // Указываем список узлов, которые можно использовать
            List<Node> scope = new List<Node>();
            scope.Add(nodes[0]);
            scope.Add(nodes[2]);
            scope.Add(nodes[3]);
            var res = g.FindPath(scope, nodes[0], nodes[3]);
            Assert.AreEqual(50D, res.Weight);
        }
        [TestMethod]
        public void PathIsCorrectTest_PathThroughDefinedNodesWithRestrickedEdgeCorrect()
        {
            // Указываем список узлов, которые можно использовать и запрещаем использовать одну из граней
            List<Node> scope = new List<Node>();
            scope.Add(nodes[0]);
            scope.Add(nodes[2]);
            scope.Add(nodes[3]);
            List<Edge> restricked = new List<Edge>();
            restricked.Add(edges[2]);
            var res = g.FindPath(scope, nodes[0], nodes[3], restricked);
            Assert.AreEqual(52D, res.Weight);
        }
    }
}
