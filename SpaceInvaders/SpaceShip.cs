using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class SpaceShip : GameObject
    {
        private double speedPixelPerSecond = 2.0;
        public Vecteur2D Position {  get; private set; }
        public int Lives { get; private set; }
        public Bitmap Image { get; private set; }

        public SpaceShip(Vecteur2D position, int lives, Bitmap image)
        {
            Position = position;
            Lives = lives;
            Image = image;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance.keyPressed.Contains(Keys.Left) && Position.x >= 0)
            {
                Position.x -= 2.0;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right) && Position.x + Image.Size.Width <= gameInstance.gameSize.Width)
            {
                Position.x += 2.0;
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }
        
        public override bool IsAlive()
        {
            return Lives > 0;
        }
    }
}
