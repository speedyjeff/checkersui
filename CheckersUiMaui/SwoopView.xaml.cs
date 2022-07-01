using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace CheckersUiMaui;

public partial class SwoopView : ContentView
{
	public SwoopView()
	{
		InitializeComponent();

        IsActive = false;

        // setup the drawable surface
        var canvasView = new SKCanvasView();
        canvasView.PaintSurface += OnCanvasView_PaintSurface;
        Content = canvasView;
    }

    static SwoopView()
    {
        // load the swoop and flip to all the necessary directions
        ImageSwoopUpLeft = Utilities.Rotate(Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.swoop.png"), SKColors.Black ), angle: 0f);
        ImageSwoopUpRight = Utilities.Rotate(Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.swoop.png"), SKColors.Black), angle: 90f);
        ImageSwoopDownLeft = Utilities.Rotate(Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.swoop.png"), SKColors.Black), angle: 270f);
        ImageSwoopDownRight = Utilities.Rotate(Utilities.MakeTransparent(Utilities.FromResource("CheckersUiMaui.Resources.Images.swoop.png"), SKColors.Black), angle: 180f);
    }

    public void AddSwoop(Cell from, Cell to)
    {
        // set the swoop direction
        From = from;
        To = to;
        IsActive = true;

        // redraw
        (Content as ISKCanvasView).InvalidateSurface();
    }

    public void ClearSwoop()
    {
        IsActive = false;

        // redraw
        (Content as ISKCanvasView).InvalidateSurface();
    }

    #region private
    private static SKBitmap ImageSwoopUpLeft;
    private static SKBitmap ImageSwoopUpRight;
    private static SKBitmap ImageSwoopDownLeft;
    private static SKBitmap ImageSwoopDownRight;

    private bool IsActive;
    private Cell From;
    private Cell To;

    private static SKPaint WithTransparency = new SKPaint()
    {
        Color = new SKColor(red: 0, green: 0, blue: 0, alpha: 255)
    };

    private void OnCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        var info = args.Info;
        var surface = args.Surface;
        var canvas = surface.Canvas;

        // clear
        canvas.Clear();

        if (IsActive)
        {
            // bounds
            var cellWidth = (float)info.Width / (float)MainPage.BoardWidthCells;
            var cellHeight = (float)info.Height / (float)MainPage.BoardHeightCells;

            // determine where to place the swoop
            var direction = Direction.Up;

            // up or down
            if (From.Row < To.Row) direction = Direction.Down;
            else direction = Direction.Up;

            // left or right
            if (From.Column < To.Column) direction |= Direction.Right;
            else direction |= Direction.Left;

            // starting where
            var x = Math.Min(From.Column, To.Column) * cellWidth;
            var y = Math.Min(From.Row, To.Row) * cellHeight;

            // the width
            var width = (Math.Abs(From.Column - To.Column) + 1) * cellWidth;
            var height = (Math.Abs(From.Row - To.Row) + 1) * cellHeight;

            SKBitmap bitmap = ImageSwoopUpLeft;
            switch (direction)
            {
                case Direction.Up_Pointing_Left:
                    bitmap = ImageSwoopUpLeft;
                    break;
                case Direction.Up_Pointing_Right:
                    bitmap = ImageSwoopUpRight;
                    break;
                case Direction.Down_Pointing_Left:
                    bitmap = ImageSwoopDownLeft;
                    break;
                case Direction.Down_Pointing_Right:
                    bitmap = ImageSwoopDownRight;
                    break;
                default:
                    throw new Exception($"unknow exception {direction}");
            }

            canvas.DrawBitmap(bitmap, new SKRect(x, y, x+width, y+height), WithTransparency);
        }
    }
    #endregion
}