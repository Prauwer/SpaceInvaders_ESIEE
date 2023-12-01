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
        public Side Side {  get; private set; }

        private double speedPixelPerSecond = 2.0;
        public Missile Missile { get; private set; }

        public SpaceShip(Vecteur2D position, int lives, Bitmap image, Side side): base(side)
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

                Missile = new Missile(new Vecteur2D(Position.x + Image.Width / 2 - missileImage.Width/2, Position.y), -400, 17, missileImage, Side.Ally);


                gameInstance.AddNewGameObject(Missile);
            }
        }

        public override void Collision(Missile m)
        {
            return;
        }
    }
}
