using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        protected SimpleObject(Side Side) : base(Side)
        {

        }

        public Vecteur2D Position { get; protected set; }

        public int Lives = 1;

        public Bitmap Image { get; protected set; }

        protected abstract void OnCollision(Missile m, int numberOfPixelsInCollision);

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive()
        {
            return Lives > 0;
        }

        public bool CollisionRectangle(Missile m)
        {
            if (m.Position.x <= this.Position.x + this.Image.Width // Le missile est en collision gauche de l'objet
             && m.Position.x + m.Image.Width >= this.Position.x    // Le missile est en collision droite de l'objet
             && m.Position.y + m.Image.Height >= this.Position.y   // Le missile est en collision haut de l'objet
             && m.Position.y <= this.Position.y + this.Image.Height)  // Le missile est en collision bas de l'objet
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
            if (CollisionRectangle(m) && this.Side!=m.Side)
            {
                int numberOfPixelsInCollision = 0;


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
                            if (this is Bunker) // La meilleure ligne de code de ce programme
                            {
                                Color newColor = Color.FromArgb(0, 0, 0, 0);
                                Image.SetPixel((int)xBunker, (int)yBunker, newColor);
                            }
                            else
                            {
                                SoundPlayer soundPlayer = new SoundPlayer("C:\\Users\\antoninmansour\\source\\repos\\projet-spaceinvaders2023-zackary-saada-antonin-mansour\\SpaceInvaders\\Resources\\shoot.wav");
                                soundPlayer.Play();
                            }

                            numberOfPixelsInCollision++;
                        }
                    }
                }
                if (numberOfPixelsInCollision> 0)
                {
                    OnCollision(m, numberOfPixelsInCollision);
                }
            }
        }
    }
}
