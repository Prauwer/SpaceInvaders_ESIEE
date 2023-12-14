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
        private double randomShootProbability = 0.1;

        public Vecteur2D Position;
        public Size size;


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
            //Pen pen = new Pen(Color.Red, 2);
            //graphics.DrawRectangle(pen, (int)Position.x, (int)Position.y, size.Width, size.Height);
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
            randomShootProbability *= 2;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            // Update enemy box size
            enemyships.RemoveWhere(gameObject => !gameObject.IsAlive());
            UpdateSize();
                
            Random rand = new Random();

            // Déplacement latéral
            Position.x += speed * deltaT * direction;
            foreach (SpaceShip ship in enemyships){
                // Déplacement latéral
                ship.Position.x += speed * deltaT * direction;

                // Tir des enemis
                double r = rand.NextDouble();
                if (r <= randomShootProbability * deltaT)
                {
                    // Le 1 correspond à la direction vers le bas
                    ship.Shoot(gameInstance, 1, Side.Enemy);
                }
            }

            // Le block a atteint un bord
            if (Position.x < 0 || (Position.x + size.Width) > gameInstance.GameSize.Width)
            {
                MoveBlockDown(gameInstance.GameSize.Width);
            }
        }

        public bool CollisionRectangle(Projectile m)
        {
            if (m.Position.x <= this.Position.x + this.size.Width   // Le missile est en collision gauche de l'objet
             && m.Position.x + m.Image.Width >= this.Position.x     // Le missile est en collision droite de l'objet
             && m.Position.y + m.Image.Height >= this.Position.y    // Le missile est en collision haut de l'objet
             && m.Position.y <= this.Position.y + this.size.Height  // Le missile est en collision bas de l'objet
             && m.Side != this.Side)                                // Le missile n'est pas dans le même camps que le vaisseau
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void Collision(Projectile m)
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
