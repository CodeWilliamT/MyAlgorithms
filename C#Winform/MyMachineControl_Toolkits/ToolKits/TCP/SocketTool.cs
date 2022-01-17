using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using AutomaticAOI;


namespace ToolKits.SocketTool
{
    //服务器类
    public class TCPServer
    {
        private TcpListener tcpListener = null;
        private TcpClient tcpClient;
        private IPAddress localIP;
        private NetworkStream stream;
        private int port;
        private int buffer_length;
        private Type type;
        private bool isConnected;
        Thread myThread = null;
        CancellationTokenSource cts;

        MainForm mf = MainForm.GetMainForm();
        /// <summary>
        /// 用于服务器
        /// </summary>
        public ConcurrentQueue<ComData> comDataQueue;
        /// <summary>
        /// 接收队列的上限
        /// </summary>
        public int comDQLength = 100;//指令集队列长度上限
        public event EventHandler<Resource> serverListening;
        public bool IsConnected
        {
            get { return this.isConnected; }
        }

        #region 图像、轮廓、大数组缓冲区
        //暂时未定义
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localIp"></param>
        /// <param name="port"></param>
        /// <param name="dataType">数据包类型</param>
        public TCPServer(string localIp, int port, Type dataType)
        {
            this.type = dataType;
            this.localIP = IPAddress.Parse(localIp);
            this.port = port;
            this.buffer_length = Marshal.SizeOf(dataType);
            comDataQueue = new ConcurrentQueue<ComData>();
        }
        public bool TcpServerStart()
        {
            try
            {
                this.tcpListener = new TcpListener(localIP, port);
                this.tcpListener.Start();

                ThreadStart myThreadDelegate = new ThreadStart(Listening);
                //实例化新线程
                cts = new CancellationTokenSource();
                myThread = new Thread(myThreadDelegate);
                myThread.IsBackground = true;
                myThread.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Listening()
        {
            try
            {
                if (this.tcpListener == null)
                {
                    this.tcpListener = new TcpListener(this.localIP, this.port);
                    this.tcpListener.Start();
                }
                // 完成一个阻塞调用来接受请求。
                // 你同样可以在这里使用 AcceptSocket()。
                //if (!this.unloadTcpListener.Pending()) continue;
                Resource r3 = new Resource();
                r3.IsConnected = false;
                r3.ErrStatus = "服务器(ip=" + this.localIP.ToString() + ")已开启";
                if (serverListening != null)
                    serverListening(this, r3);
                this.isConnected = false;

                this.tcpClient = tcpListener.AcceptTcpClient();
                Resource r4 = new Resource();
                r4.IsConnected = true;
                r4.ErrStatus = "客户端(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")连接成功";
                if (serverListening != null)
                    serverListening(this, r4);
                this.isConnected = true;

                // 获取一个数据流对象来进行读取和写入
                this.stream = tcpClient.GetStream();
                while (this.stream.CanRead)
                {
                    if (cts.Token.IsCancellationRequested) return;

                    byte[] buffer = new byte[buffer_length];
                    int bytesLength = this.stream.Read(buffer, 0, buffer.Length);
                    #region 异常情况，客户端突然断开
                    if (bytesLength==0)
                    {
                        CloseAll();
                        Resource r1 = new Resource();
                        r1.IsConnected = false;
                        r1.ErrStatus = "客户端(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")连接断开";
                        if (serverListening != null)
                            serverListening(this, r1);
                        this.isConnected = false;
                        Thread.Sleep(500);
                        while (true)
                        {
                            try
                            {
                                TcpServerStart();
                                break;
                            }
                            catch (Exception)
                            {
                            }
                        }
                        break;
                    }
                    #endregion
                    //判断之前队列长度是否达到（上限-1）
                    if (comDataQueue.Count == comDQLength - 1)
                    {
                        //达到上限后需要执行的操作
                    }
                    //string str = Encoding.Default.GetString(buffer);
                    object msg = BytesToStuct(buffer, this.type);
                    if (msg == null) continue;
                    //EventData iData = new EventData(msg);
                    //listenEventCreater.ChangeValue(iData);

                    ComData _comData = (ComData)msg;
                    _comData.status = 1;
                    //分析发来的指令信号，如果有准备接收图像、准备接收轮廓，准备接收大数组信号，提前准备
                    if (false)//准备接收图像的信号
                    { }
                    else if (false)//准备接收轮廓的信号
                    { }
                    else if (false)//准备接收大数组的信号
                    { }
                    else //正常信号
                    {
                        //系统运行正常，接收到中断指令
                        if ((!mf.sys_quit) && (_comData.quit == 1))
                        {
                            mf.sys_quit = true;
                            return;
                        }
                        //系统处于中断状态时，接收中断指令
                        if ((mf.sys_quit) && (_comData.quit == 1))
                        {
                            //删除当前正在执行的指令
                            comDataQueue.TryDequeue(out _comData);
                            return;
                        }
                        //系统处于中断指令时，接收非中断指令
                        if ((mf.sys_quit) && (_comData.quit == 0))
                        {
                            //从当前指令恢复运行
                            mf.sys_quit = false;
                            return;
                        }
                        //指令入队
                        if (comDataQueue == null) comDataQueue = new ConcurrentQueue<ComData>();
                        else comDataQueue.Enqueue(_comData);
                    }
                }
            }
            catch (System.IO.IOException)
            {
                CloseAll();
                Resource r1 = new Resource();
                r1.IsConnected = false;
                r1.ErrStatus = "客户端(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")连接断开";
                if (serverListening != null)
                    serverListening(this, r1);
                this.isConnected = false;
                Thread.Sleep(500);
                while (true)
                {
                    try
                    {
                        TcpServerStart();

                        break;
                    }
                    catch (Exception)
                    {
                    }
                }
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
            }
        }
        public bool SendMessage(string sendMsg)
        {
            //把成 ASCII 字符串转化数据字符。
            byte[] sendMsgbyte = null;
            try
            {
                this.stream = this.tcpClient.GetStream();
                if (this.stream.CanWrite)
                {
                    sendMsgbyte = System.Text.Encoding.ASCII.GetBytes(sendMsg);
                    stream.Write(sendMsgbyte, 0, sendMsgbyte.Length);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool SendMessage(object sendMsg)
        {
            //把成 ASCII 字符串转化数据字符。
            byte[] sendMsgbyte = null;
            try
            {
                this.stream = this.tcpClient.GetStream();
                if (this.stream.CanWrite)
                {
                    sendMsgbyte = StructToBytes(sendMsg);
                    stream.Write(sendMsgbyte, 0, sendMsgbyte.Length);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SendBytes(byte[] buffer)
        {
            try
            {
                this.stream = this.tcpClient.GetStream();
                if (this.stream.CanWrite)
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <not used>
        /// not used
        /// </summary>
        /// <param name="receiveMsg"></param>
        public bool ReceiveMessage(out string receiveMsg)
        {
            receiveMsg = "";
            try
            {
                // 完成一个阻塞调用来接受请求。
                // 你同样可以在这里使用 AcceptSocket()。
                //if (!this.unloadTcpListener.Pending()) continue;
                //this.tcpClient = tcpListener.AcceptTcpClient();
                // 获取一个数据流对象来进行读取和写入
                stream = tcpClient.GetStream();
                if (this.stream.CanRead)
                {
                    byte[] buffer = new byte[this.tcpClient.ReceiveBufferSize];
                    this.stream.Read(buffer, 0, this.tcpClient.ReceiveBufferSize);
                    receiveMsg = Encoding.Default.GetString(buffer);
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool ReceiveMessage(out object receiveMsg)
        {
            receiveMsg = null;
            try
            {
                // 完成一个阻塞调用来接受请求。
                // 你同样可以在这里使用 AcceptSocket()。
                //if (!this.unloadTcpListener.Pending()) continue;
                //this.tcpClient = tcpListener.AcceptTcpClient();
                // 获取一个数据流对象来进行读取和写入
                stream = tcpClient.GetStream();
                if (this.stream.CanRead)
                {
                    byte[] buffer = new byte[buffer_length];
                    this.stream.Read(buffer, 0, buffer_length);
                    receiveMsg = BytesToStuct(buffer, this.type);
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool ReceiveMessage(out ComData receiveMsg)
        {
            receiveMsg = new ComData(0);
            try
            {
                // 完成一个阻塞调用来接受请求。
                // 你同样可以在这里使用 AcceptSocket()。
                //if (!this.unloadTcpListener.Pending()) continue;
                //this.tcpClient = tcpListener.AcceptTcpClient();
                // 获取一个数据流对象来进行读取和写入
                stream = tcpClient.GetStream();
                if (this.stream.CanRead)
                {
                    byte[] buffer = new byte[buffer_length];
                    this.stream.Read(buffer, 0, buffer_length);
                    object recvMsg = BytesToStuct(buffer, this.type);
                    receiveMsg = (ComData)recvMsg;
                    receiveMsg.status = 1;
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //// <summary>
        /// 结构体转byte数组
        /// </summary>
        /// <param name="structObj">要转换的结构体</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
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
            int size = Marshal.SizeOf(type);
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

        public void CloseAll()
        {
            this.CloseResource();
        }
        private void CloseResource()
        {
            if (this != null)
            {
                cts.Cancel();
                tcpListener.Stop();
                if (this.stream != null)
                {
                    this.stream.Close();
                    this.stream.Dispose();
                }
                if (this.tcpClient != null)
                {
                    this.tcpClient.Close();
                    this.tcpClient = null;
                }
            }
        }

    }
    //客户端类
    public class TCPClient
    {
        //private TcpListener tcpListener = null;
        private TcpClient tcpClient;
        private IPAddress localIP;
        private NetworkStream stream;
        private int port;
        private Type type;
        private int buffer_length = 0;
        private bool isConnected;
        Thread myThread = null;
        CancellationTokenSource cts;

        /// <summary>
        /// 用于客户端
        /// </summary>
        public ConcurrentDictionary<int, ComData> comDataDic;
        /// <summary>
        /// 接收队列的上限
        /// </summary>
        public int comDDLength = 100;
        public event EventHandler<Resource> clientListening;
        public ToolKits.EventTool.ListenEventCreater listenCreater;
        /// <summary>
        /// 客户端的连接状态
        /// </summary>
        public bool IsConnected
        {
            get { return this.isConnected; }
        }

        public TCPClient(string localIp, int port, Type dataType)
        {
            this.type = dataType;
            this.localIP = IPAddress.Parse(localIp);
            this.port = port;
            this.buffer_length = Marshal.SizeOf(dataType);
            comDataDic = new ConcurrentDictionary<int, ComData>();
            listenCreater = new ToolKits.EventTool.ListenEventCreater();
        }
        public bool TcpClientStart()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(this.localIP, this.port);

                Resource r = new Resource();
                r.IsConnected = true;
                r.ErrStatus = "与服务器(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")连接成功";
                if (this.clientListening != null)
                    clientListening(this, r);
                this.isConnected = true;

                ThreadStart myThreadDelegate = new ThreadStart(Listening);
                //实例化新线程
                cts = new CancellationTokenSource();
                myThread = new Thread(myThreadDelegate);
                myThread.IsBackground = true;
                myThread.Start();
                return true;
            }
            catch (Exception)
            {
                Resource r = new Resource();
                r.IsConnected = false;
                r.ErrStatus = "与服务器(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")连接失败";
                if (this.clientListening != null)
                    clientListening(this, r);
                this.isConnected = false;
                return false;
            }

        }
        private void Listening()
        {
            try
            {
                while (true)
                {
                    // 获取一个数据流对象来进行读取和写入
                    this.stream = tcpClient.GetStream();
                    if (this.stream.CanRead)
                    {
                        if (cts.Token.IsCancellationRequested) return;
                        byte[] buffer = new byte[buffer_length];
                        int bytesLength = this.stream.Read(buffer, 0, buffer.Length);
                        #region 异常情况，服务器突然关闭
                        if (bytesLength == 0)
                        {
                            CloseAll();

                            Resource r1 = new Resource();
                            r1.IsConnected = false;
                            r1.ErrStatus = "服务器(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")已关闭";
                            if (this.clientListening != null)
                                clientListening(this, r1);
                            this.isConnected = false;
                            //Thread.Sleep(500);

                            //while (true)
                            //{
                            //    try
                            //    {
                            //        TcpClientStart();
                            //        break;
                            //    }
                            //    catch (Exception)
                            //    {
                            //    }
                            //}
                            //break;
                        }
                        #endregion
                        object msg = BytesToStuct(buffer, this.type);
                        if (msg == null) continue;
                        //lock (this)
                        {
                            if (comDataDic.Count == comDDLength - 1)//队列长度到达上限时的操作
                            {
                            }
                            ComData _comData = (ComData)msg;
                            _comData.status = 1;
                            if (comDataDic == null) comDataDic = new ConcurrentDictionary<int, ComData>();
                            if (comDataDic.ContainsKey(_comData.cmdID))
                            {
                                ComData removeComData;
                                comDataDic.TryRemove(_comData.cmdID, out removeComData);
                            }
                            comDataDic.TryAdd(_comData.cmdID, _comData);
                            if (this.listenCreater != null)
                            {
                                ToolKits.EventTool.EventData eventData = new EventTool.EventData(_comData);
                                this.listenCreater.ChangeValue(eventData);
                            }
                        }
                    }
                }
            }
            catch (System.IO.IOException)//服务器异常断开
            {
                CloseAll();

                Resource r1 = new Resource();
                r1.IsConnected = false;
                r1.ErrStatus = "服务器(ip=" + this.localIP.ToString() + ";port=" + this.port.ToString() + ")已关闭";
                if (this.clientListening != null)
                    clientListening(this, r1);
                this.isConnected = false;
                //Thread.Sleep(500);

                //while (true)
                //{
                //    try
                //    {
                //        TcpClientStart();
                //        break;
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}
            }
            catch (Exception)
            {

            }
        }
        public bool SendMessage(object sendMsg)
        {
            //把成 ASCII 字符串转化数据字符。
            byte[] sendMsgbyte = null;
            // NetworkStream stream = null;
            //TcpClient tcpClient = new TcpClient();
            try
            {
                //tcpClient.Connect(this.localIP, port);
                this.stream = this.tcpClient.GetStream();
                if (this.stream.CanWrite)
                {
                    sendMsgbyte = StructToBytes(sendMsg);
                    stream.Write(sendMsgbyte, 0, sendMsgbyte.Length);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SendMessage(string sendMsg)
        {
            //把成 ASCII 字符串转化数据字符。
            byte[] sendMsgbyte = null;
            try
            {
                this.stream = this.tcpClient.GetStream();
                if (this.stream.CanWrite)
                {
                    sendMsgbyte = System.Text.Encoding.ASCII.GetBytes(sendMsg);
                    stream.Write(sendMsgbyte, 0, sendMsgbyte.Length);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 清除缓存区中某条指令
        /// </summary>
        /// <param name="cmdID"></param>
        /// <returns></returns>
        public bool ClearMessage(int cmdID)
        {
            try
            {
                if (this.comDataDic.ContainsKey(cmdID))
                {
                    ComData comData;
                    this.comDataDic.TryRemove(cmdID, out comData);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 不启用监听功能时可以使用
        /// </summary>
        /// <param name="receiveMsg"></param>
        public bool ReceiveMessage(out string receiveMsg)
        {
            receiveMsg = "";
            try
            {
                // 获取一个数据流对象来进行读取和写入
                stream = tcpClient.GetStream();
                if (this.stream.CanRead)
                {
                    byte[] buffer = new byte[this.tcpClient.ReceiveBufferSize];
                    this.stream.Read(buffer, 0, this.tcpClient.ReceiveBufferSize);
                    receiveMsg = Encoding.Default.GetString(buffer);
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 不启用监听功能时可以使用
        /// </summary>
        /// <param name="receiveMsg"></param>
        public bool ReceiveMessage(out object receiveMsg)
        {
            receiveMsg = null;
            try
            {
                // 获取一个数据流对象来进行读取和写入
                stream = tcpClient.GetStream();
                if (this.stream.CanRead)
                {
                    byte[] buffer = new byte[this.tcpClient.ReceiveBufferSize];
                    this.stream.Read(buffer, 0, this.tcpClient.ReceiveBufferSize);
                    receiveMsg = BytesToStuct(buffer, this.type);
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 不启用监听功能时可以使用
        /// </summary>
        /// <param name="receiveMsg"></param>
        public bool ReceiveMessage(out ComData receiveMsg)
        {
            receiveMsg = new ComData(0);
            try
            {
                // 获取一个数据流对象来进行读取和写入
                stream = tcpClient.GetStream();
                if (this.stream.CanRead)
                {
                    byte[] buffer = new byte[this.tcpClient.ReceiveBufferSize];
                    this.stream.Read(buffer, 0, this.tcpClient.ReceiveBufferSize);
                    object recvMsg = BytesToStuct(buffer, this.type);
                    receiveMsg = (ComData)recvMsg;
                    receiveMsg.status = 1;
                }
                return true;
            }
            catch (SocketException)
            {
                return false;
                //MessageBox.Show(se.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 启用监听功能时可以使用,接收对应指令集携带的重要信息
        /// </summary>
        /// <param name="cmdID">发送指令时的指令ID</param>
        /// <param name="timeOut">接收超时时间,单位ms</param>
        /// <param name="comData">接收到的数据</param>
        /// <returns></returns>
        public bool ReceiveMessage(int cmdID, double timeOut, out ComData comData)
        {
            comData = new ComData(0);
            try
            {
                DateTime startTime = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeOut)
                        return false;
                    //lock (this)
                    {
                        if (comDataDic.ContainsKey(cmdID))
                        {
                            if (comDataDic.TryRemove(cmdID, out comData))
                            {
                                comData.status = 1;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///// <summary>
        ///// 启用监听功能时使用，默认接收队列中的第一个数据
        ///// </summary>
        ///// <param name="timeOut">超时时间，单位ms</param>
        ///// <param name="comData">返回接收到的数据</param>
        ///// <returns></returns>
        //public bool ReceiveMessage(double timeOut, out ComData comData)
        //{
        //    comData = new ComData(0);
        //    try
        //    {
        //        DateTime startTime = DateTime.Now;
        //        while (true)
        //        {
        //            if (DateTime.Now.Subtract(startTime).TotalMilliseconds > timeOut)
        //                return false;
        //            if (comDataDic.Count > 0)
        //            {
        //                foreach (var item in comDataDic.Values)
        //                {
        //                    if (comDataDic.TryRemove(item.cmdID, out comData))
        //                    {
        //                        comData.status = 1;
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 异步接收信息，防止主线程卡死
        /// </summary>
        /// <param name="cmdID"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public Task<ComData> ReceiveMsgAsync(int cmdID, double timeOut)
        {
            return Task.Run(() =>
                {
                    ComData comData;
                    ReceiveMessage(cmdID, timeOut, out comData);
                    return comData;
                });
        }
        ///// <summary>
        ///// 异步接收信息，防止主线程卡死
        ///// </summary>
        ///// <param name="cmdID"></param>
        ///// <param name="timeOut"></param>
        ///// <returns></returns>
        //public Task<ComData> ReceiveMsgAsync(double timeOut)
        //{
        //    return Task.Run(() =>
        //    {
        //        ComData comData;
        //        ReceiveMessage(timeOut, out comData);
        //        return comData;
        //    });
        //}

        //// <summary>

        /// 结构体转byte数组
        /// </summary>
        /// <param name="structObj">要转换的结构体</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
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
        //public static unsafe byte[] StructToBytes(object structObj)
        //{
        //    ComData comData = (ComData)structObj;
        //    byte[] bytes = new byte[Marshal.SizeOf(comData)];
        //    fixed (byte* parr = bytes)
        //    {
        //        *((ComData*)parr) = comData;
        //    }
        //    //返回byte数组
        //    return bytes;
        //}
        public static byte[] StructToBytes1(object structObj)
        {
            ComData comData = (ComData)structObj;
            //得到结构体的大小
            //int size = Marshal.SizeOf(comData);
            //创建byte数组
            //byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            //IntPtr structPtr = Marshal.AllocHGlobal(size);
            ////将结构体拷到分配好的内存空间
            //Marshal.StructureToPtr(comData, structPtr, false);
            ////从内存空间拷到byte数组
            //Marshal.Copy(structPtr, bytes, 0, size);
            ////释放内存空间
            //Marshal.FreeHGlobal(structPtr);
            List<byte> bytes = new List<byte>();

            byte[] bytes_cmdID = System.BitConverter.GetBytes(comData.cmdID);
            byte[] bytes_iData = new byte[comData.iData.Length*4];
            byte[] bytes_dData = new byte[comData.dData.Length*8];
            byte[] bytes_cData = System.Text.Encoding.Unicode.GetBytes(comData.cData);//new byte[comData.cData.Length];
            //bytes_cData = Encoding.Convert(Encoding.GetEncoding(0), Encoding.GetEncoding("gb2312"), bytes_cData);
            byte[] bytes_quit = System.BitConverter.GetBytes(comData.quit);
            byte[] bytes_result = System.BitConverter.GetBytes(comData.result);
            byte[] bytes_status = System.BitConverter.GetBytes(comData.status);
            byte[] _bytes_iData,_bytes_dData;
            for (int i = 0; i < comData.iData.Length; i++)
            {
                _bytes_iData = System.BitConverter.GetBytes(comData.iData[i]);
                _bytes_dData = System.BitConverter.GetBytes(comData.dData[i]);
                _bytes_iData.CopyTo(bytes_iData, i * _bytes_iData.Length);
                _bytes_dData.CopyTo(bytes_dData, i * _bytes_dData.Length);
            }
            bytes.AddRange(bytes_cmdID);
            bytes.AddRange(bytes_iData);
            bytes.AddRange(bytes_dData);
            bytes.AddRange(new List<byte>(bytes_cData).GetRange(0, bytes_cData.Length / 2));
            bytes.AddRange(bytes_quit);
            bytes.AddRange(bytes_result);
            bytes.AddRange(bytes_status);

            //返回byte数组
            return bytes.ToArray();
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
            int size = Marshal.SizeOf(type);
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
        public void CloseAll()
        {
            this.CloseResource();
        }
        private void CloseResource()
        {
            if (this != null)
            {
                cts.Cancel();
                if (this.stream != null)
                {
                    this.stream.Close();
                    this.stream.Dispose();
                }
                if (this.tcpClient != null)
                {
                    this.tcpClient.Close();
                    this.tcpClient = null;
                }
                if (this.comDataDic != null) this.comDataDic.Clear();
            }
        }
    }
    public class Resource : EventArgs
    {
        private bool isConnected;
        private string errStatus;
        public bool IsConnected
        {
            get { return this.isConnected; }
            set { this.isConnected = value; }
        }
        public string ErrStatus
        {
            get { return this.errStatus; }
            set { if (value != null) this.errStatus = value; }
        }
    }
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
    public struct ComData
    {
        public int cmdID; //命令指令
        //public int pcID; //pc指令
        //public int camID; //相机指令
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]//定义数组长度;4*16
        public int[] iData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]//定义数组长度;8*16
        public double[] dData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]//定义数组长度;
        public char[] cData;
        /// <summary>
        /// 中断信号
        /// </summary>
        public int quit; 
        /// <summary>
        /// 用于返回最终的检测结果
        /// </summary>
        public int result; 
        /// <summary>
        /// 用于指明当前指令的接收状态,1表示接收成功，0表示接收失败
        /// </summary>
        public int status;

        public ComData(int _cmdID)
        {
            cmdID = _cmdID;
            iData = new int[16];
            dData = new double[16];
            cData = new char[256];
            quit = 0;
            result = 0;
            status = 0;
        }
    }
}
