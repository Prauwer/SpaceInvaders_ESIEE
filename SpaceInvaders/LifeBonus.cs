namespace SpaceInvaders
{
    internal class LifeBonus : Projectile
    {
        public LifeBonus(Vecteur2D position, double speed, Side side) : base(position, speed, 1, Properties.Resources.bonus, side)
        {
        }

        /// <summary>
        /// Update the state of the Life Bonus
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            base.Update(gameInstance, deltaT);
            // Test collision avec les objets du jeu
            foreach (GameObject gameObject in gameInstance.gameObjects)
            {
                if (gameObject.GetType() == typeof(PlayerSpaceship))
                {
                    gameObject.Collision(this);
                }
            }
        }

        /// <summary>
        /// handle the case of the life bonus is hitted
        /// </summary>
        /// <param name="p">projectile in collision with the bonus</param>
        /// <param name="numberOfPixelsInCollision">number of pixels in collision with the bonus</param>
        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            if (p.Side == Side.Ally)
            {
                Game.game.PlayerShip.handleBonus(this);

                p.Lives = 0;
                this.Lives = 0;
            }
        }
    }
}
