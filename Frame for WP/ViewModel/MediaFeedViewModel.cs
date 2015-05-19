using Frame_for_WP.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame_for_WP.ViewModels
{
    public class MediaFeedViewModel : ViewModelBase
    {
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

        public MediaFeedViewModel()
        {
            contentFeed = new ObservableCollection<MediaContent>();
        }
    }
}
