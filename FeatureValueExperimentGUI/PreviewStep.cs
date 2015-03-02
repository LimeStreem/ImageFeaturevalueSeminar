using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatureValueExperiment;

namespace ScriptBase
{
    public class PreviewStep:StepBase
    {
        private readonly FeatureValueContext _context;
        private readonly PreviewForm form;

        public PreviewStep(FeatureValueContext context)
        {
            _context = context;
            this.form = new PreviewForm(context);
        }

        public override int StepNumber { get; set; }
        public override IExperimentStep NextStep { get;  set; }
        public override IExperimentStep ForeStep { get;  set; }
        public override IImageContent CurrentImageContent { get; set; }

        public override void UpdateStep()
        {
            if (ForeStep != null)
            {
                var targetImageContent = ForeStep.CurrentImageContent;
                if (targetImageContent == null) return;
                form.Width = targetImageContent.Width;
                form.Height = targetImageContent.Height;
                form.TargetImage=CurrentImageContent = targetImageContent;
                if (!form.Visible)
                {
                    form.Show();
                }
                if(NextStep!=null)
                    NextStep.UpdateStep();
            }
        }


    }
}
