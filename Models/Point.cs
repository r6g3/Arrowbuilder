using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public enum PointType
    {
        FieldPoint,
        Bullet,
        Broadhead,
        MechanicalBroadhead
    }

    public class Point
    {
        
        public PointType PointType { get; private set; }
        public string Manufacturer { get; private set; }
        public double Weight { get; private set; }
        public string Material { get; private set; }
        public bool ScrewIn { get; private set; }
        public double ThreadSize { get; private set; }
        public double Diameter { get; private set; }

        public Point(PointType pointType, string manufacturer, double weight, string material, bool screwIn, double threadSize = 0, double diameter = 0)
        {
            if (string.IsNullOrWhiteSpace(manufacturer))
                throw new ArgumentException("Hersteller darf nicht leer sein.", nameof(manufacturer));
            if (string.IsNullOrWhiteSpace(material))
                throw new ArgumentException("Material darf nicht leer sein.", nameof(material));
            if (weight <= 0)
                throw new ArgumentOutOfRangeException(nameof(weight), "Gewicht muss größer als 0 sein.");

            if (screwIn)
            {
                if (threadSize <= 0)
                    throw new ArgumentOutOfRangeException(nameof(threadSize), "ThreadSize muss größer als 0 sein, wenn ScrewIn true ist.");
            }
            else
            {
                if (diameter <= 0)
                    throw new ArgumentOutOfRangeException(nameof(diameter), "Diameter muss größer als 0 sein, wenn ScrewIn false ist.");
            }

            PointType = pointType;
            Manufacturer = manufacturer;
            Weight = weight;
            Material = material;
            ScrewIn = screwIn;
            ThreadSize = screwIn ? threadSize : 0;
            Diameter = screwIn ? 0 : diameter;
        }
    }
}