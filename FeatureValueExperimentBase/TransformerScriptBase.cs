using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptBase;
using SlimDX;

namespace ScriptBase
{
    public abstract class TransformerScriptBase
    {
        protected virtual IWritableImageContent GenerateImageBuffer(IImageContent source)
        {
            return new WrappedSimpleImage(source.Width,source.Height);
        }

        protected virtual void TransformImageBuffer(IWritableImageContent target,IImageContent source)
        {
            for (int x = 0; x < target.Width; x++)
            {
                for (int y = 0; y < target.Height; y++)
                {
                    target.SetAt(x,y,TransformImageBufferEachPixel(x,y,source));
                }
            }
        }

        protected virtual Vector4 TransformImageBufferEachPixel(int x, int y, IImageContent content)
        {
            return content.GetAt(x, y);
        }

        public virtual IImageContent Transform(IImageContent source)
        {
            IWritableImageContent writable = GenerateImageBuffer(source);
            TransformImageBuffer(writable,source);
            return writable;
        }
    }

    public class WrappedSimpleImage : IWritableImageContent
    {
        private Vector4[][] imageSource;

        public WrappedSimpleImage(int width,int height)
        {
            Width = width;
            Height = height;
            imageSource=new Vector4[Width][];
            for (int x = 0; x < Width; x++)
            {
                imageSource[x] = new Vector4[Height];
                for (int y = 0; y < Height; y++)
                {
                    imageSource[x][y] = Vector4.Zero;
                }
            }
        }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Vector4 GetAt(int x, int y)
        {
            return imageSource[x][y];
        }

        public void SetAt(int x, int y, Vector4 col)
        {
            imageSource[x][y] = col;
        }
    }
}
