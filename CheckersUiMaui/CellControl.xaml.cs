using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace CheckersUiMaui;

public partial class CellControl : ContentView
{
	public CellControl()
	{
		InitializeComponent();

        // setup
        State = CellState.Inactive;
        CurrentHighlight = HighLight.None;

        // setup the drawable surface
        var canvasView = new SKCanvasView();
        canvasView.PaintSurface += OnCanvasView_PaintSurface;
        Content = canvasView;

        // handle touch/mouse
        canvasView.EnableTouchEvents = true;
        canvasView.Touch += CanvasView_Touch;
    }

    static CellControl()
    {
        // debugging resources
        //var assembly = typeof(MainPage).Assembly;
        //foreach (var res in assembly.GetManifestResourceNames())
        //{
        //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
        //}

        // load images
        ImageInactive = Utilities.MakeTransparent( Utilities.FromResource("CheckersUiMaui.Resources.Images.inactive.png"), SKColors.White);
        ImageActive = Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.active.png"), SKColors.White);
        ImageKing = Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.king.png"), SKColors.White);
        ImageRed = Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.red.png"), SKColors.White);
        ImageBlack = Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.black.png"), SKColors.White);
    }

    public event Action<Cell> OnTouch;
    public int Row { get; set; }
    public int Column { get; set; }
    public int Number { get; set; }
    public CellState State { get; private set; }

    public void SetCellState(CellState state)
    {
        // set state
        State = state;
        // redraw
        (Content as ISKCanvasView).InvalidateSurface();
    }
    public void SetHighLight(HighLight highlight)
    {
        // set highlight
        CurrentHighlight = highlight;
        // redraw
        (Content as ISKCanvasView).InvalidateSurface();
    }


    #region private
    private static SKBitmap ImageInactive;
    private static SKBitmap ImageActive;
    private static SKBitmap ImageBlack;
    private static SKBitmap ImageKing;
    private static SKBitmap ImageRed;

    private HighLight CurrentHighlight;

    private static SKFont TextFont = new SKFont(SKTypeface.FromFamilyName("Arial"), size: 24);
    private static SKPaint TextColor = new SKPaint() { Color = SKColors.Black };
    private static SKPaint Target = new SKPaint()
    { 
        Color = SKColors.Yellow,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 10f,
        PathEffect = SKPathEffect.CreateDash(intervals: new float[] {2,2}, phase: 0)
    };
    private static SKPaint Selected = new SKPaint()
    { 
        Color = SKColors.White,
        Style = SKPaintStyle.Stroke,
        StrokeWidth = 10f,
        PathEffect = SKPathEffect.CreateDash(intervals: new float[] { 2, 2 }, phase: 0)
    };
    private static SKPaint WithTransparency = new SKPaint()
    {
        Color = new SKColor(red: 0, green: 0, blue: 0, alpha: 250)
    };

    //
    // callbacks
    //

    private void CanvasView_Touch(object sender, SKTouchEventArgs args)
    {
        if (args.ActionType == SKTouchAction.Released)
        {
            if (OnTouch != null) OnTouch(new Cell() { Row = Row, Column = Column });
        }
    }

    private void OnCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        var info = args.Info;
        var surface = args.Surface;
        var canvas = surface.Canvas;

        // get canvas rectangle
        var rect = new SKRect(left: 0, top: 0, right: info.Width, bottom: info.Height);

        // clear
        canvas.Clear();

        // get the base image
        if (State == CellState.Inactive) canvas.DrawBitmap(ImageInactive, rect);
        else canvas.DrawBitmap(ImageActive, rect);

        // apply the piece
        switch (State)
        {
            case CellState.Empty:
            case CellState.Inactive:
                // already applied
                break;
            case CellState.Black:
                canvas.DrawBitmap(ImageBlack, rect, WithTransparency);
                break;
            case CellState.Red:
                canvas.DrawBitmap(ImageRed, rect, WithTransparency);
                break;
            case CellState.BlackKing:
                canvas.DrawBitmap(ImageBlack, rect, WithTransparency);
                canvas.DrawBitmap(ImageKing, rect, WithTransparency);
                break;
            case CellState.RedKing:
                canvas.DrawBitmap(ImageRed, rect, WithTransparency);
                canvas.DrawBitmap(ImageKing, rect, WithTransparency);
                break;
            default:
                throw new Exception($"unknown state : {State}");
        }

        // apply highlight
        if (CurrentHighlight == HighLight.Selected)
        {
            canvas.DrawRect(rect, Selected);
        }
        else if (CurrentHighlight == HighLight.Target)
        {
            canvas.DrawRect(rect, Target);
        }

        // apply square number
        if (Number > 0)
        {
            canvas.DrawText($"{Number}", x: info.Width/18, y: info.Height/6, TextFont, TextColor);
        }
    }
    #endregion
}