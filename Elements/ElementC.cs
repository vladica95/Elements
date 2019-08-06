using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
    public class ElementC
    {

        private char grupa;
        private int vrednost;
        private static readonly Random rand = new Random();

        public ElementC()
        {
            vrednost = rand.Next(1, 9);
            grupa = (char)rand.Next(97, 122);
        }
        public ElementC(char g,int v)
        {
            grupa = g;
            vrednost = v;
        }
        
        public char Grupa

        {
            get { return this.grupa; }

        }
        public int Vrednost

        {
            get { return this.vrednost; }

        }
        public void Stampaj()
        {
            Console.WriteLine("Grupa: "+ Grupa + " | Vrednost: " + Vrednost);
        }
    }
}
