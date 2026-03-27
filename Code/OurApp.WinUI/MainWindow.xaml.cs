using OurApp.Core.Models;
using OurApp.Core.Repositories;
using OurApp.Core.Services;
using OurApp.WinUI.Views;
using OurApp.WinUI.Views.Jobs;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinRT.Interop;

namespace OurApp.WinUI
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        // Events service — kept for Robert's pages
        public EventsService service { get; } = new EventsService(new EventsRepo());

        public Frame RootFrame => rootFrame;

        public MainWindow()
        {
            this.InitializeComponent();

            var hwnd = WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new Windows.Graphics.SizeInt32(1200, 800));

            Instance = this;

            ShowMain();
        }

        // ── Job navigation ────────────────────────────────────────────
        public void ShowMain()      => rootFrame.Navigate(typeof(MainPage));
        public void ShowJobs()      => rootFrame.Navigate(typeof(JobsListPage));
        public void ShowCreateJob() => rootFrame.Navigate(typeof(CreateJobPage));
        public void ShowPastJobs()  => rootFrame.Navigate(typeof(PastJobsPage));

        public void ShowEditJob(JobPosting job, bool isRepost = false)
            => rootFrame.Navigate(typeof(EditJobPage), new EditJobParams { Job = job, IsRepost = isRepost });

        // ── Robert's top-bar handlers ─────────────────────────────────
        private void NavigateToOurJobs_Click(object sender, RoutedEventArgs e)
            => rootFrame.Navigate(typeof(OurJobsPage));

        private void NavigateToOurEvents_Click(object sender, RoutedEventArgs e)
            => rootFrame.Navigate(typeof(OurEventsPage));

        private void NavigateToPastEvents_Click(object sender, RoutedEventArgs e)
            => rootFrame.Navigate(typeof(PastEventsPage));
    }
}
