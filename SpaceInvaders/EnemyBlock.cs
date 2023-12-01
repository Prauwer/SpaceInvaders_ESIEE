using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Size size;

        public Vecteur2D Position;

        public EnemyBlock(Vecteur2D position, int width)
        {
            Position = position;
            baseWidth= width;
            direction = 1;
            speed = 20;
            speedMultiplier = 1.15;

            size.Width= baseWidth;
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

        void UpdateSize()
        {

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

        public void MoveBlockDown()
        {
            speed *= speedMultiplier;
            direction *= -1;
            Position.y += 20;
            foreach (SpaceShip ship in enemyships)
            {
                ship.Position.y += 20;
            }
        }

        public override void Update(Game gameInstance, double deltaT)
        {   
            Position.x += speed * deltaT * direction;
            foreach (SpaceShip ship in enemyships){
                // Déplacement latéral
                ship.Position.x += speed * deltaT * direction;

                // Tir des enemis
                Random rand = new Random();
                double r = rand.NextDouble();
                if (r <= randomShootProbability * deltaT)
                {
                    // Le 1 correspond à la direction vers le bas
                    ship.Shoot(gameInstance, 1);
                }
            }

            // Le block a atteint un bord
            if (Position.x < 0)
            {
                Position.x = 0;
                MoveBlockDown();
            }else if(Position.x + size.Width > gameInstance.gameSize.Width)
            {
                Position.x = gameInstance.gameSize.Width-size.Width;
                MoveBlockDown();
            }
        }

        public bool CollisionRectangle(Missile m)
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
