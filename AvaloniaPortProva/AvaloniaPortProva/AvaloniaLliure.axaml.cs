using System;
using System.Diagnostics;
using System.Drawing;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Steema.TeeChart;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;


namespace AvaloniaPortProva;

public partial class AvaloniaLliure : Window
{
    private Surface Superficie1;
    private Surface Superficie2;
    Line _liniaSeries1;
    Stopwatch stopWatch;
    private Axis axis1;
    private Axis axis2;
    private Area area1;
    private Line line1;
    private Line line2;
    private Bar3D bar3D1;
    private CursorTool cursorTool1;
    string cursorText = "";
    int cursorX = -1;
    int cursorY = -1;
    string currentTime;
    DispatcherTimer dispatcherTimer;
    Random rnd;
    int m = 0;
    bool deltaM = true;
    int OneYesOneNo = 0;



    public AvaloniaLliure()
    {
        InitializeComponent();
        MakeGraph();
        modifychart();
        ConfigureCharts();
        stopWatch = Stopwatch.StartNew();
        stopWatch.Stop();
        StartStop.Click += StartStop_Click;
        Init();
    }
    public void MakeGraph()
    {
        _liniaSeries1 = new Line();

        tChart1.Panel.Brush.Color = Color.FromArgb(32, 34, 37);
        tChart1.Panel.Brush.Gradient.Visible = false;
        tChart1.Panel.Shadow.Visible = false;

        tChart1.Walls.Visible = false;
        tChart1.Legend.Visible = false;
        tChart1.Header.Visible = false;

        tChart1.Axes.Left.Grid.Color = Color.FromArgb(55, 55, 55);

        tChart1.Axes.Bottom.Grid.Color = Color.FromArgb(55, 55, 55);

        tChart1.Axes.Left.Labels.Font.Brush.Color = Color.Gainsboro;

        tChart1.Axes.Bottom.Labels.Font.Brush.Color = Color.Gainsboro;

        _liniaSeries1 = new Line();
        _liniaSeries1.Color = Color.FromArgb(0, 170, 255);

        _liniaSeries1.LinePen.Width = 3;

        _liniaSeries1.FillSampleValues();
        tChart1.Series.Add(_liniaSeries1);
    }
    public void modifychart()
    {
        tChart2.Aspect.View3D = true;
        tChart2.Aspect.Chart3DPercent = 40;
        tChart2.Aspect.Orthogonal = false;
        tChart2.Aspect.Zoom = 80;

        tChart2.Panning.Active = false;
        tChart2.Zoom.Direction = ZoomDirections.None;

        tChart2.Header.Visible = false;

        tChart2.Axes.Left.SetMinMax(-1.5, 1.5);
        tChart2.Axes.Depth.Visible = true;

        tChart2.Walls.Visible = false;
        tChart2.Legend.Visible = false;

        tChart2.Panel.Brush.Color = Color.FromArgb(32, 34, 37);
        tChart2.Panel.Brush.Gradient.Visible = false;
        tChart2.Panel.Shadow.Visible = false;

        tChart2.Walls.Visible = false;
        tChart2.Legend.Visible = false;
        tChart2.Header.Visible = false;

        Superficie1 = new Surface(tChart2.Chart);

        Superficie2 = new Surface(tChart2.Chart)
        {
            PaletteStyle = PaletteStyles.Rainbow,
            UseColorRange = false,
            UsePalette = true,
        };

        Superficie1.Transparency = 50;
        Superficie1.FillSampleValues();

        Superficie2.Transparency = 55;
        Superficie2.FillSampleValues();

        for (int i = 0; i < Superficie2.YValues.Count; i++)
        {
            Superficie2.YValues.Value[i] = Superficie2.YValues.Value[i] - 0.7;
        }

        Rotate rotar = new Rotate(tChart2.Chart);

        Superficie1 = new Surface(tChart2.Chart);
        Superficie1.Color = Color.FromArgb(0, 120, 215);

        Superficie2 = new Surface(tChart2.Chart)
        {
            Color = Color.FromArgb(0, 200, 150),
            UsePalette = false
        };

        Superficie1.Transparency = 40;
        Superficie2.Transparency = 50;

    }
    private void ConfigureCharts()
    {
        tChart3.Panel.Brush.Color = Color.FromArgb(32, 34, 37);

        tChart3.Panel.Brush.Gradient.Visible = false;
        tChart3.Panel.Shadow.Visible = false;

        tChart3.Walls.Visible = false;
        tChart3.Legend.Visible = false;
        tChart3.Header.Visible = false;

        tChart3.Axes.Left.Grid.Color = Color.FromArgb(55, 55, 55);

        tChart3.Axes.Bottom.Grid.Color = Color.FromArgb(55, 55, 55);

        var liniaSerie = new Line();
        liniaSerie.Color = Color.FromArgb(0, 170, 255);

        liniaSerie.LinePen.Width = 3;

        var puntoSeries = new Points();
        puntoSeries.Color = Color.FromArgb(255, 210, 80);

        puntoSeries.Pointer.HorizSize = 4;
        puntoSeries.Pointer.VertSize = 4;

        var barraSeries = new Bar
        {
            BarStyle = BarStyles.Arrow,
            Color = Color.FromArgb(0, 200, 150)
        };

        double[] yValues =
        {
                100000,
                150000,
                30000,
                25000,
                45000,
                90000
            };

        foreach (var val in yValues)
        {
            liniaSerie.Add(val);
            puntoSeries.Add(val);
            barraSeries.Add(val);
        }

        tChart3.Series.Add(liniaSerie);
        tChart3.Series.Add(puntoSeries);
        tChart3.Series.Add(barraSeries);
    }
    private void TChart4_AfterDraw(object sender, IGraphics3D g)
    {
        if (!stopWatch.IsRunning)
        {
            if (cursorX != -1)
            {
                g.Font.Color = System.Drawing.Color.White;
                g.Font.Size = 12;
                g.TextOut(cursorX + 3, (int)(cursorY - g.TextHeight("H") - 2), "Freq. Hz.: " + cursorText);
            }
        }
    }
    private void CursorTool1_Change(object sender, CursorChangeEventArgs e)
    {
        cursorText = line2.YScreenToValue(e.y).ToString("#.00");
        cursorX = e.x;
        cursorY = e.y;
    }
    void dt_Tick(object sender, EventArgs e)
    {
        if (stopWatch.IsRunning)
        {
            TimeSpan ts = stopWatch.Elapsed;
            currentTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            DoTimerTick();
        }
    }
    protected void DoTimerTick()
    {
        if (deltaM)
        {
            m = m + 10;
            if (m == 200)
                deltaM = false;
        }
        else
        {
            m = m - 10;
            if (m == 10)
                deltaM = true;
        }

        addToMultiSeries();

    }
    private void addToMultiSeries()
    {
        if (area1.Count == 0 ||
            line1.Count == 0 ||
            line2.Count == 0)
        {
            return;
        }
        double delta1 = rnd.NextDouble() * 350;
        double delta2 = rnd.NextDouble() * 350;
        double delta3 = (rnd.NextDouble() * 350) + 10;

        double newAreaVal = area1.YValues.Last + ((area1.YValues.Last > 449) ? -delta1 : delta1);
        double newLine1Val = line1.YValues.Last + ((line1.YValues.Last > 449) ? -delta1 : delta1);
        double newLine2Val = line2.YValues.Last + ((line2.YValues.Last > 449) ? -delta3 : delta3);

        if (newAreaVal < 100) newAreaVal = 100;
        if (newLine1Val < 100) newLine1Val = 100;
        if (newLine2Val < 100) newLine2Val = 100;

        double timeStamp = area1.XValues.Last + (1 / 86400.0F / 2.0F);

        area1.Add(timeStamp, newAreaVal);
        area1.XValues.Modified = true;

        line1.Add(timeStamp, newLine1Val);
        line2.Add(timeStamp, newLine2Val);

        if (area1.Count == 102)
            OneYesOneNo = 0;

        OneYesOneNo++;

        if (OneYesOneNo == 1)
        {
            bar3D1.Add(timeStamp, 400 - rnd.Next(400), 400 + rnd.Next(400));
            OneYesOneNo = -1;
        }

        if (area1.Count > 110)
        {
            area1.Delete(0);
            area1.XValues.Modified = true;
            line1.Delete(0);
            line1.XValues.Modified = true;
            line2.Delete(0);
            line2.XValues.Modified = true;

            if (bar3D1.Count > 55)
            {
                bar3D1.Delete(0);
                bar3D1.XValues.Modified = true;
            }

            tChart4.Axes.Bottom.SetMinMax(area1.XValues[0], area1.XValues.Last);
        }
    }
    private DispatcherTimer getDispatcherTimer()
    {
        if (dispatcherTimer == null)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dt_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
        }

