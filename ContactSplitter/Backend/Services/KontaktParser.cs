using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Shared.DataClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        private readonly Regex anredeRegex = new("^\\w+\\.?");

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
                TitelAnredeListe = JsonConvert.DeserializeObject<List<TitelAnrede>>(streamReader.ReadToEnd());
            };

            if (GeschlechtAnredeListe is null || TitelAnredeListe is null)
            {
                throw new DirectoryNotFoundException("Eine oder beide der einzulesenden JSON Dateien konnten nicht gefunden werden. Das Programm ist somit nicht lauffähig. \n " +
                    "Bitte starten Sie das Programm neu.");
            }

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
            //Prüfen ob Zahlen oder falsche Sonderzeichen drin sind, und dann Exception

            var splitContactResponse = new SplitContactResponse();

            splitContactResponse.RawInput = input.UserInput;

            SplitAnrede(ref input, ref splitContactResponse);

            SplitTitel(ref input, ref splitContactResponse);

            SplitName(ref input, ref splitContactResponse);

            CreateBriefanrede(ref splitContactResponse);

            return splitContactResponse;
        }

        /// <summary>
        /// Untersucht den UserInput der Request auf mögliche Anreden, erhalten aus der GeschlechtAnrede.json und fügt diese dem response Objekt hinzu
        /// Eine gefundene Anrede wird aus dem String der Request entfernt und bestimmt die Sprache und das Geschlecht des Kontakt
        /// </summary>
        /// <param name="request">Der zu parsende input</param>
        /// <param name="response">Das ResponseObjekt inklusive Anrede, Geschlecht und Sprache</param>
        private void SplitAnrede(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            if (!string.IsNullOrEmpty(request.UserInput))
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
                response.Anrede = string.Empty;
            }

        }

        /// <summary>
        /// Untersucht den UserInput der Request auf mögliche Titel, erhalten aus der TitelAnrede.json und fügt diese dem response Objekt hinzu.
        /// Gefundene Titel werden aus dem String der Request entfernt
        /// </summary>
        /// <param name="request">Der zu parsende input, ohne Anrede</param>
        /// <param name="response">Das ResponseObjekt inklusive aller gefundenen Titel</param>
        private void SplitTitel(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            if (!string.IsNullOrEmpty(request.UserInput))
            {
                var moreTitlesPossible = true;
                Regex? matchingRegex;
                Match? regMatch;
                TitelAnrede? titelAnrede;

                do
                {
                    titelAnrede = null;
                    matchingRegex = null;

                    foreach (var title in TitelAnredeListe)
                    {
                        regMatch = Regex.Match(request.UserInput, $"\\s*{title.Titel}\\s+");
                        if (regMatch.Success)
                        {
                            matchingRegex = new Regex($"{title.Titel}\\s+");
                            titelAnrede = title;
                            break;
                        }
                    }

                    if (titelAnrede is not null)
                    {
                        response.ListeAllerTitel.Add(titelAnrede);
                        request.UserInput = matchingRegex.Replace(request.UserInput, string.Empty, 1);
                    }
                    else
                    {
                        moreTitlesPossible = false;
                    }
                } while (moreTitlesPossible && !string.IsNullOrEmpty(request.UserInput));
            }
        }

        /// <summary>
        /// Untersucht den UserInput der Request auf Vorname und Nachname und fügt diese dem Response Objekt hinzu
        /// </summary>
        /// <param name="request">Der zu parsende input, ohne Anrede oder Titel</param>
        /// <param name="response">Das ResponseObjekt inklusive des Namens</param>
        private void SplitName(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            if (!string.IsNullOrEmpty(request.UserInput))
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

        /// <summary>
        /// Erstellt aus den Elementen einer Response die Briefanrede und fügt diese der Response hinzu 
        /// </summary>
        /// <param name="response">Das Response Objekt, für das die Briefanrede erstellt werden soll</param>
        public void CreateBriefanrede(ref SplitContactResponse response)
        {
            switch (response.Sprache)
            {
                case Sprache.Unbekannt:
                case Sprache.Deutsch:
                    switch (response.Geschlecht)
                    {
                        case Geschlecht.m:
                            response.Briefanrede = $"Sehr geehrter Herr {response.BriefTitel}{response.Vorname} {response.Nachname}";
                            break;
                        case Geschlecht.w:
                            response.Briefanrede = $"Sehr geehrte Frau {response.BriefTitel}{response.Vorname} {response.Nachname}";
                            break;
                        default:
                            response.Briefanrede = $"Guten Tag {response.BriefTitel}{response.Vorname} {response.Nachname}";
                            break;
                    }
                    break;
                case Sprache.Englisch:
                    switch (response.Geschlecht)
                    {
                        case Geschlecht.m:
                            response.Briefanrede = string.IsNullOrEmpty(response.BriefTitel) ? $"Dear Mr. {response.Vorname} {response.Nachname}" : $"Dear {response.BriefTitel}{response.Vorname} {response.Nachname}";
                            break;
                        case Geschlecht.w:
                            response.Briefanrede = string.IsNullOrEmpty(response.BriefTitel) ? $"Dear {response.Anrede} {response.BriefTitel}{response.Vorname} {response.Nachname}" : $"Dear {response.BriefTitel}{response.Vorname} {response.Nachname}";
                            break;
                        default:
                            response.Briefanrede = $"Dear {response.BriefTitel}{response.Vorname} {response.Nachname}";
                            break;
                    }
                    break;
            }
        }
    }
}