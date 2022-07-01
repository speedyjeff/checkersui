using SkiaSharp;


namespace CheckersUiMaui
{
    internal static class Utilities
    {
        public static SKBitmap FromResource(string name)
        {
            var assembly = typeof(MainPage).Assembly;
            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null) System.Diagnostics.Debug.WriteLine($"failed to load resource {name}");
                // load image
                return SKBitmap.Decode(stream);
            }
        }

        public static SKBitmap MakeTransparent(SKBitmap bitmap, SKColor color)
        {
            var count = bitmap.Width * bitmap.Height;
            var pixels = new SKColor[count];
            var span = new Span<SKColor>(pixels);

            // copy the current set of pixels
            bitmap.Pixels.CopyTo(span);

            // change all pixels to transparent that match the color
            var changed = 0L;
            for (int n = 0; n < count; n++)
            {
                if (pixels[n].Red == color.Red &&
                    pixels[n].Green == color.Green &&
                    pixels[n].Blue == color.Blue)
                {
                    // mark as transparent
                    pixels[n] = Transparent;
                    changed++;
                }
            }

            // if there was an update, update the pixels
            if (changed > 0)
            {
                bitmap.Pixels = pixels;
            }

            return bitmap;
        }

        public static SKBitmap Rotate(SKBitmap bitmap, float angle)
        {
            // no translation
            if (angle == 0f) return bitmap;

            // calculations
            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            // create the new image
            var rotatedBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

            // translate
            using (var surface = new SKCanvas(rotatedBitmap))
            {
                surface.Translate(rotatedWidth / 2, rotatedHeight / 2);
                surface.RotateDegrees((float)angle);
                surface.Translate(-originalWidth / 2, -originalHeight / 2);
                surface.DrawBitmap(bitmap, new SKPoint());
            }
            return rotatedBitmap;
        }

        #region private
        private static SKColor Transparent = new SKColor(0, 0, 0, 0);
        #endregion
    }
}
