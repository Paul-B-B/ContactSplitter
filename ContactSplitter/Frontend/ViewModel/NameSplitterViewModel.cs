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
            set 
            { 
                Update(ref _textInputString, value);
                this.OnTextInputChanged();
            }
        }

        private string _textOutputString;
        public string TextOutputString
        {
            get { return _textOutputString; }
            set { Update(ref _textOutputString, value); }
        }

        public ObservableCollection<Person> Persons { get; private set; }

        public NameSplitterViewModel()
        {
            Persons = CreateData();
        }

        public ICommand SubmitTextCommand => new RelayCommand(x => this.OnSplitButtonClicked());

        private void OnSplitButtonClicked()
        {
            TextOutputString = this.TextInputString;
        }

        private void OnTextInputChanged()
        {
            TextOutputString = this.TextInputString;
        }

        private static ObservableCollection<Person> CreateData()
        {
            return new ObservableCollection<Person>
            {
                new Person
                {
                    Salutation = "Herr",
                    Title = new List<string>(){"Prof.", "Dr."},
                    FirstName = "Kevin",
                    LastName = "Kudlik",
                    Gender = Geschlecht.m
                },
                new Person
                {
                    Salutation = "Frau",
                    Title = new List<string>(){"Dr."},
                    FirstName = "Charlotte",
                    LastName = "Stöffler",
                    Gender = Geschlecht.w
                },
                new Person
                {
                    Salutation = "Herr",
                    Title = new List<string>(){"Dr.", "Dr."},
                    FirstName = "Paul-Benedict",
                    LastName = "Burkard",
                    Gender = Geschlecht.m
                }
            };
        }
    }
}
