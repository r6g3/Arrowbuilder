using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowbuilder.Models
{
    public class Insert
    {
        public int Id { get; set; }
        public required string Manufacturer { get; set; }
        public double Weight { get; set; }
        public double Diameter { get; set; }
        public required string ThreadSize { get; set; }
        public required string Material { get; set; }

        // Parameterloser Constructor für EF Core
        public Insert()
        {
        }

        public Insert(string manufacturer, double weight, double diameter, string threadsize, string material)
        {
            Manufacturer = manufacturer;
            Weight = weight;
            Diameter = diameter;
            ThreadSize = threadsize;
            Material = material;
        }
    }
}