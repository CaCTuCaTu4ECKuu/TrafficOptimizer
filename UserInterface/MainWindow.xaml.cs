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
using SharpGL;
using SharpGL.WPF;

namespace UserInterface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void OpenGLWindow_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            ((OpenGLControl)sender).OpenGL.ClearColor(1f, 1f, 1f, 1f);
        }

        private void OpenGLWindow_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            ((OpenGLControl)sender).OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);



            ((OpenGLControl)sender).OpenGL.Flush();
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
