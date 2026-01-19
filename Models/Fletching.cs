using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowbuilder.Models
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
        public int Id { get; set; }
        public required string Manufacturer { get; set; }
        public required string Name { get; set; }
        public double Weight { get; set; }
        public MaterialType MaterialType { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public required string Color { get; set; }

        // Parameterloser Constructor für EF Core
        public Fletching()
        {
        }

        // Constructor für normale Verwendung
        public Fletching(string manufacturer, string name, double weight, MaterialType materialtype, double length, double height, string color)
        {
            Manufacturer = manufacturer;
            Name = name;
            Weight = weight;
            MaterialType = materialtype;
            Length = length;
            Height = height;
            Color = color;
        }
    }
}