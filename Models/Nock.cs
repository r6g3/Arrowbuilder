using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowbuilder.Models
{
    public enum NockType
    {
        Lighted,
        Pin,
        Regular
    }

    public class Nock
    {
        public int Id { get; set; }
        public required string Manufacturer { get; set; }
        public double Diameter { get; set; }
        public double Weight { get; set; }
        public NockType Type { get; set; }
        public double HeightToBowstring { get; set; }

        // Parameterloser Constructor für EF Core
        public Nock()
        {
        }

        public Nock(string manufacturer, double diameter, double weight, NockType type, double heighToBowstring)
        {
            Manufacturer = manufacturer;
            Diameter = diameter;
            Weight = weight;
            Type = type;
            HeightToBowstring = heighToBowstring;
        }
    }
}