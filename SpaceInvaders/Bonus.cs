using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Bonus : Projectile
    {
        public Bonus(Vecteur2D position, double speed, Side side) : base(position, speed, 1, Properties.Resources.bonus, side)
        {
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Déplacement du missile
            Position.y += Speed * deltaT;

            // Tuer si le missile sort du cadre de jeu
            if (Position.y < 0 - Image.Height || Position.y > gameInstance.GameSize.Width)
            {
                Lives = 0;
            }

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
