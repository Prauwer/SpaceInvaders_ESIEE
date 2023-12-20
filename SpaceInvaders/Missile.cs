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

        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            p.Lives = 0;
            this.Lives = 0;
        }
    }
}