using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL;

namespace UserInterface.Visualize
{
    using TrafficOptimizer.RoadMap;

    public class Gl_RoadMap : RoadMap
    {
        public OpenGL gl
        {
            get;
            private set;
        }

        public Gl_RoadMap(OpenGL ogl)
        {
            gl = ogl;

            AddRoad(new Point(-5, 0), new Point(0, 0));
            AddRoad(new Point(-5, 0), new Point(0, 0));
            AddRoad(new Point(0, 0), new Point(10, 10));
            AddRoad(new Point(10, 10), new Point(10, -10));
            AddRoad(new Point(0, 0), new Point(-5, 5));
            AddRoad(new Point(-10, 5), new Point(-5, 5));
            AddRoad(new Point(10, -10), new Point(5, -5));
            AddRoad(new Point(-5, -5), new Point(5, -5));
        }

        public void DrawRoads()
        {
            List<uint> drawed = new List<uint>();

            foreach (var road in _roads.Values)
            {
                if (!drawed.Contains(road.ID))
                {
                    drawed.Add(road.ID);

                    gl.Begin(OpenGL.GL_LINES);
                    gl.Color(0f, 0f, 0f);
                    gl.Vertex(road.StartPoint.X, road.StartPoint.Y);
                    gl.Vertex(road.EndPoint.X, road.EndPoint.Y);

                    gl.Color(1f, 0, 0);
                    gl.Vertex(road.PrimaryLine.LineRightStart.X, road.PrimaryLine.LineRightStart.Y);
                    gl.Vertex(road.PrimaryLine.LineRightEnd.X, road.PrimaryLine.LineRightEnd.Y);

                    gl.Color(0, 1f, 0);
                    gl.Vertex(road.SlaveLine.LineRightStart.X, road.SlaveLine.LineRightStart.Y);
                    gl.Vertex(road.SlaveLine.LineRightEnd.X, road.SlaveLine.LineRightEnd.Y);

                    gl.Color(0, 0, 1f);
                    for (int i = 0; i < road.PrimaryLine.Streaks.Count - 1; i++)
                    {
                        var streak = road.PrimaryLine.Streaks[i];
                        gl.Vertex(streak.BorderRightStart.X, streak.BorderRightStart.Y);
                        gl.Vertex(streak.BorderRightEnd.X, streak.BorderRightEnd.Y);
                    }
                    for (int i = 0; i < road.SlaveLine.Streaks.Count - 1; i++)
                    {
                        var streak = road.SlaveLine.Streaks[i];
                        gl.Vertex(streak.BorderRightStart.X, streak.BorderRightStart.Y);
                        gl.Vertex(streak.BorderRightEnd.X, streak.BorderRightEnd.Y);
                    }

                    gl.End();
                }
            }
        }
        public void DrawCars()
        {

        }
        public void Draw()
        {
            gl.LoadIdentity();
            gl.Translate(0, 0, -RoadMapParametrs.ViewDistance);

            DrawRoads();
            DrawCars();
        }
    }
}
