using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizerTest.RoadMapTest
{
    using TrafficOptimizer.RoadMap;
    using TrafficOptimizer.RoadMap.Model;

    [TestClass]
    public class RoadMapTest
    {
        public RoadMap getMap(out Section[] s, out Road[] r)
        {
            RoadMap map = new RoadMap();
            var s1 = map.MakeSection();
            var s2 = map.MakeSection();
            var s3 = map.MakeSection();
            var s4 = map.MakeSection();
            var r1 = map.SetRoad(s1, s2, 1);
            var r2 = map.SetRoad(s2, s3, 1);
            var r3 = map.SetRoad(s3, s4, 1);

            s = new Section[] { s1, s2, s3, s4};
            r = new Road[] { r1, r2, r3 };

            return map;
        }
        [TestMethod]
        public void RoadMapRemoveRoadTest_RoadIsNullAfterRemove()
        {
            Section[] s;
            Road[] r;
            RoadMap map = getMap(out s, out r);

            Segment sg = new Segment(s[2], s[1]);
            map.RemoveRoad(sg);
            Assert.IsNull(map.GetRoad(sg));
        }
        [TestMethod]
        public void RoadMapRemoveRoadTest_SectionRemovesIfNoRoads()
        {
            Section[] s;
            Road[] r;
            RoadMap map = getMap(out s, out r);

            var node = map.GetNode(s[0]);

            Segment sg = new Segment(s[0], s[1]);
            map.RemoveRoad(sg);
            Assert.IsNull(map.GetSection(node));
        }
        [TestMethod]
        public void RoadMapRemoveRoadTest_RoadRemovesIfStreaksRemoved()
        {
            Section[] s;
            Road[] r;
            RoadMap map = getMap(out s, out r);

            Segment sg = new Segment(s[0], s[1]);
            var road = map.GetRoad(sg);

            map.RemoveStreak(s[0], s[1]);
            road.SlaveLine.RemoveStreak();
            Assert.IsNull(map.GetRoad(sg));
        }
        [TestMethod]
        public void RoadMapRemoveRoadTest_RoadStayIfStreaksLeft()
        {
            Section[] s;
            Road[] r;
            RoadMap map = getMap(out s, out r);

            Segment sg = new Segment(s[0], s[1]);
            var node = map.GetNode(s[0]);
            var road = map.GetRoad(sg);

            map.RemoveStreak(s[1], s[0]);
            Assert.IsNotNull(map.GetSection(node));
        }
    }
}
