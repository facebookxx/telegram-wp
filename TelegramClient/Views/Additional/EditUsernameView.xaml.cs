﻿// 
// This is the source code of Telegram for Windows Phone v. 3.x.x.
// It is licensed under GNU GPL v. 2 or later.
// You should have received a copy of the license in this archive (see LICENSE).
// 
// Copyright Evgeny Nadymov, 2013-present.
// 
using System;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Phone.Shell;
using TelegramClient.Resources;
using TelegramClient.ViewModels.Additional;

namespace TelegramClient.Views.Additional
{
    public partial class EditUsernameView
    {
        public static readonly DependencyProperty AppBarStateProperty =
            DependencyProperty.Register("AppBarState", typeof(string), typeof(EditUsernameView), new PropertyMetadata(OnAppBarStateChanged));

        private static void OnAppBarStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var state = (string)e.NewValue;

            var view = (EditUsernameView)d;

            if (state == "Working")
            {
                view._doneButton.IsEnabled = false;
                view._cancelButton.IsEnabled = false;
            }
            else
            {
                view._doneButton.IsEnabled = true;
                view._cancelButton.IsEnabled = true;
            }
        }

        public string AppBarState
        {
            get { return (string)GetValue(AppBarStateProperty); }
            set { SetValue(AppBarStateProperty, value); }
        }

        public EditUsernameViewModel ViewModel
        {
            get { return DataContext as EditUsernameViewModel; }
        }

        private readonly AppBarButton _doneButton = new AppBarButton
        {
            Text = AppResources.Done,
            IconUri = new Uri("/Images/ApplicationBar/appbar.check.png", UriKind.Relative)
        };

        private readonly AppBarButton _cancelButton = new AppBarButton
        {
            Text = AppResources.Cancel,
            IconUri = new Uri("/Images/ApplicationBar/appbar.cancel.rest.png", UriKind.Relative)
        };

        public EditUsernameView()
        {
            InitializeComponent();

            Caption.Background = ShellView.CaptionBrush;

            _doneButton.Click += (sender, args) => ViewModel.Done();
            _cancelButton.Click += (sender, args) => ViewModel.Cancel();

            Loaded += (sender, args) => BuildLocalizedAppBar();
        }

        private void BuildLocalizedAppBar()
        {
            if (ApplicationBar == null)
            {
                ApplicationBar = new ApplicationBar();

                ApplicationBar.Buttons.Add(_doneButton);
                ApplicationBar.Buttons.Add(_cancelButton);
            }
        }

        private void Username_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.Done();
            }
        }
    }
}