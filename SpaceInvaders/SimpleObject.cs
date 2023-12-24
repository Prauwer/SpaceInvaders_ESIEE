using System.Drawing;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the generic abstact base class for any interactable entity of the game
    /// </summary>
    abstract class SimpleObject : GameObject
    {
        public Vecteur2D Position { get; protected set; }

        public int Lives = 1;

        public Bitmap Image { get; protected set; }

        /// <summary>
        /// Public constructor for a SimpleObject
        /// </summary>
        /// <param name="Side">Side of the object</param>
        protected SimpleObject(Side Side) : base(Side)
        {

        }

        /// <summary>
        /// Behavior to expect when a collision occurs
        /// </summary>
        /// <param name="p">Projectile to interact with</param>
        /// <param name="numberOfPixelsInCollision">numbers of pixels in collision</param>
        protected abstract void OnCollision(Projectile p, int numberOfPixelsInCollision);

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
        /// Determines if a projectile entered the entity perimeter
        /// </summary>
        /// <param name="p">the projectile to check</param>
        /// <returns>Bool : Is projectile inside of the entity perimeter ?</returns>
        public bool CollisionRectangle(Projectile p)
        {
            if (p.Position.x <= this.Position.x + this.Image.Width    // Projectile is in collision left to the object
             && p.Position.x + p.Image.Width >= this.Position.x       // Projectile is in collision right to the object
             && p.Position.y + p.Image.Height >= this.Position.y      // Projectile is in collision up to the object
             && p.Position.y <= this.Position.y + this.Image.Height)  // Projectile is in collision down to the object) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a projectile is in collision with the game object
        /// </summary>
        /// <param name="p">projectile to check</param>
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
