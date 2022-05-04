using ContactSplitter.Backend.Model;
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Shared.DataClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Services
{
    public class KontaktParser
    {

        // Dateinamen
        private readonly string GeschlechtAnredeJsonName = "GeschlechtAnrede.json";
        private readonly string TitelAnredeJsonName = "TitelAnrede.json";

        // RegEx zur Namenserkennung
        private readonly string vornameRegex = "([A-Z]\\w*([\\s\\-]+[A-Z]\\w*)*)";
        private readonly string nachnameRegex = "(\\w+\\s+)*[A-Z]\\w*(\\-?[A-Z]\\w*)*)";
        private readonly string regexGruppeVorname = "Vorname";
        private readonly string regexGruppeNachname = "Nachname";
        private readonly string vornameNachnameRegex;

        // RegEx zur Anredenerkennung
        private readonly Regex anredeRegex = new Regex("^\\w+\\.?");

        // RegEx zur Titelerkennung
        private readonly Regex titelRegex = new Regex("^\\w+\\.?"); //Zweiter Titel fehlt noch

        //Hilfslisten (erhalten aus eingelesenen Dateien)
        private readonly List<TitelAnrede> TitelAnredeListe;
        private readonly List<GeschlechtAnrede> GeschlechtAnredeListe;

        /// <summary>
        /// Erstellt ein neues Objekt des KontaktParsers
        /// </summary>
        /// <param name="pathToData">Pfad zum Data Ordner (wird für die Tests benötigt)</param>
        public KontaktParser(string pathToData = "../Data/")
        {
            using (var streamReader = new StreamReader($"{pathToData}/{GeschlechtAnredeJsonName}"))
            {
                GeschlechtAnredeListe = JsonConvert.DeserializeObject<List<GeschlechtAnrede>>(streamReader.ReadToEnd());
            };

            using (var streamReader = new StreamReader($"{pathToData}/{TitelAnredeJsonName}"))
            {
                TitelAnredeListe = JsonConvert.DeserializeObject<List<TitelAnrede>>(streamReader.ReadToEnd()); // Diese Liste ist im Test NULL, TODO: überprüfen wieso
            };

            vornameNachnameRegex = $"(^(?<{regexGruppeVorname}>{vornameRegex})\\s+(?<{regexGruppeNachname}>{nachnameRegex})|" + // Vorname Nachname
                            $"(^(?<{regexGruppeNachname}>{nachnameRegex}),\\s+(?<{regexGruppeVorname}>{vornameRegex})"; // Nachname, Vorname
        }

        /// <summary>
        /// Parsed den NutzerInput aus einem SplitContactRequest Objekt und gibt ein SplitContactResponse Objekt zurück
        /// </summary>
        /// <param name="input">Vom Nutzer eingegebener Name</param>
        /// <returns>Der geparste Kontakt als SplitContactResponse</returns>
        public SplitContactResponse ParseKontakt(SplitContactRequest input)
        {
            var splitContactResponse = new SplitContactResponse();
            splitContactResponse.RawInput = input.UserInput;

            SplitAnrede(ref input, ref splitContactResponse);

            SplitTitel(ref input, ref splitContactResponse);

            SplitName(ref input, ref splitContactResponse);

            //Erstelle Briefanrede

            return splitContactResponse;
        }


        private void SplitAnrede(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            var firstWord = anredeRegex.Match(request.UserInput);

            var geschlechtAnrede = GeschlechtAnredeListe.FirstOrDefault(an => an.Anrede.Equals(firstWord.Value));
            if (geschlechtAnrede is not null)
            {
                response.Anrede = geschlechtAnrede.Anrede;
                response.Sprache = geschlechtAnrede.Sprache;
                response.Geschlecht = geschlechtAnrede.Geschlecht;
                request.UserInput = Regex.Replace(request.UserInput, $"^\\s*{firstWord.Value}\\s*", string.Empty);
                return;
            }
            response.Anrede = null;
            response.Geschlecht = Geschlecht.unbekannt; 
            response.Sprache = Sprache.Unbekannt;

        }

        private void SplitTitel(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            var firstWord = titelRegex.Match(request.UserInput);

            var titelAnrede = TitelAnredeListe.FirstOrDefault(ti => ti.Anrede.Equals(firstWord.Value) || ti.Titel.Equals(firstWord.Value));
            if (titelAnrede is not null)
            {
                response.Titel = titelAnrede.Anrede;
                request.UserInput = Regex.Replace(request.UserInput, $"^\\s*{firstWord.Value}\\s*", string.Empty);
                return;
            }

            response.Titel = null;
        }

        public void SplitName(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            var result = Regex.Match(request.UserInput, vornameNachnameRegex);

            if (result.Success)
            {
                response.Vorname = result.Groups[regexGruppeVorname].Value;
                response.Nachname = result.Groups[regexGruppeNachname].Value;
                return;
            }

            //Wie machen wir error handling?

        }
    }
}
