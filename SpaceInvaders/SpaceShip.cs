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

        public SpaceShip(Vecteur2D position, int lives, Bitmap image)
        {
            Position = position;
            Lives = lives;
            Image = image;
        }

        public override void Update(Game gameInstance, double deltaT)
        {

        }

        public void Shoot(Game gameInstance)
        {
            if (Missile == null || !Missile.IsAlive())
            {
                Bitmap missileImage = Properties.Resources.shoot1;

                // TODO : DEBUG/ RETIRER LE -20 APRES Position.y QUAND ON AURA LE FF
                Missile = new Missile(new Vecteur2D(Position.x + Image.Width / 2 - missileImage.Width/2, Position.y - 20), -400, 17, missileImage);


                gameInstance.AddNewGameObject(Missile);
            }
        }

        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            Console.WriteLine("OI");

            int damage = Math.Min(m.Lives, this.Lives);
            m.Lives -= damage;
            this.Lives -= damage;
        }
    }
}
