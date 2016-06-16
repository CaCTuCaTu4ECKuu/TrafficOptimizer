using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using SharpGL;
using SharpGL.WPF;

namespace UserInterface.Visualize
{
    public class Controller
    {
        private Gl_RoadMap roadmap;
        private OpenGLControl ogl
        {
            get
            {
                return roadmap.OGL_Control;
            }
        }

        public PointF? StartCapture;
        public PointF? newRoadEnd
        {
            get;
            private set;
        }
        private PointF lastView;
        private PointF currentView;

        public MouseInterractMode IntMode = MouseInterractMode.MoveView;

        public Controller(Gl_RoadMap map)
        {
            roadmap = map;
            newRoadEnd = null;
            StartCapture = null;
        }

        #region Zoom
        public void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            //roadmap.RoadMapParametrs.ViewScale /= roadmap.RoadMapParametrs.ViewScaleStep;
        }
        public void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            //roadmap.RoadMapParametrs.ViewScale *= roadmap.RoadMapParametrs.ViewScaleStep;
        }
        #endregion

        #region IO
        public void OpenGLWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartCapture = DecPointToPointF(e.GetPosition((OpenGLControl)sender));
        }
        public void OpenGLWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (StartCapture != null)
            {
                switch (IntMode)
                {
                    case MouseInterractMode.MoveView:
                        currentView.X -= lastView.X;
                        currentView.Y += lastView.Y;
                        break;
                    case MouseInterractMode.AddRoad:
                        var pos = e.GetPosition((OpenGLControl)sender);
                        if (Math.Sqrt(Math.Pow((pos.X - StartCapture.Value.X), 2) + Math.Pow((pos.Y - StartCapture.Value.Y), 2)) > 5)
                        {
                            var sp = roadmap.ScaleFromUniformCoords(DiscretteToFloat(StartCapture.Value));
                            var ep = roadmap.ScaleFromUniformCoords(DiscretteToFloat(newRoadEnd.Value));
                            //roadmap.AddRoad(sp, ep);
                            newRoadEnd = null;
                        }
                        break;
                }
                StartCapture = null;
            }
        }
        public void OpenGLWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (StartCapture != null)
            {
                var pos = e.GetPosition((OpenGLControl)sender);
                switch (IntMode)
                {
                    case MouseInterractMode.MoveView:
                        lastView.X = (float)((StartCapture.Value.X - pos.X) * (1 / ogl.ActualWidth));
                        lastView.Y = (float)((StartCapture.Value.Y - pos.Y) * (1 / ogl.ActualHeight));
                        //roadmap.RoadMapParametrs.ViewOffset.X = currentView.X - lastView.X;
                       // roadmap.RoadMapParametrs.ViewOffset.Y = currentView.Y + lastView.Y;
                    break;
                    case MouseInterractMode.AddRoad:
                        newRoadEnd = DecPointToPointF(pos);
                    break;
                }
            }
        }
        #endregion

        private void drawNewRoad()
        {
            if (StartCapture.HasValue && newRoadEnd.HasValue)
            {
                ogl.OpenGL.Begin(OpenGL.GL_LINES);
                ogl.OpenGL.Color(1f, 0f, 0f);

                var point = roadmap.ScaleFromUniformCoords(DiscretteToFloat(StartCapture.Value));
                ogl.OpenGL.Vertex(point.X, point.Y);
                point = roadmap.ScaleFromUniformCoords(DiscretteToFloat(newRoadEnd.Value));
                ogl.OpenGL.Vertex(point.X, point.Y);

                ogl.OpenGL.End();
            }
        }
        public void Draw()
        {
            switch (IntMode)
            {
                case MouseInterractMode.AddRoad:
                    drawNewRoad();
                break;
            }
        }

        #region Tools
        public PointF DecPointToPointF(object point)
        {
            if (point.GetType() == typeof(System.Windows.Point))
            {
                return DecPointToPointF((System.Windows.Point)point);
            }

            throw new FormatException();
        }
        public PointF DecPointToPointF(System.Windows.Point point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }

        public PointF DiscretteToFloat(PointF point)
        {
            var xratio = 2 / (float)ogl.ActualWidth;
            var yratio = 2 / (float)ogl.ActualHeight;

            return new PointF(point.X * xratio - 1, -(point.Y * yratio - 1));
        }
        public PointF FloatToDiscrette(PointF point)
        {
            var xratio = 2 / (float)ogl.ActualWidth;
            var yratio = 2 / (float)ogl.ActualHeight;

            return new PointF((point.X + 1) / xratio, -((point.Y + 1) / yratio));
        }
        #endregion
    }
}
