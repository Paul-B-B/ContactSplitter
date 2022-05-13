using ContactSplitter.Shared.DataClass;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ContactSplitter.Backend.Services
{
    public class TitelHandler
    {
        // Dateipfade
        private string titleSalutationJsonPath => @"Backend\Data\TitelAnrede.json";
        private string currentPath = Directory.GetCurrentDirectory();

        /// <summary>
        /// Fügt der Titel JSON Datei einen neuen Eintrag hinzu.
        /// </summary>
        /// <param name="titleSalutation">Die hinzuzufügende TitleSalutation</param>
        /// <returns>Ob die Hinzufügenaktion erfolgreich war</returns>
        /// <exception cref="IOException"></exception>
        public bool AddTitle(TitleSalutation titleSalutation)
        {
            List<TitleSalutation> titleSalutationList = LoadTitleJson();
            titleSalutationList.Insert(0, titleSalutation);

            try
            {
                using var streamWriter = new StreamWriter(Path.Combine(currentPath, titleSalutationJsonPath), false);
                streamWriter.Write(JsonConvert.SerializeObject(titleSalutationList));
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
        private List<TitleSalutation> LoadTitleJson()
        {
            using var streamReader = new StreamReader(Path.Combine(currentPath, titleSalutationJsonPath));
            var json = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<TitleSalutation>>(json);
        }
    }
}

