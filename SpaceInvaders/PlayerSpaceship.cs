using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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

        public PlayerSpaceship(Vecteur2D position, int lives) : base(position, lives, Properties.Resources.ship3, Side.Ally)
        {
            Bleed = 0;
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

            // Saignement
            if (Bleed > 0)
            {
                Bleed -= 1;
                Lives -= 1;
            }

            // Mourir si le block ennemi dépasse le vaisseau
            if ((Game.game.Enemies.Position.y + Game.game.Enemies.size.Height) > Position.y + Image.Height)
            {
                Lives = 0;
            }
        }

        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            if (p is LifeBonus)
            {
                p.Lives = 0;
            }
            else
            {
                int damage = p.Lives; // Dégâts du missile correspond à son nombre de vies

                p.Lives = 0; // Le missile ennemi est détruit à l'impact

                if (damage >= Lives) // Si le missile tue le vaisseau
                {
                    Bleed = Lives;
                }
                else // Si le missile le tue pas le vaisseau
                {
                    Bleed += damage;
                }
            }
        }

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

        public void handleEnnemieDie(GameObject gameObject, Random rand, Game game)
        {
            if (gameObject is SpaceShip && gameObject != this)
            {
                Points += gameObject.InitialLives;

                // random creation of a new bonus object
                Projectile newBonus = Projectile.RandomCreation(rand, Position.x, 100);

                // add it to the game
                if (newBonus != null)
                {
                    game.AddNewGameObject(newBonus);
                }
            }
        }


        private void DrawLifeBar(Game gameInstance, Graphics graphics)
        {
            // Calcul des valeurs
            double HPLenght;
            HPLenght = ((double)Lives - (double)Bleed) / (double)InitialLives * 200;

            double BleedLenght;
            BleedLenght = (double)Bleed / (double)InitialLives * 200;

            int percentHP;
            percentHP = (int)((double)Lives / (double)InitialLives * 100);

            graphics.DrawString($"{percentHP} %", Font, Brush, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20), StringFormatRight);

            Pen pen = new Pen(Color.Black, 2);
            SolidBrush brushMaxHP = new SolidBrush(Color.Red);
            SolidBrush brushCurrentHP = new SolidBrush(Color.Green);
            SolidBrush brushBleedHP = new SolidBrush(Color.Orange);

            graphics.DrawRectangle(pen, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20) - 3, 200, 25); // Cadre

            graphics.FillRectangle(brushMaxHP, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20) - 3, 200, 24); // Barre rouge

            if (Bleed > 0)
            {
                graphics.FillRectangle(brushBleedHP, (gameInstance.GameSize.Width / 20) + 35 + (int)HPLenght, (gameInstance.GameSize.Height * 19 / 20) - 3, (int)BleedLenght, 24); // Barre orange
            }
            graphics.FillRectangle(brushCurrentHP, (gameInstance.GameSize.Width / 20) + 35, (gameInstance.GameSize.Height * 19 / 20) - 3, (int)HPLenght, 24); // Barre verte
        }

        private void SetTextStyle()
        {
            // Style du texte
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            IntPtr fontBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(Properties.Resources.space_invaders_font, 0);
            privateFontCollection.AddMemoryFont(fontBuffer, Properties.Resources.space_invaders_font.Length);

            Brush = new SolidBrush(Color.White);
            Font = new Font(privateFontCollection.Families[0], 12);

            StringFormatRight = new StringFormat();
            StringFormatRight.Alignment = StringAlignment.Far;
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);

            SetTextStyle();

            // POINTS
            graphics.DrawString($"{Points} Points", Font, Brush, gameInstance.GameSize.Width * 16 / 20, gameInstance.GameSize.Height * 19 / 20);

            // BARRE DE VIE
            DrawLifeBar(gameInstance, graphics);
        }
    }
}

