using System.Drawing;

namespace SpaceInvaders
{
    internal class Missile : Projectile
    {

        /// <summary>
        /// Public constructor for a missile
        /// </summary>
        /// <param name="position">Position of the missile</param>
        /// <param name="speed">Speed of the missile</param>
        /// <param name="lives">lives of the missile</param>
        /// <param name="image">Image of the missile</param>
        /// <param name="side">Side of the missile</param>
        public Missile(Vecteur2D position, double speed, int lives, Bitmap image, Side side) : base(position, speed, lives, image, side)
        {
        }

        /// <summary>
        /// Update the state of the Missile
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            base.Update(gameInstance, deltaT);
            // Collision test with game objects
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if (gameObject != this)
                {
                    gameObject.Collision(this);
                }
            }
        }

        /// <summary>
        /// handle the case of the missile is hit
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