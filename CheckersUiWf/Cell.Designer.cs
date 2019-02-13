using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static CheckersUiWf.ExtInterface;
using static CheckersUiWf.IntInterface;

namespace CheckersUiWf
{
    partial class Cell
    {
        public Cell(CellState state, int square, int height, int width, HighLight highlight)
        {
            SquareNum = square;
            State = state;
            Highlight = highlight;
            IsDirty = true;
            Height = height;
            Width = width;
            Padding = new Padding(0);
            Margin = new Padding(0);
            InitializeComponent();
        }

        static Cell()
        {
            // preload the images
            Images = new Dictionary<CellState, Bitmap>();

            Images.Add(CellState.Inactive, LoadImage(@"Media\inactive.png"));
            Images.Add(CellState.Empty, LoadImage(@"Media\active.png"));
            Images.Add(CellState.Black, LoadImage(@"Media\black.png", true));
            Images.Add(CellState.White, LoadImage(@"Media\red.png", true));
            Images.Add(CellState.BlackKing, LoadImage(@"Media\king.png", true));
            Images.Add(CellState.WhiteKing, Images[CellState.BlackKing]);
        }

        internal CellState CellState
        {
            get
            {
                return State;
            }
            set
            {
                if (value != State)
                {
                    State = value;
                    IsDirty = true;
                }
            }
        }

        internal HighLight HightLight
        {
            get
            {
                return Highlight;
            }
            set
            {
                if (value != Highlight)
                {
                    Highlight = value;
                    IsDirty = true;
                }
            }
        }

        internal int Square
        {
            get
            {
                return SquareNum;
            }
            set
            {
                if (value != SquareNum)
                {
                    SquareNum = value;
                    IsDirty = true;
                }
            }
        }

        internal static string[] CellNumbers
        {
            get
            {
                return CellNumber;
            }
            set
            {
                if (value.Length != NumberSquares + 1)
                {
                    IntInterface.CallBack.Panic("Incorrect CellNumbers array size, should be " + (NumberSquares + 1).ToString());
                }
                CellNumber = value;
            }
        }

        internal bool Dirty { get => IsDirty;  set => IsDirty = value; }

#region private
        
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDirty || DoubleBuffer == null)
            {
                IsDirty = false;
                DoubleBuffer = new Bitmap(Width, Height);

                // combine the images
                using (var g = Graphics.FromImage(DoubleBuffer))
                {
                    // get the base image
                    if (State == CellState.Inactive) g.DrawImage(Images[CellState.Inactive], 0, 0, Width, Height);
                    else g.DrawImage(Images[CellState.Empty], 0, 0, Width, Height);

                    Bitmap image = null;
                    Images.TryGetValue(State, out image);

                    switch (State)
                    {
                        case CellState.Empty:
                        case CellState.Inactive:
                            // already applied
                            break;
                        case CellState.Black:
                            if (image != null) g.DrawImage(image, 0, 0);
                            else g.FillEllipse(new SolidBrush(Color.Black), Width / 4, Height / 4, Width / 2, Height / 2);
                            break;
                        case CellState.White:
                            if (image != null) g.DrawImage(image, 0, 0);
                            else g.FillEllipse(new SolidBrush(Color.Red), Width / 4, Height / 4, Width / 2, Height / 2);
                            break;
                        case CellState.BlackKing:
                            g.DrawImage(Images[CellState.Black], 0, 0);
                            g.DrawImage(Images[CellState.BlackKing], 0, 0);
                            break;
                        case CellState.WhiteKing:
                            g.DrawImage(Images[CellState.White], 0, 0);
                            g.DrawImage(Images[CellState.WhiteKing], 0, 0);
                            break;
                        default:
                            IntInterface.CallBack.Panic("Invalid cell state : " + State);
                            break;

                    }
                    if (Highlight == HighLight.Selected)
                    {
                        g.DrawRectangle(Selected, 0, 0, Width, Height);
                    }
                    else if (Highlight == HighLight.Target)
                    {
                        g.DrawRectangle(Target, 0, 0, Width, Height);
                    }

                    // apply square number
                    if (Square != 0)
                        g.DrawString(CellNumber[Square], Textfont, TextColor, 0, Textfont.Size);
                }
            }

            base.OnPaint(e);
            BackgroundImage = DoubleBuffer;
        }

        private static Bitmap LoadImage(string path, bool transparent = false)
        {
            var bitmap = new Bitmap(path);
            if (transparent)
            {
                bitmap.MakeTransparent(bitmap.GetPixel(0, 0));
            }
            return bitmap;
        }

        private Bitmap DoubleBuffer;
        private CellState State;
        private HighLight Highlight;
        private int SquareNum;
        private bool IsDirty;

        private static Font Textfont = new Font("Arial", 12);
        private static SolidBrush TextColor = new SolidBrush(Color.Black);
        private static Pen Target = new Pen(Color.Yellow, 10) { DashStyle = DashStyle.Dash };
        private static Pen Selected = new Pen(Color.White, 10) { DashStyle = DashStyle.Dash };
        private static Dictionary<CellState, Bitmap> Images;

        private static string[] CellNumber = Config.BoardNumbers; //Can be configured
    }
#endregion
}
