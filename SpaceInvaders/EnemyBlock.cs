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

        private HashSet<SpaceShip> enemyships= new HashSet<SpaceShip>();

        private int baseWidth;

        private int direction;
        private double speed;
        private double speedMultiplier;

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
                SpaceShip newspaceship = new SpaceShip(newSPPos, nbLives, shipImage);
                enemyships.Add(newspaceship);
            }
            size.Height += shipImage.Height;
        }

        void UpdateSize()
        {

        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {

            // DEBUG
            Pen pen = new Pen(Color.Red, 2);
            graphics.DrawRectangle(pen, (int)Position.x, (int)Position.y, size.Width, size.Height);
            // FIN DEBUG


            foreach (SpaceShip ship in enemyships)
            {
                ship.Draw(gameInstance, graphics);
            }
        }

        public override bool IsAlive()
        {
            return (enemyships.Count != 0);
        }

        public void MoveBlockDown() // TODO : A TERMINER. BUG = PARFOIS LE BLOCK DESCEND PLUSIEURS FOIS D'UN COUP // FIXED?
        {
            speed *= speedMultiplier;
            direction *= -1;
            Position.y += 20;
            foreach (SpaceShip ship in enemyships)
            {
                ship.Position.y += 20;
            }
        }

        public override void Update(Game gameInstance, double deltaT) // TODO : CHECK PREV. TODO
        {   
            // Déplacement latéral
            Position.x += speed * deltaT * direction;
            foreach (SpaceShip ship in enemyships){
                ship.Position.x += speed * deltaT * direction;
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

        public override void Collision(Missile m) // TODO : Ne devrait pas être là/ne fait rien. Voir GameObject.cs
        {
            //throw new NotImplementedException();
        }
    }
}
