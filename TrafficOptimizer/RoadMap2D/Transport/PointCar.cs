using System;
using System.Drawing;
using SharpGL;

namespace TrafficOptimizer.RoadMap2D.Transport
{
    using RoadMap.Model;
    using RoadMap.Model.Vehicles;

    public class PointCar : Vehicle
    {
        public float Radius
        {
            get;
            private set;
        }
        public Color Color
        {
            get;
            set;
        }

        public PointCar(Section destination, float radius, float maxSpeed, float maxAcceleration) 
            : base(destination, 0, maxSpeed, maxAcceleration)
        {
            Radius = radius;
            Color = Color.Black;
        }

        public void Draw(OpenGL OGL, PointF pos)
        {
            OGL.PointSize(Radius);
            OGL.Begin(OpenGL.GL_POINT);
            OGL.Color(Color.R, Color.G, Color.B);
            OGL.Vertex(pos.X, pos.Y);
            OGL.End();
        }
    }
}
