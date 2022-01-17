using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace LeadframeAOI
{
    /// <summary>
    /// 实体属性处理
    /// </summary>
    public class PropertyHandle
    {
        #region 反射控制只读、可见属性
        //SetPropertyVisibility(obj,   "名称 ",   true); 
        //obj指的就是你的SelectObject，   “名称”是你SelectObject的一个属性 
        //当然，调用这两个方法后，重新SelectObject一下，就可以了。
        /// <summary>
        /// 通过反射控制属性是否只读
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="readOnly"></param>
        public static void SetPropertyReadOnly(object obj, string propertyName, bool readOnly)
        {
            Type type = typeof(ReadOnlyAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("isReadOnly", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
            fld.SetValue(attrs[type], readOnly);
        }

        /// <summary>
        /// 通过反射控制属性是否可见
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="visible"></param>
        public static void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
            fld.SetValue(attrs[type], visible);
        }
        #endregion
    }

}
