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

namespace Frame_for_WP.ViewModels
{
    public class MediaContentPostViewModel : ViewModelBase
    {
        private int flashMode = 0;
        PhotoCamera camera;
        CameraType curType;
        private string tags = "";

        private Visibility shutterVisiblity;
        public Visibility ShutterVisibility
        {
            get { return shutterVisiblity; }
            set
            {
                shutterVisiblity = value;
                RaisePropertyChanged();
            }
        }

        private Visibility bracketVisibility;
        public Visibility BracketVisibility
        {
            get { return bracketVisibility; }
            set
            {
                bracketVisibility = value;
                RaisePropertyChanged();
            }
        }

        private TileBrush _previewVideoBrush;
        public TileBrush PreviewVideoBrush
        {
            get { return _previewVideoBrush; }
            set
            {
                if (Equals(value, _previewVideoBrush)) return;
                _previewVideoBrush = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<PivotItemEventArgs> LoadPivotItemAppBar
        {
            get;
            private set;
        }

        public RelayCommand ShutterCommand
        {
            get;
            private set;
        }

        public RelayCommand<CancelEventArgs> BackKeyPressCommand
        {
            get;
            private set;
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

        private readonly NavigationService navigationService;

        public MediaContentPostViewModel(INavigationService navigationService)
        {
            this.navigationService = (NavigationService)navigationService;
            ShutterCommand = new RelayCommand(() => ShutterButtonClick());
            BackKeyPressCommand = new RelayCommand<CancelEventArgs>((x) => InterceptBackKeyPress(x));

            SetUpAppBar();
            AppBar.Opacity = .65;
            ShutterVisibility = Visibility.Visible;
            BracketVisibility = Visibility.Collapsed;
        }

        private void InterceptBackKeyPress(CancelEventArgs x)
        {
            if (ShutterVisibility == Visibility.Collapsed)
            {
                //Toggle over to picture-taking mode. Show the shutter button and re-set up the appbar, open camera.
                x.Cancel = true;
                ShutterVisibility = Visibility.Visible;
                SetUpAppBar();

                openCamera();
            }
            else
            {
                //We are currently in picture-edit mode, so the user wants to return to the media feed. Release resources.
                releaseCamera();
            }
        }

        private void ShutterButtonClick()
        {
            if(camera != null)
            {
                try
                {
                    camera.CaptureImage();
                }
                catch(Exception e)
                {

                }
            }
        }

        public void openCamera()
        {
            if(PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true ||
                PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) == true)
            {
                //If there is a front-facing cam, open that. Otherwise open rear-facing camera
                if (PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing))
                {
                    camera = new PhotoCamera(CameraType.FrontFacing);
                    curType = CameraType.FrontFacing;
                }
                else
                {
                    camera = new PhotoCamera(CameraType.Primary);
                    curType = CameraType.Primary;
                }
            }
            else
            {
                //No camera. We shouldn't get here because having a camera is required. But just incase, lock the UI
            }

            // Event is fired when the PhotoCamera object has been initialized.
            camera.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(camera_initialized);

            // Event is fired when the capture sequence is complete and an image is available.
            camera.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(camera_captureImageAvailable);

            // Event is fired when the capture sequence is complete and a thumbnail image is available.
            camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camera_captureThumbnailAvailable);

            //Set the VideoBrush source to the camera.
            PreviewVideoBrush = new VideoBrush();
            VideoBrush vidBrush = (VideoBrush)PreviewVideoBrush;

            if(camera.CameraType == CameraType.FrontFacing)
                vidBrush.RelativeTransform = new CompositeTransform() { CenterX = .5, CenterY = .5, Rotation = -90 };
            else
                vidBrush.RelativeTransform = new CompositeTransform() { CenterX = .5, CenterY = .5, Rotation = 90 };
            vidBrush.SetSource(camera);
            PreviewVideoBrush = vidBrush;
        }

        private void toggleCamera()
        {
            if(curType == CameraType.Primary)
            {
                curType = CameraType.FrontFacing;
                camera = new PhotoCamera(CameraType.FrontFacing);
            }
            else
            {
                curType = CameraType.Primary;
                camera = new PhotoCamera(CameraType.Primary);
            }

            // Event is fired when the PhotoCamera object has been initialized.
            camera.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(camera_initialized);

            // Event is fired when the capture sequence is complete and an image is available.
            camera.CaptureImageAvailable += new EventHandler<Microsoft.Devices.ContentReadyEventArgs>(camera_captureImageAvailable);

            // Event is fired when the capture sequence is complete and a thumbnail image is available.
            camera.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(camera_captureThumbnailAvailable);

            //Set the VideoBrush source to the camera.
            PreviewVideoBrush = new VideoBrush();
            VideoBrush vidBrush = (VideoBrush)PreviewVideoBrush;

            if (camera.CameraType == CameraType.FrontFacing)
                vidBrush.RelativeTransform = new CompositeTransform() { CenterX = .5, CenterY = .5, Rotation = -90 };
            else
                vidBrush.RelativeTransform = new CompositeTransform() { CenterX = .5, CenterY = .5, Rotation = 90 };
            vidBrush.SetSource(camera);
            PreviewVideoBrush = vidBrush;
        }

