using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Missile : GameObject
    {
        //Attributs
        public Vecteur2D Position { get; private set; }
        
        public double Speed { get; private set; }

        public int Lives { get; private set; }

        public Bitmap Image { get; private set; }

        //Constructeur
        public Missile(Vecteur2D position, double speed, int lives, Bitmap image)
        {
            Position = position;
            Speed = speed;
            Lives = lives;
            Image = image;
        }

        //Méthodes
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive()
        {
            return Lives > 0;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Déplacement du missile
            Position.y += Speed * deltaT;

            // Tuer si le missile sort du cadre de jeu

            if(Position.y < 0 + Image.Height || Position.y > gameInstance.gameSize.Width)
            {
                Lives = 0;
            }

        }
    }
}
