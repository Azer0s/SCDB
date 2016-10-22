﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linguistics
{
    public class SPO
    {
        public string Subject { get; set; }
        public string Predicate { get; set; }
        public string Object { get; set; }

        public string this[int index]
        {
            get { return FromIndex(index); }

            set { ToIndex(index, value); }
        }

        private void ToIndex(int index, string value)
        {
            switch (index)
            {
                case 0:
                    Subject = value;
                    break;
                case 1:
                    Predicate = value;
                    break;
                case 2:
                    Object = value;
                    break;
            }
        }

        public string FromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return Subject;
                case 1:
                    return Predicate;
                case 2:
                    return Object;
            }
            return null;
        }

        public override string ToString()
        {
            return Subject + "\n" + Predicate + "\n" + Object;
        }
    }
}