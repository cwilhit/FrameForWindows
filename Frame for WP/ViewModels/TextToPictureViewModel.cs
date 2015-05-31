using Frame_for_WP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Devices;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;
using System.IO;

namespace Frame_for_WP.ViewModels
{
    public class TextToPictureViewModel : ViewModelBase
    {
        public RelayCommand<CancelEventArgs> BackKeyPressCommand
        {
            get;
            private set;
        }

        public RelayCommand TapBackground
        {
            get;
            private set;
        }

        public RelayCommand TapText
        {
            get;
            private set;
        }

        private Brush textColor;
        public Brush TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                RaisePropertyChanged();
            }
        }

        private Brush backgroundColor;
        public Brush BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                RaisePropertyChanged();
            }
        }

        private string tags;

        private string toConvert = "";
        public string ToConvert
        {
            get { return toConvert; }
            set 
            {
                if (CharsLeft > 0)
                {
                    toConvert = value;
                    RaisePropertyChanged("ToConvert");
                }
            }
        }
        private int charsLeft;
        public int CharsLeft
        {
            get 
            {
                charsLeft = 100 - toConvert.Length;
                return charsLeft;
            }
        }

        private BitmapImage prev;
        public BitmapImage Preview
        {
            get { return prev; }
            private set
            {
                prev = value;
                RaisePropertyChanged();
            }
        }

        private List<SimpleFontItem> fonts;
        public List<SimpleFontItem> Fonts
        {
            get { return fonts; }
        }

        private SimpleFontItem desiredFont;
        public SimpleFontItem DesiredFont
        {
            get { return desiredFont; }
            set
            {
                desiredFont = value;
                RaisePropertyChanged();
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

        private bool isPreviewing = false;
        private bool showingMessageBox = false;

        private readonly NavigationService navigationService;

        public TextToPictureViewModel(INavigationService navigationService)
        {
            this.navigationService = (NavigationService)navigationService;

            TapBackground = new RelayCommand(() => DisplayBackgroundColorPicker());
            TapText = new RelayCommand(() => DisplayTextColorPicker());
            BackKeyPressCommand = new RelayCommand<CancelEventArgs>((x) => InterceptBackKey(x));
            BackgroundColor = new SolidColorBrush(Colors.Black);
            TextColor = new SolidColorBrush(Colors.White);

            fonts = new List<SimpleFontItem>();
            fonts.Add(new SimpleFontItem("Verdana"));
            fonts.Add(new SimpleFontItem("Times New Roman"));
            fonts.Add(new SimpleFontItem("Segoe WP"));
            fonts.Add(new SimpleFontItem("Jokerman"));
            fonts.Add(new SimpleFontItem("Broadway"));
            fonts.Add(new SimpleFontItem("MS Gothic"));
            fonts.Add(new SimpleFontItem("Arial"));

            DesiredFont = fonts[0];

            SetUpAppBar();
        }



        private void SetUpAppBar()
        {
            while (AppBar.Buttons.Count > 0)
            {
                AppBar.Buttons.RemoveAt(0);
            }

            if (isPreviewing)
            {
                ApplicationBarIconButton postButton = new ApplicationBarIconButton();
                postButton.IconUri = new Uri("/Assets/AppBar/appbar.arrow.right.png", UriKind.Relative);
                postButton.Text = "preview";
                AppBar.Buttons.Add(postButton);
                postButton.Click += new EventHandler(postPicture);
            }
            else
            {
                ApplicationBarIconButton tagsButton = new ApplicationBarIconButton();
                tagsButton.IconUri = new Uri("/Assets/AppBar/appbar.tag.png", UriKind.Relative);
                tagsButton.Text = "add tags";
                AppBar.Buttons.Add(tagsButton);
                tagsButton.Click += new EventHandler(addTags);

                ApplicationBarIconButton previewButton = new ApplicationBarIconButton();
                previewButton.IconUri = new Uri("/Assets/AppBar/appbar.arrow.right.png", UriKind.Relative);
                previewButton.Text = "preview";
                AppBar.Buttons.Add(previewButton);
                previewButton.Click += new EventHandler(previewConversion);
            }
        }

        private void postPicture(object sender, EventArgs e)
        {
        }

        private void addTags(object sender, EventArgs e)
        {
            StackPanel panel = new StackPanel();
            TextBlock label = new TextBlock();
            label.Text = "Insert tags separated by a ',' to allow for users to search for content with similar tags.";
            label.Margin = new Thickness(10, 10, 10, 10);
            label.TextWrapping = TextWrapping.Wrap;
            TextBox input = new TextBox();
            InputScope inputScope = new InputScope();
            InputScopeName inputScopeName = new InputScopeName();
            inputScopeName.NameValue = InputScopeNameValue.AlphanumericFullWidth;
            inputScope.Names.Add(inputScopeName);
            input.InputScope = inputScope;
            panel.Children.Add(label);
            panel.Children.Add(input);

            showingMessageBox = true;

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Title = "Add tags",
                Content = panel,
                LeftButtonContent = "ok",
                RightButtonContent = "cancel",
            };

            messageBox.Dismissing += (s1, e1) =>
            {
            };

            messageBox.Dismissed += (s2, e2) =>
            {
                showingMessageBox = false;
                switch (e2.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        {
                            tags = input.Text;
                            break;
                        }

                    case CustomMessageBoxResult.RightButton:
                    case CustomMessageBoxResult.None:
                        // Do nothing
                        break;
                    default:
                        break;
                }
            };

            messageBox.Show();
        }

        private void DisplayBackgroundColorPicker()
        {
            StackPanel panel = new StackPanel();
            ColorPicker picker = new ColorPicker();
            SolidColorBrush b = (SolidColorBrush)BackgroundColor;
            picker.Margin = new Thickness(10, 10, 10, 10);
            picker.Height = (App.Current.Host.Content.ActualHeight / 2);
            picker.Color = b.Color;
            panel.Children.Add(picker);

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Title = "Background Color",
                Content = panel,
                LeftButtonContent = "ok",
                RightButtonContent = "cancel",
            };

            messageBox.Dismissing += (s1, e1) =>
            {
            };

            messageBox.Dismissed += (s2, e2) =>
            {
                switch (e2.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        {
                            b.Color = picker.Color;
                            break;
                        }

                    case CustomMessageBoxResult.RightButton:
                    case CustomMessageBoxResult.None:
                        // Do nothing
                        break;
                    default:
                        break;
                }
            };

            messageBox.Show();
        }

        private void DisplayTextColorPicker()
        {
            StackPanel panel = new StackPanel();
            ColorPicker picker = new ColorPicker();
            picker.Margin = new Thickness(10, 10, 10, 10);
            picker.Height = (App.Current.Host.Content.ActualHeight / 2);
            SolidColorBrush b = (SolidColorBrush)TextColor;
            picker.Color = b.Color;
            panel.Children.Add(picker);

            showingMessageBox = true;

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Title = "Text Color",
                Content = panel,
                LeftButtonContent = "ok",
                RightButtonContent = "cancel",
            };

            messageBox.Dismissing += (s1, e1) =>
            {
            };

            messageBox.Dismissed += (s2, e2) =>
            {
                showingMessageBox = false;
                switch (e2.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        {
                            b.Color = picker.Color;
                            break;
                        }

                    case CustomMessageBoxResult.RightButton:
                    case CustomMessageBoxResult.None:
                        // Do nothing
                        break;
                    default:
                        break;
                }
            };

            messageBox.Show();
        }

        private void previewConversion(object sender, EventArgs e)
        {
            navigationService.NavigateTo("PreviewConversionPage");
            isPreviewing = true;
            SetUpAppBar();
            makePreview();
        }

        private void makePreview()
        {
            WriteableBitmap preview = new WriteableBitmap(600, 600);
            TextBlock temp = new TextBlock();
            temp.Text = toConvert;
            temp.FontFamily = desiredFont.Font;
            temp.Foreground = textColor;
            Canvas background = new Canvas();
            background.Background = backgroundColor;
            preview.Render(background, null); //render the background color
            preview.Render(temp, null); //Render the text over that
            preview.Invalidate();


            using(MemoryStream ms = new MemoryStream())
            {
                preview.SaveJpeg(ms, 600, 600, 0, 100);
                Preview = new BitmapImage();
                Preview.SetSource(ms);
                
            }
        }

        private void InterceptBackKey(CancelEventArgs x)
        {
            if(showingMessageBox)
            {
                x.Cancel = true;
                return;
            }

            if (isPreviewing)
            {
                isPreviewing = false;
                //Clean up the preview image
                navigationService.GoBack();
                SetUpAppBar();
            }
        }
    }
}
