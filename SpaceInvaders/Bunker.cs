using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Bunker : SimpleObject
    {
        public Bunker(Vecteur2D Position)
        {
            this.Position = Position;
            this.Image = Properties.Resources.bunker;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            return;
        }

        public bool CollisionRectangle(Missile m)
        {
            if (m.Position.x <= this.Position.x + this.Image.Width // Le missile est en collision gauche du bunker
             && m.Position.x + m.Image.Width >= this.Position.x    // Le missile est en collision droite du bunker
             && m.Position.y + m.Image.Height >= this.Position.y   // Le missile est en collision haut du bunker
             && m.Position.y <= this.Position.y + this.Image.Height)  // Le missile est en collision bas du bunker
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Collision(Missile m)
        {
            if (CollisionRectangle(m))
            {
                // TODO : TESTER PIXEL PAR PIXEL
            }
        }

    }
}
