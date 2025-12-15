using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Models
{
    public class Shaft
    {
        private double outerDiameter { get; set; }
        private double innerDiameter { get; set; }
        private double stockLength { get; set; }
        private int spine { get; set; }
        private double gpi { get; set; }
        private double straightnessTolerance { get; set; }
        private string material { get; set; }
        private string manufacturer { get; set; }

        public Shaft(double outerDiameter, double innerDiameter, double stockLength, int spine, double gpi, double straightnessTolerance, string material, string manufacturer) 
        {
            this.outerDiameter = outerDiameter;
            this.innerDiameter = innerDiameter; 
            this.stockLength = stockLength;
            this.spine = spine;
            this.gpi = gpi;
            this.straightnessTolerance = straightnessTolerance;
            this.material = material;
            this.manufacturer = manufacturer;
        }
    }
}
