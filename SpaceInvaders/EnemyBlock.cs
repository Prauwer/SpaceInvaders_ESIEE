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

        public Size size;

        public Vecteur2D Position;

        public EnemyBlock(Vecteur2D position, int width)
        {

            Position = position;
            baseWidth= width;

            size.Width= baseWidth;
            size.Height = 0;

            // DEBUG
            AddLine(1, 1, Properties.Resources.ship6);
            AddLine(5, 1, Properties.Resources.ship1);
            AddLine(6, 1, Properties.Resources.ship4);
            AddLine(7, 1, Properties.Resources.ship2);

            // FIN DEBUG
        }

        void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            for(int i = 0; i<nbShips; i++)
            {
                Vecteur2D newSPPos = new Vecteur2D(0, Position.y + size.Height);
                // TODO : ERREUR D'ALIGNEMENT
                if (nbShips == 1)
                {
                    newSPPos.x = Position.x + baseWidth / 2 - shipImage.Width / 2;
                }
                else
                {
                    //newSPPos.x = Position.x + (baseWidth / nbShips + 1) * i;
                    newSPPos.x = Position.x + ((baseWidth-nbShips*shipImage.Width)/(nbShips+1))*i+i*shipImage.Width;
                    //newSPPos.x = Position.x + i * (baseWidth - (nbShips - 1) * shipImage.Width / nbShips);
                    //newSPPos.x = Position.x + i * (baseWidth - nbShips * shipImage.Width / nbShips - 1);

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

        public override void Update(Game gameInstance, double deltaT)
        {
            //throw new NotImplementedException();
        }

        public override void Collision(Missile m) // TODO : Ne devrait pas être là/ne fait rien. Voir GameObject.cs
        {
            //throw new NotImplementedException();
        }
    }
}
