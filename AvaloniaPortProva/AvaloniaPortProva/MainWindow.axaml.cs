using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;

namespace AvaloniaPortProva
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void onClickBtn(object sender, RoutedEventArgs e)
        {
            MainWindow_txtBlock_TxtBlockPrueva.Text = "";
            string valorTxt = MainWindow_txtBox_TxtBoxPrueva.Text;
            for (int i = 0; i < 10; i++)
                MainWindow_txtBlock_TxtBlockPrueva.Text += valorTxt + "\n";
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            if (MainWindow_chckBox_MajusMin.IsChecked == true)
                MainWindow_lbl_TextoPrueva.Content = MainWindow_lbl_TextoPrueva.Content.ToString().ToUpper();
            else
                MainWindow_lbl_TextoPrueva.Content = MainWindow_lbl_TextoPrueva.Content.ToString().ToLower();

        }

        private void onClick_wndAnntClckEvent(object sender, RoutedEventArgs e)
        {
            WindowAnnotationClickEvent windowAnnotationClickEvent = new WindowAnnotationClickEvent();
            windowAnnotationClickEvent.Show();

            this.Hide();
        }

        private void onClick_wndRealtimeDash(object sender, RoutedEventArgs e)
        {
            WindowRealtimeDash windowRealtimeDash = new WindowRealtimeDash();
            windowRealtimeDash.Show();

            this.Hide();
        }

        private void onClick_wndAvaloniaLliure(object sender, RoutedEventArgs e)
        {
            AvaloniaLliure windowAvaloniaLliure = new AvaloniaLliure();
            windowAvaloniaLliure.Show();

            this.Hide();
        }

    }
}