using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class PlayerSpaceship : SpaceShip
    {
        private int Bleed;
        public int Points;
        public int MissileCounter;

        private Font Font;
        private SolidBrush Brush;
        private StringFormat StringFormatRight;

        /// <summary>
        /// Public constructor for the player spaceship
        /// </summary>
        /// <param name="position">Position of the spaceship</param>
        /// <param name="lives">Lives of the spaceship</param>
        public PlayerSpaceship(Vecteur2D position, int lives) : base(position, lives, Properties.Resources.ship3, Side.Ally)
        {
            Bleed = 0;
        }

        /// <summary>
        /// Update the state of the Player Spaceship
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            // Player movements
            if (gameInstance.keyPressed.Contains(Keys.Left) && Position.x >= 0)
            {
                Position.x -= 2.0;
            }
            if (gameInstance.keyPressed.Contains(Keys.Right) && Position.x + Image.Size.Width <= gameInstance.GameSize.Width)
            {
                Position.x += 2.0;
            }

            // Shoot a missile
            if (gameInstance.keyPressed.Contains(Keys.Up))
            {
                // -1 corresponds to direction UP
                Shoot(gameInstance, -1, Side.Ally);
            }

            // Bleeding
            if (Bleed > 0)
            {
                Bleed -= 1;
                Lives -= 1;
            }

            // Die if the enemy block goes past the ally spaceship
            if ((Game.game.Enemies.Position.y + Game.game.Enemies.size.Height) > Position.y + Image.Height)
            {
                Lives = 0;
            }
        }

        /// <summary>
        /// handle the case of the player spaceship is hit
        /// </summary>
        /// <param name="p">projectile in collision with the player spaceship</param>
        /// <param name="numberOfPixelsInCollision">number of pixels in collision with the player spaceship</param>
        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            // Handle bonuses
            if (p is LifeBonus || p is MissileBonus)
            {
                p.Lives = 0;
                handleBonus(p);
            }
            else
            {
                int damage = p.Lives; // Missile damage corresponds to its lives

                p.Lives = 0; // Enemy missile is detroyed upon impact

                if (damage >= Lives) // If the missile would kill the spaeceship
                {
                    Bleed = Lives;
                }
                else // If the missile would not kill the spaeceship
                {
                    Bleed += damage;
                }
            }
        }

        /// <summary>
        /// check if a game object is a Bonus, handle the case if it's one
        /// </summary>
        /// <param name="gameObject">object to check</param>
        public void handleBonus(GameObject gameObject)
        {   
            if (gameObject is LifeBonus)
            {
                Lives += Math.Min(50, InitialLives - Lives);
            }
            else if (gameObject is MissileBonus)
            {
                MissileCounter++;
            }
        }

        /// <summary>
        /// handle the case of an ennemy dying
        /// </summary>
        /// <param name="gameObject">object to check</param>
        /// <param name="rand">random object (we put it in params to avoid that every random have the same value)</param>
        /// <param name="gameInstance">instance of the current game</param>
        public void handleEnnemieDie(GameObject gameObject, Random rand, Game gameInstance)
        {
            if (gameObject is SpaceShip && gameObject != this)
            {
                Points += gameObject.InitialLives;

                // Creating a random bonus
                Projectile newBonus = Projectile.RandomCreation(rand, Position.x, 100);

                // Adding this bonus to the game
                if (newBonus != null)
                {
                    gameInstance.AddNewGameObject(newBonus);
                }
            }
        }

        /// <summary>
        /// Render life bar
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="graphics">Graphics to draw in</param>
        private void DrawLifeBar(Game gameInstance, Graphics graphics)
        {
            // Values calculus
            double HPLenght;
            HPLenght = ((double)Lives - (double)Bleed) / (double)InitialLives * 200;

            double BleedLenght;
            BleedLenght = (double)Bleed / (double)InitialLives * 200;

            int percentHP;
            percentHP = (int)((double)Lives / (double)InitialLives * 100);

            graphics.DrawString($"{percentHP} %", Font, Brush, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20), StringFormatRight);

            // Graphics parameters
            Pen pen = new Pen(Color.Black, 2);
            SolidBrush brushMaxHP = new SolidBrush(Color.Red);
            SolidBrush brushCurrentHP = new SolidBrush(Color.Green);
            SolidBrush brushBleedHP = new SolidBrush(Color.Orange);

            // Health bar drawing
            graphics.DrawRectangle(pen, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20) - 3, 200, 25); // Cadre

            graphics.FillRectangle(brushMaxHP, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20) - 3, 200, 24); // Barre rouge

            if (Bleed > 0)
            {
                graphics.FillRectangle(brushBleedHP, (gameInstance.GameSize.Width / 20) + 35 + (int)HPLenght, (gameInstance.GameSize.Height * 19 / 20) - 3, (int)BleedLenght, 24); // Barre orange
            }
            graphics.FillRectangle(brushCurrentHP, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20) - 3, (int)HPLenght, 24); // Barre verte
        }


        /// <summary>
        /// Set the variable to render text
        /// </summary>
        private void SetTextStyle()
        {
            // Text Style
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            IntPtr fontBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(Properties.Resources.space_invaders_font, 0);
            privateFontCollection.AddMemoryFont(fontBuffer, Properties.Resources.space_invaders_font.Length);

            Brush = new SolidBrush(Color.White);
            Font = new Font(privateFontCollection.Families[0], 12);

            StringFormatRight = new StringFormat();
            StringFormatRight.Alignment = StringAlignment.Far;
        }

        /// <summary>
        /// Render the Player Spaceship, its health bar and its score
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="graphics">graphic object where to perform rendering</param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            // SPACESHIP
            base.Draw(gameInstance, graphics);

            SetTextStyle();

            // POINTS
            graphics.DrawString($"{Points} Points", Font, Brush, gameInstance.GameSize.Width * 16 / 20, gameInstance.GameSize.Height * 19 / 20);

            // HEALTH BAR
            DrawLifeBar(gameInstance, graphics);
        }
    }
}

