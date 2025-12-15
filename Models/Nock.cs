using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public enum NockType
    {
        Lighted,
        Pin,
        Regular
    }
    public class Nock
    {
        private string manufacturer { get; set; }
        private double diameter { get; set; }
        private double weight { get; set; }
        private NockType type{ get; set; } // next time an enum lighted, pin, regular, etc.
        private double heightToBowstring { get; set; }
        public Nock(string manufacturer, double diameter, double weight, NockType type, double heighToBowstring)
        {
            this.manufacturer = manufacturer;
            this.diameter = diameter;
            this.weight = weight;
            this.type = type;
            this.heightToBowstring = heighToBowstring;
        }
    }
}
