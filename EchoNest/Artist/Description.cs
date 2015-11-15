﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoNest.Artist
{
    public class Description : Attribute
    {
        // The constructor is called when the attribute is set.
        public Description(string description)
        {
            this.description = description;
        }

        // Keep a variable internally ...
        protected string description;

        // .. and show a copy to the outside world.
        public string Text
        {
            get { return this.description; }
            set { this.description = value; }
        }

    }
}
