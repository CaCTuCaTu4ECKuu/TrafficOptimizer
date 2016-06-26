using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL;
using SharpGL.WPF;

namespace UserInterface.Visualize
{
    using TrafficOptimizer.RoadMap2D;

    public class Gl_RoadMap
    {
        public OpenGLControl OGL_Control
        {
            get;
            private set;
        }
        public OpenGL gl
        {
            get
            {
                return OGL_Control.OpenGL;
            }
        }
        public RoadMap2D RoadMap
        {
            get;
            private set;
        }
        public Controller Controller
        {
            get;
            private set;
        }

        public Gl_RoadMap(OpenGLControl ogl)
        {
            OGL_Control = ogl;
            RoadMap = new RoadMap2D(gl);
            Controller = new Controller(this);

            RoadMap.AddRoad(new Point(-50, -50), new PointF(-50, 50));
            RoadMap.AddRoad(new Point(-50, 50), new PointF(50, 50));
            RoadMap.AddRoad(new Point(50, 50), new PointF(50, -50));
            RoadMap.AddRoad(new Point(50, -50), new PointF(-50, -50));
            RoadMap.AddRoad(new Point(50, 50), new Point(-50, -50));

            RoadMap.AddRoad(new PointF(-75, 75), new PointF(-50, 50));
            RoadMap.AddRoad(new PointF(50, -50), new PointF(75, -75));

            RoadMap.AddRoad(new PointF(-200, 100), new PointF(-100, 100));
            RoadMap.AddRoad(new PointF(-100, 100), new PointF(0, 100));

            RoadMap.AddRoad(new PointF(0, 100), new PointF(50, 50));
        }

        public void Draw()
        {
            gl.LoadIdentity();
            gl.Translate(0, 0, -2);

            /*
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(0.5f, 0.5f, 0.5f);

            gl.Vertex(0, 0);
            gl.Vertex(0, 1);
            gl.Vertex(0, 0);
            gl.Vertex(0, -1);
            gl.Vertex(0, 0);
            gl.Vertex(1, 0);
            gl.Vertex(0, 0);
            gl.Vertex(-1, 0);

            gl.End();
            */

            gl.Scale(0.005f, 0.005f, 0.005f);
            RoadMap.Draw();
            Controller.Draw();
        }
        #region Tools
        public PointF ScaleFromUniformCoords(PointF uniPoint)
        {
            return new PointF();
            /*
            var scale = RoadMapParametrs.ViewScale;
            var angle = (float)(Math.Sqrt(3) / 2);
            var x = uniPoint.X / scale / 2 * angle;
            var y = uniPoint.Y / scale / 2 * angle;
            var xo = RoadMapParametrs.ViewOffset.X / scale;
            var yo = RoadMapParametrs.ViewOffset.Y / scale;
            return new PointF((float)Math.Round((x + xo), 2), (float)Math.Round((y + yo), 2));
            */
        }
        public PointF ScaleToUniformCoords(PointF point)
        {
            return new PointF();
            /*
            var scale = RoadMapParametrs.ViewScale;
            var angle = (float)Math.Sin(45);
            return new PointF((point.X - RoadMapParametrs.ViewOffset.X) * scale * 2 / angle, (point.Y - RoadMapParametrs.ViewOffset.Y) * scale * 2 / angle);
            */
        }
        #endregion
    }
}
