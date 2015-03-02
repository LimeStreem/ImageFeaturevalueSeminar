using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FeatureValueExperiment;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using MapFlags = SlimDX.Direct3D11.MapFlags;
using Resource = SlimDX.Direct3D11.Resource;

namespace ScriptBase
{
    public partial class PreviewForm : Form
    {
        private readonly FeatureValueContext _context;
        private IImageContent _targetImage;
        private SwapChain swapChain;
        private RenderTargetView targetView;
        private Texture2D targetTexture;
        private Texture2D stagingTexture;

        public IImageContent TargetImage
        {
            get { return _targetImage; }
            set
            {
                _targetImage = value;
                UpdateImages();
            }
        }

        private unsafe void UpdateImages()
        {
            if (swapChain==null)
            {
                InitSwapchains();
            }
            else
            {
                if (swapChain.Description.ModeDescription.Width != Width ||
                    swapChain.Description.ModeDescription.Height != Height)
                {
                    swapChain.ResizeBuffers(2, Width, Height, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
                }
            }
            CheckAndDispose(stagingTexture);
            targetTexture = Resource.FromSwapChain<Texture2D>(swapChain, 0);
            stagingTexture = new Texture2D(_context.device, new Texture2DDescription()
            {
                ArraySize = 1,
                BindFlags = BindFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                Width = Width,
                Height = Height,
                Usage = ResourceUsage.Staging,
                CpuAccessFlags = CpuAccessFlags.Write,
                Format = Format.R8G8B8A8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.None
            });
            var dataBox = _context.device.ImmediateContext.MapSubresource(stagingTexture, 0, 0, MapMode.Write, MapFlags.None);
            for (int x = 0; x < _targetImage.Width; x++)
            {
                for (int y = 0; y < _targetImage.Height; y++)
                {
                    byte* fPtr = (byte*)(dataBox.Data.DataPointer + (4 * y * _targetImage.Width + x * 4));
                    var v = _targetImage.GetAt(x, y);
                    (*fPtr) = (byte)v.X;
                    (*(fPtr + 1)) = (byte)v.Y;
                    (*(fPtr + 2)) = (byte)v.Z;
                    (*(fPtr + 3)) = (byte)v.W;
                }
            }
            _context.device.ImmediateContext.UnmapSubresource(stagingTexture, 0);
            _context.device.ImmediateContext.CopyResource(stagingTexture, targetTexture);
            Invalidate();
        }

        private void InitSwapchains()
        {
            using (Factory factory=new Factory())
            {
                 swapChain = new SwapChain(factory,_context.device,new SwapChainDescription()
                 {
                     BufferCount = 2,
                     Flags = SwapChainFlags.AllowModeSwitch,
                     IsWindowed = true,
                     SampleDescription = new SampleDescription(1,0),
                     ModeDescription = new ModeDescription(Width,Height,new Rational(1,60),Format.R8G8B8A8_UNorm ),
                     OutputHandle = Handle,
                     SwapEffect = SwapEffect.Discard,
                     Usage = Usage.RenderTargetOutput
                 });
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CheckAndDispose(swapChain);
            CheckAndDispose(stagingTexture);
            base.OnClosing(e);
        }

        private void CheckAndDispose(IDisposable disposable)
        {
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (swapChain != null)
            {
                swapChain.Present(0, PresentFlags.None);
            }
        }

        public PreviewForm(FeatureValueContext context)
        {
            _context = context;
            InitializeComponent();
        }


    }
}