        public void releaseCamera()
        {
            if(camera != null)
            {
                //dispose camera to free resources
                camera.Dispose();

                //release memory for garbage collection
                camera.Initialized -= camera_initialized;
                camera.CaptureImageAvailable -= camera_captureImageAvailable;
                camera.CaptureThumbnailAvailable -= camera_captureThumbnailAvailable;
            }
        }

        private void camera_captureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {
                    ShutterVisibility = Visibility.Collapsed;
                    SetUpAppBar();

                    releaseCamera();

                    PreviewVideoBrush = new ImageBrush();
                    ImageBrush imgBrush = (ImageBrush)PreviewVideoBrush;

                    BitmapImage img = new BitmapImage();
                    img.SetSource(e.ImageStream);
                    imgBrush.ImageSource = img;
                    if (camera.CameraType == CameraType.FrontFacing)
                        imgBrush.RelativeTransform = new CompositeTransform() { CenterX = .5, CenterY = .5, Rotation = -90 };
                    else
                        imgBrush.RelativeTransform = new CompositeTransform() { CenterX = .5, CenterY = .5, Rotation = 90 };
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void camera_captureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
        }

        private void camera_initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if(e.Succeeded)
            {
                System.Diagnostics.Debug.WriteLine("Camera initialized");
            }

        }

        private void SetUpAppBar()
        {
            while (AppBar.Buttons.Count > 0)
            {
                AppBar.Buttons.RemoveAt(0);
            }

            //Shutter visibility determines if we are in edit mode or not.
            //If the shutter is visible, then this means we are in picture-taking mode and not edit mode.
            if (ShutterVisibility == Visibility.Visible)
            {
                ApplicationBarIconButton changeCam = new ApplicationBarIconButton();
                changeCam.IconUri = new Uri("/Assets/AppBar/appbar.camera.switch.png", UriKind.Relative);
                changeCam.Text = "change cam";
                AppBar.Buttons.Add(changeCam);
                changeCam.IsEnabled = PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing);
                changeCam.Click += new EventHandler(toggleCamera);

                ApplicationBarIconButton flashButton = new ApplicationBarIconButton();
                flashButton.IconUri = new Uri("/Assets/AppBar/appbar.camera.flash.auto.png", UriKind.Relative);
                flashButton.Text = "auto flash";
                AppBar.Buttons.Add(flashButton);
                flashButton.Click += new EventHandler(toggleFlash);
            }
            else
            {
                ApplicationBarIconButton tagsButton = new ApplicationBarIconButton();
                tagsButton.IconUri = new Uri("/Assets/AppBar/appbar.tag.png", UriKind.Relative);
                tagsButton.Text = "add tags";
                AppBar.Buttons.Add(tagsButton);
                tagsButton.Click += new EventHandler(addTags);

                ApplicationBarIconButton postButton = new ApplicationBarIconButton();
                postButton.IconUri = new Uri("/Assets/AppBar/appbar.arrow.right.png", UriKind.Relative);
                postButton.Text = "post";
                AppBar.Buttons.Add(postButton);
                postButton.Click += new EventHandler(postPicture);
            }
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

        private void postPicture(object sender, EventArgs e)
        {
            //Send picture to the server.
            navigationService.GoBack();
            ShutterVisibility = Visibility.Visible;
            SetUpAppBar();
        }

        private void toggleCamera(object sender, EventArgs e)
        {
            releaseCamera();
            toggleCamera();
        }

        private void toggleFlash(object sender, EventArgs e)
        {
            try
            {
                ApplicationBarIconButton flashButton = sender as ApplicationBarIconButton;

                flashMode = (flashMode + 1) % 3;
                switch (flashMode)
                {
                    case 0:
                        flashButton.IconUri = new Uri("/Assets/AppBar/appbar.camera.flash.auto.png", UriKind.Relative);
                        flashButton.Text = "auto flash";
                        camera.FlashMode = FlashMode.Auto;
                        break;
                    case 1:
                        flashButton.IconUri = new Uri("/Assets/AppBar/appbar.camera.flash.off.png", UriKind.Relative);
                        flashButton.Text = "flash off";
                        camera.FlashMode = FlashMode.Off;
                        break;
                    case 2:
                        flashButton.IconUri = new Uri("/Assets/AppBar/appbar.camera.flash.png", UriKind.Relative);
                        flashButton.Text = "flash";
                        camera.FlashMode = FlashMode.On;
                        break;
                }
            }
            catch(Exception ex)
            {
                //Suppress exception. The only time we get exceptions are if the user clicks the button while
                //the camera is being set up.
            }
        }
    }
}
