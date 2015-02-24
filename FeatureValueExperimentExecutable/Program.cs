using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatureValueExperiment;

namespace FeatureValueExperimentExecutable
{
    class Program
    {
        static void Main(string[] args)
        {
            FeatureValueContext context=new FeatureValueContext();
            using (var data = context.GetWrappedImage(@"C:\Users\LimeStreem\Pictures\ee9f30179b25b4a265df6536f05d38ca.png"))
            {
                for (int x = 0; x < data.Width; x++)
                {
                    for (int y = 0; y < data.Height; y++)
                    {
                        var d = data[x, y];
                        Console.WriteLine("(X,Y)=({0},{1})=RGB({2},{3},{4})",x,y,d.X,d.Y,d.Z);
                    }
                }
            }
            
        }
    }
}
