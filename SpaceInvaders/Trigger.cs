using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Trigger : GameObject
    {
        public Vecteur2D Position { get; protected set; }
        public Size Size { get; protected set; }
        public Image Image { get; protected set; }

        public Trigger(Vecteur2D position, Size size)
        {
            Position = position;
            Size = size;
            Image = Properties.Resources.Trigger;
        }

        public override void Collision(Missile m)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            Console.WriteLine(gameInstance.gameSize.Width);
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive()
        {
            return true;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            //throw new NotImplementedException();
        }
    }
}
