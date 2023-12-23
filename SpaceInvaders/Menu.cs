﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            "Settings",
            "High Score"
        };

        string selectedItem;

        SoundPlayer soundPlayer;

        SolidBrush brush = new SolidBrush(Color.White);
        PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        IntPtr fontBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(Properties.Resources.space_invaders_font, 0);
        Font font;


        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="game">game instance to take care of</param>
        /// 
        /// <returns></returns>
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

            privateFontCollection.AddMemoryFont(fontBuffer, Properties.Resources.space_invaders_font.Length);
            font = new Font(privateFontCollection.Families[0], 22);
        }

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

        public void StopMusic()
        {
            // Stop the background music if it's playing and kill the soundPlayer
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
            }

            soundPlayer = null;
        }

        internal void DrawPause(Graphics g)
        {
            g.DrawString("PAUSE", font, brush, game.GameSize.Width / 2 - 40, game.GameSize.Height / 2 - 24);
        }

        internal void DrawLost(Graphics g)
        {
            PlayLostMusic();

            font = new Font(privateFontCollection.Families[0], 16);
            g.DrawString($"YOU LOSE!\n(press <ENTER> to go to the Main Menu)\n{game.PlayerShip.Points} Points", font, brush, game.GameSize.Width / 2 - 260, game.GameSize.Height / 2 - 24);
        }

        internal void DrawWon(Graphics g)
        {
            PlayWonMusic();

            font = new Font(privateFontCollection.Families[0], 16);
            g.DrawString($"YOU WIN!\n(press <ENTER> to go to the Main Menu\nor press <SPACE> to continue playing)\n{game.PlayerShip.Points} Points", font, brush, game.GameSize.Width / 2 - 260, game.GameSize.Height / 2 - 24);
        }
        internal void DrawSettings(Graphics g)
        {
            font = new Font(privateFontCollection.Families[0], 16);
            g.DrawString("COMING SOON\n(press <ENTER> to go to the Main Menu)", font, brush, game.GameSize.Width / 2 - 260, game.GameSize.Height / 2 - 24);
        }

        internal void DrawHighScore(Graphics g)
        {
            g.DrawString($"HIGH SCORE: {game.HighScore}\n(press <ENTER> to go to the Main Menu)", font, brush, game.GameSize.Width / 2 - 260, game.GameSize.Height / 2 - 24);
        }

        internal void DrawMainMenu(Graphics g)
        {
            // Musique
            PlayMenuMusic();

            // Affichage du logo
            Image BackgroundImage = Properties.Resources.logo;
            Rectangle rectangle = new Rectangle(40, 40, game.GameSize.Width-80, game.GameSize.Height*2/5);
            g.DrawImage(BackgroundImage, rectangle);

            // Affichage du texte
            Font font = new Font(privateFontCollection.Families[0], 14);
            g.DrawString("PRESS <ENTER> TO SELECT", font, brush, game.GameSize.Width / 2 - 150, game.GameSize.Height / 2 + 60);

            // Affichage du menu selectif
            int width = game.GameSize.Width / 2;
            int height = game.GameSize.Height / 2 + 120;
            foreach (var item in menuItems)
            {
                if (item == selectedItem)
                {
                    // Couleur de l'item sélectionné
                    brush.Color = ColorTranslator.FromHtml("#ffe213");
                }
                g.DrawString($"{item}", font, brush, width-item.Length*7.4f, height);
                height += 50;

                // Couleur par défaut du texte
                brush.Color = ColorTranslator.FromHtml("#ed2c0b");
            }
        }

        internal void UpdateMenu(double deltaT, HashSet<Keys> keyPressed)
        {
            switch (game.State)
            {
                // Navigation dans le Menu
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
                            game.State = Game.GameStates.Settings;
                        }
                        else if (selectedItem == menuItems[2])
                        {
                            game.State = Game.GameStates.HighScore;
                        }
                        keyPressed.Remove(Keys.Enter);
                    }

                    break;

                case Game.GameStates.Settings:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        game.State = Game.GameStates.Menu;
                    }
                    keyPressed.Remove(Keys.Enter);
                    break;

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

                case Game.GameStates.Lost:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        StopMusic();

                        game.ResetGame();
                        game.State = Game.GameStates.Menu;
                        keyPressed.Remove(Keys.Enter);
                    }
                    break;

                case Game.GameStates.Won:
                    if (keyPressed.Contains(Keys.Enter))
                    {
                        StopMusic();

                        game.ResetGame();
                        game.State = Game.GameStates.Menu;
                        keyPressed.Remove(Keys.Enter);
                    }
                    else if (keyPressed.Contains(Keys.Space)) {
                        StopMusic();

                        game.ResetGame();
                        game.State = Game.GameStates.Play;
                        keyPressed.Remove(Keys.Space);
                    }

                    break;

                default:
                    Console.WriteLine("DEFAULT");
                    break;
            }
        }
    }
}
