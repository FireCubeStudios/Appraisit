using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;

namespace Appraisit.Helpers
{
    internal class CircleDrawer : IContentDrawer

    {

        private float _radius;

        private Color _color;



        public CircleDrawer(float radius, Color color)

        {

            _radius = radius;

            _color = color;

        }



        public float Radius

        {

            get { return _radius; }

        }



        public Color Color

        {

            get { return _color; }

        }



#pragma warning disable 1998

        public async Task Draw(CompositionGraphicsDevice device, Object drawingLock, CompositionDrawingSurface surface, Size size)

        {

            using (var ds = CanvasComposition.CreateDrawingSession(surface))

            {

                ds.Clear(Colors.Transparent);

                ds.FillCircle(new Vector2(_radius, _radius), _radius, _color);

            }

        }
    }
}
