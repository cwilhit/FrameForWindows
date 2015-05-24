using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame_for_WP.Core
{
    public interface INavigationService
    {
        void Navigate(Uri sourcePageType);
        void Navigate(Type sourcePageType, object parameter);
        void GoBack();
    }
}
