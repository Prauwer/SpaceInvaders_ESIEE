using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    abstract class Projectile : SimpleObject
    {
        //Attributs
       
        public double Speed { get; private set; }

        //Constructeur
        public Projectile(Vecteur2D position, double speed, int lives, Bitmap image, Side side): base(side)
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
            if(Position.y < 0 - Image.Height || Position.y > gameInstance.GameSize.Width)
            {
                Lives = 0;
            }
        }

        public static Projectile RandomCreation(Random rand, double posX, double posY)
        {
            if (rand.NextDouble() < 0.1)
            {
                if (rand.NextDouble() > 0.5)
                {
                    // create a new lifeBonus object
                    LifeBonus newBonus = new LifeBonus(new Vecteur2D(posX, posY), 100, Side.Bonus);
                    return newBonus;
                }
                else
                {
                    // create a new MissileBonus object
                    MissileBonus newBonus = new MissileBonus(new Vecteur2D(posX, posY), 100, Side.Bonus);
                    return newBonus;
                }
            }
            return null;
        }
    }
}