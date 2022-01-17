/* 
 * 飞拍单张图像信息
 * byLL 2018/8/7
 */

using HalconDotNet;
using System.Collections.Generic;

namespace MobileShooting
{
    public class ImageCache
    {
        /// <summary>
        /// 2d图片集合
        /// </summary>
        public HObject _2dImage;
        /// <summary>
        /// 2d图片名
        /// </summary>
        public List<string> _2dImgKeys;
        /// <summary>
        /// 3d图片集合
        /// </summary>
        public HObject _3dImage;
        /// <summary>
        /// 3d图片名
        /// </summary>
        public List<string> _3dImgKeys;
        /// <summary>
        /// 所在block
        /// </summary>
        public int b { get; set; }
        /// <summary>
        /// 所在拍照位的行
        /// </summary>
        public int r { get; set; }
        /// <summary>
        /// 所在拍照位的列
        /// </summary>
        public int c { get; set; }
        /// <summary>
        /// 当前拍照位X
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// 当前拍照位Y
        /// </summary>
        public double Y { get; set; }

        public ImageCache()
        {
            _2dImage = new HObject();
            HOperatorSet.GenEmptyObj(out _2dImage);
            _2dImgKeys = new List<string>();

            _3dImage = new HObject();
            HOperatorSet.GenEmptyObj(out _3dImage);
            _3dImgKeys = new List<string>();
            b = 0;
            r = 0;
            c = 0;
        }

        public void Dispose()
        {
            if (_2dImage != null) _2dImage.Dispose();
            if (_2dImgKeys != null) _2dImgKeys.Clear();
            if (_3dImage != null) _3dImage.Dispose();
            if (_3dImgKeys != null) _3dImgKeys.Clear();
            b = 0;
            r = 0;
            c = 0;
        }
    }
}
