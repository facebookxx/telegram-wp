﻿// 
// This is the source code of Telegram for Windows Phone v. 3.x.x.
// It is licensed under GNU GPL v. 2 or later.
// You should have received a copy of the license in this archive (see LICENSE).
// 
// Copyright Evgeny Nadymov, 2013-present.
// 
using System;
using System.Windows;
using System.Windows.Media;
using Telegram.Api.TL;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TelegramClient.Views.Controls
{
    public partial class MediaVideoControl : IMediaControl
    {
        public static readonly DependencyProperty TTLParamsProperty = DependencyProperty.Register(
            "TTLParams", typeof(TTLParams), typeof(MediaVideoControl), new PropertyMetadata(default(TTLParams), OnTTLParamsChanged));

        private static void OnTTLParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                if (control._secretPhotoPlaceholder != null)
                {
                    control._secretPhotoPlaceholder.TTLParams = e.NewValue as TTLParams;
                }
            }
        }

        public TTLParams TTLParams
        {
            get { return (TTLParams)GetValue(TTLParamsProperty); }
            set { SetValue(TTLParamsProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof(Stretch), typeof(MediaVideoControl), new PropertyMetadata(default(Stretch), OnStretchChanged));

        private static void OnStretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                control.Video.Stretch = (Stretch) e.NewValue;
            }
        }

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(MediaVideoControl), new PropertyMetadata(default(bool), OnIsSelectedChanged));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                control.SelectionBorder.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty ProgressScaleProperty = DependencyProperty.Register(
            "ProgressScale", typeof(double), typeof(MediaVideoControl), new PropertyMetadata(default(double), OnProgressScaleChanged));

        private static void OnProgressScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                control.Progress.RenderTransform = new ScaleTransform { ScaleX = (double)e.NewValue, ScaleY = (double)e.NewValue };
            }
        }

        public double ProgressScale
        {
            get { return (double)GetValue(ProgressScaleProperty); }
            set { SetValue(ProgressScaleProperty, value); }
        }

        public static readonly DependencyProperty MediaProperty = DependencyProperty.Register(
            "Media", typeof(TLObject), typeof(MediaVideoControl), new PropertyMetadata(default(TLObject), OnMediaChanged));

        private static void OnMediaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                //System.Diagnostics.Debug.WriteLine("  Change Media old={0} new={1}", e.OldValue != null ? e.OldValue.GetHashCode() : 0, e.NewValue != null ? e.NewValue.GetHashCode() : 0);
                //control.DownloadIcon.Visibility = Visibility.Visible;
                var mediaPhoto = e.NewValue as TLMessageMediaDocument70;
                if (mediaPhoto != null)
                {
                    if (mediaPhoto.TTLSeconds != null)
                    {
                        control.GoToSecretMediaState();
                    }
                    else
                    {
                        control.GoToNormalMediaState();
                    }
                }

                var decryptedMedia = e.NewValue as TLDecryptedMessageMediaBase;
                if (decryptedMedia != null)
                {
                    if (decryptedMedia.TTLSeconds != null && decryptedMedia.TTLSeconds.Value > 0)
                    {
                        control.GoToSecretMediaState();
                    }
                    else
                    {
                        control.GoToNormalMediaState();
                    }
                }
            }
        }

        private void GoToNormalMediaState()
        {
            Root.Children.Remove(Progress);
            if (_secretPhotoPlaceholder != null) Root.Children.Remove(_secretPhotoPlaceholder);

            Root.Children.Insert(1, Progress);
        }

        private SecretPhotoPlaceholder _secretPhotoPlaceholder;

        private void GoToSecretMediaState()
        {
            Root.Children.Remove(Progress);
            if (_secretPhotoPlaceholder != null) Root.Children.Remove(_secretPhotoPlaceholder);

            _secretPhotoPlaceholder = _secretPhotoPlaceholder ?? new SecretPhotoPlaceholder
            {
                IsHitTestVisible = false,
                ShowHint = false,
                TTLParams = TTLParams,
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new ScaleTransform { ScaleX = ProgressScale, ScaleY = ProgressScale },
                Margin = new Thickness(-12.0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Root.Children.Insert(1, _secretPhotoPlaceholder);
        }

        public TLObject Media
        {
            get { return (TLObject)GetValue(MediaProperty); }
            set { SetValue(MediaProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(MediaVideoControl), new PropertyMetadata(default(ImageSource), OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                control.Video.Source = e.NewValue as ImageSource;
            }
        }

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty DownloadIconVisibilityProperty = DependencyProperty.Register(
            "DownloadIconVisibility", typeof(Visibility), typeof(MediaVideoControl), new PropertyMetadata(default(Visibility), OnDownloadIconVisibilityChanged));

        private static void OnDownloadIconVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                control.Progress.DownloadIconVisibility = (Visibility) e.NewValue;
            }
        }

        public Visibility DownloadIconVisibility
        {
            get { return (Visibility) GetValue(DownloadIconVisibilityProperty); }
            set { SetValue(DownloadIconVisibilityProperty, value); }
        }

        public static readonly DependencyProperty DownloadingProgressProperty = DependencyProperty.Register(
            "DownloadingProgress", typeof(double), typeof(MediaVideoControl), new PropertyMetadata(default(double), OnProgressChanged));

        public double DownloadingProgress
        {
            get { return (double) GetValue(DownloadingProgressProperty); }
            set { SetValue(DownloadingProgressProperty, value); }
        }

        public static readonly DependencyProperty UploadingProgressProperty = DependencyProperty.Register(
            "UploadingProgress", typeof(double), typeof(MediaVideoControl), new PropertyMetadata(default(double), OnProgressChanged));

        public double UploadingProgress
        {
            get { return (double) GetValue(UploadingProgressProperty); }
            set { SetValue(UploadingProgressProperty, value); }
        }

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MediaVideoControl;
            if (control != null)
            {
                var progress = (double)e.NewValue;

                control.Progress.DownloadingProgress = progress;
                if (control._secretPhotoPlaceholder != null)
                {
                    control._secretPhotoPlaceholder.DownloadingProgress = progress;
                }
            }
        }

        public MediaVideoControl()
        {
            InitializeComponent();
        }

        public event EventHandler CancelDownloading;

        protected virtual void RaiseCancelDownloading()
        {
            var handler = CancelDownloading;
            if (handler != null) handler(this, System.EventArgs.Empty);
        }

        public event EventHandler CancelUploading;

        protected virtual void RaiseCancelUploading()
        {
            var handler = CancelUploading;
            if (handler != null) handler(this, System.EventArgs.Empty);
        }

        private void LayoutRoot_OnTap(object sender, GestureEventArgs e)
        {
            if (DownloadingProgress > 0.0 && DownloadingProgress < 1.0)
            {
                e.Handled = true;
                RaiseCancelDownloading();
            }
            else if (UploadingProgress > 0.0 && UploadingProgress < 1.0)
            {
                e.Handled = true;
                RaiseCancelUploading();
            }
        }
    }
}
