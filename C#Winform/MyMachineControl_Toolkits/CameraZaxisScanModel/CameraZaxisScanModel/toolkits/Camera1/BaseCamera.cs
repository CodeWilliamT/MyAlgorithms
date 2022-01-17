using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace HTCamera
{
    /// <summary>
    /// 相机类型
    /// </summary>
    [System.Serializable]
    public enum CameraEnum
    {
        /// <summary>
        /// basler相机使用NI工具
        /// </summary>
        BaslerNI = -1,
        /// <summary>
        /// basler相机
        /// </summary>
        Basler = 0,
        /// <summary>
        /// 灰点相机
        /// </summary>
        PointGray = 1,
        /// <summary>
        /// SVS相机
        /// </summary>
        SVS = 2,
        /// <summary>
        /// matrox cameralink采集卡+basler相机
        /// </summary>
        BaslerMatroxCamLink=3
    }
    /// <summary>
    /// Basler相机使用NI库的像素格式
    /// </summary>
    [System.Serializable]
    public enum BaslerFromNIPixelFormat
    {
        Mono_8 = 17301505,
        Mono_12 = 17825797,
        Mono_12_packed = 17563654,
        YUV_422_packed = 34603039,
        YUV_422_YUYV__packed = 34603058
    }
    /// <summary>
    /// Basler相机像素格式
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BaslerPixelFormat
    {
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer BG 10.
        //     Applies to: CameraLink, GigE
        public static string BayerBG10 = "BayerBG10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer BG 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerBG12 = "BayerBG12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer BG 12p.
        //     Applies to: USB
        public static string BayerBG12p = "BayerBG12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer BG 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerBG12Packed = "BayerBG12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer BG 16.
        //     Applies to: CameraLink, GigE
        public static string BayerBG16 = "BayerBG16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer BG 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerBG8 = "BayerBG8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GB 10.
        //     Applies to: CameraLink, GigE
        public static string BayerGB10 = "BayerGB10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GB 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGB12 = "BayerGB12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GB 12p.
        //     Applies to: USB
        public static string BayerGB12p = "BayerGB12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GB 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerGB12Packed = "BayerGB12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GB 16.
        //     Applies to: CameraLink, GigE
        public static string BayerGB16 = "BayerGB16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GB 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGB8 = "BayerGB8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GR 10.
        //     Applies to: CameraLink, GigE
        public static string BayerGR10 = "BayerGR10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GR 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGR12 = "BayerGR12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GR 12p.
        //     Applies to: USB
        public static string BayerGR12p = "BayerGR12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GR 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerGR12Packed = "BayerGR12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GR 16.
        //     Applies to: CameraLink, GigE
        public static string BayerGR16 = "BayerGR16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GR 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGR8 = "BayerGR8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer RG 10.
        //     Applies to: CameraLink, GigE
        public static string BayerRG10 = "BayerRG10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer RG 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerRG12 = "BayerRG12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer RG 12p.
        //     Applies to: USB
        public static string BayerRG12p = "BayerRG12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer RG 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerRG12Packed = "BayerRG12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer RG 16.
        //     Applies to: CameraLink, GigE
        public static string BayerRG16 = "BayerRG16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer RG 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerRG8 = "BayerRG8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGR 10 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGR10Packed = "BGR10Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGR 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGR12Packed = "BGR12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to BGR 8.
        //     Applies to: USB
        public static string BGR8 = "BGR8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGR 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGR8Packed = "BGR8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGRA 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGRA8Packed = "BGRA8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 10.
        //     Applies to: CameraLink, GigE
        public static string Mono10 = "Mono10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 10 Packed.
        //     Applies to: CameraLink, GigE
        public static string Mono10Packed = "Mono10Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Mono 12.
        //     Applies to: CameraLink, GigE, USB
        public static string Mono12 = "Mono12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Mono 12p.
        //     Applies to: USB
        public static string Mono12p = "Mono12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string Mono12Packed = "Mono12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 16.
        //     Applies to: CameraLink, GigE
        public static string Mono16 = "Mono16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Mono 8.
        //     Applies to: CameraLink, GigE, USB
        public static string Mono8 = "Mono8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 8 Signed.
        //     Applies to: CameraLink, GigE
        public static string Mono8Signed = "Mono8Signed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB10Packed = "RGB10Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB10Planar = "RGB10Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10V1 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB10V1Packed = "RGB10V1Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10V2 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB10V2Packed = "RGB10V2Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB12Packed = "RGB12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 12 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB12Planar = "RGB12Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB12V1Packed = "RGB12V1Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 16 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB16Planar = "RGB16Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to RGB 8.
        //     Applies to: USB
        public static string RGB8 = "RGB8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB8Packed = "RGB8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 8 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB8Planar = "RGB8Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGBA 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGBA8Packed = "RGBA8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to YCbCr 422.
        //     Applies to: USB
        public static string YCbCr422_8 = "YCbCr422_8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 411 Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV411Packed = "YUV411Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 422 (YUYV) Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV422_YUYV_Packed = "YUV422_YUYV_Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 422 Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV422Packed = "YUV422Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 444 Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV444Packed = "YUV444Packed";
    }
    /// <summary>
    /// PointGray相机像素格式
    /// </summary>
    [System.Serializable]
    public enum PointGreyPixelFormat
    {
        PixelFormatMono8 = -2147483648,
        PixelFormatBgr = -2147483640,
        NumberOfPixelFormats = 20,
        PixelFormatRaw12 = 524288,
        PixelFormatMono12 = 1048576,
        PixelFormatRaw16 = 2097152,
        PixelFormatRaw8 = 4194304,
        PixelFormatSignedRgb16 = 8388608,
        PixelFormatSignedMono16 = 16777216,
        PixelFormatRgb16 = 33554432,
        PixelFormatBgr16 = 33554433,
        PixelFormatBgru16 = 33554434,
        PixelFormatMono16 = 67108864,
        PixelFormatRgb8 = 134217728,
        PixelFormatRgb = 134217728,
        PixelFormat444Yuv8 = 268435456,
        PixelFormat422Yuv8 = 536870912,
        PixelFormat411Yuv8 = 1073741824,
        PixelFormat422Yuv8Jpeg = 1073741825,
        PixelFormatRgbu = 1073741826,
        PixelFormatBgru = 1073741832,
    }

    class BasePixelFormat
    {
        #region BaslerFromNIPixelFormat
        public static int Mono_8 = 17301505;
        public static int Mono_12 = 1782579;
        public static int Mono_12_packed = 17563654;
        public static int YUV_422_packed = 34603039;
        public static int YUV_422_YUYV__packed = 34603058;
        #endregion

        #region BaslerPixelFormat
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer BG 10.
        //     Applies to: CameraLink, GigE
        public static string BayerBG10 = "BayerBG10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer BG 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerBG12 = "BayerBG12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer BG 12p.
        //     Applies to: USB
        public static string BayerBG12p = "BayerBG12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer BG 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerBG12Packed = "BayerBG12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer BG 16.
        //     Applies to: CameraLink, GigE
        public static string BayerBG16 = "BayerBG16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer BG 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerBG8 = "BayerBG8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GB 10.
        //     Applies to: CameraLink, GigE
        public static string BayerGB10 = "BayerGB10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GB 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGB12 = "BayerGB12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GB 12p.
        //     Applies to: USB
        public static string BayerGB12p = "BayerGB12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GB 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerGB12Packed = "BayerGB12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GB 16.
        //     Applies to: CameraLink, GigE
        public static string BayerGB16 = "BayerGB16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GB 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGB8 = "BayerGB8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GR 10.
        //     Applies to: CameraLink, GigE
        public static string BayerGR10 = "BayerGR10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GR 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGR12 = "BayerGR12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GR 12p.
        //     Applies to: USB
        public static string BayerGR12p = "BayerGR12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GR 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerGR12Packed = "BayerGR12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer GR 16.
        //     Applies to: CameraLink, GigE
        public static string BayerGR16 = "BayerGR16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer GR 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerGR8 = "BayerGR8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer RG 10.
        //     Applies to: CameraLink, GigE
        public static string BayerRG10 = "BayerRG10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer RG 12.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerRG12 = "BayerRG12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer RG 12p.
        //     Applies to: USB
        public static string BayerRG12p = "BayerRG12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer RG 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BayerRG12Packed = "BayerRG12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Bayer RG 16.
        //     Applies to: CameraLink, GigE
        public static string BayerRG16 = "BayerRG16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Bayer RG 8.
        //     Applies to: CameraLink, GigE, USB
        public static string BayerRG8 = "BayerRG8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGR 10 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGR10Packed = "BGR10Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGR 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGR12Packed = "BGR12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to BGR 8.
        //     Applies to: USB
        public static string BGR8 = "BGR8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGR 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGR8Packed = "BGR8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to BGRA 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string BGRA8Packed = "BGRA8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 10.
        //     Applies to: CameraLink, GigE
        public static string Mono10 = "Mono10";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 10 Packed.
        //     Applies to: CameraLink, GigE
        public static string Mono10Packed = "Mono10Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Mono 12.
        //     Applies to: CameraLink, GigE, USB
        public static string Mono12 = "Mono12";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Mono 12p.
        //     Applies to: USB
        public static string Mono12p = "Mono12p";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string Mono12Packed = "Mono12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 16.
        //     Applies to: CameraLink, GigE
        public static string Mono16 = "Mono16";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to Mono 8.
        //     Applies to: CameraLink, GigE, USB
        public static string Mono8 = "Mono8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to Mono 8 Signed.
        //     Applies to: CameraLink, GigE
        public static string Mono8Signed = "Mono8Signed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB10Packed = "RGB10Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB10Planar = "RGB10Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10V1 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB10V1Packed = "RGB10V1Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 10V2 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB10V2Packed = "RGB10V2Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB12Packed = "RGB12Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 12 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB12Planar = "RGB12Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 12 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB12V1Packed = "RGB12V1Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 16 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB16Planar = "RGB16Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to RGB 8.
        //     Applies to: USB
        public static string RGB8 = "RGB8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGB8Packed = "RGB8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGB 8 Planar.
        //     Applies to: CameraLink, GigE
        public static string RGB8Planar = "RGB8Planar";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to RGBA 8 Packed.
        //     Applies to: CameraLink, GigE
        public static string RGBA8Packed = "RGBA8Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format of the image data transmitted
        //     by the camera to YCbCr 422.
        //     Applies to: USB
        public static string YCbCr422_8 = "YCbCr422_8";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 411 Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV411Packed = "YUV411Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 422 (YUYV) Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV422_YUYV_Packed = "YUV422_YUYV_Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 422 Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV422Packed = "YUV422Packed";
        //
        // 摘要: 
        //     This enumeration value sets the pixel format to YUV 444 Packed.
        //     Applies to: CameraLink, GigE
        public static string YUV444Packed = "YUV444Packed";
        #endregion

        #region PointGreyPixelFormat
        public static int  PixelFormatMono8 = -2147483648;
        public static int  PixelFormatBgr = -2147483640;
        public static int  NumberOfPixelFormats = 20;
        public static int  PixelFormatRaw12 = 524288;
        public static int  PixelFormatMono12 = 1048576;
        public static int  PixelFormatRaw16 = 2097152;
        public static int  PixelFormatRaw8 = 4194304;
        public static int  PixelFormatSignedRgb16 = 8388608;
        public static int  PixelFormatSignedMono16 = 16777216;
        public static int  PixelFormatRgb16 = 33554432;
        public static int  PixelFormatBgr16 = 33554433;
        public static int  PixelFormatBgru16 = 33554434;
        public static int  PixelFormatMono16 = 67108864;
        public static int  PixelFormatRgb8 = 134217728;
        public static int  PixelFormatRgb = 134217728;
        public static int  PixelFormat444Yuv8 = 268435456;
        public static int  PixelFormat422Yuv8 = 536870912;
        public static int  PixelFormat411Yuv8 = 1073741824;
        public static int  PixelFormat422Yuv8Jpeg = 1073741825;
        public static int  PixelFormatRgbu = 1073741826;
        public static int  PixelFormatBgru = 1073741832;
        #endregion
    }
    public abstract class BaseCamera
    {
        public abstract bool IsConnected { get; }
        public virtual bool IsSoftwareTrigger { get; set; }
        public virtual bool IsMirrorX { get; set; }
        public virtual bool IsMirrorY { get; set; }
        public virtual bool InitCamera() { return true; }
        public virtual bool OpenCamera() { return true; }
        public virtual bool CloseCamera() { return true; }
        public virtual bool ChangeTriggerSource(bool isSoftwareTrigger) { return true; }
        public virtual bool SetGain(double value) { return true; }
        public virtual void GetGain(out double value) { value = -1.0; }
        public virtual bool SetExposure(double value) { return true; }
        public virtual void GetExposure(out double value) { value = -1.0; }
        //public bool SetPixelFormat(object pixelFormat)
        //{
        //bool val=false;
        //switch (this.camera_type)
        //{
        //    case CameraEnum.BaslerNI:
        //        val= BaslerFromNISetPixelFormat((BaslerFromNIPixelFormat)pixelFormat);
        //        break;
        //    case CameraEnum.Basler:
        //        val=BaslerSetPixelFormat(pixelFormat.ToString());
        //        break;
        //    case CameraEnum.PointGray:
        //        val=PointGraySetPixelFormat((PointGreyPixelFormat)pixelFormat);
        //        break;
        //    case CameraEnum.SVS:
        //        break;
        //    default:
        //        MessageBox.Show("未知的像素格式");
        //        return false;
        //}
        //return val;
        //    return true;
        //}
        //public  void GetPixelFormat(out object pixelFormat)
        //{
        //    pixelFormat = "";
        //switch (this.camera_type)
        //{
        //    case CameraEnum.BaslerNI:
        //        BaslerFromNIPixelFormat _pixelFormat1;
        //        BaslerFromNIGetPixelFormat(out _pixelFormat1);
        //        pixelFormat = _pixelFormat1;
        //        break;
        //    case CameraEnum.Basler:
        //        string _pixelFormat2;
        //        BaslerGetPixelFormat(out _pixelFormat2);
        //        pixelFormat = _pixelFormat2;
        //        break;
        //    case CameraEnum.PointGray:
        //        PointGreyPixelFormat _pixelFormat3;
        //        PointGrayGetPixelFormat(out _pixelFormat3);
        //        pixelFormat = _pixelFormat3;
        //        break;
        //    case CameraEnum.SVS:
        //        break;
        //    default:
        //        break;
        //}
        //}
        public virtual bool BaslerFromNISetPixelFormat(BaslerFromNIPixelFormat pixelFormat) { return true; }
        public virtual bool BaslerSetPixelFormat(string pixelFormat) { return true; }
        public virtual bool PointGraySetPixelFormat(PointGreyPixelFormat pixelFormat) { return true; }
        public virtual void BaslerFromNIGetPixelFormat(out BaslerFromNIPixelFormat pixelFormat)
        {
            object _pixelFormat = "";
            pixelFormat = (BaslerFromNIPixelFormat)_pixelFormat;
        }
        public virtual void BaslerGetPixelFormat(out string pixelFormat) { pixelFormat = ""; }
        public virtual void PointGrayGetPixelFormat(out PointGreyPixelFormat pixelFormat)
        {
            object _pixelFormat = "";
            pixelFormat = (PointGreyPixelFormat)_pixelFormat;
        }
        public virtual Acqusition Snap(int imageNum=1, int timeOut=-1) { return new Acqusition(); }
        public virtual Acqusition Hard_Snap() { return new Acqusition(); }
        public virtual Acqusition GetFrames(int imageNum = 1, int timeOut = -1) { return new Acqusition(); }
        public virtual Task<Acqusition> GetFramesAsync(int imageNum = 1, int timeOut = -1)
        {
            return Task.Run(() =>
                {
                    return new Acqusition();
                });
        }
        public virtual bool ClearImageQueue() { return true; }
    }

    public partial class Resource : EventArgs
    {
        public Resource() { }
        //public Resource(Acqusition a)
        //{
        //    this.Image = a.Image;
        //    this.GrabStatus = a.GrabStatus;
        //    this.Error = a.Error;
        //}
        //public HObject Image = new HObject();// { get; set; }
        //public string GrabStatus;// { get; set; }
        //public string Error;// { get; set; }
        public Queue<Acqusition> GrabResult = new Queue<Acqusition>();
    }
    public class Acqusition
    {
        public Acqusition()
        {
            HOperatorSet.GenEmptyObj(out Image);
            GrabStatus = "";
            index = -1;
        }

        public HObject Image = new HObject();// { get; set; }
        public string GrabStatus;// { get; set; }
        /// <summary>
        /// 当前图像索引,初始值为-1
        /// </summary>
        public int index;// { get; set; }
        public void Dispose()
        {
            this.Image.Dispose();
            this.GrabStatus = "";
            this.index = -1;
        }
    }
}
