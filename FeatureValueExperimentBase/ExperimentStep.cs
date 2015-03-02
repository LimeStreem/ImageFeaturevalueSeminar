using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SlimDX;

namespace ScriptBase
{
    public interface IExperimentStep
    {
        int StepNumber { get; }

        IExperimentStep NextStep { get; set; }

        IExperimentStep ForeStep { get; set; }

        void UpdateStep();

        IImageContent CurrentImageContent { get;  }

        void SetPreview(IExperimentStep step);
    }

    public interface IImageContent
    {
        int Width { get; }

        int Height { get; }

        Vector4 GetAt(int x, int y);
    }

    public interface IWritableImageContent : IImageContent
    {
        void SetAt(int x,int y,Vector4 col);
    }
}
