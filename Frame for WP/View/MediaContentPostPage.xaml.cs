using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Frame_for_WP.ViewModel;
using Frame_for_WP.ViewModels;

namespace Frame_for_WP.View
{
    public partial class MediaContentPostPage : PhoneApplicationPage
    {
        public MediaContentPostPage()
        {
            InitializeComponent();
        }

        MediaContentPostViewModel viewModel
        {
            get
            {
                return this.DataContext as MediaContentPostViewModel;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            viewModel.openCamera();
        }
    }
}