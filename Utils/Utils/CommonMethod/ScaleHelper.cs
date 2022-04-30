//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;

namespace Utils
{
    /// <summary>
    /// High DPI display related helpers
    /// </summary>
    public static class ScaleHelper
    {
        private const float defaultScaleX = 1;

        /// <summary>
        /// Gets the scaled size on high dpi display
        /// </summary>
        /// <param name="originalSize">the original size</param>
        /// <returns>the scaled size</returns>
        public static int GetScaledSize(int originalSize = 16)
        {
            return Convert.ToInt32(originalSize * GetDisplayScaleFactor());
        }

        /// <summary>
        /// Gets the scale factor on high dpi display
        /// </summary>
        /// <returns>the scale factor</returns>
        public static float GetDisplayScaleFactor()
        {
            try
            {
                IntPtr MainWindow = Process.GetCurrentProcess().MainWindowHandle;
                using (Graphics g = Graphics.FromHwnd(MainWindow))
                {
                    return (float)(g.DpiX / 96);
                }
            }
            catch
            {
                return defaultScaleX;
            }
        }

        /// <summary>
        /// Get Scaled Bitmap
        /// </summary>
        /// <param name="icon">Original Bitmap</param>
        /// <returns>Scaled Bitmap</returns>
        public static Bitmap GetScaledBitmap(Bitmap bitmap)
        {
            float scaleFactor = DpiUtil.GetDisplayScaleFactor();
            Bitmap scaledBitmap = new Bitmap(bitmap, (int)(bitmap.Width * scaleFactor), (int)(bitmap.Height * scaleFactor));
            return scaledBitmap;
        }

        /// <summary>
        /// Get Scaled Icon
        /// </summary>
        /// <param name="icon">Original Icon</param>
        /// <returns>Scaled Icon</returns>
        public static Icon GetScaledIcon(Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            Bitmap scaledBitmap = GetScaledBitmap(bitmap);
            Icon scaledIcon = Icon.FromHandle(scaledBitmap.GetHicon());
            bitmap.Dispose();
            scaledBitmap.Dispose();
            return scaledIcon;
        }
    }
}
