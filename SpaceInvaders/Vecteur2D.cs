using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Vecteur2D
    {
        public double x;
        public double y;
        public double Norme;

        public Vecteur2D(double x, double y)
        {
            this.x = x;
            this.y = y;
            Norme = Math.Abs(x) + Math.Abs(y);

        }

        public static Vecteur2D operator + (Vecteur2D vecteur1, Vecteur2D vecteur2)
        {
            return new Vecteur2D(vecteur1.x + vecteur2.x, vecteur1.y + vecteur2.y);
        }

        public static Vecteur2D operator -(Vecteur2D vecteur1, Vecteur2D vecteur2)
        {
            return new Vecteur2D(vecteur1.x - vecteur2.x, vecteur1.y - vecteur2.y);
        }

        public static Vecteur2D operator -(Vecteur2D vecteur1)
        {
            return new Vecteur2D(-vecteur1.x, -vecteur1.y);
        }

        public static Vecteur2D operator *(Vecteur2D vecteur1, int scalar)
        {
            return new Vecteur2D(vecteur1.x * scalar, vecteur1.y * scalar);
        }

        public static Vecteur2D operator *(int scalar, Vecteur2D vecteur1)
        {
            return vecteur1 * scalar;
        }

        public static Vecteur2D operator /(Vecteur2D vecteur1, int scalar)
        {
            return new Vecteur2D(vecteur1.x / scalar, vecteur1.y / scalar);
        }
    }
}
