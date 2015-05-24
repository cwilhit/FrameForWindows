using Frame_for_WP.Core;
using Frame_for_WP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Frame_for_WP.ViewModels
{
    public class MediaFeedViewModel : ViewModelBase
    {
        private const string BaseUrl = "http://1-dot-august-clover-86805.appspot.com";

        public RelayCommand<PivotItemEventArgs> LoadPivotItemAppBar
        {
            get;
            private set;
        }

        public RelayCommand OpenCameraCommand
        {
            get;
            private set;
        }

        private ObservableCollection<MediaContent> contentFeed;
        public ObservableCollection<MediaContent> ContentFeed
        {
            get { return contentFeed; }
            set
            {
                contentFeed = value;
                RaisePropertyChanged("ContentFeed");
            }
        }

        private ApplicationBar _appBar = new ApplicationBar();
        public ApplicationBar AppBar
        {
            get { return _appBar; }
            set
            {
                _appBar = value;
                RaisePropertyChanged(() => AppBar);
            }
        }

        private RestClient client;
        private readonly NavigationService navigationService;

        public MediaFeedViewModel(INavigationService navigationService)
        {
            this.navigationService = (NavigationService)navigationService;

            contentFeed = new ObservableCollection<MediaContent>();
            client = new RestClient(BaseUrl);

            mediaFeedAppBarSetup();

            LoadPivotItemAppBar = new RelayCommand<PivotItemEventArgs>((s) => SetUpAppBar(s));
            OpenCameraCommand = new RelayCommand(() => onOpenCamera());
        }

        private void onOpenCamera()
        {
            navigationService.Navigate(new Uri("/Main/Lifting.xaml", UriKind.Relative));
        }

        private void mediaFeedAppBarSetup()
        {
            AppBar.IsVisible = true;
            ApplicationBarIconButton settingsButton = new ApplicationBarIconButton();
            settingsButton.IconUri = new Uri("/Assets/AppBar/refresh.png", UriKind.Relative);
            settingsButton.Text = "refresh";
            AppBar.Buttons.Add(settingsButton);
            settingsButton.Click += new EventHandler(refreshMediaFeed);
        }

        private void otherFeedAppBarSetup()
        {
            AppBar.IsVisible = true;
            ApplicationBarIconButton settingsButton = new ApplicationBarIconButton();
            settingsButton.IconUri = new Uri("/Assets/AppBar/refresh.png", UriKind.Relative);
            settingsButton.Text = "refresh";
            AppBar.Buttons.Add(settingsButton);
            settingsButton.Click += new EventHandler(refreshMediaFeed);
        }

        private void postAppBarSetup()
        {
            AppBar.IsVisible = false;
        }

        private void SetUpAppBar(PivotItemEventArgs arg)
        {
            while (AppBar.Buttons.Count > 0)
            {
                AppBar.Buttons.RemoveAt(0);
            }

            string header = arg.Item.Header.ToString();
            switch (header)
            {
                case "home":
                    mediaFeedAppBarSetup();
                    break;
                case "other":
                    otherFeedAppBarSetup();
                    break;
                case "post":
                    postAppBarSetup();
                    break;
            }
        }

        Task refreshContent = null;

        private void refreshMediaFeed(object sender, EventArgs e)
        {
            this.refreshContent = awaitMediaContent();
        }

        private async Task awaitMediaContent()
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Get";
            request.RequestFormat = DataFormat.Json;

            string query = JSONMessage.getPosts(1, "", 0, 0, 0);

            request.AddBody(query);

            //Returns a list of the new media content
            List<MediaContent> newContent = await getMediaContent<List<MediaContent>>(request);
            updateContent(newContent);
        }

        private void updateContent(List<MediaContent> newContent)
        {
            if (newContent == null)
            {
                MessageBox.Show("No new content.");
                return;
            }
        }

        private Task<T> getMediaContent<T>(RestRequest request) where T : new()
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            client.ExecuteAsync<T>(request, (response) => taskCompletionSource.SetResult(response.Data));
            return taskCompletionSource.Task;
        }
    }
}
