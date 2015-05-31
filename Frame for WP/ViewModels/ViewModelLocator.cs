/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Frame_for_WP"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Frame_for_WP.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;

namespace Frame_for_WP.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var navService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navService);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MediaFeedViewModel>();
            SimpleIoc.Default.Register<MediaContentPostViewModel>();
            SimpleIoc.Default.Register<TextToPictureViewModel>();
        }

        private INavigationService CreateNavigationService()
        {
            var navService = new NavigationService();

            navService.Configure("TextToPicturePage", new Uri("/View/TextToPicturePage.xaml", UriKind.Relative));
            navService.Configure("PreviewConversionPage", new Uri("/View/PreviewConversionPage.xaml", UriKind.Relative));
            navService.Configure("MediaContentPostPage", new Uri("/View/MediaContentPostPage.xaml", UriKind.Relative));
            navService.Configure("MediaFeedPage", new Uri("/View/MainPage.xaml", UriKind.Relative));

            return navService;
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MediaFeedViewModel MediaFeedVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MediaFeedViewModel>();
            }
        }

        public MediaContentPostViewModel MediaContentPostVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MediaContentPostViewModel>();
            }
        }

        public TextToPictureViewModel TextToPictureVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TextToPictureViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}