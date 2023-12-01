using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpaceInvaders
{
    internal class EnemyBlock : GameObject
    {

        public HashSet<SpaceShip> enemyships {get; private set; } = new HashSet<SpaceShip>() ;

        private int baseWidth;

        private int direction;
        private double speed;
        private double speedMultiplier;

        public Vecteur2D Position;
        public Size size;


        public EnemyBlock(Vecteur2D position, int width)
        {
            Position = position;
            baseWidth = width;
            direction = 1;
            speed = 20;
            speedMultiplier = 1.15;

            size.Width = baseWidth;
            size.Height = 0;
        }
            
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
                SpaceShip newspaceship = new SpaceShip(newSPPos, nbLives, shipImage);
                enemyships.Add(newspaceship);
            }
            size.Height += shipImage.Height;
        }

        void UpdateSize() // Dessine la boite autour de tous les vaisseaux
        {
            if (enemyships.Count > 0)
            {
                Vecteur2D newPos = new Vecteur2D(enemyships.Min(SpaceShip => SpaceShip.Position.x), enemyships.Min(SpaceShip => SpaceShip.Position.y));
                Size newSize = new Size((int)(enemyships.Max(SpaceShip => SpaceShip.Position.x + SpaceShip.Image.Width)-newPos.x), (int)(enemyships.Max(SpaceShip => SpaceShip.Position.y + SpaceShip.Image.Height)-newPos.y));

                Position = newPos;
                size = newSize;
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics) // DEBUG
        {
            Pen pen = new Pen(Color.Red, 2);
            graphics.DrawRectangle(pen, (int)Position.x, (int)Position.y, size.Width, size.Height);
        }

        public override bool IsAlive()
        {
            return (enemyships.Count != 0);
        }

        public void MoveBlockDown(int gameSizeWidth)
        {
            speed *= Math.Round(speedMultiplier, 2); // Augmenter la vitesse de déplacement
            Position.y += 20;                        // Baisser d'un bloc
            foreach (SpaceShip ship in enemyships)
            {
                ship.Position.y += 20;
                if (direction == -1)                 // Placer le bloc à gauche
                {
                    ship.Position.x -= Position.x;
                }
                else                                 // OU Placer le bloc à droite
                {
                    ship.Position.x -= Position.x + size.Width - gameSizeWidth;
                }
            }
            direction *= -1;                         // Inverser la direction
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Update enemy box size
            enemyships.RemoveWhere(gameObject => !gameObject.IsAlive());
            UpdateSize();

            // Déplacement latéral
            Position.x += speed * deltaT * direction;
            foreach (SpaceShip ship in enemyships){
                ship.Position.x += speed * deltaT * direction;
            }

            // Le block a atteint un bord
            if (Position.x < 0 || (Position.x + size.Width) > gameInstance.gameSize.Width)
            {
                MoveBlockDown(gameInstance.gameSize.Width);
            }
        }

        public bool CollisionRectangle(Missile m)
        {
            if (m.Position.x <= this.Position.x + this.size.Width // Le missile est en collision gauche de l'objet
             && m.Position.x + m.Image.Width >= this.Position.x    // Le missile est en collision droite de l'objet
             && m.Position.y + m.Image.Height >= this.Position.y   // Le missile est en collision haut de l'objet
             && m.Position.y <= this.Position.y + this.size.Height)  // Le missile est en collision bas de l'objet
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
                foreach (SpaceShip ship in enemyships)
                {
                    ship.Collision(m);
                }
            }

        }
    }
}
