using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Bunker : SimpleObject
    {

        public Side Side { get; set; } = Side.Neutral;

        public Bunker(Vecteur2D Position)
        {
            this.Position = Position;
            this.Image = Properties.Resources.bunker;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            return;
        }

        public bool CollisionRectangle(Missile m)
        {
            if (m.Position.x <= this.Position.x + this.Image.Width // Le missile est en collision gauche du bunker
             && m.Position.x + m.Image.Width >= this.Position.x    // Le missile est en collision droite du bunker
             && m.Position.y + m.Image.Height >= this.Position.y   // Le missile est en collision haut du bunker
             && m.Position.y <= this.Position.y + this.Image.Height)  // Le missile est en collision bas du bunker
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Collision(Missile m)
        {
            if (CollisionRectangle(m))
            {
                for (int y = 0; y < m.Image.Height; y++)
                {
                    for (int x = 0; x < m.Image.Width; x++)
                    {
                        Color currentPixelColor = m.Image.GetPixel(x, y);

                        if (currentPixelColor.A == 0)
                        {
                            continue;
                        }

                        double xBunker = m.Position.x + x - Position.x;
                        double yBunker = m.Position.y + y - Position.y;

                        if (xBunker < 0 || xBunker >= Image.Width
                            || yBunker < 0 || yBunker >= Image.Height)
                        {
                            continue;
                        }

                        Color bunkerPixelColor = Image.GetPixel((int)xBunker, (int)yBunker);
                        if (bunkerPixelColor.A != 0)
                        {
                            Color newColor = Color.FromArgb(0, 0, 0, 0);
                            Image.SetPixel((int)xBunker, (int)yBunker, newColor);
                            m.Lives -= 1;
                        }
                    }
                }
            }
        }
    }
}
