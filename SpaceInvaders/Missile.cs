using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Missile : GameObject
    {

        public Vecteur2D Position { get; private set; }
        
        public double Speed { get; private set; }

        public int Lives { get; private set; }

        public Bitmap Image { get; private set; }

        // TODO : INSERER CONSTRUCTEUR

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            throw new NotImplementedException();
        }

        public override bool IsAlive()
        {
            throw new NotImplementedException();
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            throw new NotImplementedException();
        }
    }
}
