using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class PlayerSpaceship : SpaceShip
    {
        public PlayerSpaceship(Vecteur2D position, int lives) : base(position, lives)
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
                Shoot(gameInstance);
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);
            Font font = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);
            graphics.DrawString($"{Lives} vies restantes", font, brush, gameInstance.gameSize.Width /20, gameInstance.gameSize.Height*19/20);
        }
    }
}
