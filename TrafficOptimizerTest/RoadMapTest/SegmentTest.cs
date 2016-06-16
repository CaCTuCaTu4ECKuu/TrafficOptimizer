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
        public void SegmentEqualityTest1_1()
        {
            Section s1 = new Section(null);
            Section s2 = new Section(null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);

            Assert.AreEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest1_2()
        {
            Section s1 = new Section(null);
            Section s2 = new Section(null);
            Section s3 = new Section(null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s1, s3);

            Assert.AreNotEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest1_3()
        {
            Section s1 = new Section(null);
            Section s2 = new Section(null);
            Road r = new Road(null, s1, s2, null, null);
            s1.AddRoad(r);
            s2.AddRoad(r);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);

            Assert.AreEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest1_4()
        {
            Section s1 = new Section(null);
            Section s2 = new Section(null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s1, s2);

            Assert.AreEqual(sg1, sg2);
        }
        [TestMethod]
        public void SegmentEqualityTest1_5()
        {
            List<Segment> list = new List<Segment>();
            Section s1 = new Section(null);
            Section s2 = new Section(null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);
            list.Add(sg1);

            Assert.IsTrue(list.Contains(sg2));
        }
        [TestMethod]
        public void SegmentEqualityTest1_6()
        {
            List<Segment> list = new List<Segment>();
            Section s1 = new Section(null);
            Section s2 = new Section(null);
            Section s3 = new Section(null);
            Segment sg1 = new Segment(s1, s2);
            Segment sg2 = new Segment(s2, s1);
            Segment sg3 = new Segment(s2, s3);
            list.Add(sg1);
            list.Add(sg2);

            Assert.IsFalse(list.Contains(sg3));
        }
    }
}
