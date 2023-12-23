using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceInvaders
{
    internal class SpaceShip : SimpleObject
    {
        public Projectile Missile { get; private set; }

        public SpaceShip(Vecteur2D position, int lives, Bitmap image, Side side): base(side)
        {
            Position = position;
            Lives = lives;
            Image = image;
            InitialLives = lives;
        }

        public override void Update(Game gameInstance, double deltaT)
        {

        }

        /// <summary>
        /// shoot a missile
        /// </summary>
        /// <param name="direction">direction (up or down)</param>
        /// <param name="side">side (ally or enemy)</param>
        /// <param name="gameInstance">instance of the current game</param>
        public void Shoot(Game gameInstance, int direction, Side side)
        {
            if (Missile == null || !Missile.IsAlive())
            {
                Bitmap missileImage = Properties.Resources.shoot1;

                Missile = new Missile(new Vecteur2D(Position.x + Image.Width / 2 - missileImage.Width/2, Position.y), direction*400, 20, missileImage, side);


                gameInstance.AddNewGameObject(Missile);
            }
        }

        /// <summary>
        /// handle the case of the spaceship is hitted
        /// </summary>
        /// <param name="p">projectile in collision with the spaceship</param>
        /// <param name="numberOfPixelsInCollision">number of pixels in collision with the spaceship</param>
        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            int damage = Math.Min(p.Lives, this.Lives);
            p.Lives -= damage;
            this.Lives -= damage;
        }
    }
}
