using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class MissileBonus : Projectile
    {
        public MissileBonus(Vecteur2D position, double speed, Side side) : base(position, speed, 1, Properties.Resources.bonus2, side)
        {
        }

        /// <summary>
        /// Update the state of the Missile Bonus
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            base.Update(gameInstance, deltaT);
            // Test collision avec les objets du jeu
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if (gameObject.GetType() == typeof(PlayerSpaceship))
                {
                    gameObject.Collision(this);
                }
            }
        }

        /// <summary>
        /// handle the case of the missile bonus is hitted
        /// </summary>
        /// <param name="p">projectile in collision with the missile bonus</param>
        /// <param name="numberOfPixelsInCollision">number of pixels in collision with the missile bonus</param>
        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            if (p.Side == Side.Ally)
            {
                p.Lives = 0;
                this.Lives = 0;

                Game.game.PlayerShip.handleBonus(this);
            }
        }
    }
}
