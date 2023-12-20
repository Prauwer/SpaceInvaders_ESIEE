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
    internal class LifeBonus : Projectile
    {
        public LifeBonus(Vecteur2D position, double speed, Side side) : base(position, speed, 1, Properties.Resources.bonus, side)
        {
        }

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

        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            if (p.Side == Side.Ally)
            {
                p.Lives = 0;
                this.Lives = 0;
            }
        }
    }
}
