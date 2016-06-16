using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizerTest.GraphTest
{
    using TrafficOptimizer.Graph.Model;

    [TestClass]
    public class DirectionTest
    {
        [TestMethod]
        public void DirectionsEqalityTest_InversedDirectionsAreNotEqual()
        {
            Node n1 = new Node();
            Node n2 = new Node();
            Direction d1 = new Direction(n1, n2);
            Direction d2 = new Direction(n2, n1);

            Assert.AreNotEqual(d1, d2);
        }
        [TestMethod]
        public void DirectionsEqalityTest_InversedDirectionOfInstanceNotEqual()
        {
            Node n1 = new Node();
            Node n2 = new Node();
            Direction d1 = new Direction(n1, n2);
            Direction d2 = d1.Inversed;

            Assert.AreNotEqual(d1, d2);
        }
        [TestMethod]
        public void DirectionsEqalityTest_DifferentInstancesOfSameDirectionAreEqual()
        {
            Node n1 = new Node();
            Node n2 = new Node();
            Direction d1 = new Direction(n1, n2);
            Direction d2 = new Direction(n1, n2);

            Assert.AreEqual(d1, d2);
        }
    }
}
