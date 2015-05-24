using Frame_for_WP.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using System.Windows.Media;

namespace Frame_for_WP.ViewModels
{
    public class MediaContentPostViewModel : ViewModelBase
    {
        private int photoCounter = 0;
        PhotoCamera camera;

        private VideoBrush _previewVideoBrush;
        public VideoBrush PreviewVideoBrush
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

        public MediaContentPostViewModel()
        {
            LoadPivotItemAppBar = new RelayCommand<PivotItemEventArgs>((s) => SetUpAppBar(s));
        }

        private void openCamera()
        {
            if(PhotoCamera.IsCameraTypeSupported(CameraType.Primary) == true ||
                PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) == true)
            {
                //If there is a front-facing cam, open that. Otherwise open rear-facing camera
                if (PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing))
                {
                    camera = new PhotoCamera(CameraType.FrontFacing);
                }
                else
                    camera = new PhotoCamera(CameraType.Primary);
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
            PreviewVideoBrush.SetSource(camera);
        }

        private void releaseCamera()
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
            //Now that we have 
        }

        private void camera_captureThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void camera_initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if(e.Succeeded)
            {
                System.Diagnostics.Debug.WriteLine("Camera initialized");
            }
        }

        private void SetUpAppBar(PivotItemEventArgs arg)
        {
            while (AppBar.Buttons.Count > 0)
            {
                AppBar.Buttons.RemoveAt(0);
            }
        }
    }
}
