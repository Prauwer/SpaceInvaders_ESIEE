using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        public HashSet<string> menuItems = new HashSet<string>();


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
        }

        public void UpdateMenu(Graphics g)
        {
            Image BackgroundImage = Properties.Resources.background;
            Rectangle rectangle = new Rectangle(0, 0, game.GameSize.Width, game.GameSize.Height);
            g.DrawImage(BackgroundImage, rectangle);
            SolidBrush brush = new SolidBrush(Color.White);
            PrivateFontCollection privateFontCollection = new PrivateFontCollection();
            IntPtr fontBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(Properties.Resources.space_invaders_font, 0);
            privateFontCollection.AddMemoryFont(fontBuffer, Properties.Resources.space_invaders_font.Length);

            // Draw "PAUSE" in the windows if the game is in Pause state 
            if (game.State == Game.GameStates.Pause) // TODO : réfléchir si on passe de Game.GameStates à Menu.GameStates
            {
                Font font = new Font(privateFontCollection.Families[0], 22);
                g.DrawString("PAUSE", font, brush, game.GameSize.Width / 2 - 40, game.GameSize.Height / 2 - 24);

            }
            else if (game.State == Game.GameStates.Lost)
            {
                Font font = new Font(privateFontCollection.Families[0], 16);
                g.DrawString($"YOU LOOSE ! (press <space> to retry)\n{game.PlayerShip.Points} Points", font, brush, game.GameSize.Width / 2 - 240, game.GameSize.Height / 2 - 24);
            }
            else if (game.State == Game.GameStates.Win)
            {
                Font font = new Font(privateFontCollection.Families[0], 16);
                g.DrawString($"YOU WIN ! (press <space> to retry)\n{game.PlayerShip.Points} Points", font, brush, game.GameSize.Width / 2 - 240, game.GameSize.Height / 2 - 24);
            }

            if (game.State != Game.GameStates.Initial) // TODO : créer une fonction DrawAllGameObjects et l'appeler ici
            {
                foreach (GameObject gameObject in game.gameObjects)
                    gameObject.Draw(game, g);
            }
            else
            {
                Font font = new Font(privateFontCollection.Families[0], 14);
                g.DrawString("PRESS <ENTER> TO START", font, brush, game.GameSize.Width / 2 - 140, game.GameSize.Height / 2 - 24);
            }
        }
    }
}
