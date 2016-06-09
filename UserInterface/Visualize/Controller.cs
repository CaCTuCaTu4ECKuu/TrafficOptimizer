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

        public System.Windows.Point? StartCapture = null;
        private PointF lastView;
        private PointF currentView;
        public PointF? newRoadEnd
        {
            get;
            set;
        }

        public MouseInterractMode IntMode = MouseInterractMode.MoveView;

        public Controller(Gl_RoadMap map)
        {
            roadmap = map;
        }

        public void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            roadmap.RoadMapParametrs.ViewScale /= roadmap.RoadMapParametrs.ViewScaleStep;
        }
        public void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            roadmap.RoadMapParametrs.ViewScale *= roadmap.RoadMapParametrs.ViewScaleStep;
        }

        public void OpenGLWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartCapture = e.GetPosition((OpenGLControl)sender);
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
                        lastView.X = (float)((StartCapture.Value.X - pos.X) * (1 / ((OpenGLControl)sender).ActualWidth));
                        lastView.Y = (float)((StartCapture.Value.Y - pos.Y) * (1 / ((OpenGLControl)sender).ActualHeight));
                        roadmap.RoadMapParametrs.ViewOffset.X = currentView.X - lastView.X;
                        roadmap.RoadMapParametrs.ViewOffset.Y = currentView.Y + lastView.Y;
                    break;
                    case MouseInterractMode.AddRoad:
                        newRoadEnd = new PointF((float)pos.X, (float)pos.Y);
                    break;
                }
            }
        }

        public void Draw()
        {

        }
    }
}
