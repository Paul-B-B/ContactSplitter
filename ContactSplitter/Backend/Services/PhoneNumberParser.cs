using Newtonsoft.Json;
using ContactSplitter.Shared.DataClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PhoneNumbers;
using PhoneNumber = ContactSplitter.Shared.DataClass.PhoneNumber;

namespace ContactSplitter.Backend.Services
{
    public class PhoneNumberParser
    {

        private readonly List<Country> _CountryList;
        private readonly Country _DefaultCountry = new Country() { PhoneCode = "+49", Abbrevation = "DE", Name = "Germany" };
        private readonly PhoneNumberUtil _PhoneNumberUtil;

        /// <summary>
        /// Parsed und trennt Telefonnummern auf
        /// </summary>
        /// <param name="CountryCodesFilePath">Nur relevant für Tests</param>
        public PhoneNumberParser(string CountryCodesFilePath = "..//..//..//Backend//Data//CountryCodes.json")
        {
            using (StreamReader reader = new StreamReader(CountryCodesFilePath))
            {
                var readFile = reader.ReadToEnd();
                _CountryList = JsonConvert.DeserializeObject<List<Country>>(readFile);
            }
            _PhoneNumberUtil = PhoneNumberUtil.GetInstance();

            if(_CountryList is null)
            {
                throw new FileNotFoundException("Die einzulesende Country JSON Datei konnte nicht gefunden werden. Das Programm ist somit nicht lauffähig. \n " +
                    "Bitte starten Sie das Programm neu.");
            }
        }

        /// <summary>
        /// Nimmt eine Nummer entgegen, parsed ihre Elemente und liefert die aufgeteilte Nummer als PhoneNumber Objekt zurück
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public PhoneNumber ParseNumber(string userInput)
        {
            var resultNumber = new PhoneNumber() { RawNumberInput = userInput };
            if (Regex.IsMatch(userInput, "^[([]?(00|0|\\+)[)\\]]?\\d{2}\\s?(\\(\\d+\\)|0?\\d{3})[\\s/]?\\[?(\\d{6}|\\d+\\-\\d+)\\]?"))
            {
                AddCountry(ref resultNumber);

                FindAreaCode(ref resultNumber);

                FindMainNumberAndDirectDial(ref resultNumber);

                FormatPhoneNumber(ref resultNumber);

                return resultNumber;
            }

            return ParseNumberWithFramework(userInput);

        }

        /// <summary>
        /// Untersucht die gegebene PhoneNumber auf eine Ländervorwahl, fügt diese dem entsprechenden Attribut hinzu und entfernt sie von der MainNumber
        /// Wird keine Vorwahl erkannt, wird Deutschland als Standard gewählt
        /// </summary>
        /// <param name="resultNumber">Ein PhoneNumber Objekt mit einer MainNumber</param>
        private void AddCountry(ref PhoneNumber resultNumber)
        {
            try
            {
                var originCountryCode = Regex.Match(resultNumber.RawNumberInput, "^[([]?(00|0|\\+)[)\\]]?\\d{2}").Value;
                resultNumber.Country = _CountryList.First(country => country.PhoneCode.Equals(Regex.Replace(originCountryCode, "^[(\\[]?(00|0)[)\\]]?", "+")));
                resultNumber.MainNumber = Regex.Replace(resultNumber.RawNumberInput, originCountryCode.StartsWith("+") ? "\\" + originCountryCode : originCountryCode, "").Trim();
            }
            catch (InvalidOperationException)
            {
                resultNumber.Country = _DefaultCountry;
                resultNumber.MainNumber = resultNumber.RawNumberInput;
            }
        }

