using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;

namespace Appraisit.Helpers
{
    public class ImageLoader
    {

        private static bool _intialized;
        private static ImageLoader _imageLoader;
        private Compositor _compositor;

        private CanvasDevice _canvasDevice;

        private CompositionGraphicsDevice _graphicsDevice;

        private Object _drawingLock;
       
        public ManagedSurface LoadCircle(float radius, Color color)

        {
            _graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, _canvasDevice);


            ManagedSurface surface = new ManagedSurface(CreateSurface(new Size(radius * 2, radius * 2)));

            var ignored = surface.Draw(_graphicsDevice, _drawingLock, new CircleDrawer(radius, color));



            return surface;

        }
        private CompositionDrawingSurface CreateSurface(Size size)

        {

            Size surfaceSize = size;

            if (surfaceSize.IsEmpty)

            {

                //

                // We start out with a size of 0,0 for the surface, because we don't know

                // the size of the image at this time. We resize the surface later.

                //

                surfaceSize = default(Size);

            }



            var surface = _graphicsDevice.CreateDrawingSurface(surfaceSize, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);



            return surface;

        }
    }
}
