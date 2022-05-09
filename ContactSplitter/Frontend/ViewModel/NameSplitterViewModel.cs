using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Frontend.Core;
using ContactSplitter.Shared.DataClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ContactSplitter.Frontend.ViewModel
{
    class NameSplitterViewModel : ObservableObject
    {
        #region Properties

        private string _textInputString;
        public string TextInputString
        {
            get { return _textInputString; }
            set
            {
                Update(ref _textInputString, value);
                this.OnTextInputChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { Update(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { Update(ref _lastName, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { Update(ref _title, value); }
        }

        private Gender _gender;
        public Gender Gender
        {
            get { return _gender; }
            set { Update(ref _gender, value); }
        }

        private Language _language;
        public Language Language
        {
            get { return _language; }
            set { Update(ref _language, value); }
        }

        private ObservableCollection<TitleSalutation> _titleList;
        public ObservableCollection<TitleSalutation> TitleList
        {
            get { return _titleList; }
            set { Update(ref _titleList, value); }
        }

        private string _newTitle;
        public string NewTitle
        {
            get { return _newTitle; }
            set { Update(ref _newTitle, value); }
        }

        private string _newSalutation;
        public string NewSalutation
        {
            get { return _newSalutation; }
            set { Update(ref _newSalutation, value); }
        }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                Update(ref _selectedContact, value);
                this.OnSelectedContactChanged();
            }
        }

        private string _germanSalutation;
        public string GermanSalutation
        {
            get { return _germanSalutation; }
            private set { Update(ref _germanSalutation, value); }
        }

        private string _englishSalutation;
        public string EnglishSalutation
        {
            get { return _englishSalutation; }
            set { Update(ref _englishSalutation, value); }
        }

        private string _spanishSalutation;
        public string SpanishSalutation
        {
            get { return _spanishSalutation; }
            set { Update(ref _spanishSalutation, value); }
        }

        private bool _recognize;
        public bool Recognize
        {
            get { return _recognize; }
            set { Update(ref _recognize, value); }
        }

        private string _contactSalutation;
        public string ContactSalutation
        {
            get { return _contactSalutation; }
            set { Update(ref _contactSalutation, value); }
        }

        private string _nameErrorMessage;

        public string NameErrorMessage
        {
            get { return _nameErrorMessage; }
            set { Update(ref _nameErrorMessage, value); }
        }

        private string _salutation;
        private string _letterSalutation;
        private string _rawInput;


        #endregion

        public ObservableCollection<Contact> Contacts { get; private set; }

        private readonly INameSplitterModel _nameSplitterModel;

        #region Konstruktor
        public NameSplitterViewModel(INameSplitterModel nameSplitterModel)
        {
            this._nameSplitterModel = nameSplitterModel;
            this.ClearBoxes();
            this.Contacts = new ObservableCollection<Contact>();
            this.TitleList = new ObservableCollection<TitleSalutation>();
        }
        #endregion

        #region Commands
        public ICommand SubmitTextCommand => new RelayCommand(x => this.OnTextInputChanged());

        public ICommand AddContactCommand => new RelayCommand(x => this.OnAddContactClicked());

        public ICommand AddTitleCommand => new RelayCommand(x => this.OnAddTitleClicked());

        public ICommand CopySalutationCommand => new RelayCommand(x => this.OnCopySalutationClicked());
        #endregion

        #region private Methoden

        /// <summary>
        /// Fügt den eingegebenen Titel der Json hinzu, so dass dieser bei künftigen Eingaben berücksichtigt wird
        /// </summary>
        private void OnAddTitleClicked()
        {
            if (this.NewTitle == string.Empty) return;
            var newTitle = new TitleSalutation() { Salutation = this.NewSalutation, Title = this.NewTitle };
            this.TitleList.Add(newTitle);
            if (this.Recognize)
            {
                this._nameSplitterModel.AddTitle(newTitle);
            }
            this.NewTitle = string.Empty;
            this.NewSalutation = string.Empty;
        }

        /// <summary>
        /// Formatiert die Eingabe und gibt diese in bearbeitbaren Feldern aus
        /// </summary>
        private void OnTextInputChanged()
        {
            if (this.TextInputString == string.Empty) return;
            try
            {
                this.ClearBoxes();
                var output = this._nameSplitterModel.GetSplitContact(this.TextInputString);
                this._salutation = output.Salutation;
                this._letterSalutation = output.LetterSalutation;
                this.FirstName = output.FirstName;
                this.LastName = output.LastName;
                this._rawInput = output.RawInput;
                this.Gender = output.Gender;
                this.Language = output.Language;
                if (output.TitleList != null)
                {
                    foreach (var item in output.TitleList)
                    {
                        if (item != null) this.TitleList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                NameErrorMessage = "The following Error occured: " + ex.Message;
            }
        }

        /// <summary>
        /// Fügt die formatierten Einträge in die Contacts Liste hinzu, sobald der Hinzufügen-Button betätigt wird
        /// </summary>
        private void OnAddContactClicked()
        {
            if (this.LastName == string.Empty) return;
            var contact = new Contact();
            var titles = new List<TitleSalutation>();
            foreach (var title in this.TitleList)
            {
                titles.Add(title);
            }
            contact.TitleList = titles;
            contact.Salutation = this._salutation;
            contact.FirstName = this.FirstName;
            contact.LastName = this.LastName;
            contact.RawInput = this._rawInput;
            contact.Gender = this.Gender;
            contact.Language = this.Language;
            contact.LetterSalutation = this._nameSplitterModel.GetLetterSalutation(contact);
            Contacts.Add(contact);
            this.ClearBoxes();
            this.TextInputString = string.Empty;
        }

        /// <summary>
        /// Zeigt die Briefanrede der ausgewählten Person an
        /// </summary>
        private void OnSelectedContactChanged()
        {
            if (this.SelectedContact == null) return;
            this.ContactSalutation = this.SelectedContact.LetterSalutation;
        }

        /// <summary>
        /// Kopiert die Briefanrede der aktuell ausgewählten Contact
        /// </summary>
        private void OnCopySalutationClicked()
        {
            if (this.SelectedContact == null || this.SelectedContact.LetterSalutation == null) return;
            Clipboard.SetText(this.SelectedContact.LetterSalutation);
        }

        /// <summary>
        /// Hilfsmethode um alle Eingabefelder zurückzusetzen
        /// /// </summary>
        private void ClearBoxes()
        {
            this._salutation = null;
            this._letterSalutation = null;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this._rawInput = string.Empty;
            this.Gender = Gender.unknown;
            this.Language = Language.Unknown;
            this.Title = string.Empty;
            this.TitleList = new ObservableCollection<TitleSalutation>();
            this._salutation = string.Empty;
            this._contactSalutation = string.Empty;
            this.NameErrorMessage = string.Empty;
        }

        #region Delete

        /// <summary>
        /// Löscht die selektierte Person aus der Tabelle
        /// </summary>
        private ICommand m_deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (m_deleteCommand == null)
                {
                    m_deleteCommand = new RelayCommand(param => Delete((Contact)param), param => true);
                }
                return m_deleteCommand;
            }
        }

        private void Delete(Contact result)
        {
            this.Contacts.Remove(result);
        }
        #endregion


        #endregion
    }
}
