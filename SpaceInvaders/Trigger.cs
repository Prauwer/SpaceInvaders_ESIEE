using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    internal class Trigger : GameObject
    {
        public Vecteur2D Position { get; protected set; }
        public Size Size { get; protected set; }
        public Image Image { get; protected set; }
        public bool isTriggered { get; protected set; }

        /// <summary>
        /// Public constructor for the trigger
        /// </summary>
        /// <param name="position">Position of the trigger</param>
        /// <param name="size">Size of the trigger</param>
        public Trigger(Vecteur2D position, Size size)
        {
            Position = position;
            Size = size;
            Image = Properties.Resources.Trigger;
            isTriggered = false;
        }

        /// <summary>
        /// Determines an projectile is in collision with the trigger
        /// </summary>
        /// <param name="p">projectile to check</param>
        public override void Collision(Projectile p)
        {
            
        }

        /// <summary>
        /// Render the Trigger
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="graphics">graphic object where to perform rendering</param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        /// <summary>
        /// Determines if the triger is alive (=is the ennemyblock are passed through it). If false, the object will be removed automatically.
        /// </summary>
        /// <returns>Am I alive ?</returns>
        public override bool IsAlive()
        {
            return !isTriggered;
        }

        /// <summary>
        /// Update the state of the Trigger
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            double lastEnemyPosY = Game.game.Enemies.Position.y + Game.game.Enemies.size.Height;

            if (lastEnemyPosY > Position.y)
            {
                isTriggered = true;
                foreach (SimpleObject bunker in Game.game.gameObjects.OfType<Bunker>())
                {
                    bunker.Lives = 0;
                }
            }

        }
    }
}