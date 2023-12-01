using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    class Game
    {

        #region GameObjects management
        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        public enum GameStates
        {
            Play,
            Pause,
        }

        public GameStates State;

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }
        #endregion

        #region game technical elements

        /// <summary>
        ///  enemy ship block
        /// </summary>
        public EnemyBlock enemies;


        /// <summary>
        ///  player's ship
        /// </summary>
        public PlayerSpaceship playerShip;

        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        #endregion


        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
                game = new Game(gameSize);
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;

            // Création du bloc d'ennemis

            int enemyBlockOffsetX = 80;
            int enemyBlockOffsetY = Properties.Resources.ship6.Height;
            this.enemies = new EnemyBlock(new Vecteur2D(enemyBlockOffsetX,enemyBlockOffsetY),gameSize.Width-enemyBlockOffsetX*2);

            enemies.AddLine(1, 50, Properties.Resources.ship6);
            enemies.AddLine(2, 20, Properties.Resources.ship7);
            enemies.AddLine(5, 10, Properties.Resources.ship1);
            enemies.AddLine(6, 10, Properties.Resources.ship4);
            enemies.AddLine(7, 10, Properties.Resources.ship2);

            AddNewGameObject(this.enemies);
            foreach (SpaceShip enemyship in enemies.enemyships)
            {
                AddNewGameObject(enemyship);
            }


            // Creation du vaisseau

            this.playerShip = new PlayerSpaceship(new Vecteur2D(0,0), 300);
            playerShip.Position.x = (gameSize.Width / 2) - playerShip.Image.Width / 2;
            playerShip.Position.y = gameSize.Height - 100;
            AddNewGameObject(this.playerShip);

            for (int i = 0; i < 3; i++) {
                int imageWidth = Properties.Resources.bunker.Width;
                Vecteur2D Position = new Vecteur2D((gameSize.Width) / 3 * (i + 1) - (gameSize.Width/6 + imageWidth/2), gameSize.Height - 200);
                Bunker bunker = new Bunker(Position);
                AddNewGameObject(bunker);
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            // Draw "PAUSE" in the windows if the game is in Pause state 
            if (State == GameStates.Pause) {
                Font font = new Font("Arial", 24);
                SolidBrush brush = new SolidBrush(Color.Black);
                g.DrawString("PAUSE", font, brush, gameSize.Width / 2 - 60, gameSize.Height / 2 - 24);

            }
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(this, g);
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            // add new game objects
            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();


            // if space is pressed DEBUG SPAWN BALLE QUI TOMBE
            if (keyPressed.Contains(Keys.Space))
            {
                // create new BalleQuiTombe
                GameObject newObject = new BalleQuiTombe(gameSize.Width / 2, 0);
                // add it to the game
                AddNewGameObject(newObject);
                // release key space (no autofire)
                ReleaseKey(Keys.Space);
            }

            //DEBUG SPAWN MISSILE
            if (keyPressed.Contains(Keys.Down))
            {
                // create new BalleQuiTombe
                GameObject newObject = new Missile(new Vecteur2D(playerShip.Position.x, 0), 100, 150, Properties.Resources.shoot2);
                // add it to the game
                AddNewGameObject(newObject);
                // release key space (no autofire)
                ReleaseKey(Keys.Down);
            }

            //Switch the game to Play or Pause if p key is pressed
            if (keyPressed.Contains(Keys.P) && State == GameStates.Pause)
            {
                State = GameStates.Play;
                ReleaseKey(Keys.P);
            }
            else if (keyPressed.Contains(Keys.P) && State == GameStates.Play)
            {
                State = GameStates.Pause;
                ReleaseKey(Keys.P);
            }

            // update each game object
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(this, deltaT);
            }

            // remove dead objects
            gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
        }
        #endregion
    }
}
