using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL;

namespace UserInterface.Visualization
{
    using TrafficOptimizer.RoadMap;
    using TrafficOptimizer.Tools;

    public class GL_RoadMap : RoadMap
    {
        private OpenGL gl;
        public RoadDrawParametrs Parametrs
        {
            get;
            private set;
        }

		public void DrawRoads()
        {
            var uniqRoads = _roads.Values.AsQueryable().Distinct();
            foreach (var r in uniqRoads)
            {
                gl.LineWidth(Parametrs.BorderSize);

                var length = r.PrimaryLine.Edge.Weight; // Первая сторона
                float width = r.Streaks * Parametrs.StreakSize;
                float lineWidth = width / 2;    // Вторая сторона

                Point src = _points[r.PrimaryLine.Edge.Source];
                Point dst = _points[r.PrimaryLine.Edge.Destination];
                Point p3 = new Point();
                double angle = 0;

                // Выбираем точку ближе к той оси, по которой будем поворачивать (ось X)
                if (src.Y < dst.Y)
                {
                    p3.X = dst.X;
                    p3.Y = src.Y;
                }
                else
                {
                    p3.X = src.X;
                    p3.Y = dst.Y;
                }
                var nearEdge = Math.Sqrt(Math.Pow(p3.X - src.X, 2) + Math.Pow(p3.Y - src.Y, 2));
                var oppositeEdge = Math.Sqrt(Math.Pow(p3.X - dst.X, 2) + Math.Pow(p3.Y - dst.Y, 2));
                var sinA = nearEdge / length; // получаем синус
                var sinB = oppositeEdge / length;
                if (sinB != 0)
                    angle = 45 * sinA / sinB;
                if (p3.Y < 0)
                    angle = -angle;

                if (sinA == 0)
                    sinA = 1;
                if (sinB == 0)
                    sinB = 1;

                if (r.SlaveLine != null)
                {
                    // Разделяющая полоса
                    gl.Begin(OpenGL.GL_LINES);
                    gl.Color(0, 0, 0);
                    gl.Vertex(src.X, src.Y);
                    gl.Vertex(dst.X, dst.Y);
                    gl.End();
                }

                gl.Rotate((float)angle, 0, 0);
                gl.Begin(OpenGL.GL_LINES);
                for (float shift = Parametrs.StreakSize; shift <= r.PrimaryLine.Streaks * Parametrs.StreakSize; shift += Parametrs.StreakSize)
                {
                    gl.Color(0, 0.5f, 0);
                    if (src.X == dst.X)
                    {
                        gl.Vertex(src.X - shift, src.Y);
                        gl.Vertex(dst.X - shift, dst.Y);
                    }
                    else
                    {
                        gl.Vertex(src.X, src.Y - shift);
                        gl.Vertex(dst.X, dst.Y - shift);
                    }
                }
                for (float shift = Parametrs.StreakSize; shift <= r.SlaveLine.Streaks * Parametrs.StreakSize; shift += Parametrs.StreakSize)
                {
                    gl.Color(0.5f, 0, 0);
                    if (src.X == dst.X)
                    {
                        gl.Vertex(src.X + shift, src.Y);
                        gl.Vertex(dst.X + shift, dst.Y);
                    }
                    else
                    {
                        gl.Vertex(src.X, src.Y + shift);
                        gl.Vertex(dst.X, dst.Y + shift);
                    }
                }
                gl.End();
                gl.Rotate((float)-angle, 0, 0);


                
            }
        }
		public void DrawCars()
        {
            gl.LoadIdentity();

            gl.PointSize(1f);
        }
        public void DrawMap()
        {
            Step();

            gl.LoadIdentity();
            gl.Translate(0, 0, -Parametrs.ViewDistance);

            DrawRoads();
            DrawCars();
        }

        public GL_RoadMap(OpenGL OGL) : base()
        {
            gl = OGL;
            Parametrs = new RoadDrawParametrs();

            AddRoad(new Point(-5, 0), new Point(0, 0));
            AddRoad(new Point(0, 0), new Point(10, 10));
            AddRoad(new Point(10, 10), new Point(10, -10));
            AddRoad(new Point(0, 0), new Point(-5, 5));
            AddRoad(new Point(-10, 5), new Point(-5, 5));
            AddRoad(new Point(10, -10), new Point(5, -5));
            AddRoad(new Point(-5, -5), new Point(5, -5));
        }
    }
}
