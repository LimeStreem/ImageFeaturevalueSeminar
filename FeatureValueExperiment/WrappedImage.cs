using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;

namespace FeatureValueExperiment
{
    public class WrappedImage:IDisposable
    {
        private readonly Texture2D _texture;
        private FeatureValueContext context;

        private DataBox dataBox;

        public int Height { get; set; }

        public int Width { get; set; }

        public WrappedImage(Texture2D texture, FeatureValueContext context)
        {
            _texture = texture;
            this.context = context;
            dataBox = context.device.ImmediateContext.MapSubresource(texture, 0, 0, MapMode.Read, MapFlags.None);
            this.Height = texture.Description.Height;
            this.Width = texture.Description.Width;
        }

        public unsafe Vector4 this[int x, int y]
        {
            get
            {
                byte* fPtr=(byte*)(dataBox.Data.DataPointer+(4*y*Width + x*4));
                return new Vector4(*fPtr,*(fPtr+1),*(fPtr+2),*(fPtr+3));
            }
        }



        public void Dispose()
        {
            context.device.ImmediateContext.UnmapSubresource(_texture,0);
        }


    }
}
