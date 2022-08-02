using System;
using System.IO;
using System.Text;

namespace ToolKits.DataManage.Xml
{
    public class SerializeFileTool<T>
    {
        public T GetConfig(string fileName)
        {
            T local = default(T);
            byte[] bStr = null;
            if (this.ReadBytes(fileName, out bStr) == FileOperationResult.OK)
            {
                local = SerializeTool<T>.XmlSerializerDeserialize(bStr);
            }
            return local;
        }

        private FileOperationResult ReadBytes(string path, out byte[] bStr)
        {
            bStr = null;
            if (!File.Exists(path))
            {
                return FileOperationResult.NOTEXIST;
            }
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    bStr = new byte[stream.Length];
                    stream.Read(bStr, 0, bStr.Length);
                }
            }
            catch
            {
                return FileOperationResult.WRITEERROR;
            }
            return FileOperationResult.OK;
        }

        public void SetConfig(string fileName, T config)
        {
            this.WriteBytes(fileName, FileMode.Create, SerializeTool<T>.XmlSerializerSerialize(config));
        }

        private FileOperationResult WriteBytes(string path, FileMode filemode, byte[] bStr)
        {
            try
            {
                using (FileStream stream = new FileStream(path, filemode, FileAccess.ReadWrite))
                {
                    stream.Write(bStr, 0, bStr.Length);
                }
            }
            catch (Exception)
            {
                return FileOperationResult.WRITEERROR;
            }
            return FileOperationResult.OK;
        }

        private FileOperationResult WriteString(string path, FileMode filemode, string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return this.WriteBytes(path, filemode, bytes);
        }
    }
}

