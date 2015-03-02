using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatureValueExperiment;
using SlimDX;

namespace ScriptBase
{
    public class SourceStep:StepBase
    {
        public SourceStep(WrappedImage image,IExperimentStep next)
        {
            _currentImageContent=new ContentImageWrapper(image);
            next.SetPreview(this);
        }

        private int _stepNumber;
        private IExperimentStep _nextStep;
        private IExperimentStep _foreStep;
        private IImageContent _currentImageContent;

        public override int StepNumber
        {
            get { return _stepNumber; }
            set { _stepNumber = value; }
        }

        public override IExperimentStep NextStep { get; set; }

        public override IExperimentStep ForeStep
        {
            get { throw new NotImplementedException(); }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void UpdateStep()
        {
            if(NextStep!=null)NextStep.UpdateStep();
        }

        public override IImageContent CurrentImageContent
        {
            get { return _currentImageContent; }
            set { _currentImageContent = value; }
        }

        private class ContentImageWrapper:IImageContent
        {
            private readonly WrappedImage _wrappedImage;

            public ContentImageWrapper(WrappedImage wrappedImage)
            {
                _wrappedImage = wrappedImage;
            }

            public int Width
            {
                get { return _wrappedImage.Width; }
            }

            public int Height
            {
                get { return _wrappedImage.Height; }
            }

            public Vector4 GetAt(int x, int y)
            {
                return _wrappedImage[x, y];
            }
        }
    }
}
