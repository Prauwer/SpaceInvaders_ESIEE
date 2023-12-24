﻿using System.Drawing;

namespace SpaceInvaders
{

    public enum Side
    {
        Ally,
        Enemy,
        Neutral,
        Bonus,
    }

    /// <summary>
    /// This is the generic abstact base class for any entity in the game
    /// </summary>
    abstract class GameObject
    {
        public int InitialLives;

        public Side Side { get; private set; }
        
        public GameObject(Side Side)
        {
            this.Side = Side;
        }

        public GameObject()
        {
            Side = Side.Neutral;
        }

        /// <summary>
        /// Update the state of a game objet
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public abstract void Update(Game gameInstance, double deltaT);

        /// <summary>
        /// Render the game object
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="graphics">graphic object where to perform rendering</param>
        public abstract void Draw(Game gameInstance, Graphics graphics);

        /// <summary>
        /// Determines if object is alive. If false, the object will be removed automatically.
        /// </summary>
        /// <returns>Am I alive ?</returns>
        public abstract bool IsAlive();

        /// <summary>
        /// Determines an projectile is in collision of the game object
        /// </summary>
        /// <oaram name="p">projectile to check</oaram>
        /// <returns>Am I alive ?</returns>
        public abstract void Collision(Projectile p);
    }
}
