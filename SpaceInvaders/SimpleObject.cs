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

        protected abstract void OnCollision(Projectile m, int numberOfPixelsInCollision);

        /// <summary>
        /// Render the simple object
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="graphics">graphic object where to perform rendering</param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        /// <summary>
        /// Determines if object is alive. If false, the object will be removed automatically.
        /// </summary>
        /// <returns>Am I alive ?</returns>
        public override bool IsAlive()
        {
            return Lives > 0;
        }

        /// <summary>
        /// Play sound on ship getting hit
        /// </summary>
        public void PlayHit() 
        {
            SoundPlayer soundPlayer = new SoundPlayer();
            soundPlayer.Stream = Properties.Resources.shoot_sound;
            soundPlayer.Play();
        }

        public bool CollisionRectangle(Projectile p)
        {
            if (p.Position.x <= this.Position.x + this.Image.Width // Le missile est en collision gauche de l'objet
             && p.Position.x + p.Image.Width >= this.Position.x    // Le missile est en collision droite de l'objet
             && p.Position.y + p.Image.Height >= this.Position.y   // Le missile est en collision haut de l'objet
             && p.Position.y <= this.Position.y + this.Image.Height)  // Le missile est en collision bas de l'objet
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines an projectile is in collision of the game object
        /// </summary>
        /// <oaram name="p">projectile to check</oaram>
        /// <returns>Am I alive ?</returns>
        public override void Collision(Projectile p)
        {
            if (CollisionRectangle(p) && (this.Side!=p.Side && (p.Side != Side.Bonus || (this.Side == Side.Ally && p.Side == Side.Bonus))))
            {
                int numberOfPixelsInCollision = 0;


                for (int y = 0; y < p.Image.Height; y++)
                {
                    for (int x = 0; x < p.Image.Width; x++)
                    {
                        Color currentPixelColor = p.Image.GetPixel(x, y);

                        if (currentPixelColor.A == 0)
                        {
                            continue;
                        }

                        double xBunker = p.Position.x + x - Position.x;
                        double yBunker = p.Position.y + y - Position.y;

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
                                PlayHit();
                            }

                            numberOfPixelsInCollision++;
                        }
                    }
                }
                if (numberOfPixelsInCollision> 0)
                {
                    OnCollision(p, numberOfPixelsInCollision);
                }
            }
        }
    }
}
