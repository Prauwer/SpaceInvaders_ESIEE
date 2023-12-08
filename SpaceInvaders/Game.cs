using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Text;

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
            Initial,
            Play,
            Pause,
            Win,
            Lost,
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
            this.EnemiesBlockCreation();

            // Creation du vaisseau
            this.PlayerSpaceShipCreation();

            // Création des bunkers
            this.BunkersCreation();
        }

        #endregion

        #region methods


        // <summary>
        // EnemiesBlock creation
        // </summary>
        private void EnemiesBlockCreation()
        {
            int enemyBlockOffsetX = 80;
            int enemyBlockOffsetY = Properties.Resources.ship6.Height;
            this.enemies = new EnemyBlock(new Vecteur2D(enemyBlockOffsetX, enemyBlockOffsetY), gameSize.Width - enemyBlockOffsetX * 2);

            enemies.AddLine(1, 50, Properties.Resources.ship6);
            enemies.AddLine(2, 30, Properties.Resources.ship7);
            enemies.AddLine(5, 20, Properties.Resources.ship1);
            enemies.AddLine(6, 20, Properties.Resources.ship4);
            enemies.AddLine(7, 20, Properties.Resources.ship2);

            AddNewGameObject(this.enemies);
            foreach (SpaceShip enemyship in enemies.enemyships)
            {
                AddNewGameObject(enemyship);
            }
        }

        // <summary>
        // Player Space Ship creation
        // </summary>
        private void PlayerSpaceShipCreation()
        {
            this.playerShip = new PlayerSpaceship(new Vecteur2D(0, 0), 150);
            playerShip.Position.x = (gameSize.Width / 2) - playerShip.Image.Width / 2;
            playerShip.Position.y = gameSize.Height - 100;
            AddNewGameObject(this.playerShip);
        }

        // <summary>
        // Bunkers creation
        // </summary>
        private void BunkersCreation()
        {
            for (int i = 0; i < 3; i++)
            {
                int imageWidth = Properties.Resources.bunker.Width;
                Vecteur2D Position = new Vecteur2D((gameSize.Width) / 3 * (i + 1) - (gameSize.Width / 6 + imageWidth / 2), gameSize.Height - 200);
                Bunker bunker = new Bunker(Position);
                AddNewGameObject(bunker);
            }
        }

        private void ResetGame()
        {
            // Suppression tous les objets du jeu
            this.playerShip = null;
            this.enemies = null;
            this.gameObjects.Clear();

            // Création du bloc d'ennemis
            this.EnemiesBlockCreation();

            // Creation du vaisseau
            this.PlayerSpaceShipCreation();

            // Création des bunkers
            this.BunkersCreation();

            State = GameStates.Play;

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
            Image BackgroundImage = Image.FromFile("C:\\Users\\antoninmansour\\source\\repos\\projet-spaceinvaders2023-zackary-saada-antonin-mansour\\SpaceInvaders\\Resources\\background.jpeg");
            Rectangle rectangle = new Rectangle(0, 0, gameSize.Width, gameSize.Height);
            g.DrawImage(BackgroundImage, rectangle);
            SolidBrush brush = new SolidBrush(Color.White);
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            privateFontCollection.AddFontFile("C:\\Users\\antoninmansour\\source\\repos\\projet-spaceinvaders2023-zackary-saada-antonin-mansour\\SpaceInvaders\\Resources\\space_invaders.ttf");
            // Draw "PAUSE" in the windows if the game is in Pause state 
            if (State == GameStates.Pause) {
                Font font = new Font(privateFontCollection.Families[0], 22);
                g.DrawString("PAUSE", font, brush, gameSize.Width / 2 - 40, gameSize.Height / 2 - 24);

            }
            else if (State == GameStates.Lost)
            {
                Font font = new Font(privateFontCollection.Families[0], 16);
                g.DrawString("YOU LOOSE ! (press <space> to retry)", font, brush, gameSize.Width / 2 - 240, gameSize.Height / 2 - 24);
            }
            else if (State == GameStates.Win)
            {
                Font font = new Font(privateFontCollection.Families[0], 16);
                g.DrawString("YOU WIN ! (press <space> to retry)", font, brush, gameSize.Width / 2 - 240, gameSize.Height / 2 - 24);
            }

            else if (State != GameStates.Initial)
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.Draw(this, g);
            }
            else
            {
                Font font = new Font(privateFontCollection.Families[0], 14);
                g.DrawString("PRESS <ENTER> TO START", font, brush, gameSize.Width / 2 - 140, gameSize.Height / 2 - 24);
            }
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            // add new game objects
            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();
          
            //// if space is pressed DEBUG SPAWN BALLE QUI TOMBE
            //if (keyPressed.Contains(Keys.Space))
            //{
            //    // create new BalleQuiTombe
            //    GameObject newObject = new BalleQuiTombe(gameSize.Width / 2, 0);
            //    // add it to the game
            //    AddNewGameObject(newObject);
            //    // release key space (no autofire)
            //    ReleaseKey(Keys.Space);
            //}

            //launch the game when Enter is pressed
            if (keyPressed.Contains(Keys.Enter) && State == GameStates.Initial)
            {
                State = GameStates.Play;
                ReleaseKey(Keys.Enter);
            }
            //DEBUG SPAWN MISSILE
            else if (keyPressed.Contains(Keys.Down))
            {
                // create new BalleQuiTombe
                GameObject newObject = new Missile(new Vecteur2D(playerShip.Position.x, 0), 100, 150, Properties.Resources.shoot2, Side.Neutral);
                // add it to the game
                AddNewGameObject(newObject);
                // release key space (no autofire)
                ReleaseKey(Keys.Down);
            }

            //Switch the game to Play or Pause if p key is pressed
            else if (keyPressed.Contains(Keys.P) && State == GameStates.Pause)
            {
                State = GameStates.Play;
                ReleaseKey(Keys.P);
            }
            else if (keyPressed.Contains(Keys.P) && State == GameStates.Play)
            {
                State = GameStates.Pause;
                ReleaseKey(Keys.P);
            }
            //Don't update the gameOjects if the game is in Pause's state
            else if (State == GameStates.Pause || State == GameStates.Initial)
            {
                return;
            }
            else if (!enemies.enemyships.Any() && State != GameStates.Win)
            {
                State = GameStates.Win;
            }
            else if (!playerShip.IsAlive() && State != GameStates.Lost)
            {
                State = GameStates.Lost;
            }
            else if (keyPressed.Contains(Keys.Space) && ( State == GameStates.Lost || State == GameStates.Win)){
                this.ResetGame();
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