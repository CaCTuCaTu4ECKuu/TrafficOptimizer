using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizerTest.ToolsTest
{
    using TrafficOptimizer.Tools;

    [TestClass]
    public class ToolsTest
    {
        [TestMethod]
        public void LineIntersectionTest_LinesIntersectionCalculates()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(1, 1);
            PointF p3 = new PointF(0, 1);
            PointF p4 = new PointF(1, 0);
            Assert.IsTrue(Tools.IsLineIntersect(p1, p2, p3, p4));
        }
        [TestMethod]
        public void LineIntersectionTest_NotIntersectedLinesCalculates()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(1, 1);
            PointF p3 = new PointF(1.01f, 1.01f);
            PointF p4 = new PointF(0.1f, 0.1f);
            Assert.IsFalse(Tools.IsLineIntersect(p1, p2, p3, p4));
        }
        [TestMethod]
        public void LineIntersectionTest_SegmentsOnLineIntersectionCalculates()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(0, 2);
            PointF p3 = new PointF(0, 1);
            PointF p4 = new PointF(0, 3);
            Assert.IsFalse(Tools.IsLineIntersect(p1, p2, p3, p4));
        }
        [TestMethod]
        public void LineIntersectionTest_SegmentsOnLineNoIntersectionCalculates()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(0, 2);
            PointF p3 = new PointF(0, 3);
            PointF p4 = new PointF(0, 5);
            Assert.IsFalse(Tools.IsLineIntersect(p1, p2, p3, p4));
        }
        [TestMethod]
        public void SegmentsOverlappingTest_SegmentsOverlapCalculates()
        {
            float x1 = 0;
            float x2 = 2;
            float y1 = 1;
            float y2 = 3;
            Assert.IsTrue(Tools.IsSegmentsOverlapping(x1, x2, y1, y2));
        }
        [TestMethod]
        public void SegmentsOverlappingTest_SegmentsNoOverlapCalculates()
        {
            float x1 = 0;
            float x2 = 2;
            float y1 = 3;
            float y2 = 4;
            Assert.IsFalse(Tools.IsSegmentsOverlapping(x1, x2, y1, y2));
        }
        [TestMethod]
        public void SegmentOverlapTest_SegmentsAreSolidLineNoOverlapCalculates()
        {
            float x1 = 0;
            float x2 = 2;
            float y1 = 2;
            float y2 = 4;
            Assert.IsFalse(Tools.IsSegmentsOverlapping(x1, x2, y1, y2));
        }
        [TestMethod]
        public void SegmentOverlapTest_PointOnSegmentOverlapCalculates()
        {
            float x1 = 0;
            float x2 = 4;
            float y1 = 2;
            float y2 = 2;
            Assert.IsTrue(Tools.IsSegmentsOverlapping(x1, x2, y1, y2));
        }
        [TestMethod]
        public void SegmentOverlapTest_PointOutOfSegmentNoOverlapCalculates()
        {
            float x1 = 0;
            float x2 = 4;
            float y1 = -1;
            float y2 = -1;
            Assert.IsFalse(Tools.IsSegmentsOverlapping(x1, x2, y1, y2));
        }
        [TestMethod]
        public void PointInsideTest_PointInsideRectangleCalculates()
        {
            PointF[] block = new PointF[4];
            block[0] = new PointF(1, 1);
            block[1] = new PointF(3, 3);
            block[2] = new PointF(5, 1);
            block[3] = new PointF(3, -1);
            PointF p = new PointF(3, 1);
            Assert.IsTrue(Tools.IsPointInside(p, block));
        }
        [TestMethod]
        public void PointInsideTest_PointOutsideRectangleCalculates_1()
        {
            PointF[] block = new PointF[4];
            block[0] = new PointF(1, 1);
            block[1] = new PointF(3, 3);
            block[2] = new PointF(5, 1);
            block[3] = new PointF(3, -1);
            PointF p = new PointF(2, 3);
            Assert.IsFalse(Tools.IsPointInside(p, block));
        }
        [TestMethod]
        public void PointInsideTest_PointOutsideRectangleCalculates_2()
        {
            PointF[] block = new PointF[4];
            block[0] = new PointF(0, 0);
            block[1] = new PointF(0, 2);
            block[2] = new PointF(2, 2);
            block[3] = new PointF(2, 0);
            PointF p = new PointF(3, 3);
            Assert.IsFalse(Tools.IsPointInside(p, block));
        }
        [TestMethod]
        public void PointInsideTest_PointOnEdgeCalculatesNotIntersect()
        {
            PointF[] block = new PointF[4];
            block[0] = new PointF(0, 0);
            block[1] = new PointF(0, 2);
            block[2] = new PointF(2, 2);
            block[3] = new PointF(2, 0);
            PointF p = new PointF(2, 2);
            Assert.IsFalse(Tools.IsPointInside(p, block));
        }
        [TestMethod]
        public void PointInsideTest_PointInsideCircleCalculates()
        {
            PointF center = new PointF(2, 2);
            float radius = 2;
            PointF point = new PointF(3, 3);
            Assert.IsTrue(Tools.IsPointInside(point, center, radius));
        }
        [TestMethod]
        public void PointInsideTest_PointOutsideCircleCalculates()
        {
            PointF center = new PointF(2, 2);
            float radius = 2;
            PointF point = new PointF(4, 4);
            Assert.IsFalse(Tools.IsPointInside(point, center, radius));
        }
        [TestMethod]
        public void CirclesIntersectTest_CirclesIntersectCalculates()
        {
            float radius = 2;
            PointF center1 = new PointF(2, 2);
            PointF center2 = new PointF(5, 2);
            Assert.IsTrue(Tools.IsRoundsIntersect(center1, radius, center2, radius));
        }
        [TestMethod]
        public void CirclesIntersectTest_CirclesNotIntersectCalculates()
        {
            float radius = 2;
            PointF center1 = new PointF(2, 2);
            PointF center2 = new PointF(5, 5);
            Assert.IsFalse(Tools.IsRoundsIntersect(center1, radius, center2, radius));
        }
    }
}
