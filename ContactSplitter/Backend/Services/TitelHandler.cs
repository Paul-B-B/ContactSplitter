using ContactSplitter.Shared.DataClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ContactSplitter.Backend.Services
{
    public class TitelHandler
    {
        // Dateipfade
        private string titelAnredeJsonPfad => @"Backend\Data\TitelAnrede.json";
        private string aktuellerPfad = Directory.GetCurrentDirectory();

        /// <summary>
        /// Fügt der Titel JSON Datei einen neuen Eintrag hinzu.
        /// </summary>
        /// <param name="titelAnrede">Die hinzuzufügende TitelAnrede</param>
        /// <returns>Ob die Hinzufügenaktion erfolgreich war</returns>
        /// <exception cref="IOException"></exception>
        public bool AddTitel(TitelAnrede titelAnrede)
        {
            List<TitelAnrede> titelAnredeListe = LoadTitleJson();
            titelAnredeListe.Insert(0, titelAnrede);

            try
            {
                using var streamWriter = new StreamWriter(Path.Combine(aktuellerPfad, titelAnredeJsonPfad), false);
                streamWriter.Write(JsonConvert.SerializeObject(titelAnredeListe));
                streamWriter.Flush();
            }
            catch
            {
                return false;
                throw new IOException(message: "Beim Schreiben des Titels ist etwas schiefgelaufen, bitte starten Sie das Programm neu und versuchen Sie es erneut.");
            }

            return true;
        }

        /// <summary>
        /// Liest die JSON Dateien ein um den serialisierten Listen ein Objekt hinzufügen zu können. 
        /// </summary>
        /// <returns></returns>
        private List<TitelAnrede> LoadTitleJson()
        {
            using var streamReader = new StreamReader(Path.Combine(aktuellerPfad, titelAnredeJsonPfad));
            var json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<TitelAnrede>>(json);
        }
    }
}

