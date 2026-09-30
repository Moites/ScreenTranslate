using ScreanTranslate.View;
using ScreanTranslate.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace ScreanTranslate.Model
{
    public class CopyScreen
    {
        private static Rectangle rectangle = Screen.GetBounds(Point.Empty);
        private static bool MinScreen;
        public static string Base64 {  get; set; }
        public static bool MakeScreen()
        {
            var choice = new ChoiceAreaWindow();

            var viewModel = (ChoiceAreaWindowModel)choice.DataContext;
            viewModel.SelectionCompleted += (Area) =>
            {
                MakeScreenshot(Area);
            };

            choice.ShowDialog();
            
            return MinScreen;
        }
        private static void MakeScreenshot(Rectangle rectangle)
        {
            using (Graphics screenGraphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = screenGraphics.DpiX / 96.0f;
                float dpiY = screenGraphics.DpiY / 96.0f;

                if (rectangle.Width < 10 || rectangle.Height < 10)
                {
                    MinScreen = false;
                    return;
                }
                else
                {
                    MinScreen = true;
                }

                Rectangle scaledRect = new Rectangle(
                    (int)(rectangle.X * dpiX),
                    (int)(rectangle.Y * dpiY),
                    (int)(rectangle.Width * dpiX),
                    (int)(rectangle.Height * dpiY)
                );

                using (var bitmap = new Bitmap(scaledRect.Width, scaledRect.Height))
                using (var g = Graphics.FromImage(bitmap))
                using (var memoryStream = new MemoryStream())
                {
                    g.CopyFromScreen(
                        scaledRect.X,
                        scaledRect.Y,
                        0,
                        0,
                        scaledRect.Size,
                        CopyPixelOperation.SourceCopy);

                    bitmap.Save(memoryStream, ImageFormat.Jpeg);

                    Base64 = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
    }
}