        /// <summary>
        /// Untersucht die gegebene PhoneNumber auf eine Durchwahl, fügt diese dem entsprechenden Attribut hinzu und entfernt sie von der MainNumber
        /// </summary>
        /// <param name="resultNumber">Ein PhoneNumber Objekt mit MainNumber ohne Vorwahl</param>
        private void FindAreaCode(ref PhoneNumber resultNumber)
        {
            if (Regex.IsMatch(resultNumber.MainNumber, "^\\(\\d+\\)\\s?"))
            {
                resultNumber.AreaCode = Regex.Match(resultNumber.MainNumber, "^\\((\\d+)\\)[\\s/]?").Groups[1].Value;
                resultNumber.MainNumber = Regex.Replace(resultNumber.MainNumber, "^\\((\\d+)\\)[\\s/]?", "").Trim();
                return;
            }
            resultNumber.AreaCode = Regex.Match(resultNumber.MainNumber, "^0?(\\d{3})").Groups[1].Value;
            resultNumber.MainNumber = Regex.Replace(resultNumber.MainNumber, "^\\(?0?\\d{3}\\)?[\\s/]?", "").Trim();
        }

        /// <summary>
        /// Untersucht die gegebene PhoneNumber auf die Hauptnummer und Durchwahl und fügt diese Elemente dem entsprechenden Attribut hinzu
        /// Falls diese Untersuchung aufgrund einer ungewöhnlichen Eingabe fehlschlägt, wird auf das Google-Framework zur Telefonnummererkennung zurückgegriffen
        /// </summary>
        /// <param name="resultNumber">Ein PhoneNumber Objekt mit MainNumber ohne Vorwahl und Durchwahl </param>
        private void FindMainNumberAndDirectDial(ref PhoneNumber resultNumber)
        {
            if (Regex.IsMatch(resultNumber.MainNumber, "^\\[.+\\]"))
            {
                resultNumber.MainNumber = resultNumber.MainNumber.Substring(1, resultNumber.MainNumber.Length - 2);
            }
            if (Regex.IsMatch(resultNumber.MainNumber, "\\d+\\-\\d+"))
            {
                resultNumber.HasDirectDial = true;
                resultNumber.DirectDial = Regex.Match(resultNumber.MainNumber, "\\-\\d+").Value.Substring(1);
                resultNumber.MainNumber = Regex.Match(resultNumber.MainNumber, "^\\d+").Value;
            }
            else
            {
                resultNumber.HasDirectDial = false;
            }
        }

        /// <summary>
        /// Fallback Methode, sollte die Hauptnummer nicht erkannt werden können
        /// </summary>
        /// <param name="userInput">Falls auch das Google Framework die Nummer nicht erkennen kann</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private PhoneNumber ParseNumberWithFramework(string userInput)
        {
            var resultNumber = new PhoneNumber() { RawNumberInput = userInput };
            var splitNumber = _PhoneNumberUtil.Parse(userInput, "DE");

            if (splitNumber.HasCountryCode)
            {
                resultNumber.Country = _CountryList.First(country => country.PhoneCode.Equals("+" + splitNumber.CountryCode.ToString()));
                resultNumber.MainNumber = splitNumber.NationalNumber.ToString();
            }
            else
            {
                throw new ArgumentException("Phone number was not in a readable format");
            }

            FindAreaCode(ref resultNumber);

            FindMainNumberAndDirectDial(ref resultNumber);

            FormatPhoneNumber(ref resultNumber);

            return resultNumber;
        }

        /// <summary>
        /// Formatiert die einzelnen Bestandteile eines PhoneNumber Objekts und fügt die formatierte, vollständige Nummer dem entsprechenden Attribut hinzu 
        /// </summary>
        /// <param name="phoneNumber"></param>
        private void FormatPhoneNumber(ref PhoneNumber phoneNumber)
        {
            phoneNumber.FormattedNumber =
                phoneNumber.Country.PhoneCode +
                " " +
                phoneNumber.AreaCode +
                " " +
                (phoneNumber.HasDirectDial ? phoneNumber.MainNumber + "-" + phoneNumber.DirectDial : phoneNumber.MainNumber);
        }

    }
}
