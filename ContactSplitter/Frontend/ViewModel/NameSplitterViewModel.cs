using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Services;
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

        private Geschlecht _gender;
        public Geschlecht Gender
        {
            get { return _gender; }
            set { Update(ref _gender, value); }
        }

        private Sprache _language;
        public Sprache Language
        {
            get { return _language; }
            set { Update(ref _language, value); }
        }

        private ObservableCollection<string> _titleList;
        public ObservableCollection<string> TitleList
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

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                Update(ref _selectedPerson, value);
                this.OnSelectedPersonChanged();
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

        private string _fullSalutation;
        public string FullSalutation
        {
            get { return _fullSalutation; }
            set { Update(ref _fullSalutation, value); }
        }

        private string _nameErrorMessage;

        public string NameErrorMessage
        {
            get { return _nameErrorMessage; }
            set { Update(ref _nameErrorMessage, value); }
        }

        private string _salutation;
        private string _letterSalutation;

        #endregion

        public ObservableCollection<Person> Persons { get; private set; }

        private readonly INameSplitterModel _nameSplitterModel;

        #region Konstruktor
        public NameSplitterViewModel(INameSplitterModel nameSplitterModel)
        {
            this._nameSplitterModel = nameSplitterModel;
            this.ClearBoxes();
            this.Persons = new ObservableCollection<Person>();
            this.TitleList = new ObservableCollection<string>();
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
            this.TitleList.Add(this.NewTitle);
            if (this.Recognize)
            {
                this._nameSplitterModel.AddTitle(this.NewTitle);
            }
            this.NewTitle = string.Empty;
        }

        /// <summary>
        /// Formatiert die Eingabe und gibt diese in bearbeitbaren Feldern aus
        /// </summary>
        private void OnTextInputChanged()
        {
            if(this.TextInputString == string.Empty) return;
            try
            {
                this.ClearBoxes();
                var output = this._nameSplitterModel.GetSplitContact(this.TextInputString);
                this.FirstName = output.Vorname;
                this.LastName = output.Nachname;
                this.Gender = output.Geschlecht;
                this.Language = output.Sprache;
                this._salutation = output.Anrede;
                this._letterSalutation = output.Briefanrede;
                if (output.ListeAllerTitel != null)
                {
                    foreach (var item in output.ListeAllerTitel)
                    {
                        if (item != null) this.TitleList.Add(item.Titel);
                    }
                }
            }
            catch (Exception ex)
            { 
                NameErrorMessage = "The following Error occured: " + ex.Message;
            }
        }

        /// <summary>
        /// Fügt die formatierten Einträge in die Persons Liste hinzu, sobald der Hinzufügen-Button betätigt wird
        /// </summary>
        private void OnAddContactClicked()
        {
            if(this.LastName == string.Empty) return;
            Person person = new Person();
            var titles = string.Empty;
            foreach (var title in this.TitleList)
            {
                titles = titles += $"{title} ";
            }
            person.Title = titles;
            person.LastName = this.LastName;
            person.FirstName = this.FirstName;
            person.Gender = this.Gender;
            person.Language = this.Language;
            person.Salutation = this._salutation;
            person.FullSalutation = this._letterSalutation;
            Persons.Add(person);
            this.ClearBoxes();
            this.TextInputString = string.Empty;
        }

        /// <summary>
        /// Kopiert die Briefanrede der aktuell ausgewählten Person
        /// </summary>
        private void OnCopySalutationClicked()
        {
            if(this.SelectedPerson == null || this.SelectedPerson.FullSalutation == null) return;
            Clipboard.SetText(this.SelectedPerson.FullSalutation);
        }

        /// <summary>
        /// Hilfsmethode um alle Eingabefelder zurückzusetzen
        /// /// </summary>
        private void ClearBoxes()
        {
            //this.TextInputString = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Gender = Geschlecht.unbekannt;
            this.Language = Sprache.Unbekannt;
            this.Title = string.Empty;
            this.TitleList = new ObservableCollection<string>();
            this._salutation = string.Empty;
            this._letterSalutation = string.Empty;
            this.NameErrorMessage = string.Empty;
        }

        #region Delete
        private bool CanDelete
        {
            get { return SelectedPerson != null; }
        }

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
                    m_deleteCommand = new RelayCommand(param => Delete((Person)param), param => CanDelete);
                }
                return m_deleteCommand;
            }
        }

        private void Delete(Person result)
        {
            this.Persons.Remove(result);
        }
        #endregion

        private void OnSelectedPersonChanged()
        {
            if (this.SelectedPerson == null) return;
            this.FullSalutation = this.SelectedPerson.FullSalutation;
            //switch (this.SelectedPerson.Gender) 
            //{
            //    case Geschlecht.m:
            //        this.GermanSalutation = "Sehr geehrter Herr " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
            //        this.EnglishSalutation = "Dear Mr. " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
            //        this.SpanishSalutation = "Estimado señor " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
            //        break;
            //    case Geschlecht.w:
            //        this.GermanSalutation = "Sehr geehrte Frau " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
            //        this.EnglishSalutation = "Dear Ms. " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
            //        this.SpanishSalutation = "Estimada señora " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
            //        break;
            //    default:
            //        this.GermanSalutation = "Sehr geehrte Damen und Herren";
            //        this.EnglishSalutation = "Dear Ladies and Gentleman";
            //        this.SpanishSalutation = "Estimadas señoras y señores";
            //        break;
            //}
        }

        //private static ObservableCollection<Person> CreateData()
        //{
        //    return new ObservableCollection<Person>
        //    {
        //        new Person
        //        {
        //            Salutation = "Herr",
        //            Title = "Prof. Dr.",
        //            FirstName = "Kevin",
        //            LastName = "Kudlik",
        //            Gender = Geschlecht.m
        //        },
        //        new Person
        //        {
        //            Salutation = "Frau",
        //            Title = "Dr.",
        //            FirstName = "Charlotte",
        //            LastName = "Stöffler",
        //            Gender = Geschlecht.w
        //        },
        //        new Person
        //        {
        //            Salutation = "Herr",
        //            Title = "Dr. Dr.",
        //            FirstName = "Paul-Benedict",
        //            LastName = "Burkard",
        //            Gender = Geschlecht.m
        //        }
        //    };
        //}
        #endregion
    }
}
