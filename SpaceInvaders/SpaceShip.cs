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

        public void Shoot(Game gameInstance, int direction, Side side)
        {
            if (Missile == null || !Missile.IsAlive())
            {
                Bitmap missileImage = Properties.Resources.shoot1;

                Missile = new Missile(new Vecteur2D(Position.x + Image.Width / 2 - missileImage.Width/2, Position.y), direction*400, 20, missileImage, side);


                gameInstance.AddNewGameObject(Missile);
            }
        }

        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            int damage = Math.Min(m.Lives, this.Lives);
            m.Lives -= damage;
            this.Lives -= damage;
        }
    }
}
