﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
    class ElementC
    {

        private char grupa;
        private int vrednost;
        private static readonly Random rand = new Random();

        public ElementC()
        {
            vrednost = rand.Next(1, 9);
            grupa = (char)rand.Next(97, 122);
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
