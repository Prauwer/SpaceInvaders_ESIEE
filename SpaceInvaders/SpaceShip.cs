using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class SpaceShip : SimpleObject
    {
        private double speedPixelPerSecond = 2.0;

        public SpaceShip(Vecteur2D position, int lives)
        {
            Position = position;
            Lives = lives;
            Image = Properties.Resources.ship3;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance.keyPressed.Contains(Keys.Left) && Position.x >= 0)
            {
                Position.x -= 2.0;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right) && Position.x + Image.Size.Width <= gameInstance.gameSize.Width)
            {
                Position.x += 2.0;
            }
        }
    }
}
