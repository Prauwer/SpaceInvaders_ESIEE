using System;
using System.Drawing;

namespace SpaceInvaders
{
    internal class SpaceShip : SimpleObject
    {
        public Projectile Missile;

        /// <summary>
        /// Public constructor for a SpaceShip
        /// </summary>
        /// <param name="position">Position of the spaceship</param>
        /// <param name="lives">lives of the spaceship</param>
        /// <param name="image">Image of the spaceship</param>
        /// <param name="side">Side of the spaceship</param>
        public SpaceShip(Vecteur2D position, int lives, Bitmap image, Side side): base(side)
        {
            Position = position;
            Lives = lives;
            Image = image;
            InitialLives = lives;
        }

        /// <summary>
        /// Update the state of a spaceship
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
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
        /// handle the case of the spaceship is hit
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