        return dispatcherTimer;
    }
    private void StartStop_Click(object sender, RoutedEventArgs e)
    {
        currentTime = string.Empty;

        if (stopWatch.IsRunning) stopWatch.Stop();
        else
            stopWatch.Start();

        getDispatcherTimer();

        if (dispatcherTimer.IsEnabled) dispatcherTimer.Stop();
        else
            dispatcherTimer.Start();

        cursorTool1.Active = !stopWatch.IsRunning;
        if (cursorTool1.Active)
        {
            cursorTool1.XValue = tChart4.Axes.Bottom.Minimum + (tChart4.Axes.Bottom._iRange / 2);
            cursorTool1.YValue = tChart4.Axes.Left.Minimum + (tChart4.Axes.Left._iRange / 2);
        }
    }
    private void onClick_Tornar(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();

        mainWindow.Show();
        this.Close();

    }
    public void Init()
    {
        axis1 = new Axis();
        axis2 = new Axis();
        area1 = new Area();
        line1 = new Line();
        line2 = new Line();
        bar3D1 = new Bar3D();
        cursorTool1 = new CursorTool();
        rnd = new Random();

        #region tChart4
        tChart4.Aspect.ColorPaletteIndex = -1;

        tChart4.Axes.Bottom.Labels.Angle = 45;

        tChart4.Axes.Bottom.Labels.Brush.Color = Color.White;
        tChart4.Axes.Bottom.Labels.Brush.Solid = true;
        tChart4.Axes.Bottom.Labels.Brush.Visible = true;
        tChart4.Axes.Bottom.Labels.DateTimeFormat = "hh:mm:ss";

        tChart4.Axes.Bottom.Labels.Font.Bold = false;

        tChart4.Axes.Bottom.Labels.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Bottom.Labels.Font.Brush.Solid = true;
        tChart4.Axes.Bottom.Labels.Font.Brush.Visible = true;

        tChart4.Axes.Bottom.Labels.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Bottom.Labels.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Bottom.Labels.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Bottom.Labels.Font.Size = 6;
        tChart4.Axes.Bottom.Labels.Font.SizeFloat = 6F;
        tChart4.Axes.Bottom.Labels.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Bottom.Labels.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Bottom.Labels.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Bottom.Labels.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Bottom.Labels.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Bottom.Labels.Shadow.Brush.Solid = true;
        tChart4.Axes.Bottom.Labels.Shadow.Brush.Visible = true;

        tChart4.Axes.Bottom.Title.Brush.Color = Color.Silver;
        tChart4.Axes.Bottom.Title.Brush.Solid = true;
        tChart4.Axes.Bottom.Title.Brush.Visible = true;

        tChart4.Axes.Bottom.Title.Font.Bold = false;

        tChart4.Axes.Bottom.Title.Font.Brush.Color = Color.FromArgb((int)(byte)0, (int)(byte)0, (int)(byte)0);
        tChart4.Axes.Bottom.Title.Font.Brush.Solid = true;
        tChart4.Axes.Bottom.Title.Font.Brush.Visible = true;

        tChart4.Axes.Bottom.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Bottom.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Bottom.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Bottom.Title.Font.Size = 8;
        tChart4.Axes.Bottom.Title.Font.SizeFloat = 8F;
        tChart4.Axes.Bottom.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Bottom.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Bottom.Title.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Bottom.Title.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Bottom.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Bottom.Title.Shadow.Brush.Solid = true;
        tChart4.Axes.Bottom.Title.Shadow.Brush.Visible = true;
        tChart4.Axes.Custom.Add(axis1);
        tChart4.Axes.Custom.Add(axis2);

        tChart4.Axes.Depth.Labels.Brush.Color = Color.White;
        tChart4.Axes.Depth.Labels.Brush.Solid = true;
        tChart4.Axes.Depth.Labels.Brush.Visible = true;

        tChart4.Axes.Depth.Labels.Font.Bold = false;

        tChart4.Axes.Depth.Labels.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Depth.Labels.Font.Brush.Solid = true;
        tChart4.Axes.Depth.Labels.Font.Brush.Visible = true;

        tChart4.Axes.Depth.Labels.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Depth.Labels.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Depth.Labels.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Depth.Labels.Font.Size = 8;
        tChart4.Axes.Depth.Labels.Font.SizeFloat = 8F;
        tChart4.Axes.Depth.Labels.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Depth.Labels.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Depth.Labels.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Depth.Labels.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Depth.Labels.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Depth.Labels.Shadow.Brush.Solid = true;
        tChart4.Axes.Depth.Labels.Shadow.Brush.Visible = true;

        tChart4.Axes.Depth.Title.Brush.Color = Color.Silver;
        tChart4.Axes.Depth.Title.Brush.Solid = true;
        tChart4.Axes.Depth.Title.Brush.Visible = true;

        tChart4.Axes.Depth.Title.Font.Bold = false;

        tChart4.Axes.Depth.Title.Font.Brush.Color = Color.FromArgb((int)(byte)0, (int)(byte)0, (int)(byte)0);
        tChart4.Axes.Depth.Title.Font.Brush.Solid = true;
        tChart4.Axes.Depth.Title.Font.Brush.Visible = true;

        tChart4.Axes.Depth.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Depth.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Depth.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Depth.Title.Font.Size = 8;
        tChart4.Axes.Depth.Title.Font.SizeFloat = 8F;
        tChart4.Axes.Depth.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Depth.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Depth.Title.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Depth.Title.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Depth.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Depth.Title.Shadow.Brush.Solid = true;
        tChart4.Axes.Depth.Title.Shadow.Brush.Visible = true;

        tChart4.Axes.DepthTop.Labels.Brush.Color = Color.White;
        tChart4.Axes.DepthTop.Labels.Brush.Solid = true;
        tChart4.Axes.DepthTop.Labels.Brush.Visible = true;

        tChart4.Axes.DepthTop.Labels.Font.Bold = false;

        tChart4.Axes.DepthTop.Labels.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.DepthTop.Labels.Font.Brush.Solid = true;
        tChart4.Axes.DepthTop.Labels.Font.Brush.Visible = true;

        tChart4.Axes.DepthTop.Labels.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.DepthTop.Labels.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.DepthTop.Labels.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.DepthTop.Labels.Font.Size = 8;
        tChart4.Axes.DepthTop.Labels.Font.SizeFloat = 8F;
        tChart4.Axes.DepthTop.Labels.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.DepthTop.Labels.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.DepthTop.Labels.ImageBevel.Brush.Solid = true;
        tChart4.Axes.DepthTop.Labels.ImageBevel.Brush.Visible = true;

        tChart4.Axes.DepthTop.Labels.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.DepthTop.Labels.Shadow.Brush.Solid = true;
        tChart4.Axes.DepthTop.Labels.Shadow.Brush.Visible = true;

        tChart4.Axes.DepthTop.Title.Brush.Color = Color.Silver;
        tChart4.Axes.DepthTop.Title.Brush.Solid = true;
        tChart4.Axes.DepthTop.Title.Brush.Visible = true;

        tChart4.Axes.DepthTop.Title.Font.Bold = false;

        tChart4.Axes.DepthTop.Title.Font.Brush.Color = Color.FromArgb((int)(byte)0, (int)(byte)0, (int)(byte)0);
        tChart4.Axes.DepthTop.Title.Font.Brush.Solid = true;
        tChart4.Axes.DepthTop.Title.Font.Brush.Visible = true;

        tChart4.Axes.DepthTop.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.DepthTop.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.DepthTop.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.DepthTop.Title.Font.Size = 8;
        tChart4.Axes.DepthTop.Title.Font.SizeFloat = 8F;
        tChart4.Axes.DepthTop.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.DepthTop.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.DepthTop.Title.ImageBevel.Brush.Solid = true;
        tChart4.Axes.DepthTop.Title.ImageBevel.Brush.Visible = true;

        tChart4.Axes.DepthTop.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.DepthTop.Title.Shadow.Brush.Solid = true;
        tChart4.Axes.DepthTop.Title.Shadow.Brush.Visible = true;

        tChart4.Axes.Left.AxisPen.Color = Color.FromArgb((int)(byte)128, (int)(byte)128, (int)(byte)255);
        tChart4.Axes.Left.AxisPen.Visible = true;
        tChart4.Axes.Left.EndPosition = 70D;

        tChart4.Axes.Left.Labels.Brush.Color = Color.White;
        tChart4.Axes.Left.Labels.Brush.Solid = true;
        tChart4.Axes.Left.Labels.Brush.Visible = true;

        tChart4.Axes.Left.Labels.Font.Bold = false;

        tChart4.Axes.Left.Labels.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Left.Labels.Font.Brush.Solid = true;
        tChart4.Axes.Left.Labels.Font.Brush.Visible = true;

        tChart4.Axes.Left.Labels.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Left.Labels.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Left.Labels.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Left.Labels.Font.Size = 8;
        tChart4.Axes.Left.Labels.Font.SizeFloat = 8F;
        tChart4.Axes.Left.Labels.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Left.Labels.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Left.Labels.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Left.Labels.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Left.Labels.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Left.Labels.Shadow.Brush.Solid = true;
        tChart4.Axes.Left.Labels.Shadow.Brush.Visible = true;

        tChart4.Axes.Left.Title.Brush.Color = Color.Silver;
        tChart4.Axes.Left.Title.Brush.Solid = true;
        tChart4.Axes.Left.Title.Brush.Visible = true;
        tChart4.Axes.Left.Title.Caption = "Volume GBs";

        tChart4.Axes.Left.Title.Font.Bold = false;

        tChart4.Axes.Left.Title.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Left.Title.Font.Brush.Solid = true;
        tChart4.Axes.Left.Title.Font.Brush.Visible = true;

        tChart4.Axes.Left.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Left.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Left.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Left.Title.Font.Size = 8;
        tChart4.Axes.Left.Title.Font.SizeFloat = 8F;
        tChart4.Axes.Left.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Left.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Left.Title.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Left.Title.ImageBevel.Brush.Visible = true;
        tChart4.Axes.Left.Title.Lines = new string[]
        {
                "Volume GBs"
        };

        tChart4.Axes.Left.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Left.Title.Shadow.Brush.Solid = true;
        tChart4.Axes.Left.Title.Shadow.Brush.Visible = true;

        tChart4.Axes.Right.AxisPen.Color = Color.FromArgb((int)(byte)255, (int)(byte)192, (int)(byte)128);
        tChart4.Axes.Right.AxisPen.Visible = true;
        tChart4.Axes.Right.EndPosition = 70D;

        tChart4.Axes.Right.Grid.Visible = false;

        tChart4.Axes.Right.Labels.Brush.Color = Color.White;
        tChart4.Axes.Right.Labels.Brush.Solid = true;
        tChart4.Axes.Right.Labels.Brush.Visible = true;

        tChart4.Axes.Right.Labels.Font.Bold = false;

        tChart4.Axes.Right.Labels.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Right.Labels.Font.Brush.Solid = true;
        tChart4.Axes.Right.Labels.Font.Brush.Visible = true;

        tChart4.Axes.Right.Labels.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Right.Labels.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Right.Labels.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Right.Labels.Font.Size = 8;
        tChart4.Axes.Right.Labels.Font.SizeFloat = 8F;
        tChart4.Axes.Right.Labels.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Right.Labels.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Right.Labels.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Right.Labels.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Right.Labels.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Right.Labels.Shadow.Brush.Solid = true;
        tChart4.Axes.Right.Labels.Shadow.Brush.Visible = true;

        tChart4.Axes.Right.Title.Brush.Color = Color.Silver;
        tChart4.Axes.Right.Title.Brush.Solid = true;
        tChart4.Axes.Right.Title.Brush.Visible = true;
        tChart4.Axes.Right.Title.Caption = "Throughput GB/s";

        tChart4.Axes.Right.Title.Font.Bold = false;

        tChart4.Axes.Right.Title.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Right.Title.Font.Brush.Solid = true;
        tChart4.Axes.Right.Title.Font.Brush.Visible = true;

        tChart4.Axes.Right.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Right.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Right.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Right.Title.Font.Size = 8;
        tChart4.Axes.Right.Title.Font.SizeFloat = 8F;
        tChart4.Axes.Right.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Right.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Right.Title.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Right.Title.ImageBevel.Brush.Visible = true;
        tChart4.Axes.Right.Title.Lines = new string[]
        {
                "Throughput GB/s"
        };

        tChart4.Axes.Right.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Right.Title.Shadow.Brush.Solid = true;
        tChart4.Axes.Right.Title.Shadow.Brush.Visible = true;

        tChart4.Axes.Top.Labels.Brush.Color = Color.White;
        tChart4.Axes.Top.Labels.Brush.Solid = true;
        tChart4.Axes.Top.Labels.Brush.Visible = true;

        tChart4.Axes.Top.Labels.Font.Bold = false;

        tChart4.Axes.Top.Labels.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Axes.Top.Labels.Font.Brush.Solid = true;
        tChart4.Axes.Top.Labels.Font.Brush.Visible = true;

        tChart4.Axes.Top.Labels.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Top.Labels.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Top.Labels.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Top.Labels.Font.Size = 8;
        tChart4.Axes.Top.Labels.Font.SizeFloat = 8F;
        tChart4.Axes.Top.Labels.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Top.Labels.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Top.Labels.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Top.Labels.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Top.Labels.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Top.Labels.Shadow.Brush.Solid = true;
        tChart4.Axes.Top.Labels.Shadow.Brush.Visible = true;

        tChart4.Axes.Top.Title.Brush.Color = Color.Silver;
        tChart4.Axes.Top.Title.Brush.Solid = true;
        tChart4.Axes.Top.Title.Brush.Visible = true;

        tChart4.Axes.Top.Title.Font.Bold = false;

        tChart4.Axes.Top.Title.Font.Brush.Color = Color.FromArgb((int)(byte)0, (int)(byte)0, (int)(byte)0);
        tChart4.Axes.Top.Title.Font.Brush.Solid = true;
        tChart4.Axes.Top.Title.Font.Brush.Visible = true;

        tChart4.Axes.Top.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Top.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Axes.Top.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Axes.Top.Title.Font.Size = 8;
        tChart4.Axes.Top.Title.Font.SizeFloat = 8F;
        tChart4.Axes.Top.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Axes.Top.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Axes.Top.Title.ImageBevel.Brush.Solid = true;
        tChart4.Axes.Top.Title.ImageBevel.Brush.Visible = true;

        tChart4.Axes.Top.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Axes.Top.Title.Shadow.Brush.Solid = true;
        tChart4.Axes.Top.Title.Shadow.Brush.Visible = true;

        tChart4.Header.Brush.Color = Color.FromArgb((int)(byte)192, (int)(byte)192, (int)(byte)192);

        tChart4.Header.Brush.Gradient.SigmaFocus = 0F;
        tChart4.Header.Brush.Gradient.SigmaScale = 0F;
        tChart4.Header.Brush.Solid = true;
        tChart4.Header.Brush.Visible = true;

        tChart4.Header.Font.Bold = false;

        tChart4.Header.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Header.Font.Brush.Solid = true;
        tChart4.Header.Font.Brush.Visible = true;

        tChart4.Header.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Header.Font.Shadow.Brush.Solid = true;
        tChart4.Header.Font.Shadow.Brush.Visible = true;
        tChart4.Header.Font.Size = 8;
        tChart4.Header.Font.SizeFloat = 8F;
        tChart4.Header.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Header.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Header.ImageBevel.Brush.Solid = true;
        tChart4.Header.ImageBevel.Brush.Visible = true;

        tChart4.Header.Shadow.Brush.Color = Color.FromArgb((int)(byte)169, (int)(byte)169, (int)(byte)169);
        tChart4.Header.Shadow.Brush.Solid = true;
        tChart4.Header.Shadow.Brush.Visible = true;
        tChart4.Header.Shadow.Height = 0;
        tChart4.Header.Shadow.Width = 0;
        tChart4.Header.Visible = false;

        tChart4.Legend.Brush.Color = Color.White;

        tChart4.Legend.Brush.Gradient.SigmaFocus = 0F;
        tChart4.Legend.Brush.Gradient.SigmaScale = 0F;
        tChart4.Legend.Brush.Gradient.Visible = true;
        tChart4.Legend.Brush.Solid = true;
        tChart4.Legend.Brush.Visible = true;
        tChart4.Legend.CheckBoxes = false;
        tChart4.Legend.ClipText = false;

        tChart4.Legend.Font.Bold = false;

        tChart4.Legend.Font.Brush.Color = Color.FromArgb((int)(byte)255, (int)(byte)255, (int)(byte)255);
        tChart4.Legend.Font.Brush.Solid = true;
        tChart4.Legend.Font.Brush.Visible = true;

        tChart4.Legend.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Legend.Font.Shadow.Brush.Solid = true;
        tChart4.Legend.Font.Shadow.Brush.Visible = true;
        tChart4.Legend.Font.Size = 8;
        tChart4.Legend.Font.SizeFloat = 8F;
        tChart4.Legend.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.Legend.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Legend.ImageBevel.Brush.Solid = true;
        tChart4.Legend.ImageBevel.Brush.Visible = true;

        tChart4.Legend.Shadow.Brush.Color = Color.FromArgb((int)(byte)0, (int)(byte)0, (int)(byte)0);
        tChart4.Legend.Shadow.Brush.Solid = true;
        tChart4.Legend.Shadow.Brush.Visible = true;
        tChart4.Legend.Shadow.Width = 0;

        tChart4.Legend.Symbol.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Legend.Symbol.Shadow.Brush.Solid = true;
        tChart4.Legend.Symbol.Shadow.Brush.Visible = true;

        tChart4.Legend.Title.Brush.Color = Color.White;
        tChart4.Legend.Title.Brush.Solid = true;
        tChart4.Legend.Title.Brush.Visible = true;

        tChart4.Legend.Title.Font.Bold = true;

        tChart4.Legend.Title.Font.Brush.Color = Color.Black;
        tChart4.Legend.Title.Font.Brush.Solid = true;
        tChart4.Legend.Title.Font.Brush.Visible = true;

        tChart4.Legend.Title.Font.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Legend.Title.Font.Shadow.Brush.Solid = true;
        tChart4.Legend.Title.Font.Shadow.Brush.Visible = true;
        tChart4.Legend.Title.Font.Size = 8;
        tChart4.Legend.Title.Font.SizeFloat = 8F;
        tChart4.Legend.Title.Font.Style = Steema.TeeChart.Drawing.FontStyle.Bold;

        tChart4.Legend.Title.ImageBevel.Brush.Color = Color.LightGray;
        tChart4.Legend.Title.ImageBevel.Brush.Solid = true;
        tChart4.Legend.Title.ImageBevel.Brush.Visible = true;

        tChart4.Legend.Title.Shadow.Brush.Color = Color.DarkGray;
        tChart4.Legend.Title.Shadow.Brush.Solid = true;
        tChart4.Legend.Title.Shadow.Brush.Visible = true;
        tChart4.Legend.Visible = false;

        tChart4.Panel.Bevel.Outer = BevelStyles.None;
        tChart4.Panel.Bevel.Inner = BevelStyles.None;

        tChart4.Panel.Brush.Color = Color.FromArgb(32, 34, 37);
        tChart4.Panel.Brush.Solid = true;
        tChart4.Panel.Brush.Visible = true;

        tChart4.Panel.Brush.Gradient.Visible = false;

        tChart4.Panel.Shadow.Visible = false;

        tChart4.Panel.MarginBottom = 7D;
        tChart4.Panel.MarginLeft = 4D;
        tChart4.Panel.MarginRight = 10D;

        area1.Color = Color.FromArgb(70, 130, 255);
        area1.Transparency = 50;

        line1.Color = Color.FromArgb(255, 210, 80);
        line1.LinePen.Width = 3;

        line2.Color = Color.FromArgb(255, 120, 70);
        line2.LinePen.Width = 3;

        bar3D1.Color = Color.FromArgb(80, 220, 180);

        tChart4.Series.Add(area1);
        tChart4.Series.Add(line1);
        tChart4.Series.Add(line2);
        tChart4.Series.Add(bar3D1);

        tChart4.SubFooter.Brush.Color = System.Drawing.Color.Silver;
        tChart4.SubFooter.Brush.Solid = true;
        tChart4.SubFooter.Brush.Visible = true;

        tChart4.SubFooter.Font.Bold = false;

        tChart4.SubFooter.Font.Brush.Color = System.Drawing.Color.Red;
        tChart4.SubFooter.Font.Brush.Solid = true;
        tChart4.SubFooter.Font.Brush.Visible = true;

        tChart4.SubFooter.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.SubFooter.Font.Shadow.Brush.Solid = true;
        tChart4.SubFooter.Font.Shadow.Brush.Visible = true;
        tChart4.SubFooter.Font.Size = 8;
        tChart4.SubFooter.Font.SizeFloat = 8F;
        tChart4.SubFooter.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.SubFooter.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
        tChart4.SubFooter.ImageBevel.Brush.Solid = true;
        tChart4.SubFooter.ImageBevel.Brush.Visible = true;

        tChart4.SubFooter.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.SubFooter.Shadow.Brush.Solid = true;
        tChart4.SubFooter.Shadow.Brush.Visible = true;

        tChart4.SubHeader.Brush.Color = System.Drawing.Color.FromArgb((int)(byte)192, (int)(byte)192, (int)(byte)192);
        tChart4.SubHeader.Brush.Solid = true;
        tChart4.SubHeader.Brush.Visible = true;

        tChart4.SubHeader.Font.Bold = false;

        tChart4.SubHeader.Font.Brush.Color = System.Drawing.Color.FromArgb((int)(byte)128, (int)(byte)128, (int)(byte)128);
        tChart4.SubHeader.Font.Brush.Solid = true;
        tChart4.SubHeader.Font.Brush.Visible = true;

        tChart4.SubHeader.Font.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.SubHeader.Font.Shadow.Brush.Solid = true;
        tChart4.SubHeader.Font.Shadow.Brush.Visible = true;
        tChart4.SubHeader.Font.Size = 12;
        tChart4.SubHeader.Font.SizeFloat = 12F;
        tChart4.SubHeader.Font.Style = Steema.TeeChart.Drawing.FontStyle.Regular;

        tChart4.SubHeader.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
        tChart4.SubHeader.ImageBevel.Brush.Solid = true;
        tChart4.SubHeader.ImageBevel.Brush.Visible = true;

        tChart4.SubHeader.Shadow.Brush.Color = System.Drawing.Color.FromArgb((int)(byte)169, (int)(byte)169, (int)(byte)169);
        tChart4.SubHeader.Shadow.Brush.Solid = true;
        tChart4.SubHeader.Shadow.Brush.Visible = true;
        tChart4.TabIndex = 0;
        tChart4.Tools.Add(cursorTool1);

        tChart4.Walls.Back.Brush.Color = System.Drawing.Color.Silver;

        tChart4.Walls.Back.Brush.Gradient.EndColor = System.Drawing.Color.FromArgb((int)(byte)120, (int)(byte)120, (int)(byte)120);
        tChart4.Walls.Back.Brush.Gradient.StartColor = System.Drawing.Color.FromArgb((int)(byte)70, (int)(byte)70, (int)(byte)70);
        tChart4.Walls.Back.Brush.Solid = true;
        tChart4.Walls.Back.Brush.Visible = false;

        tChart4.Walls.Back.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
        tChart4.Walls.Back.ImageBevel.Brush.Solid = true;
        tChart4.Walls.Back.ImageBevel.Brush.Visible = true;

        tChart4.Walls.Back.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.Walls.Back.Shadow.Brush.Solid = true;
        tChart4.Walls.Back.Shadow.Brush.Visible = true;

        tChart4.Walls.Bottom.Brush.Color = System.Drawing.Color.White;

        tChart4.Walls.Bottom.Brush.Gradient.SigmaFocus = 0F;
        tChart4.Walls.Bottom.Brush.Gradient.SigmaScale = 0F;
        tChart4.Walls.Bottom.Brush.Solid = true;
        tChart4.Walls.Bottom.Brush.Visible = true;

        tChart4.Walls.Bottom.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
        tChart4.Walls.Bottom.ImageBevel.Brush.Solid = true;
        tChart4.Walls.Bottom.ImageBevel.Brush.Visible = true;

        tChart4.Walls.Bottom.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.Walls.Bottom.Shadow.Brush.Solid = true;
        tChart4.Walls.Bottom.Shadow.Brush.Visible = true;
        tChart4.Walls.Bottom.Visible = false;

        tChart4.Walls.Left.Brush.Color = System.Drawing.Color.LightYellow;

        tChart4.Walls.Left.Brush.Gradient.SigmaFocus = 0F;
        tChart4.Walls.Left.Brush.Gradient.SigmaScale = 0F;
        tChart4.Walls.Left.Brush.Solid = true;
        tChart4.Walls.Left.Brush.Visible = true;

        tChart4.Walls.Left.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
        tChart4.Walls.Left.ImageBevel.Brush.Solid = true;
        tChart4.Walls.Left.ImageBevel.Brush.Visible = true;

        tChart4.Walls.Left.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.Walls.Left.Shadow.Brush.Solid = true;
        tChart4.Walls.Left.Shadow.Brush.Visible = true;
        tChart4.Walls.Left.Visible = false;

        tChart4.Walls.Right.Brush.Color = System.Drawing.Color.FromArgb((int)(byte)192, (int)(byte)192, (int)(byte)192);

        tChart4.Walls.Right.Brush.Gradient.SigmaFocus = 0F;
        tChart4.Walls.Right.Brush.Gradient.SigmaScale = 0F;
        tChart4.Walls.Right.Brush.Solid = true;
        tChart4.Walls.Right.Brush.Visible = true;

        tChart4.Walls.Right.ImageBevel.Brush.Color = System.Drawing.Color.LightGray;
        tChart4.Walls.Right.ImageBevel.Brush.Solid = true;
        tChart4.Walls.Right.ImageBevel.Brush.Visible = true;

        tChart4.Walls.Right.Shadow.Brush.Color = System.Drawing.Color.DarkGray;
        tChart4.Walls.Right.Shadow.Brush.Solid = true;
        tChart4.Walls.Right.Shadow.Brush.Visible = true;

        tChart4.Zoom.Brush.Color = System.Drawing.Color.FromArgb((int)(byte)127, (int)(byte)0, (int)(byte)0, (int)(byte)255);
        tChart4.Zoom.Brush.Solid = true;
        tChart4.Zoom.Brush.Visible = false;
        tChart4.AfterDraw += TChart4_AfterDraw;
        DateTime dt = DateTime.Now.AddSeconds(-100);

        for (int i = 0; i < 100; i++)
        {
            double x = dt.AddSeconds(i).ToOADate();

            area1.Add(x, 900 + rnd.Next(800));
            line1.Add(x, 600 + rnd.Next(350));
            line2.Add(x, 600 + rnd.Next(350));

            if ((i % 2) == 0)
            {
                bar3D1.Add(
                    x,
                    400 - rnd.Next(400),
                    400 + rnd.Next(400));
            }
        }

        tChart4.Axes.Bottom.SetMinMax(
            area1.XValues[0],
            area1.XValues.Last);
        #endregion

        #region Axis 1
        axis1.AxisPen.Color = Color.FromArgb(255, 180, 70);
        axis1.AxisPen.Width = 2;
        axis1.AxisPen.Visible = true;

        axis1.EndPosition = 70D;
        axis1.Grid.Visible = false;
        axis1.Horizontal = false;

        axis1.Labels.Font.Size = 9;
        axis1.Labels.Font.Brush.Color = Color.FromArgb(210, 210, 210);

        axis1.OtherSide = true;
        axis1.RelativePosition = -11D;

        axis1.Title.Angle = 270;
        axis1.Title.Caption = "Freq. Hz.";

        axis1.Title.Font.Bold = true;
        axis1.Title.Font.Size = 10;
        axis1.Title.Font.Brush.Color = Color.FromArgb(255, 180, 70);

        axis1.ZPosition = 0D;
        #endregion

        #region Axis 2
        axis2.AxisPen.Color = Color.FromArgb(0, 220, 180);
        axis2.AxisPen.Width = 2;
        axis2.AxisPen.Visible = true;

        axis2.Horizontal = false;

        axis2.Labels.Font.Size = 9;
        axis2.Labels.Font.Brush.Color = Color.FromArgb(210, 210, 210);

        axis2.OtherSide = false;
        axis2.StartPosition = 72D;

        axis2.Title.Angle = 90;
        axis2.Title.Caption = "Rang de Buffer";

        axis2.Title.Font.Bold = true;
        axis2.Title.Font.Size = 10;
        axis2.Title.Font.Brush.Color = Color.FromArgb(0, 220, 180);
        #endregion

        #region Area 1
        area1.Color = Color.FromArgb(0, 170, 255);

        area1.Brush.Color = Color.FromArgb(0, 170, 255);

        area1.Transparency = 45;

        area1.LinePen.Color = Color.FromArgb(80, 190, 255);

        area1.LinePen.Width = 2;

        area1.Gradient.Visible = false;

        area1.Pointer.Visible = false;

        area1.Title = "Volum";

        area1.XValues.DataMember = "X";
        area1.XValues.DateTime = true;
        area1.XValues.Order = ValueListOrder.Ascending;

        area1.YValues.DataMember = "Y";
        #endregion

        #region Line 1
        line1.Color = Color.FromArgb(255, 190, 60);

        line1.LinePen.Width = 3;

        line1.VertAxis = VerticalAxis.Right;

        line1.Title = "Throughput";

        line1.XValues.DataMember = "X";
        line1.XValues.Order = ValueListOrder.Ascending;

        line1.YValues.DataMember = "Y";
        #endregion

        #region Line 2

        line2.Color = Color.FromArgb(0, 220, 180);

        line2.CustomVertAxis = axis1;
        line2.VertAxis = VerticalAxis.Custom;

        line2.LinePen.Width = 3;

        line2.Title = "Freqüència";

        line2.XValues.DataMember = "X";
        line2.XValues.Order = ValueListOrder.Ascending;

        line2.YValues.DataMember = "Y";
        #endregion

        #region Bar 3D
        bar3D1.Color = Color.FromArgb(170, 120, 255);

        bar3D1.Brush.Color = Color.FromArgb(170, 120, 255);

        bar3D1.Transparency = 20;

        bar3D1.BarRound = BarRounding.AtValue;

        bar3D1.CustomVertAxis = axis2;

        bar3D1.VertAxis = VerticalAxis.Custom;

        bar3D1.Marks.Visible = false;

        bar3D1.Title = "Buffer";

        bar3D1.XValues.DataMember = "X";
        bar3D1.XValues.Order = ValueListOrder.Ascending;

        bar3D1.YValues.DataMember = "Bar";
        #endregion

        #region Cursor Tool
        cursorTool1.FastCursor = false;

        cursorTool1.Pen.Color = Color.FromArgb(0, 220, 255);

        cursorTool1.Pen.Width = 2;

        cursorTool1.Pen.Style = DashStyle.Dash;

        cursorTool1.Series = line2;
        cursorTool1.SeriesIndex = -1;

        cursorTool1.Change += CursorTool1_Change;
        #endregion
    }
}