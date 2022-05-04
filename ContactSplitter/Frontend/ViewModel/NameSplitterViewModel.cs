using ContactSplitter.Frontend.Core;
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

        public ICommand SubmitTextCommand => new RelayCommand(x => this.OnSplitButtonClicked());

        private void OnSplitButtonClicked()
        {
            TextOutputString = this.TextInputString;
        }

        private void OnTextInputChanged()
        {
            TextOutputString = this.TextInputString;
        }


    }
}
