using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class PlayerSpaceship : SpaceShip
    {
        public int Points;

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
            if (gameInstance.keyPressed.Contains(Keys.Right) && Position.x + Image.Size.Width <= gameInstance.GameSize.Width)
            {
                Position.x += 2.0;
            }


            // Tirer un missile
            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                // Le -1 correspond a la direction vers le haut
                Shoot(gameInstance, -1, Side.Ally);
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            privateFontCollection.AddFontFile("C:\\Users\\antoninmansour\\source\\repos\\projet-spaceinvaders2023-zackary-saada-antonin-mansour\\SpaceInvaders\\Resources\\space_invaders.ttf");
            SolidBrush brush = new SolidBrush(Color.White);
            Font font = new Font(privateFontCollection.Families[0], 12);
            graphics.DrawString($"{Lives} lives remaining", font, brush, gameInstance.GameSize.Width /20, gameInstance.GameSize.Height*19/20);
            graphics.DrawString($"{Points} Points", font, brush, gameInstance.GameSize.Width * 16 / 20, gameInstance.GameSize.Height * 19 / 20);
        }
    }
}
