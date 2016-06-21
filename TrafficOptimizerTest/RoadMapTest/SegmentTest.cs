using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizerTest.RoadMapTest
{
    using TrafficOptimizer.RoadMap.Model;
    [TestClass]
    public class SegmentTest
    {
        [TestMethod]
        public void SegmentEqualityTest_InversedSegmentsAreEqual()
        {
            Section s1 = new Section(null, null);
            Section s2 = new Section(null, null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);

            Assert.AreEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest_DifferentSegmentsAreNotEqual()
        {
            Section s1 = new Section(null, null);
            Section s2 = new Section(null, null);
            Section s3 = new Section(null, null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s1, s3);

            Assert.AreNotEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest_InversedSegmentsWithDifferentRoadsAreEqual()
        {
            Section s1 = new Section(null, null);
            Section s2 = new Section(null, null);
            Road r = new Road(null, s1, s2, null, null);
            Road r2 = new Road(null, null, null, null, null);
            s1.AddRoad(r);
            s2.AddRoad(r2);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);

            Assert.AreEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest_DifferentInstancesOfSameSegmentSectionsAreEqual()
        {
            Section s1 = new Section(null, null);
            Section s2 = new Section(null, null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s1, s2);

            Assert.AreEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest_ListAssignInversedSegmentsAsEqual()
        {
            List<Segment> list = new List<Segment>();
            Section s1 = new Section(null, null);
            Section s2 = new Section(null, null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);
            list.Add(sg1);

            Assert.IsTrue(list.Contains(sg2));
        }
        [TestMethod]
        public void SegmentEqualityTest_ListDoNotAssignDifferentSegmentAsAnother()
        {
            List<Segment> list = new List<Segment>();
            Section s1 = new Section(null, null);
            Section s2 = new Section(null, null);
            Section s3 = new Section(null, null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);
            Segment sg3 = new Segment(s2, s3);
            list.Add(sg1);
            list.Add(sg2);

            Assert.IsFalse(list.Contains(sg3));
        }
    }
}
