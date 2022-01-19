using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ToolKits.FunctionModule
{
    #region 泛型使用说明

    /*
        列出了五类约束：
            约束
            描述
            where T: struct
            类型参数必须为值类型。
            where T : class
            类型参数必须为类型。
            where T : new()
            类型参数必须有一个公有、无参的构造函数。当于其它约束联合使用时,new()约束必须放在最后。
            where T : <base class name>
            类型参数必须是指定的基类型或是派生自指定的基类型。
            where T : <interface name>
            类型参数必须是指定的接口或是指定接口的实现。可以指定多个接口约束。接口约束也可以是泛型的。
 
 
        类型参数的约束，增加了可调用的操作和方法的数量。这些操作和方法受约束类型及其派生层次中的类型的支持。因此，设计泛型类或方法时，如果对泛型成员执行任何赋值以外的操作，或者是调用System.Object中所没有的方法，就需要在类型参数上使用约束。
 
        无限制类型参数的一般用法
        没有约束的类型参数,如公有类MyClass<T>{...}中的T, 被称为无限制类型参数(unbounded type parameters)。无限制类型参数有以下规则:
             不能使用运算符 != 和 == ，因为无法保证具体的类型参数能够支持这些运算符。
             它们可以与System.Object相互转换，也可显式地转换成任何接口类型。
             可以与null比较。如果一个无限制类型参数与null比较，当此类型参数为值类型时，比较的结果总为false。
     */

    #endregion

    /// <summary>
    /// 序列化反序列化结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SerializeData<T>
    //where T : class,new()
    {
        /// <summary>
        /// 构造器
        /// </summary>
        static SerializeData()
        {
        }

        /// <summary>
        /// 通过序列化和反序列化的方式实现对象的克隆
        /// </summary>
        /// <param name="class_object"></param>
        /// <returns></returns>
        public static T Clone(T class_object)
        {
            try
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();

                //序列化
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
                Formatter.Serialize(stream, class_object);

                //反序列化
                stream.Position = 0;
                T clonedObj = (T)Formatter.Deserialize(stream);

                stream.Close();
                return clonedObj;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 序列化结果
        /// </summary>
        /// <returns></returns>
        public static void Serialize(string dirPath, string fileName, T class_object)
        {
            //防止路径为空白字符或不是路径字符导致的异常
            try
            {
                if (!System.IO.Directory.Exists(dirPath))
                    System.IO.Directory.CreateDirectory(dirPath);
            }
            catch
            {
                return;
            }
            dirPath = dirPath.TrimEnd('\\') + "\\" + fileName;
            System.IO.Stream sestream = null;
            try
            {
                //创建一个文件流             
                sestream = new System.IO.FileStream(dirPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None);

                //序列化
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(sestream, class_object);
            }
            catch (Exception)
            {
                //MsgModule.LogException(ex);
            }
            finally
            {
                if (sestream != null)
                    sestream.Close();
            }
        }

        /// <summary>
        /// 反序列结果
        /// </summary>
        /// <returns>反序列化不成功就返回null</returns>
        public static T Deserialize(string dirPath, string fileName)
        {
            //防止路径为空白字符或不是路径字符导致的异常
            try
            {
                if (!System.IO.Directory.Exists(dirPath))
                    return default(T);
            }
            catch
            {
                return default(T);
            }
            dirPath = dirPath.TrimEnd('\\') + "\\" + fileName;
            if (System.IO.File.Exists(dirPath))
            {
                System.IO.Stream destream = null;
                //程序会先执行finally，然后再执行return：
                try
                {
                    //打开一个文件流
                    destream = new System.IO.FileStream(dirPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                    //反序列化
                    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var deserializeResult = (T)formatter.Deserialize(destream);
                    return deserializeResult;
                }
                catch
                {
                    return default(T);
                }
                finally
                {
                    if (destream != null)
                        destream.Close();
                }
            }
            else
                return default(T);
        }
    }

    /// <summary>
    /// 利用反射实现的深拷贝
    /// </summary>
    public class ObjectCopy
    {
        public static object Copy(object obj)
        {
            Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //值类型
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            //引用类型
            else
            {
                targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
                        Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, Copy(fieldValue));
                        }

                    }
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = myProperty.GetValue(obj, null);
                            if (propertyValue is ICloneable)
                            {
                                myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
                            }
                        }

                    }
                }
            }
            return targetDeepCopyObj;
        }
    }

    /// <summary>
    /// 数据格式转换
    /// </summary>
    public class DataTransformer
    {
        #region 数据转换
        /// <summary>
        ///将对象转成流， T类型必须支持序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object_T"></param>
        /// <returns></returns>
        public static Stream Read<T>(T object_T)
        {
            System.IO.MemoryStream stream = null;

            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
                stream = new System.IO.MemoryStream();
                Formatter.Serialize(stream, object_T);
                return stream;
            }
            catch (Exception ex)
            {
                //MsgModule.LogException(ex);
                if (stream != null)
                    stream.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 将字节装换成数据，T类型必须支持序列化
        /// </summary>
        public static T Transfer<T>(byte[] buffer)
        {
            Stream stream = new System.IO.MemoryStream(buffer);
            return Transfer<T>(stream, true);
        }
        public static T Transfer<T>(byte[] buffer, Int32 index, Int32 count)
        {
            Stream stream = new System.IO.MemoryStream(buffer, index, count);
            return Transfer<T>(stream, true);
        }
        static T Transfer<T>(Stream stream, bool isCloseStream)
        {
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter(null, new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
                stream.Position = 0;
                T clonedObj = (T)Formatter.Deserialize(stream);
                if (isCloseStream)
                    stream.Close();
                return clonedObj;
            }
            catch (Exception ex)
            {
                //MsgModule.LogException(ex);
                throw ex;
            }
        }
        public static byte[] GetBuffer<T>(T object_T)
        {
            Stream stream = Read(object_T);
            return GetBuffer(stream, true);
        }
        public static byte[] GetBuffer(Stream stream, bool isCloseStream)
        {
            stream.Position = 0;

            List<byte> lstBuffer = new List<byte>();
            Int32 num;
            do
            {
                const Int32 bufferSize = 1024;
                byte[] bytes = new byte[bufferSize];
                num = stream.Read(bytes, 0, bufferSize);

                for (Int32 i = 0; i < num; i++)
                {
                    lstBuffer.Add(bytes[i]);
                }

            } while (num != 0);
            if (isCloseStream)
                stream.Close();
            return lstBuffer.ToArray();
        }        
        #endregion

        #region 数据交换
        public static void swap<T>(ref T src, ref T dest)
        {
            T var = src;
            src = dest;
            dest = var;
        }
        #endregion

        #region data transfer
        //// <summary>
        /// 结构体转byte数组
        /// </summary>
        /// <param name="structObj">要转换的结构体</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            Int32 size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }

        /// <summary>
        /// byte数组转结构体
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <param name="type">结构体类型</param>
        /// <returns>转换后的结构体</returns>
        public static object BytesToStuct(byte[] bytes, Type type)
        {
            //得到结构体的大小
            Int32 size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }
        #endregion
    }
}
