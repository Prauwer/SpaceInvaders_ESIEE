﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace SpaceInvaders
{
    class Menu
    {

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Menu menu { get; private set; }

        public Game game;

        List<string> menuItems = new List<string>
        {
            "Play",
            "Controls",
            "High Score"
        };

        string selectedItem;

        SoundPlayer soundPlayer;

        // Drawing variables
        SolidBrush brush = new SolidBrush(Color.White);

        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        IntPtr fontBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(Properties.Resources.space_invaders_font, 0);
        Font font;

        RectangleF drawingArea;
        StringFormat stringFormat;


        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="game">game instance to take care of</param>
        public static Menu CreateMenu(Game game)
        {
            if (menu == null)
                menu = new Menu(game);
            return menu;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="game">game instance to take care of</param>
        private Menu(Game game)
        {
            this.game = game;

            selectedItem = menuItems[0];

            // Drawing font
            privateFontCollection.AddMemoryFont(fontBuffer, Properties.Resources.space_invaders_font.Length);
            font = new Font(privateFontCollection.Families[0], 16);

            // Make string drawn centered
            drawingArea = new RectangleF(0, 0, game.GameSize.Width, game.GameSize.Height);
            stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }

        /// <summary>
        /// Plays the Main Menu Music if music isn't already playing
        /// </summary>
        public void PlayMenuMusic()
        {
            if (soundPlayer != null)
            {
                return;
            }

            soundPlayer = new SoundPlayer();
            soundPlayer.Stream = Properties.Resources.menu_music;
            soundPlayer.PlayLooping();
        }

        /// <summary>
        /// Plays the Game Music if music isn't already playing
        /// </summary>
        public void PlayGameMusic()
        {
            if (soundPlayer != null)
            {
                return;
            }

            soundPlayer = new SoundPlayer();
            soundPlayer.Stream = Properties.Resources.game_music;
            soundPlayer.PlayLooping();
        }

        /// <summary>
        /// Plays the Lost Menu music if music isn't already playing
        /// </summary>
        public void PlayLostMusic()
        {
            if (soundPlayer != null)
            {
                return;
            }

            soundPlayer = new SoundPlayer();
            soundPlayer.Stream = Properties.Resources.lost_music;
            soundPlayer.Play();
        }

        /// <summary>
        /// Plays the Won Menu music if music isn't already playing
        /// </summary>
        public void PlayWonMusic()
        {
            if (soundPlayer != null)
            {
                return;
            }

            soundPlayer = new SoundPlayer();
            soundPlayer.Stream = Properties.Resources.won_music;
            soundPlayer.Play();
        }

        /// <summary>
        /// Stops the current music if music is currently playing
        /// </summary>
        public void StopMusic()
        {
            // Stop the background music if it's playing and kill the soundPlayer
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
            }

            soundPlayer = null;
        }

        /// <summary>
        /// Draws the Pause Menu
        /// </summary>
        /// <param name="g">Graphics instance</param>
        internal void DrawPause(Graphics g)
        {
            g.DrawString("PAUSE", font, brush, game.GameSize.Width / 2 - 40, game.GameSize.Height / 2 - 24);
        }

        /// <summary>
        /// Draws the Lost Menu
        /// </summary>
        /// <param name="g">Graphics instance</param>
        internal void DrawLost(Graphics g)
        {
            PlayLostMusic();

            string text = $"YOU LOSE!\n(press <ENTER> to go to the Main Menu)\n\n" +
                $"{game.PlayerShip.Points} Points";
            g.DrawString(text, font, brush, drawingArea, stringFormat);
        }

        /// <summary>
        /// Draws the Won Menu
        /// </summary>
        /// <param name="g">Graphics instance</param>
        internal void DrawWon(Graphics g)
        {
            PlayWonMusic();

            string text = $"YOU WIN!\n(press <ENTER> to go to the Main Menu\n" +
                $"or press <SPACE> to continue playing)\n\n" +
                $"{game.PlayerShip.Points} Points";
            g.DrawString(text, font, brush, drawingArea, stringFormat);
        }

        /// <summary>
        /// Draws the Controls Menu
        /// </summary>
        /// <param name="g">Graphics instance</param>
        internal void DrawControls(Graphics g)
        {
            string text = "Use <LEFT> and <RIGHT> arrows to move your ship :          \0\n" +
                "Use <UP> arrow to shoot enemy ships\n" +
                "Bonuses :       \0 drop from enemy ships\nCollect them by shooting or touching\n" +
                "Use <DOWN> arrow to drop a missile coming from the sky if you have one :           \0\n" +
                "\n(press <ENTER> to go to the Main Menu)";


            g.DrawString(text, font, brush, drawingArea, stringFormat);

            g.DrawImage(Properties.Resources.ship3, game.GameSize.Width / 2 + 50, game.GameSize.Height / 2 - 105, 25, 25);
            g.DrawImage(Properties.Resources.bonus, game.GameSize.Width / 2 - 105, game.GameSize.Height / 2 - 45, 25, 25);
            g.DrawImage(Properties.Resources.shoot2, game.GameSize.Width / 2 + 170, game.GameSize.Height / 2 + 40, 25, 40);

            Rectangle rectangle = new Rectangle (game.GameSize.Width / 2 + 49, game.GameSize.Height / 2 - 106, 26, 26);
            g.DrawRectangle(new Pen(Color.Gold, 2), rectangle);

            rectangle = new Rectangle(game.GameSize.Width / 2 - 106, game.GameSize.Height / 2 - 46, 26, 26);
            g.DrawRectangle(new Pen(Color.Gold, 2), rectangle);

            rectangle = new Rectangle(game.GameSize.Width / 2 + 169, game.GameSize.Height / 2 + 39, 26, 41);
            g.DrawRectangle(new Pen(Color.Gold, 2), rectangle);
        }

        /// <summary>
        /// Draws the High Score Menu
        /// </summary>
        /// <param name="g">Graphics instance</param>
        internal void DrawHighScore(Graphics g)
        {
            string text = $"HIGH SCORE: {game.HighScore}\n" +
                $"(press <ENTER> to go to the Main Menu)";
            g.DrawString(text, font, brush, drawingArea, stringFormat);
        }

        /// <summary>
        /// Draws the Main Menu
        /// </summary>
        /// <param name="g">Graphics instance</param>
        internal void DrawMainMenu(Graphics g)
        {
            // Music
            PlayMenuMusic();

            // Logo display
            Image BackgroundImage = Properties.Resources.logo;
            Rectangle rectangle = new Rectangle(40, 40, game.GameSize.Width-80, game.GameSize.Height*2/5);
            g.DrawImage(BackgroundImage, rectangle);

            // Text display
            g.DrawString("PRESS <ENTER> TO SELECT", font, brush, game.GameSize.Width / 2 - 150, game.GameSize.Height / 2 + 60);

            // Selective menu display
            int width = game.GameSize.Width / 2;
            int height = game.GameSize.Height / 2 + 120;
            foreach (var item in menuItems)
            {
                if (item == selectedItem)
                {
                    // Selected item color
                    brush.Color = ColorTranslator.FromHtml("#ffe213");
                }
                g.DrawString($"{item}", font, brush, width-item.Length*7.4f, height);
                height += 50;

                // Default text color
                brush.Color = ColorTranslator.FromHtml("#ed2c0b");
            }
        }

        /// <summary>
        /// Handles all the Menu Options
        /// </summary>
        /// <param name="keyPressed">HashSet of all keys pressed</param>
        internal void UpdateMenu(HashSet<Keys> keyPressed)
        {
            switch (game.State)
            {
                // Main Menu Navigation
                case Game.GameStates.Menu:
                    if (keyPressed.Contains(Keys.Up))
                    {
                        selectedItem = menuItems[(menuItems.IndexOf(selectedItem) - 1 + menuItems.Count()) % menuItems.Count()];
                        keyPressed.Remove(Keys.Up);

                    }
                    if (keyPressed.Contains(Keys.Down))
                    {
                        selectedItem = menuItems[(menuItems.IndexOf(selectedItem) + 1 + menuItems.Count()) % menuItems.Count()];
                        keyPressed.Remove(Keys.Down);
                    }
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        if (selectedItem == menuItems[0])
                        {
                            StopMusic();
                            game.State = Game.GameStates.Play;
                        }
                        else if (selectedItem == menuItems[1])
                        {
                            game.State = Game.GameStates.Controls;
                        }
                        else if (selectedItem == menuItems[2])
                        {
                            game.State = Game.GameStates.HighScore;
                        }
                        keyPressed.Remove(Keys.Enter);
                    }

                    break;

                // Controls Menu Navigation
                case Game.GameStates.Controls:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        game.State = Game.GameStates.Menu;
                    }
                    keyPressed.Remove(Keys.Enter);
                    break;

                // High Score Menu Navigation
                case Game.GameStates.HighScore:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        game.State = Game.GameStates.Menu;
                    }
                    keyPressed.Remove(Keys.Enter);
                    break;

                //Switch the game to Play or Pause if p key is pressed
                case Game.GameStates.Pause:
                    if (keyPressed.Contains(Keys.P))
                    {
                        game.State = Game.GameStates.Play;
                        keyPressed.Remove(Keys.P);
                    }
                    break;

                case Game.GameStates.Play:
                    if (keyPressed.Contains(Keys.P))
                    {
                        game.State = Game.GameStates.Pause;
                        keyPressed.Remove(Keys.P);
                    }
                    break;

                // Lost Menu Navigation
                case Game.GameStates.Lost:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        // Music stop to prepare for next one
                        StopMusic();
                        
                        // Reset game and go to next State
                        game.ResetGame();
                        game.State = Game.GameStates.Menu;

                        keyPressed.Remove(Keys.Enter);
                    }
                    break;

                // Won Menu Navigation
                case Game.GameStates.Won:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        // Music stop to prepare for next one
                        StopMusic();

                        // Reset game and go to next State
                        game.ResetGame();
                        game.State = Game.GameStates.Menu;

                        keyPressed.Remove(Keys.Enter);
                    }
                    else if (keyPressed.Contains(Keys.Space)) {
                        // Music stop to prepare for next one
                        StopMusic();

                        // Reset game and go to next State
                        game.ResetGame();
                        game.State = Game.GameStates.Play;
                        keyPressed.Remove(Keys.Space);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
