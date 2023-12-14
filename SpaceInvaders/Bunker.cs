using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Bunker : SimpleObject
    {


        public Bunker(Vecteur2D Position): base(Side.Neutral)
        {
            this.Position = Position;
            this.Image = Properties.Resources.bunker;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            return;
        }

        protected override void OnCollision(Projectile m, int numberOfPixelsInCollision)
        {
            m.Lives -= numberOfPixelsInCollision;
        }
    }
}
