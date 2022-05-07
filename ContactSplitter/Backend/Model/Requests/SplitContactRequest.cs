namespace ContactSplitter.Backend.Model.Requests
{
    /// <summary>
    /// Request eines aufzuteilenden Kontakts
    /// </summary>
    public class SplitContactRequest
    {
        /// <summary>
        /// Die Nutzereingabe, beinhaltet den zu erkennenden Kontakt
        /// </summary>
        public string UserInput { get; set; }

    }
}
