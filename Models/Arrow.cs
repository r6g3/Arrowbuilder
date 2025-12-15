using System;

namespace Models
{
    // Plan (Pseudocode, detailliert):
    // - Klasse Arrow mit nicht-nullbaren benötigten Komponenten: Shaft, Fletching, Nock, Point und Purpose.
    // - Insert darf optional sein (nullable), weil bei nicht verschraubten Spitzen kein Insert vorhanden ist.
    // - Konstruktor erwartet alle erforderlichen Komponenten als Parameter und validiert sie mit ArgumentNullException.
    // - Wenn Point.ScrewIn true ist, ist ein Insert erforderlich -> Insert darf nicht null sein, sonst wird eine Ausnahme geworfen.
    // - Wenn Point.ScrewIn false ist, wird Insert auf null gesetzt.
    // - Zusätzliche Werte wie TotalLengthToString und Weight werden als optionale Parameter übernommen.
    // - So werden alle bisherigen Compiler-Warnungen/Fehler (fehlendes Semikolon, mögliche Nullzuweisungen,
    //   Dereferenzierung von null, Nicht-Initialisierung von Non-Nullable-Eigenschaften) beseitigt.

    public class Arrow
    {
        public Shaft Shaft { get; }
        public Fletching Fletching { get; }
        public Nock Nock { get; }
        public Insert? Insert { get; }
        public Point Point { get; }
        public double TotalLengthToString { get; }
        public double Weight { get; }
        public string Purpose { get; }

        public Arrow(
            Shaft shaft,
            Fletching fletching,
            Nock nock,
            Point point,
            Insert? insert,
            string purpose,
            double totalLengthToString = 0,
            double weight = 0)
        {
            this.Shaft = shaft ?? throw new ArgumentNullException(nameof(shaft));
            this.Fletching = fletching ?? throw new ArgumentNullException(nameof(fletching));
            this.Nock = nock ?? throw new ArgumentNullException(nameof(nock));
            this.Point = point ?? throw new ArgumentNullException(nameof(point));
            this.Purpose = purpose ?? throw new ArgumentNullException(nameof(purpose));
            this.TotalLengthToString = totalLengthToString;
            this.Weight = weight;

            if (point.ScrewIn)
            {
                // Bei verschraubter Spitze ist ein Insert erforderlich
                this.Insert = insert ?? throw new ArgumentNullException(nameof(insert), "Insert is required for screw-in points.");
            }
            else
            {
                // Bei nicht-verschraubter Spitze kein Insert
                this.Insert = null;
            }
        }
    }
}
