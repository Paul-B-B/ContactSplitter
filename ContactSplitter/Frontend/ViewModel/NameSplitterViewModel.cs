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

        private KontaktParser _kontaktParser; 

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

        private string _vorname;
        public string Vorname
        {
            get { return _vorname; }
            set { Update(ref _vorname, value); }
        }

        private string _nachname;
        public string Nachname
        {
            get { return _nachname; }
            set { Update(ref _nachname, value); }
        }

        private string _titel;
        public string Titel
        {
            get { return _titel; }
            set { Update(ref _titel, value); }
        }

        private Geschlecht _geschlecht;
        public Geschlecht Geschlecht
        {
            get { return _geschlecht; }
            set { Update(ref _geschlecht, value); }
        }

        public ObservableCollection<Person> Persons { get; private set; }

        public NameSplitterViewModel()
        {
            Persons = CreateData();
            _kontaktParser = new KontaktParser(@"Backend\Data\");
        }

        public ICommand SubmitTextCommand => new RelayCommand(x => this.OnSplitButtonClicked());

        private void OnSplitButtonClicked()
        {
            var input = new SplitContactRequest() { UserInput = TextInputString };
            var output = _kontaktParser.ParseKontakt(input);
            Person person = new Person();
            person.Title = this.Titel;
            person.LastName = this.Nachname;
            person.FirstName = this.Vorname;
            person.Gender = this.Geschlecht;
            person.Salutation = output.Anrede;
            Persons.Add(person);
            this.TextInputString = "";
        }

        private void OnTextInputChanged()
        {
            var input = new SplitContactRequest() { UserInput = TextInputString };
            var output = _kontaktParser.ParseKontakt(input);
            this.Vorname = output.Vorname;
            this.Nachname = output.Nachname;
            this.Geschlecht = output.Geschlecht;
            this.Titel = output.AlleTitel;
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
