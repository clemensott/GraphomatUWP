using MathFunction;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace GraphomatUWP
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static TextBlock tblTicksStatic;
        private Graph graph;

        public MainPage()
        {
            this.InitializeComponent();
            tblTicksStatic = tblTicks;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MoveScollManager.Current.SetEvents(canvasControl, gdMain, 90, 160, 0, 0);
            AxesGraph ag = AxesGraph.Current;
            graph = new Graph("5-5*e^(-2*x)", Color.FromArgb(255, 0, 0, 0));

            canvasControl.Invalidate();

            Calc();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calc();
        }

        private void Calc()
        {
            /*  Function f = Function.Parse(tbxEquation.Text);

              tblImprove.Text = f.ImprovedEquation;

              tblY.Text = f[double.Parse(tbxX.Text)].ToString();      //  */
        }

        private void canvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            AxesGraph.Current.Draw(args.DrawingSession);

            graph.Draw(args.DrawingSession);
        }

        public static void WriteLine(object obj)
        {
            string output = "";
            List<string> lines = tblTicksStatic.Text.Split('\n').ToList();

            while (lines.Count > 500) lines.RemoveAt(0);

            lines.Add(obj.ToString());

            for (int i = lines.Count - 1, j = 0; i >= 0 && j < 40; i--)
            {
                if (lines[i].Length != 0)
                {
                    output = lines[i] + "\n" + output;
                    j++;
                }
            }

            output = "\n" + output.TrimEnd('\n');

            tblTicksStatic.Text = output;
        }
    }
}
