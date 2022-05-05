using ContactSplitter.Backend.Services;
using ContactSplitter.Frontend.Core;

namespace ContactSplitter.Frontend.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand NameSplitterViewCommand { get; set; }

        public RelayCommand PhoneNumberSplitterViewCommand { get; set; }

        public NameSplitterViewModel NameSplitterVM { get; set; }

        public PhoneNumberSplitterViewModel PhoneNumberSplitterVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            var phoneNumberModel = new PhoneNumberSplitterModel();
            var nameSplitterModel = new NameSplitterModel();

            NameSplitterVM = new NameSplitterViewModel(nameSplitterModel);
            PhoneNumberSplitterVM = new PhoneNumberSplitterViewModel(phoneNumberModel);

            CurrentView = NameSplitterVM;


            NameSplitterViewCommand = new RelayCommand(o =>
            {
                CurrentView = NameSplitterVM;
            });

            PhoneNumberSplitterViewCommand = new RelayCommand(o =>
            {
                CurrentView = PhoneNumberSplitterVM;
            });


        }

    }
}
