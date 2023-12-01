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
       
        public double Speed { get; private set; }

        //Constructeur
        public Missile(Vecteur2D position, double speed, int lives, Bitmap image, Side side): base(side)
        {
            Position = position;
            Speed = speed;
            Lives = lives;
            Image = image;
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
                gameObject.Collision(this);
            }
        }

        public override void Collision(Missile m)
        {
            return;
        }
    }
}