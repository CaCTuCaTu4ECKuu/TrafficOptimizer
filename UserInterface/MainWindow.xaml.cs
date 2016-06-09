using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using SharpGL;
using SharpGL.WPF;

namespace UserInterface
{
    using Visualize;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gl_RoadMap roadMap;
        object lastModeBtn;

        public MainWindow()
        {
            InitializeComponent();

            roadMap = new Gl_RoadMap(OpenGLWindow.OpenGL);

            OpenGLWindow.MouseLeftButtonDown += roadMap.Controller.OpenGLWindow_MouseLeftButtonDown;
            OpenGLWindow.MouseLeftButtonUp += roadMap.Controller.OpenGLWindow_MouseLeftButtonUp;
            OpenGLWindow.MouseMove += roadMap.Controller.OpenGLWindow_MouseMove;
            ZoomIn.Click += roadMap.Controller.ZoomIn_Click;
            ZoomOut.Click += roadMap.Controller.ZoomOut_Click;

            lastModeBtn = MoveModeBtn;
            MoveModeBtn.FontWeight = FontWeights.Bold;
        }

        private void OpenGLWindow_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var control = ((OpenGLControl)sender);
            control.OpenGL.ClearColor(1f, 1f, 1f, 1f);
            control.OpenGL.Perspective(60f, control.ActualWidth / control.ActualHeight, 1f, 100f);
        }

        private void OpenGLWindow_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = ((OpenGLControl)sender).OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            roadMap.Draw();

            gl.Flush();
        }
        
        public void setFont(object newBtn)
        {
            ((Button)newBtn).FontWeight = FontWeights.Bold;
            ((Button)lastModeBtn).FontWeight = FontWeights.Normal;
            lastModeBtn = newBtn;
        }
        private void MoveModeBtn_Click(object sender, RoutedEventArgs e)
        {
            roadMap.Controller.IntMode = MouseInterractMode.MoveView;
            setFont(sender);
        }
        private void AddModeBtn_Click(object sender, RoutedEventArgs e)
        {
            roadMap.Controller.IntMode = MouseInterractMode.AddRoad;
            setFont(sender);
        }
        private void EditModeBtn_Click(object sender, RoutedEventArgs e)
        {
            roadMap.Controller.IntMode = MouseInterractMode.Select;
            setFont(sender);
        }
    }
}
