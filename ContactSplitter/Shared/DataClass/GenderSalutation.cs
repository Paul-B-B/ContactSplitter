namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Modellierung einer Anrede mit zugehörigem Geschlecht und zugehöriger Sprache
    /// </summary>
    public class GenderSalutation
    {
        /// <summary>
        /// Die Anrede des Kontakts, Bsp.: Herr, Frau, Mr, Mrs
        /// </summary>
        public string Salutation { get; set; }

        /// <summary>
        /// Geschlecht des Kontakts
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Sprache des Kontakts
        /// </summary>
        public Language Language { get; set; }
    }
}
