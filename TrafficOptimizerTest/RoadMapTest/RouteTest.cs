using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrafficOptimizerTest.RoadMapTest
{
    using TrafficOptimizer.RoadMap;
    using TrafficOptimizer.RoadMap.Model;

    [TestClass]
    public class RouteTest
    {
        [TestMethod]
        public void RouteTest_FindCorrectRoute()
        {
            RoadMap map = new RoadMap();
            var s1 = map.MakeSection();
            var s2 = map.MakeSection();
            var s3 = map.MakeSection();
            var r1 = map.SetRoad(s1, s2, 100);
            var r2 = map.SetRoad(s1, s3, 50);
            var r3 = map.SetRoad(s3, s2, 22);
            s3.SetLink(r2.PrimaryLine, r3.PrimaryLine);

            Route route = new Route(s2);
            route.BuildRoute(null, s1);
            Assert.AreEqual(route.Length, 72f);
        }
        [TestMethod]
        public void RouteTest_CircuitFindRoute()
        {
            RoadMap map = new RoadMap();
            Section[] s = new Section[7];
            Road[] r = new Road[10];
            s[0] = map.MakeSection();
            s[1] = map.MakeSection();
            s[2] = map.MakeSection();
            s[3] = map.MakeSection();
            r[0] = map.SetRoad(s[0], s[1], 10);
            r[1] = map.SetRoad(s[1], s[2], 20);
            r[2] = map.SetRoad(s[1], s[3], 10);

            s[1].SetLink(r[0].PrimaryLine, r[1].PrimaryLine);
            s[1].SetLink(r[1].SlaveLine, r[2].PrimaryLine);
            s[2].SetLink(r[1].PrimaryLine, r[1].SlaveLine);

            Route route = new Route(s[3]);
            route.BuildRoute(null, s[0]);

            Assert.AreEqual(route.Length, 60f);
        }
    }
}
