﻿// ***********************************************************************
// Assembly         : IronyModManager
// Author           : Mario
// Created          : 01-10-2020
//
// Last Modified By : Mario
// Last Modified On : 02-07-2020
// ***********************************************************************
// <copyright file="App.xaml.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using IronyModManager.Common;
using IronyModManager.Common.Events;
using IronyModManager.DI;
using IronyModManager.Localization;
using IronyModManager.Models.Common;
using IronyModManager.Services;
using IronyModManager.Services.Common;
using IronyModManager.Shared;
using IronyModManager.ViewModels;
using IronyModManager.Views;
using ReactiveUI;

namespace IronyModManager
{
    /// <summary>
    /// Class App.
    /// Implements the <see cref="Avalonia.Application" />
    /// </summary>
    /// <seealso cref="Avalonia.Application" />
    [ExcludeFromCoverage("This should be tested via functional testing.")]
    public class App : Application
    {
        #region Methods

        /// <summary>
        /// The compile theme
        /// </summary>
        private static readonly Func<string, StyleInclude> compileTheme = (style) =>
        {
            return new StyleInclude(new Uri("resm:Styles"))
            {
                Source = new Uri(style)
            };
        };

        /// <summary>
        /// The theme setter
        /// </summary>
        private static readonly Func<Application, ITheme, bool> themeSetter = (app, theme) =>
        {
            if (app != null && theme != null)
            {
                app.Styles.Clear();
                foreach (var item in theme.StyleIncludes)
                {
                    var style = compileTheme(item);
                    app.Styles.Add(style);
                }
                return true;
            }
            return false;
        };

        /// <summary>
        /// Initializes the application by loading XAML etc.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            if (!Design.IsDesignMode)
            {
                InitThemes();
            }
        }

        /// <summary>
        /// Called when [framework initialization completed].
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                InitCulture();
                InitApp(desktop);
                InitAppTitle(desktop);
                InitAppSizeDefaults(desktop);
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Initializes the application size defaults.
        /// </summary>
        /// <param name="desktop">The desktop.</param>
        protected virtual void InitAppSizeDefaults(IClassicDesktopStyleApplicationLifetime desktop)
        {
            var stateService = DIResolver.Get<IWindowStateService>();
            if (!stateService.IsDefined() && !stateService.IsMaximized())
            {
                desktop.MainWindow.SizeToContent = SizeToContent.WidthAndHeight;
                desktop.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        /// <summary>
        /// Initializes the culture.
        /// </summary>
        protected virtual void InitCulture()
        {
            var langService = DIResolver.Get<ILanguagesService>();
            langService.ApplySelected();
        }

        /// <summary>
        /// Initializes the application title.
        /// </summary>
        /// <param name="desktop">The desktop.</param>
        protected virtual void InitAppTitle(IClassicDesktopStyleApplicationLifetime desktop)
        {
            SetAppTitle(desktop);
            var listener = MessageBus.Current.Listen<LocaleChangedEventArgs>();
            listener.SubscribeObservable(x =>
            {
                SetAppTitle(desktop);
            });
        }

        /// <summary>
        /// Sets the application title.
        /// </summary>
        /// <param name="desktop">The desktop.</param>
        protected virtual void SetAppTitle(IClassicDesktopStyleApplicationLifetime desktop)
        {
            var appTitle = DIResolver.Get<ILocalizationManager>().GetResource(LocalizationResources.App.Title);
            desktop.MainWindow.Title = appTitle;
        }

        /// <summary>
        /// Reinitializes the application.
        /// </summary>
        /// <param name="desktop">The desktop.</param>
        private void InitApp(IClassicDesktopStyleApplicationLifetime desktop)
        {
            var resolver = DIResolver.Get<IViewResolver>();
            var mainWindow = DIResolver.Get<MainWindow>();
            var vm = (MainWindowViewModel)resolver.ResolveViewModel<MainWindow>();
            mainWindow.DataContext = vm;
            desktop.MainWindow = mainWindow;
        }

        /// <summary>
        /// Initializes the themes.
        /// </summary>
        private void InitThemes()
        {
            var currentTheme = DIResolver.Get<IThemeService>().GetSelected();
            themeSetter(this, currentTheme);

            var listener = MessageBus.Current.Listen<ThemeChangedEventArgs>();
            listener.SubscribeObservable(x =>
            {
                OnThemeChanged().ConfigureAwait(true);
            });
        }

        /// <summary>
        /// Called when [theme changed].
        /// </summary>
        /// <returns>Task.</returns>
        private async Task OnThemeChanged()
        {
            var locManager = DIResolver.Get<ILocalizationManager>();
            var title = locManager.GetResource(LocalizationResources.Themes.Restart_Title);
            var message = locManager.GetResource(LocalizationResources.Themes.Restart_Message);
            var header = locManager.GetResource(LocalizationResources.Themes.Restart_Header);
            var prompt = MessageBoxes.GetYesNoWindow(title, header, message, MessageBox.Avalonia.Enums.Icon.Info);
            var desktop = ((IClassicDesktopStyleApplicationLifetime)Current.ApplicationLifetime);
            var mainWindow = desktop.MainWindow;
            var result = await prompt.ShowDialog(mainWindow);
            if (result == MessageBox.Avalonia.Enums.ButtonResult.Yes)
            {
                var path = Process.GetCurrentProcess().MainModule.FileName;
                Process.Start(path);
                desktop.Shutdown();
            }
        }
    }

    #endregion Methods
}