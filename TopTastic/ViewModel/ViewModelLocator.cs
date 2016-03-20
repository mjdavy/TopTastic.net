using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;

namespace TopTastic.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SearchViewModel Search
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchViewModel>();
            }
        }

        static ViewModelLocator()
        {
            RegisterViewModels();
            RegisterMessages();
        }

        static void RegisterViewModels()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
        }

        static void RegisterMessages()
        {
            var main = ServiceLocator.Current.GetInstance<MainViewModel>();
            Messenger.Default.Register<SearchMessage>(main, 1, main.OnSearch);

            var search = ServiceLocator.Current.GetInstance<SearchViewModel>();
            Messenger.Default.Register<SearchMessage>(search, 0, search.Open);
        }

    }
}
