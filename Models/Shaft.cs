using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Arrowbuilder.Models
{
    public class Shaft
    {
        public int Id { get; set; }
        public double OuterDiameter { get; set; }
        public double InnerDiameter { get; set; }
        public double StockLength { get; set; }
        public int Spine { get; set; }
        public double Gpi { get; set; }
        public double StraightnessTolerance { get; set; }
        public required string Material { get; set; }
        public required string Manufacturer { get; set; }

        // Parameterloser Constructor für EF Core
        public Shaft()
        {
        }

        public Shaft(double outerDiameter, double innerDiameter, double stockLength, int spine, double gpi, double straightnessTolerance, string material, string manufacturer)
        {
            OuterDiameter = outerDiameter;
            InnerDiameter = innerDiameter;
            StockLength = stockLength;
            Spine = spine;
            Gpi = gpi;
            StraightnessTolerance = straightnessTolerance;
            Material = material;
            Manufacturer = manufacturer;
        }
    }
}