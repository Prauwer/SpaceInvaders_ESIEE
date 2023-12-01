using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        protected SimpleObject(Side Side) : base(Side)
        {
        }

        public Side Side { get; private set; }
        public Vecteur2D Position { get; protected set; }
        public int Lives { get; set; } = 1;
        public Bitmap Image { get; protected set; }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive()
        {
            return Lives > 0;
        }
    }
}
