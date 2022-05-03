using ContactSplitter.Backend.Model;
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
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

        private string GeschlechtAnredeJsonName = "GeschlechtAnrede.json";
        private string TitelAnredeJsonName = "TitelAnrede.json";

        private List<TitelAnrede> TitelAnredeListe;
        private List<GeschlechtAnrede> GeschlechtAnredeListe;

        public KontaktParser(string pathToData = "../Data/")
        {
            using var streamReader = new StreamReader($"{pathToData}/{GeschlechtAnredeJsonName}");
            GeschlechtAnredeListe = JsonConvert.DeserializeObject<List<GeschlechtAnrede>>(streamReader.ReadToEnd());

            using var StreamReader = new StreamReader($"{pathToData}/{TitelAnredeJsonName}");
            TitelAnredeListe = JsonConvert.DeserializeObject<List<TitelAnrede>>(streamReader.ReadToEnd());
        }

        public SplitContactResponse ParseKontakt(SplitContactRequest input)
        {
            var splitContactResponse = new SplitContactResponse();
            splitContactResponse.RawInput = input.UserInput;

            SplitAnrede(ref input, ref splitContactResponse);

            SplitTitel(ref input, ref splitContactResponse);

            SplitName(ref input, ref splitContactResponse);



            return null;
        }

        private void SplitAnrede(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            var firstWord = Regex.Match(request.UserInput, "^\\w+");

            var anrede = GeschlechtAnredeListe.FirstOrDefault(an => an.Anrede.Equals(firstWord.Value));
            if (anrede is not null)
            {
                var Sprache = GeschlechtAnredeListe.First(an => an.Anrede.Equals(firstWord.Value)).Sprache;
                response.Sprache = Sprache.Unbekannt;
                Regex.Replace(request.UserInput, "^\\w+\\s", string.Empty);

                return;
            }

            response.Anrede = null;
            response.Sprache = Model.Sprache.Unbekannt;


        }

        private void SplitTitel(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            var firstWord = Regex.Match(request.UserInput, "^\\w+.?"); // zweiter Titel fehlt noch

            var titel = TitelAnredeListe.FirstOrDefault(ti => ti.Anrede.Equals(firstWord.Value) || ti.Titel.Equals(firstWord.Value));
            if (titel is not null)
            {
                response.Titel = titel.Anrede;
                Regex.Replace(request.UserInput, "^\\w+.?\\s", string.Empty);
                return;
            }

            response.Titel = null;
        }

        public void SplitName(ref SplitContactRequest request, ref SplitContactResponse response)
        {
            var vornameRegex = "([A-Z]\\w*([\\s\\-]+[A-Z]\\w*)*)";
            var nachnameRegex = "(\\w+\\s+)*[A-Z]\\w*(\\-?[A-Z]\\w*)*)";
            var regexGruppenNameVorname = "Vorname";
            var regexGruppenNameNachname = "Nachname";
            var nameRegex = $"(^(?<{regexGruppenNameVorname}>{vornameRegex})\\s+(?<{regexGruppenNameNachname}>{nachnameRegex})|" + // Vorname Nachname
                            $"(^(?<{regexGruppenNameNachname}>{nachnameRegex}),\\s+(?<{regexGruppenNameVorname}>{vornameRegex})"; // Nachname, Vorname

            //var result = nameRegex.Match(request.UserInput);

            var result = Regex.Match(request.UserInput, nameRegex);

            if (result is not null)
            {

            }


        }
    }
}
