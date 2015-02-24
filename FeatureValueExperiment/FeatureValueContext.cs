using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Device = SlimDX.Direct3D11.Device;
using MapFlags = SlimDX.Direct3D11.MapFlags;

namespace FeatureValueExperiment
{
    public class FeatureValueContext
    {
        public Device device;

        public FeatureValueContext()
        {
            this.device = new Device(DriverType.Hardware, DeviceCreationFlags.Debug,
                new FeatureLevel[] {FeatureLevel.Level_11_0,});
        }

        public WrappedImage GetWrappedImage(string filePath)
        {
            return new WrappedImage(GetTexture2D(filePath),this);
        }

        public Texture2D GetTexture2D(string fileName)
        {
            var imgInfo=Texture2D.FromFile(device, fileName);
            var d = imgInfo.Description;
            var imgInfoConverted = Texture2D.FromFile(device, fileName, new ImageLoadInformation()
            {
                BindFlags =BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                Format = d.Format,
                Width = d.Width,
                Height = d.Height,
                MipLevels = d.MipLevels,
                Usage =ResourceUsage.Staging
                
            });
            return imgInfoConverted;
        }

        public DataBox GetAsFloatArray(Texture2D texture)
        {
            return device.ImmediateContext.MapSubresource(texture,0,0, MapMode.Read, MapFlags.None);
        }
    }
}
