namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Modell eines Titels eines Kontakts
    /// </summary>
    public class TitleSalutation
    {
        /// <summary>
        /// Die Schreibweise des Titels
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Die in der Briefanrede zu verwendende Schreibweise
        /// </summary>
        public string Salutation { get; set; }
    }
}