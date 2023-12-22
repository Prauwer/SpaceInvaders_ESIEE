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

        /// <summary>
        /// Update the state of the Projecile
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
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

        /// <summary>
        /// Render the Player Spaceship
        /// </summary>
        /// <param name="rand">random object (we put it in params to avoid that every random have the same value)</param>
        /// <param name="posX">Position X where we want to create the bonus</param>
        /// <param name="posY">Position Y where we want to create the bonus</param>

        public static Projectile RandomCreation(Random rand, double posX, double posY)
        {
            if (rand.NextDouble() < 0.15)
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