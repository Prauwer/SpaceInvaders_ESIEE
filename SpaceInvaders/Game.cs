using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using SpaceInvaders.Properties;

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
            Menu,
            Play,
            Pause,
            Won,
            Lost,
            Settings,
            HighScore
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

        public int WaveCounter = 0;

        /// <summary>
        ///  enemy ship block
        /// </summary>
        public EnemyBlock Enemies;

        /// <summary>
        ///  trigger for bunker deletion
        /// </summary>
        public Trigger trigger;

        /// <summary>
        ///  player's ship
        /// </summary>
        public PlayerSpaceship PlayerShip;

        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size GameSize;

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
        /// Singleton for easy access
        /// </summary>
        public static Menu menu { get; private set; }

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public int HighScore { get; internal set; }

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
            this.GameSize = gameSize;

            // Menu creation
            menu = Menu.CreateMenu(this);

            // Game objects creation
            this.GameSize = gameSize;

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
            int enemyBlockOffsetY = Resources.ship6.Height;
            this.Enemies = new EnemyBlock(new Vecteur2D(enemyBlockOffsetX, enemyBlockOffsetY), GameSize.Width - enemyBlockOffsetX * 2);

            List<Bitmap> sprites = new List<Bitmap>
            {
                Resources.ship1,
                Resources.ship2,
                Resources.ship4,
                Resources.ship5,
                Resources.ship6,
                Resources.ship7,
                Resources.ship8,
                Resources.ship9,
            };

            Enemies.AddLine(1, 50 * (1+WaveCounter / 6), sprites[(4 + WaveCounter) % 7]);
            Enemies.AddLine(2, 30 * (1+WaveCounter / 6), sprites[(5 + WaveCounter) % 7]);
            Enemies.AddLine(5, 20 * (1+WaveCounter / 6), sprites[(0 + WaveCounter) % 7]);
            Enemies.AddLine(6, 20 * (1+WaveCounter / 6), sprites[(2 + WaveCounter) % 7]);
            Enemies.AddLine(7, 20 * (1+WaveCounter / 6), sprites[(1 + WaveCounter) % 7]);

            AddNewGameObject(this.Enemies);
            foreach (SpaceShip enemyship in Enemies.enemyships)
            {
                AddNewGameObject(enemyship);
            }
        }

        // <summary>
        // Bunker Trigger creation
        // </summary>
        private void TriggerCreation()
        {
            trigger = new Trigger(new Vecteur2D(0, GameSize.Height - 220), new Size(605, 10));
            AddNewGameObject(this.trigger);
        }

        // <summary>
        // Player Space Ship creation
        // </summary>
        private void PlayerSpaceShipCreation()
        {
            this.PlayerShip = new PlayerSpaceship(new Vecteur2D(0, 0), 150);
            PlayerShip.Position.x = (GameSize.Width / 2) - PlayerShip.Image.Width / 2;
            PlayerShip.Position.y = GameSize.Height - 100;
            AddNewGameObject(this.PlayerShip);
        }

        // <summary>
        // Bunkers creation
        // </summary>
        private void BunkersCreation()
        {
            for (int i = 0; i < 3; i++)
            {
                int imageWidth = Resources.bunker.Width;
                Vecteur2D Position = new Vecteur2D((GameSize.Width) / 3 * (i + 1) - (GameSize.Width / 6 + imageWidth / 2), GameSize.Height - 200);
                Bunker bunker = new Bunker(Position);
                AddNewGameObject(bunker);
            }
          
            // Création du trigger pour les bunkers
            this.TriggerCreation();
        }

        public void ResetGame()
        {
            // Suppression tous les objets du jeu
            this.Enemies = null;
            this.gameObjects.Clear();
            this.gameObjects.Add(this.PlayerShip);
            this.PlayerShip.Missile = null;

            if (State == GameStates.Lost)
            {
                WaveCounter = 0;
                PlayerShip.Lives = PlayerShip.InitialLives;
                PlayerShip.Points = 0;
                PlayerShip.MissileCounter = 0;
            }
            else
            {
                // Incrémentation du compteur de vagues
                WaveCounter++;
                PlayerShip.Lives += Math.Min(50, PlayerShip.InitialLives - PlayerShip.Lives);
            }
            

            // Création du bloc d'ennemis
            this.EnemiesBlockCreation();

            // Création des bunkers
            this.BunkersCreation();
        }

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

            // Background drawing
            Image BackgroundImage = Properties.Resources.background;
            Rectangle rectangle = new Rectangle(0, 0, game.GameSize.Width, game.GameSize.Height);
            g.DrawImage(BackgroundImage, rectangle);

            int missileWidth = 10;
            for (int i = 0; i<PlayerShip.MissileCounter; i++)
            {
                Rectangle missileRectangle = new Rectangle(missileWidth, 10, Resources.shoot2.Width, Resources.shoot2.Height);
                g.DrawImage(Resources.shoot2, missileRectangle);
                missileWidth += Resources.shoot2.Width + 5;

            }

            // Different drawing depending of game.State
            switch (State)
            {
                // Pause
                case GameStates.Pause:
                    menu.DrawPause(g);
                    break;

                // Lost
                case GameStates.Lost:
                    menu.DrawLost(g);
                    break;

                // Won
                case GameStates.Won:
                    menu.DrawWon(g);
                    break;

                // Play
                case GameStates.Play:
                    // draw all objects from the main game
                    foreach (GameObject gameObject in gameObjects)
                    gameObject.Draw(this, g);
                    break;

                // Menu
                case GameStates.Menu:
                    menu.DrawMainMenu(g);
                    break;

                // Settings
                case GameStates.Settings:
                    menu.DrawSettings(g);
                    break;

                case GameStates.HighScore:
                    menu.DrawHighScore(g);
                    break;

                // Default
                default:
                    Console.WriteLine("You shouldn't be there.");
                    break;
            }
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            // add pending game objects
            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();

            // Taking care of all game cases below

            //Update Menu -> takes care of all the menu tasks
            menu.UpdateMenu(deltaT, keyPressed);

            // spawn super missile if the player have missile in stock
            if (keyPressed.Contains(Keys.Down) && PlayerShip.MissileCounter > 0)
            {
                // create new Missile
                GameObject newObject = new Missile(new Vecteur2D(PlayerShip.Position.x, 0), 100, 150, Resources.shoot2, Side.Ally);
                // add it to the game
                AddNewGameObject(newObject);
                // release key space (no autofire)
                ReleaseKey(Keys.Down);
                // decrement missile counter
                PlayerShip.MissileCounter--;
            }
            else if (keyPressed.Contains(Keys.B))
            {
                LifeBonus newBonus = new LifeBonus(new Vecteur2D(PlayerShip.Position.x, 0), 100, Side.Bonus);
                // add it to the game
                AddNewGameObject(newBonus);
                // release key space (no autofire)
                ReleaseKey(Keys.B);
            }

            //Win if all ships are destroyed
            if (!Enemies.enemyships.Any() && State != GameStates.Won)
            {
                State = GameStates.Won;
            }
            //Lose if ally ship is destroyed
            else if (!PlayerShip.IsAlive() && State != GameStates.Lost)
            {
                State = GameStates.Lost;
            }

            // update each game object if we're playing
            if (State == GameStates.Play)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(this, deltaT);
                }
            }

            // remove dead objects
            Random rand = new Random();
            gameObjects.RemoveWhere(gameObject => {
                if (!gameObject.IsAlive())
                {
                    PlayerShip.handleBonus(gameObject);
                    PlayerShip.handleEnnemieDie(gameObject, rand, this);
                }
                return !gameObject.IsAlive();
            }
            );
        }
        #endregion
    }
}