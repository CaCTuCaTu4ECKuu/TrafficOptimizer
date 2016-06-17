using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL;

namespace TrafficOptimizer.RoadMap2D.Transport
{
    using RoadMap.Model;
    using RoadMap.Model.Vehicles;

    public class Car : Vehicle
    {
        public float Width
        {
            get;
            private set;
        }
        public Color Color
        {
            get;
            set;
        }

        public Car(float maxSpeed, float maxAcceleration, float length, float width) 
            : base(length, maxSpeed, maxAcceleration)
        {
            Width = width;
        }
        public void Draw(OpenGL OGL, PointF pos)
        {
            OGL.Begin(OpenGL.GL_QUADS);
            OGL.Color(Color.R, Color.G, Color.B);

            OGL.End();
        }
    }
}
