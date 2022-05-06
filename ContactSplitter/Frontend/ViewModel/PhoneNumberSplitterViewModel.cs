using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Frontend.Core;
using ContactSplitter.Shared.DataClass;
using System;
using System.Windows;
using System.Windows.Input;

namespace ContactSplitter.Frontend.ViewModel
{
    internal class PhoneNumberSplitterViewModel : ObservableObject
    {
        #region Properties
        private string _numberInputString;
        public string NumberInputString
        {
            get { return _numberInputString; }
            set { Update(ref _numberInputString, value); }
        }

        private PhoneNumber _formattedNumber;
        public PhoneNumber FormattedNumber
        {
            get { return _formattedNumber; }
            set { Update(ref _formattedNumber, value); }
        }

        private string _ErrorMessage;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { Update(ref _ErrorMessage, value); }
        }

        #endregion

        private readonly IPhoneNumberSplitter _phoneNumberModel;

        #region Konstruktor
        public PhoneNumberSplitterViewModel(IPhoneNumberSplitter numberModel)
        {
            this._phoneNumberModel = numberModel;
        }
        #endregion

        #region Commands
        public ICommand SubmitNumberCommand => new RelayCommand(x => this.OnSubmitButtonClicked());

        public ICommand CopyNumberCommand => new RelayCommand(x => this.OnCopyButtonClicked());

        #endregion

        /// <summary>
        /// Formatiert eine Telefonnumer in ihre einzelnen Bestandteile mit Hilfe des Parsers, sobald der Formatieren-Button betätigt wird
        /// </summary>

        #region private Methoden
        private void OnSubmitButtonClicked()
        {
            try
            {
                FormattedNumber = this._phoneNumberModel.GetFormattedNumber(NumberInputString);
            }
            catch (Exception ex)
            {
                ErrorMessage = "The phone number could not be read, the following exception occured: \n" + ex.Message;
            }
            NumberInputString = string.Empty;
        }

        /// <summary>
        /// Kopiert die formatierte Telefonnummer in den Zwischenspeicher, sobald der Kopieren-Button betätigt wird
        /// </summary>
        private void OnCopyButtonClicked()
        {
            Clipboard.SetText(FormattedNumber.FormattedNumber);
        }
        #endregion

    }
}
