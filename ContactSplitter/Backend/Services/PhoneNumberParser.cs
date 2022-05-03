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

        public PhoneNumberParser(string CountryCodesFilePath = "..//..//..//PhoneNumberOptimization//Data//CountryCodes.json")
        {
            using (StreamReader reader = new StreamReader(CountryCodesFilePath))
            {
                var readFile = reader.ReadToEnd();
                _CountryList = JsonConvert.DeserializeObject<List<Country>>(readFile);
            }
            _PhoneNumberUtil = PhoneNumberUtil.GetInstance();
        }

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
