using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;

namespace AvaloniaPortProva;

public partial class WindowAnnotationClickEvent : Window
{
    Annotation _annotation1;
    Line _lineSeries1;

    public WindowAnnotationClickEvent()
    {
        InitializeComponent();
        MakeGraph();
    }
    public void MakeGraph()
    {
        _lineSeries1 = new Line();
        _lineSeries1.FillSampleValues();
        TChart.Series.Add(_lineSeries1);

        _annotation1 = new Annotation
        {
            AutoSize = true,
            Position = AnnotationPositions.LeftBottom,
            //Text = "Afegeix Linia"
            Text = "Some Text"
        };
        TChart.Tools.Add(_annotation1);

        _annotation1.Click += Annotation1_Click;
    }
    private async void Annotation1_Click(object sender, MouseEventArgs e)
    {
        //Si Vols Afegir una linia al fer clic

        //_lineSeries1 = new Line();
        //_lineSeries1.FillSampleValues();
        //TChart.Series.Add(_lineSeries1);

        //MessageBoxManager.GetMessageBoxStandard(_annotation1.Text, "Linia Afegida").ShowWindowDialogAsync(this);

        MessageBoxManager.GetMessageBoxStandard(_annotation1.Text, "Clicked").ShowWindowDialogAsync(this);
    }


    private void onClick_Tornar(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();

        mainWindow.Show();
        this.Close();

    }
}