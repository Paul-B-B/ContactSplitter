using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Services;
using ContactSplitter.Frontend.Core;
using ContactSplitter.Shared.DataClass;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ContactSplitter.Frontend.ViewModel
{
    class NameSplitterViewModel : ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _textInputString;
        public string TextInputString
        {
            get { return _textInputString; }
            set { Update(ref _textInputString, value); }
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

        private string _salutation;
        public string Salutation
        {
            get { return _salutation; }
            set { Update(ref _salutation, value); }
        }

        private Geschlecht _gender;
        public Geschlecht Gender
        {
            get { return _gender; }
            set { Update(ref _gender, value); }
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


        public ObservableCollection<Person> Persons { get; private set; }

        private readonly INameSplitterModel _nameSplitterModel;

        public NameSplitterViewModel(INameSplitterModel nameSplitterModel)
        {
            this._nameSplitterModel = nameSplitterModel;
            this.ClearBoxes();
            Persons = CreateData();
            this.TitleList = new ObservableCollection<string>();
        }

        public ICommand SubmitTextCommand => new RelayCommand(x => this.OnSplitButtonClicked());

        public ICommand AddContactCommand => new RelayCommand(x => this.OnAddContactClicked());

        public ICommand AddTitleCommand => new RelayCommand(x => this.OnAddTitleClicked());

        private void OnAddTitleClicked()
        {
            if (this.NewTitle == string.Empty) return;
            this.TitleList.Add(this.NewTitle);
            this.NewTitle = string.Empty;
        }

        private void OnSplitButtonClicked()
        {
            this.ClearBoxes();
            var output = this._nameSplitterModel.GetSplitContact(this.TextInputString);
            this.FirstName = output.Vorname;
            this.LastName = output.Nachname;
            this.Gender = output.Geschlecht;
            this.Salutation = output.Anrede;
            if (output.ListeAllerTitel != null)
            {
                foreach (var item in output.ListeAllerTitel)
                {
                    if (item != null) this.TitleList.Add(item.Anrede);
                }
            }
        }

        private void OnAddContactClicked()
        {
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
            person.Salutation = this.Salutation;
            Persons.Add(person);
            this.ClearBoxes();
        }

        private void ClearBoxes()
        {
            this.TextInputString = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Gender = Geschlecht.unbekannt;
            this.Salutation = string.Empty;
            this.Title = string.Empty;
            this.TitleList = new ObservableCollection<string>();
        }

        private void OnSelectedPersonChanged()
        {
            if (this.SelectedPerson == null) return;
            switch (this.SelectedPerson.Gender) 
            {
                case Geschlecht.m:
                    this.GermanSalutation = "Sehr geehrter Herr " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
                    this.EnglishSalutation = "Dear Mr. " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
                    this.SpanishSalutation = "Estimado señor " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
                    break;
                case Geschlecht.w:
                    this.GermanSalutation = "Sehr geehrte Frau " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
                    this.EnglishSalutation = "Dear Ms. " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
                    this.SpanishSalutation = "Estimada señora " + this.SelectedPerson.Title + " " + this.SelectedPerson.LastName;
                    break;
                default:
                    this.GermanSalutation = "Sehr geehrte Damen und Herren";
                    this.EnglishSalutation = "Dear Ladies and Gentleman";
                    this.SpanishSalutation = "Estimadas señoras y señores";
                    break;
            }
        }

        private static ObservableCollection<Person> CreateData()
        {
            return new ObservableCollection<Person>
            {
                new Person
                {
                    Salutation = "Herr",
                    Title = "Prof. Dr.",
                    FirstName = "Kevin",
                    LastName = "Kudlik",
                    Gender = Geschlecht.m
                },
                new Person
                {
                    Salutation = "Frau",
                    Title = "Dr.",
                    FirstName = "Charlotte",
                    LastName = "Stöffler",
                    Gender = Geschlecht.w
                },
                new Person
                {
                    Salutation = "Herr",
                    Title = "Dr. Dr.",
                    FirstName = "Paul-Benedict",
                    LastName = "Burkard",
                    Gender = Geschlecht.m
                }
            };
        }
    }
}
