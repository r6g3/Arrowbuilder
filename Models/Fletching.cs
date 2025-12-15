using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public enum MaterialType
    {
        Feather,
        SyntheticFeather,
        Vane,
        RecurveVane,

    }
    public class Fletching
    {
        private string manufacturer { get; set; }
        private string name { get; set; }
        private double weight { get; set; }
        private MaterialType materialtype { get; set; }
        private double length { get; set; }
        private double height { get; set; }
        private string color { get; set; }

        public Fletching(string manufacturer, string name, double weight, MaterialType materialtype, double length, double height, string color)
        {
            this.manufacturer = manufacturer;
            this.name = name;
            this.weight = weight;
            this.materialtype = materialtype;
            this.length = length;
            this.height = height;
            this.color = color;
        }
    }
}
