namespace SpaceInvaders
{
    internal class Bunker : SimpleObject
    {

        /// <summary>
        /// Public constructor for a bunker
        /// </summary>
        /// <param name="position">Position of the Bunker</param>
        public Bunker(Vecteur2D Position): base(Side.Neutral)
        {
            this.Position = Position;
            this.Image = Properties.Resources.bunker;
        }

        /// <summary>
        /// Update the state of the bunkers
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public override void Update(Game gameInstance, double deltaT)
        {
            return;
        }

        /// <summary>
        /// Behavior to expect when a collision occurs
        /// </summary>
        /// <param name="p">Projectile to interact with</param>
        /// <param name="numberOfPixelsInCollision">numbers of pixels in collision</param>
        protected override void OnCollision(Projectile p, int numberOfPixelsInCollision)
        {
            p.Lives -= numberOfPixelsInCollision;
        }
    }
}
