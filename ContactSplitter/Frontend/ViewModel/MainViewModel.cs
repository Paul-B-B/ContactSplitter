using ContactSplitter.Frontend.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Frontend.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand Page2ViewCommand { get; set; }

        public NameSplitterViewModel NameSplitterVM { get; set; }

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
            NameSplitterVM = new NameSplitterViewModel();

            CurrentView = NameSplitterVM;


            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = NameSplitterVM;
            });


        }

    }
}
