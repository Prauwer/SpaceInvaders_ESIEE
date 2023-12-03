﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class PlayerSpaceship : SpaceShip
    {
        public PlayerSpaceship(Vecteur2D position, int lives) : base(position, lives, Properties.Resources.ship3, Side.Ally)
        {
        }

        public override void Update(Game gameInstance, double deltaT)
        {

            // Déplacement du joueur
            if (gameInstance.keyPressed.Contains(Keys.Left) && Position.x >= 0)
            {
                Position.x -= 2.0;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right) && Position.x + Image.Size.Width <= gameInstance.gameSize.Width)
            {
                Position.x += 2.0;
            }


            // Tirer un missile
            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                // Le -1 correspond a la direction vers le haut
                Shoot(gameInstance, -1);
            }
        }
    }
}