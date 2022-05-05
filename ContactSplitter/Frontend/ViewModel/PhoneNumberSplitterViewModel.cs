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


        private readonly IPhoneNumberSplitterModel _phoneNumberModel;

        public PhoneNumberSplitterViewModel(IPhoneNumberSplitterModel numberModel)
        {
            this._phoneNumberModel = numberModel;
        }

        public ICommand SubmitNumberCommand => new RelayCommand(x => this.OnSubmitButtonClicked());

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

        public ICommand CopyNumberCommand => new RelayCommand(x => this.OnCopyButtonClicked());

        private void OnCopyButtonClicked()
        {
            Clipboard.SetText(FormattedNumber.FormattedNumber);
        }

    }
}
