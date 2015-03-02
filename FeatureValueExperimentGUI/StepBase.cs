
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptBase
{
    public abstract class StepBase:IExperimentStep
    {
        public abstract int StepNumber { get; set; }
        public abstract IExperimentStep NextStep { get; set; }
        public abstract IExperimentStep ForeStep { get; set; }
        public abstract void UpdateStep();
        public abstract IImageContent CurrentImageContent { get; set; }

        public void SetPreview(IExperimentStep step)
        {
            if (step != null)
            {
                step.NextStep = this;
                ForeStep = step;
            }
        }
    }
}
