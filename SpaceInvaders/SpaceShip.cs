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
        public Missile Missile { get; private set; }

        public SpaceShip(Vecteur2D position, int lives)
        {
            Position = position;
            Lives = lives;
            Image = Properties.Resources.ship3;
        }

        public override void Update(Game gameInstance, double deltaT)
        {

            // Déplacement du joueur
            if (gameInstance.keyPressed.Contains(Keys.Left) && Position.x >= 0)
            {
                Position.x -= 2.0;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right) && Position.x + Image.Size.Width <= gameInstance.gameSize.Width)
            {
                Position.x += 2.0;
            }


            // Tirer un missile
            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                Shoot(gameInstance);
            }
        }

        public void Shoot(Game gameInstance)
        {
            if (Missile == null || !Missile.IsAlive())
            {
                Bitmap missileImage = Properties.Resources.shoot1;

                Missile = new Missile(new Vecteur2D(Position.x + Image.Width / 2 - missileImage.Width/2, Position.y), -400, 1, missileImage);


                gameInstance.AddNewGameObject(Missile);
            }
        }

        public override void Collision(Missile m)
        {
            return;
        }
    }
}
