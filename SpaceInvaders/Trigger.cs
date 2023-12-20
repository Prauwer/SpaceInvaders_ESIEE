using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Trigger : SimpleObject
    {
        public Size Size { get; protected set; }

        public bool isTriggered { get; protected set; }

        public Trigger(Vecteur2D position, Size size): base(Side.Neutral)
        {
            Position = position;
            Size = size;
            Image = Properties.Resources.Trigger;
            isTriggered = false;
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive()
        {
            return !isTriggered;
        }

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

        protected override void OnCollision(Projectile m, int numberOfPixelsInCollision)
        {
        }
    }
}
