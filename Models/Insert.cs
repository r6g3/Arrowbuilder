using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    
    public class Insert
    {
        private string manufacturer { get; set; }
        private double weight { get; set; }
        private double Diameter { get; set; }
        private string threadSize { get; set; }
        private string material { get; set; }
        public Insert(string manufacturer, double weight, double diameter, string threadsize, string material)
        {
            this.manufacturer = manufacturer;
            this.weight = weight;
            this.Diameter = diameter;
            this.threadSize = threadsize;
            this.material = material;
        }
    }
}
