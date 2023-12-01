using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Missile : SimpleObject
    {
        //Attributs

        public Side Side { get; private set; }
       
        public double Speed { get; private set; }

        //Constructeur
        public Missile(Vecteur2D position, double speed, int lives, Bitmap image, Side side)
        {
            Position = position;
            Speed = speed;
            Lives = lives;
            Image = image;
            Side = side;
        }

        //Méthodes

        public override void Update(Game gameInstance, double deltaT)
        {
            // Déplacement du missile
            Position.y += Speed * deltaT;

            // Tuer si le missile sort du cadre de jeu
            if(Position.y < 0 - Image.Height || Position.y > gameInstance.gameSize.Width)
            {
                Lives = 0;
            }

            // Test collision avec les objets du jeu
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if (gameObject != this) {
                    gameObject.Collision(this);
                }
            }
        }

        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            m.Lives = 0;
            this.Lives = 0;
        }
    }
}