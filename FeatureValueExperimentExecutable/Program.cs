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
            var data = context.GetWrappedImage(@"C:\Users\LimeStreem\Pictures\ee9f30179b25b4a265df6536f05d38ca.png");
            var pix = data[100, 100];
        }
    }
}
