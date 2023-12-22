using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Missile : Projectile
    {

        //Constructeur
        public Missile(Vecteur2D position, double speed, int lives, Bitmap image, Side side) : base(position, speed, lives, image, side)
        {
        }

        //Méthodes

        /// <summary>
        /// Update the state of the Missile
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            base.Update(gameInstance, deltaT);
            // Test collision avec les objets du jeu
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if (gameObject != this)
                {
                    gameObject.Collision(this);
                }
            }
        }

        /// <summary>
        /// handle the case of the missile is hitted
        /// </summary>
        /// <param name="p">projectile in collision with the missile</param>
        /// <param name="numberOfPixelsInCollision">number of pixels in collision with the missile</param>
        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            p.Lives = 0;
            this.Lives = 0;
        }
    }
}