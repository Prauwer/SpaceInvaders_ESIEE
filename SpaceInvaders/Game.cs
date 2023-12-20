﻿using System;
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

        public enum GameStates
        {
            Initial,
            Play,
            Pause,
            Win,
            Lost,
        }

        public GameStates State;

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

            List<Bitmap> sprites = new List<Bitmap>();
            sprites.Add(Resources.ship1);
            sprites.Add(Resources.ship2);
            sprites.Add(Resources.ship4);
            sprites.Add(Resources.ship5);
            sprites.Add(Resources.ship6);
            sprites.Add(Resources.ship7);
            sprites.Add(Resources.ship8);
            sprites.Add(Resources.ship9);

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

        private void ResetGame()
        {
            // Suppression tous les objets du jeu
            this.Enemies = null;
            this.gameObjects.Clear();
            this.gameObjects.Add(this.PlayerShip);

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
                PlayerShip.Lives += 50;
            }
            

            // Création du bloc d'ennemis
            this.EnemiesBlockCreation();

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
            Image BackgroundImage = Resources.background;
            Rectangle backgroundRectangle = new Rectangle(0, 0, GameSize.Width, GameSize.Height);
            g.DrawImage(BackgroundImage, backgroundRectangle);
            SolidBrush brush = new SolidBrush(Color.White);
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            IntPtr fontBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(Resources.space_invaders_font, 0);
            privateFontCollection.AddMemoryFont(fontBuffer, Resources.space_invaders_font.Length);

            int missileWidth = 10;
            for (int i = 0; i<PlayerShip.MissileCounter; i++)
            {
                Rectangle missileRectangle = new Rectangle(missileWidth, 10, Resources.shoot2.Width, Resources.shoot2.Height);
                g.DrawImage(Resources.shoot2, missileRectangle);
                missileWidth += Resources.shoot2.Width + 5;

            }
            
            // Draw "PAUSE" in the windows if the game is in Pause state 
            if (State == GameStates.Pause) {
                Font font = new Font(privateFontCollection.Families[0], 22);
                g.DrawString("PAUSE", font, brush, GameSize.Width / 2 - 40, GameSize.Height / 2 - 24);

            }
            else if (State == GameStates.Lost)
            {
                Font font = new Font(privateFontCollection.Families[0], 16);
                g.DrawString($"YOU LOOSE ! (press <space> to retry)\n{PlayerShip.Points} Points", font, brush, GameSize.Width / 2 - 240, GameSize.Height / 2 - 24);
            }
            else if (State == GameStates.Win)
            {
                Font font = new Font(privateFontCollection.Families[0], 16);
                g.DrawString($"YOU WIN ! (press <space> to retry)\n{PlayerShip.Points} Points", font, brush, GameSize.Width / 2 - 240, GameSize.Height / 2 - 24);
            }

            else if (State != GameStates.Initial)
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.Draw(this, g);
            }
            else
            {
                Font font = new Font(privateFontCollection.Families[0], 14);
                g.DrawString("PRESS <ENTER> TO START", font, brush, GameSize.Width / 2 - 140, GameSize.Height / 2 - 24);
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

            //launch the game when Enter is pressed
            if (keyPressed.Contains(Keys.Enter) && State == GameStates.Initial)
            {
                State = GameStates.Play;
                ReleaseKey(Keys.Enter);
            }
            // spawn super missile if the player have missile in stock
            else if (keyPressed.Contains(Keys.Down) && PlayerShip.MissileCounter > 0)
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
            else if (!Enemies.enemyships.Any() && State != GameStates.Win)
            {
                State = GameStates.Win;
            }
            else if (!PlayerShip.IsAlive() && State != GameStates.Lost)
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