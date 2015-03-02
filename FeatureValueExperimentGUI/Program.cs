using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FeatureValueExperiment;

namespace ScriptBase
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            FeatureValueContext context=new FeatureValueContext();
            var image=context.GetWrappedImage("IMG\\img.jpg");
            SourceStep step=new SourceStep(image,new PreviewStep(context));
            TransformStep step2=new TransformStep(context,step.NextStep);
            PreviewStep preview2=new PreviewStep(context);
            preview2.SetPreview(step2);
            step.UpdateStep();
            Application.Run(new ControlForm());
        }
    }
}
