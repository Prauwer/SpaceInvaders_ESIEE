using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SpaceInvaders
{
    internal class EnemyBlock : GameObject
    {

        public HashSet<SpaceShip> enemyships {get; private set; } = new HashSet<SpaceShip>() ;

        private int baseWidth;

        private int direction;
        private double speed;
        private double speedMultiplier;
        private double randomShootProbability = 0.1;

        public Vecteur2D Position;
        public Size size;

        /// <summary>
        /// Public constructor for the enemy block
        /// </summary>
        /// <param name="position">Position of the block</param>
        /// <param name="width">Width of the block</param>

        public EnemyBlock(Vecteur2D position, int width): base(Side.Enemy)
        {
            Position = position;
            baseWidth = width;
            direction = 1;
            speed = 20;
            speedMultiplier = 1.15;

            size.Width = baseWidth;
            size.Height = 0;
        }

        /// <summary>
        /// Adds a line of spaceships to the enemy block
        /// </summary>
        /// <param name="nbShips">number of ships to add</param>
        /// <param name="nbLives">amount of life for these ships</param
        /// <param name="shipImage">image for the line of ships</param>
        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            for(int i = 0; i<nbShips; i++)
            {
                Vecteur2D newSPPos = new Vecteur2D(0, Position.y + size.Height);
                if (nbShips == 1)
                {
                    newSPPos.x = Position.x + baseWidth / 2 - shipImage.Width / 2;
                }
                else
                {
                    newSPPos.x = Position.x + i * (baseWidth - shipImage.Width) / (nbShips - 1);
                }
                SpaceShip newspaceship = new SpaceShip(newSPPos, nbLives, shipImage, Side.Enemy);
                enemyships.Add(newspaceship);
            }
            size.Height += shipImage.Height;
        }

        /// <summary>
        /// Constantly updates the size of the spaceship block
        /// </summary>
        void UpdateSize()
        {
            if (enemyships.Count > 0)
            {
                Vecteur2D newPos = new Vecteur2D(enemyships.Min(SpaceShip => SpaceShip.Position.x), enemyships.Min(SpaceShip => SpaceShip.Position.y));
                Size newSize = new Size((int)(enemyships.Max(SpaceShip => SpaceShip.Position.x + SpaceShip.Image.Width)-newPos.x), (int)(enemyships.Max(SpaceShip => SpaceShip.Position.y + SpaceShip.Image.Height)-newPos.y));

                Position = newPos;
                size = newSize;
            }
        }

        /// <summary>
        /// Does nothing, the enemy block entity is invisible (the ships are visible). Can be used for 
        /// </summary>
        /// <param name="gameInstance">Instance of the game</param>
        /// <param name="graphics">Graphics to draw in</param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
        }

        /// <summary>
        /// Determines if object is alive. If false, the object will be removed automatically.
        /// </summary>
        /// <returns>Am I alive ?</returns>
        public override bool IsAlive()
        {
            return (enemyships.Count != 0);
        }

        /// <summary>
        /// Automatically moves the block down if it touches the edge
        /// </summary>
        /// <param name="gameSizeWidth">Width of the game instance</param>
        public void MoveBlockDown(int gameSizeWidth)
        {
            speed *= Math.Round(speedMultiplier, 2); // Increase move speed
            Position.y += 20;                        // Bring 20 pixels down
            foreach (SpaceShip ship in enemyships)
            {
                ship.Position.y += 20;
                if (direction == -1)                 // Place the block on the left
                {
                    ship.Position.x -= Position.x;
                }
                else                                 // OR place the block on the right
                {
                    ship.Position.x -= Position.x + size.Width - gameSizeWidth;
                }
            }
            direction *= -1;                         // Reverse speed
            randomShootProbability *= 2;             // Increase random shoot probability
        }

        /// <summary>
        /// Update the state of a game objet
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            // Update enemy box size
            enemyships.RemoveWhere(gameObject => !gameObject.IsAlive());
            UpdateSize();
                
            Random rand = new Random();

            // Block lateral movement
            Position.x += speed * deltaT * direction;

            foreach (SpaceShip ship in enemyships){
                // Ships lateral movement
                ship.Position.x += speed * deltaT * direction;

                // Enemy ships shoot
                double r = rand.NextDouble();
                if (r <= randomShootProbability * deltaT)
                {
                    // 1 corresponds to direction "DOWN"
                    ship.Shoot(gameInstance, 1, Side.Enemy);
                }
            }

            // Block has reached an edge
            if (Position.x < 0 || (Position.x + size.Width) > gameInstance.GameSize.Width)
            {
                MoveBlockDown(gameInstance.GameSize.Width);
            }
        }

        /// <summary>
        /// Determines if a projectile entered the entity perimeter
        /// </summary>
        /// <param name="p">the projectile to check</param>
        /// <returns>Bool : Is projectile inside of the block ?</returns>
        public bool CollisionRectangle(Projectile p)
        {
            if (p.Position.x <= this.Position.x + this.size.Width   // Projectile is in collision left to the object
             && p.Position.x + p.Image.Width >= this.Position.x     // Projectile is in collision right to the object
             && p.Position.y + p.Image.Height >= this.Position.y    // Projectile is in collision up to the object
             && p.Position.y <= this.Position.y + this.size.Height  // Projectile is in collision down to the object
             && p.Side != this.Side)                                // Projectile isn't in the same Side of the object
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a projectile is in collision with any of the enemy ships
        /// </summary>
        /// <param name="p">projectile to check</param>
        public override void Collision(Projectile p)
        {
            if (CollisionRectangle(p))
            {
                foreach (SpaceShip ship in enemyships)
                {
                    ship.Collision(p);
                }
            }
        }
    }
}
