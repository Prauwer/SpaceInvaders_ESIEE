﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
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

        /// <summary>
        /// Enum of all the states the game can have
        /// </summary>
        public enum GameStates
        {
            Menu,
            Play,
            Pause,
            Won,
            Lost,
            Controls,
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

        /// <summary>
        ///  Level Counter
        /// </summary>
        public int WaveCounter = 0;

        /// <summary>
        ///  High Score
        /// </summary>
        public int HighScore = 0;

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

            // Enemy block creation
            this.EnemiesBlockCreation();

            // Player Spaceship creation
            this.PlayerSpaceShipCreation();

            // Bunkers creation
            this.BunkersCreation();
        }

        #endregion

        #region methods


        /// <summary>
        /// Creates the enemy block
        /// </summary>
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

        /// <summary>
        /// Creates the bunker trigger
        /// </summary>
        private void TriggerCreation()
        {
            trigger = new Trigger(new Vecteur2D(0, GameSize.Height - 220), new Size(605, 10));
            AddNewGameObject(this.trigger);
        }

        /// <summary>
        /// Creates the player spaceship
        /// </summary>
        private void PlayerSpaceShipCreation()
        {
            this.PlayerShip = new PlayerSpaceship(new Vecteur2D(0, 0), 150);
            PlayerShip.Position.x = (GameSize.Width / 2) - PlayerShip.Image.Width / 2;
            PlayerShip.Position.y = GameSize.Height - 100;
            AddNewGameObject(this.PlayerShip);
        }

        /// <summary>
        /// Creates the bunkers
        /// </summary>
        private void BunkersCreation()
        {
            for (int i = 0; i < 3; i++)
            {
                int imageWidth = Resources.bunker.Width;
                Vecteur2D Position = new Vecteur2D((GameSize.Width) / 3 * (i + 1) - (GameSize.Width / 6 + imageWidth / 2), GameSize.Height - 200);
                Bunker bunker = new Bunker(Position);
                AddNewGameObject(bunker);
            }
          
            // Trigger creation for the bunkers
            this.TriggerCreation();
        }

        /// <summary>
        /// Resets all game elements and increases difficulty if a wave was won, else resets the score
        /// </summary>
        public void ResetGame()
        {
            // Deleting all game objects
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
                // Incrementing wave counter
                WaveCounter++;
                PlayerShip.Lives += Math.Min(50, PlayerShip.InitialLives - PlayerShip.Lives);
            }
            

            // Enemy block creation
            this.EnemiesBlockCreation();

            // Bunkers creation
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

            // Super missiles counter drawing
            int missileWidth = 10;
            for (int i = 0; i<PlayerShip.MissileCounter; i++)
            {
                Rectangle missileRectangle = new Rectangle(missileWidth, 10, Resources.shoot2.Width, Resources.shoot2.Height);
                g.DrawImage(Resources.shoot2, missileRectangle);
                missileWidth += Resources.shoot2.Width + 5;

            }

            // Different menu drawing depending of game.State
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

                // Controls
                case GameStates.Controls:
                    menu.DrawControls(g);
                    break;

                // High score
                case GameStates.HighScore:
                    menu.DrawHighScore(g);
                    break;

                // Default
                default:
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
            menu.UpdateMenu(keyPressed);

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

            //Win if all ships are destroyed
            if (!Enemies.enemyships.Any() && State != GameStates.Won)
            {
                // Music stop to prepare for next one
                menu.StopMusic();

                // HighScore calculation
                game.PlayerShip.Points += game.PlayerShip.Lives;
                game.HighScore = Math.Max(game.HighScore, game.PlayerShip.Points);

                // Go to Won State
                State = GameStates.Won;
            }
            //Lose if ally ship is destroyed
            else if (!PlayerShip.IsAlive() && State != GameStates.Lost)
            {
                // Music stop to prepare for next one
                menu.StopMusic();

                // HighScore calculation
                game.HighScore = Math.Max(game.HighScore, game.PlayerShip.Points);

                // Go to Lost State
                State = GameStates.Lost;
            }

            // Actions to do if we're playing the game
            if (State == GameStates.Play)
            {
                // Play the Game Music
                menu.PlayGameMusic();

                // Update each game object
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(this, deltaT);
                }
            }

            // Remove dead objects. Random is used to roll a bonus when an enemy dies
            Random rand = new Random();
            gameObjects.RemoveWhere(gameObject => {
                if (!gameObject.IsAlive())
                {
                    PlayerShip.handleEnnemieDie(gameObject, rand, this);
                }
                return !gameObject.IsAlive();
            }
            );
        }
        #endregion
    }
}