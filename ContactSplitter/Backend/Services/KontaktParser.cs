using ContactSplitter.Shared.DataClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactSplitter.Backend.Services
{
    public class KontaktParser
    {

        // Dateipfade
        private readonly string CurrentPath = Directory.GetCurrentDirectory();
        private string GenderSalutationJsonPath => @"Backend\Data\GeschlechtAnrede.json";
        private string TitleSalutationJsonPath => @"Backend\Data\TitelAnrede.json";

        // RegEx zur Namenserkennung
        private readonly string firstNameRegex = "([A-Z]\\w*([\\s\\-]+[A-Z]\\w*)*)";
        private readonly string lastNameRegex = "(([a-z]+\\s+)*[A-Z]\\w*(\\-?[A-Z]\\w*)*)";
        private readonly string regexGroupFirstName = "Vorname";
        private readonly string regexGroupLastName = "Nachname";
        private readonly string FirstLastRegex;

        // RegEx zur Anredenerkennung
        private readonly Regex salutationRegex = new("^\\w+\\.?");

        // RegEx zur Erkennung eines Sonderzeichens
        private readonly Regex specialCharacterRegex = new("[!@#$%^&*()_=+\\[\\]\\(\\)\\{\\};:'\"\\\\<>/?`~\\|]");

        //Hilfslisten (erhalten aus eingelesenen Dateien)
        private List<TitleSalutation> TitleSalutationList;
        private List<GenderSalutation> GenderSalutationList;

        /// <summary>
        /// Erstellt ein neues Objekt des KontaktParsers
        /// </summary>
        public KontaktParser()
        {
            ReadJsonFiles();
            
            FirstLastRegex = $"(^(?<{regexGroupLastName}>{lastNameRegex}),\\s+(?<{regexGroupFirstName}>{firstNameRegex}))|" + // Nachname, Vorname
                            $"(^((?<{regexGroupFirstName}>{firstNameRegex})\\s+)?(?<{regexGroupLastName}>{lastNameRegex}))"; // Vorname Nachname
        }

        /// <summary>
        /// Liest die JSON Dateien, die die Titel und Anreden enthalten, neu ein
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void ReadJsonFiles()
        {
            using (var streamReader = new StreamReader(Path.Combine(CurrentPath, GenderSalutationJsonPath), false))
            {
                GenderSalutationList = JsonConvert.DeserializeObject<List<GenderSalutation>>(streamReader.ReadToEnd());
            };

            using (var streamReader = new StreamReader(Path.Combine(CurrentPath, TitleSalutationJsonPath), false))
            {
                TitleSalutationList = JsonConvert.DeserializeObject<List<TitleSalutation>>(streamReader.ReadToEnd());
            };

            if (GenderSalutationList is null || TitleSalutationList is null)
            {
                throw new DirectoryNotFoundException("Eine oder beide der einzulesenden JSON Dateien konnten nicht gefunden werden. Das Programm ist somit nicht lauffähig. \n " +
                    "Bitte starten Sie das Programm neu.");
            }

        }

        /// <summary>
        /// Parsed den NutzerInput aus einem SplitContactRequest Objekt und gibt ein Kontakt Objekt zurück
        /// </summary>
        /// <param name="input">Vom Nutzer eingegebener Name</param>
        /// <returns>Der geparste Kontakt als Kontakt</returns>
        public Contact ParseKontakt(string input)
        {
            //Prüfen ob Zahlen oder falsche Sonderzeichen drin sind, und dann Exception
            if (specialCharacterRegex.Match(input).Success)
            {
                throw new Exception("Der Name enthält ein nicht erlaubtes Sonderzeichen. Bitte entfernen Sie dieses.");
            }

            var contact = new Contact();

            contact.RawInput = input;

            SplitSalutation(ref input, ref contact);

            SplitTitle(ref input, ref contact);

            SplitName(ref input, ref contact);

            contact.LetterSalutation = CreateLetterSalutation(contact);

            return contact;
        }

        /// <summary>
        /// Untersucht den UserInput der Request auf mögliche Anreden, erhalten aus der GeschlechtAnrede.json und fügt diese dem response Objekt hinzu
        /// Eine gefundene Anrede wird aus dem String der Request entfernt und bestimmt die Sprache und das Geschlecht des Kontakt
        /// </summary>
        /// <param name="request">Der zu parsende input</param>
        /// <param name="response">Das ResponseObjekt inklusive Anrede, Geschlecht und Sprache</param>
        private void SplitSalutation(ref string request, ref Contact response)
        {
            if (!string.IsNullOrEmpty(request))
            {
                var firstWord = salutationRegex.Match(request);

                var genderSalutation = GenderSalutationList.FirstOrDefault(an => an.Salutation.Equals(firstWord.Value));
                if (genderSalutation is not null)
                {
                    response.Salutation = genderSalutation.Salutation;
                    response.Language = genderSalutation.Language;
                    response.Gender = genderSalutation.Gender;
                    request = Regex.Replace(request, $"^\\s*{firstWord.Value}\\s*", string.Empty);
                    return;
                }
                response.Salutation = string.Empty;
            }
        }

        /// <summary>
        /// Untersucht den UserInput der Request auf mögliche Titel, erhalten aus der TitleSalutation.json und fügt diese dem response Objekt hinzu.
        /// Gefundene Titel werden aus dem String der Request entfernt
        /// </summary>
        /// <param name="request">Der zu parsende input, ohne Anrede</param>
        /// <param name="response">Das ResponseObjekt inklusive aller gefundenen Titel</param>
        private void SplitTitle(ref string request, ref Contact response)
        {
            if (!string.IsNullOrEmpty(request))
            {
                var moreTitlesPossible = true;
                Regex? matchingRegex;
                Match? regMatch;
                TitleSalutation? titleSalutation;

                do
                {
                    titleSalutation = null;
                    matchingRegex = null;

                    foreach (var title in TitleSalutationList)
                    {
                        regMatch = Regex.Match(request, $"\\s*{title.Title}\\s+");
                        if (regMatch.Success)
                        {
                            matchingRegex = new Regex($"{title.Title}\\s+");
                            titleSalutation = title;
                            break;
                        }
                    }

                    if (titleSalutation is not null)
                    {
                        response.TitleList.Add(titleSalutation);
                        request = matchingRegex.Replace(request, string.Empty, 1);
                    }
                    else
                    {
                        moreTitlesPossible = false;
                    }
                } while (moreTitlesPossible && !string.IsNullOrEmpty(request));
            }
        }

        /// <summary>
        /// Untersucht den UserInput der Request auf Vorname und Nachname und fügt diese dem Response Objekt hinzu
        /// </summary>
        /// <param name="request">Der zu parsende input, ohne Anrede oder Titel</param>
        /// <param name="response">Das ResponseObjekt inklusive des Namens</param>
        private void SplitName(ref string request, ref Contact response)
        {
            if (!string.IsNullOrEmpty(request))
            {
                var result = Regex.Match(request, FirstLastRegex);

                if (result.Success)
                {
                    response.FirstName = result.Groups[regexGroupFirstName].Value;
                    response.LastName = result.Groups[regexGroupLastName].Value;
                    return;
                }
            }
        }

        /// <summary>
        /// Erstellt aus den Elementen einer Response die Briefanrede und fügt diese der Response hinzu 
        /// </summary>
        /// <param name="response">Das Response Objekt, für das die Briefanrede erstellt werden soll</param>
        public string CreateLetterSalutation(Contact response)
        {
            switch (response.Language)
            {
                case Language.Unknown:
                case Language.German:
                    switch (response.Gender)
                    {
                        case Gender.m:
                            return $"Sehr geehrter Herr {response.LetterTitle}{response.FirstName} {response.LastName}";
                        case Gender.f:
                            return $"Sehr geehrte Frau {response.LetterTitle}{response.FirstName} {response.LastName}";
                        default:
                            return $"Guten Tag {response.LetterTitle}{response.FirstName} {response.LastName}";
                    }
                case Language.English:
                    switch (response.Gender)
                    {
                        case Gender.m:
                            return string.IsNullOrEmpty(response.LetterTitle) ? $"Dear Mr. {response.FirstName} {response.LastName}" : $"Dear {response.LetterTitle}{response.FirstName} {response.LastName}";
                        case Gender.f:
                            return string.IsNullOrEmpty(response.LetterTitle) ? $"Dear Ms. {response.LetterTitle}{response.FirstName} {response.LastName}" : $"Dear {response.LetterTitle}{response.FirstName} {response.LastName}";
                        default:
                            return $"Dear {response.LetterTitle}{response.FirstName} {response.LastName}";
                    }
            }
            return null;
        }
    }
}