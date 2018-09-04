using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathFunction;
using Windows.UI;

namespace GraphomatDrawingLibUwp
{
    public class RandomGraph
    {
        private static Random ran = new Random();

        public static Graph Get()
        {
            Graph graph = new Graph();

            graph.Name = string.Format("Random {0}", ran.Next(100));

            do
            {
                string equation = Function.GetRandomEquation(ran);

                System.Diagnostics.Debug.WriteLine(equation);
                graph.OriginalEquation = equation;

            } while (!graph.IsPossible || !IsGoodGraph(graph));

            byte[] rgb = new byte[3];

            ran.NextBytes(rgb);

            graph.Color = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]);

            return graph;
        }

        private string GetRandomNumber()
        {
            return (ran.NextDouble() * ran.Next(-100000, 100000)).ToString();
        }

        private static bool IsGoodGraph(Graph graph)
        {
            double[] xes = new double[10];

            for (int i = 0; i < xes.Length; i++)
            {
                xes[i] = graph[ran.NextDouble() * ran.Next(-100000, 100000)];
            }

            for (int i = 0; i < xes.Length; i++)
            {
                for (int j = i + 1; j < xes.Length; j++)
                {
                    if (xes[i] != xes[j]) return true;
                }
            }

            return false;
        }
    }
}
