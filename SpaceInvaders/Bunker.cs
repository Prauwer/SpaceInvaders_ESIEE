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
    }
}