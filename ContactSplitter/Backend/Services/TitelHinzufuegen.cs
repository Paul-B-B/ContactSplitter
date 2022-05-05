using ContactSplitter.Shared.DataClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Services
{
    public class TitelHinzufuegen
    {
        private string titelAnredeJsonPfad => @"Backend\Data\TitelAnrede.json";
        private string aktuellerPfad = Directory.GetCurrentDirectory();



        public bool AddTitel(TitelAnrede titelAnrede)
        {

            List<TitelAnrede> titelAnredeListe = LoadTitleJson();
            titelAnredeListe.Add(titelAnrede);

            try
            {   
                using var streamWriter = new StreamWriter(Path.Combine(aktuellerPfad,titelAnredeJsonPfad), false);
                streamWriter.Write(JsonConvert.SerializeObject(titelAnredeListe));
                streamWriter.Flush();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private List<TitelAnrede> LoadTitleJson()
        {
            try
            {
                using var streamReader = new StreamReader(Path.Combine(aktuellerPfad, titelAnredeJsonPfad));
                var json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<TitelAnrede>>(json);
            }
            catch
            {
                return new List<TitelAnrede>();
            }
        }
    }
}

