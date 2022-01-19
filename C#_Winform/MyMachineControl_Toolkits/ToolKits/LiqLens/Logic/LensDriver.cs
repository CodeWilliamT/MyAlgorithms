using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Management;
using LensDriverController.Dto;


namespace LensDriverController.Logic
{
    /// <summary>
    /// Lens Driver message event handler delegate
    /// </summary>
    public delegate void LensDriverMessageEventHandler(object o, LensDriverMessageEventArgs e);

    /// <summary>
    /// Class to augment the standard event args for Lens Driver information
    /// </summary>
    public class LensDriverMessageEventArgs : EventArgs
    {
        public readonly LensDriver.MessageType MessageType;
        public readonly string Message;

        public LensDriverMessageEventArgs(LensDriver.MessageType newMessageType, string message = "")
        {
            MessageType = newMessageType;
            Message = message;
        }
    }



    /// <summary>
    /// Lens Driver communication class - singleton
    /// </summary>
    public class LensDriver
    {
        //class tempratureCompensationPoint
        //{
        //    public double dFdT;
        //    public double F;
        //}

        //class focalPowerCalibrationPoint
        //{
        //    public double F;
        //    public double FShifted;
        //    public double I;
        //    public int IBinary;
        //}

        // Instance of Lens Driver singleton
        private static LensDriver lensDriverInstance = null;
        // Simple thread-safe lock
        private static readonly Object instanceLock = new Object();

        private SerialPort _comport = new SerialPort();
        private System.Windows.Forms.Timer _jobTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer _dataAcquisitionTimer = new System.Windows.Forms.Timer();
        private Dto.FixedSizedQueue<byte[]> _jobQueue = new Dto.FixedSizedQueue<byte[]>(10);
        private static object _serialPortLocker = new Object();
        private bool _dontRunHandler = false;
        private string _deviceManagerHardwareIdentificationString;
        private List<LensDriverController.Dto.FirmwareVersion> compatibilityList;
        private List<LensDriverController.Dto.FirmwareVersion> incompatibilityList;
        private System.Timers.Timer _timerConnectionCheck;
        private object _connectionCheckTimerLock = new Object();

        //analog input
        private int _analogInputReadInterval = 50; //ms
        private long _lastAnalogReadUpdate = 0;

        //temperature compensation member variables
        private long _temperatureReadInterval = 500; //ms
        private long _lastTemperatureUpdate = 0;
        public List<DataPoint> sensorControlCalibration = new List<DataPoint>();

        public int focalPower { get; private set; }
        public int lowerFocalPowerLimit { get; private set; }
        public int upperFocalPowerLimit { get; private set; }

        // Sensor control
        public bool sensorControlCalibrationAvailable { get; private set; }
        private bool _sensorControlEnabled;
        public bool sensorControlAvailable { get; private set; }


        // state variables
        //private VirtualOperatingMode _virtualOperationModeChannelA;
        public OperatingMode operationMode { get; private set; }

        //hardware variables
        public string ID { get; private set; }
        private bool _IDOK;
        public LensDriverController.Dto.FirmwareVersion firmwareVersionObject { get; private set; }
        public bool linkEstablished { get; private set; }

        public int currentLowerSoftwareLimit { get; private set; }
        public int currentUpperSoftwareLimit { get; private set; }
        public int currentDriftGain { get; private set; }
        public int maxCurrent { get; private set; }
        public bool communicationWithLensEstablished { get; private set; }
        public bool focalLengthOutsideFirmwareRange { get; private set; }
        public bool temperatureOutsideFirmwareRange { get; private set; }
        public bool cannotReachFocalPower { get; private set; }
        public bool EEPROMAvailable { get; private set; }
        public bool calibratedLensConnected { get; private set; }
        public int systemTimeConstantFalling { get; private set; }
        public int systemTimeConstantRising { get; private set; }

        public int current { get; private set; }
        public double temperature { get; private set; }
        public int analog { get; private set; }
        public int signalFrequency { get; private set; }
        public int singalLowerCurrentLevel { get; private set; }
        public int signalUpperCurrentLevel { get; private set; }

        private static bool DEBUG_MODE = true;

        private CRC16IBM CRC16IBMCalculator;

        private HardwareErrors _hardwareErrors = new HardwareErrors();

        #region ENUMs

        public enum MessageType
        {
            SensorControl,
            Timeout,
            CommunicationError,
            Initialization,
            Calibration,
            OperationModeUpdate,
            DataAcquisition,
            OutdatedFirmwareVersion,
            WrongHardwareID,
            HardwareID,
            CommunicationCRCError,
            FocalPowerModeError,
            Analog,
            SignalUpdate,
            CurrentUpdate,
            FocalPowerLimits,
            FocalPowerLimitsError,
            FocalPowerUpdate,
            SystemTimeConstantsUpdate,
            ExpiredFirmwareVersion,
            TemperatureUpdate
        };



        public enum CalibrationCommandType
        {
            MaxCurrent,
            UpperCurrentSoftwareLimit,
            LowerCurrentSoftwareLimit,
            DriftGain
        };

        public enum ReadOrWrite
        {
            Read,
            Write
        };

        public enum OperatingMode
        {
            Undefined,
            Current,
            FocalPower,
            Sinusoidal,
            Triangular,
            Rectangular,
            Analog
        };


        public enum Services
        {
            SensorControl
        }

        public enum LimitType
        {
            Upper,
            Lower
        };


        #endregion //ENUMs

        public event LensDriverMessageEventHandler Message;
        private string serialID = String.Empty;
        private bool oldCommunicationEstablished = false;
        private bool serialIDErrorMessageShown = false;

        private LensDriver(string nlensDriverHardwareID)
        {
            // Suscribe to the serial port event handler
            _comport.DataReceived += new SerialDataReceivedEventHandler(ReceivedData);
            _jobTimer.Interval = 2;
            _jobTimer.Enabled = false;
            _jobTimer.Tick += new EventHandler(JobTimer_Tick);

            _dataAcquisitionTimer.Interval = 10;
            _dataAcquisitionTimer.Enabled = false;
            _dataAcquisitionTimer.Tick += new EventHandler(DataAcquisitionTimer_Tick);

            _timerConnectionCheck = new System.Timers.Timer();
            _timerConnectionCheck.Interval = 1000;
            _timerConnectionCheck.Elapsed += new System.Timers.ElapsedEventHandler(ConnectionCheck_Tick);
            _timerConnectionCheck.Enabled = false;


            ID = String.Empty;
            string version = String.Empty;
            firmwareVersionObject = new LensDriverController.Dto.FirmwareVersion();
            compatibilityList = new List<LensDriverController.Dto.FirmwareVersion>() { new LensDriverController.Dto.FirmwareVersion(1, 8) };
            incompatibilityList = new List<LensDriverController.Dto.FirmwareVersion>() { new LensDriverController.Dto.FirmwareVersion(1, 3) };

            linkEstablished = false;
            _IDOK = false;

            currentLowerSoftwareLimit = -4095; //minimum lower software limit --> will be initialized correctly
            currentUpperSoftwareLimit = 4095; //maximum upper software limit --> will be initialized correctly
            maxCurrent = 30000; //certainly higher than the physical limit --> will be initialized correctly
            currentDriftGain = 0;
            //_virtualOperationModeChannelA = VirtualOperatingMode.None;
            operationMode = OperatingMode.Undefined;
            temperature = 0;
            communicationWithLensEstablished = false;
            _deviceManagerHardwareIdentificationString = nlensDriverHardwareID;
            analog = 0;
            sensorControlCalibrationAvailable = false;
            _sensorControlEnabled = false;

            focalLengthOutsideFirmwareRange = false;
            temperatureOutsideFirmwareRange = false;
            EEPROMAvailable = false;
            cannotReachFocalPower = false;
            calibratedLensConnected = false;

            CRC16IBMCalculator = new CRC16IBM();
        }

        public static LensDriver getInstance(string nlensDriverHardwareID)
        {
            lock (instanceLock)
            {
                if (lensDriverInstance == null)
                {
                    lensDriverInstance = new LensDriver(nlensDriverHardwareID);
                }
                return lensDriverInstance;
            }
        }


        #region Init and DeInit

        public void UpdateHardwareID(string nlensDriverHardwareID)
        {
            _deviceManagerHardwareIdentificationString = nlensDriverHardwareID;
        }

        public bool Initalize(OperatingMode modeChannelA)
        {
            List<string> foundComPortsWithDriverAttached;

            string comPort = String.Empty;
            bool skipInitialization = false;

            foundComPortsWithDriverAttached = Service.DeviceManager.DetectComDevice(_deviceManagerHardwareIdentificationString);
            
            
            if (foundComPortsWithDriverAttached.Count > 1)
            {
                
                /*
                Forms.ComPortChooser comPortChooser = new Forms.ComPortChooser();
                comPortChooser.loadPortSuggestions(foundComPortsWithDriverAttached);
                if (comPortChooser.ShowDialog() == DialogResult.OK)
                {
                    comPort = comPortChooser.selectedPort;
                }
                else
                {
                    skipInitialization = true;
                }
                */

                //do nothing
            }
            else if (foundComPortsWithDriverAttached.Count == 1)
            {
                comPort = foundComPortsWithDriverAttached[0];
            }
            else
            {
                //MessageBox.Show("没有发现wafer检测镜头，请确认镜头上的USB连接线是否正确连接.");
                skipInitialization = true;
                return false;
            }

            if (skipInitialization == false)
            {
                LowLevelInitialize(comPort, 115200, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One, modeChannelA);
                //MessageBox.Show("变焦镜头连接成功！");
            }

            return true;
        }



        private bool LowLevelInitialize(string PortName, int Baudrate, int DataBits, Parity _Parity, StopBits _StopBits, OperatingMode modeChannelA)
        {
            if (_comport.IsOpen == false)
            {
                // Set port's GlobalSettings
                _comport.BaudRate = Baudrate;
                _comport.DataBits = DataBits;
                _comport.StopBits = _StopBits;
                _comport.Parity = _Parity;
                _comport.PortName = PortName;
                _comport.DtrEnable = true;
                _comport.ReadTimeout = 1000;

                // Open the port
                try
                {
                    FlushInBuffer();
                    FlushOutBuffer();
                    _comport.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }


                SendStartCommand();
                SendIdentCommand();

                if (_IDOK)
                {
                    SendFirmwareVersionCommand();
                }

                if (linkEstablished)
                {
                    SetOperationMode(modeChannelA, true);
                    StartJobQueue();
                    ReadAllCalibration();
                    StartDataAcquisition();
                    StartConnectionCheckTimer();
                }
                return true;

            }
            else
            {
                return false;
            }

        }


        public void Deinitialize()
        {
            if (_comport.IsOpen)
            {
                SetZeroCurrent();
                SetZeroSignal();
                LowLevelDeinitialize();
            }
        }



        private bool LowLevelDeinitialize(bool force = false)
        {
            StopConnectionCheckTimer();

            // If the port is open, close it
            if (_comport.IsOpen)
            {
                try
                {
                    if (force == false)
                    {
                        //_controlElectronicsReady = false;
                        SetZeroCurrent();
                        FlushInBuffer();
                        FlushOutBuffer();
                    }
                    StopDataAcquisition();
                    StopJobQueue(true);
                    _comport.Close();
                    linkEstablished = false;
                    _sensorControlEnabled = false;
                    if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Initialization));
                    return true;
                }
                catch (Exception)
                {
                    if (_comport.IsOpen == false)
                    {
                        linkEstablished = false;
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Initialization));
                    }
                    else
                    {
                        MessageBox.Show("Could not disconnect from Lens Driver!");
                    }
                    return false;
                }
            }
            else
            {
                return true;
            }




        }
        #endregion




        #region Commands and Dependencies

        /// <summary>
        /// Sets Tau time constants
        /// </summary>
        /// <param name="tauRise">Rise time in seconds</param>
        /// <param name="tauFall">Fall time in seconds</param>
        //public void SystemTimeConstants(ReadOrWrite readOrWrite, int tauRise = 0, int tauFall = 0)
        //{
        //    if (linkEstablished == false) return;

        //    // constrain taus to defined range
        //    if (tauRise < 0) tauRise = 0;
        //    if (tauFall < 0) tauFall = 0;
        //    if (tauRise > 255) tauRise = 255;
        //    if (tauFall > 255) tauFall = 255;

        //    DisableReceivedEventHandler();

        //    FlushInBuffer();

        //    byte[] dataPayloadAsBytes = new byte[4] { (byte)(tauRise & 0xFF), (byte)(tauFall & 0xFF), (byte)(0), (byte)(0) };
        //    byte[] dataPackageAsBytes;

        //    if (readOrWrite == ReadOrWrite.Read)
        //    {
        //        dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PrKA"), dataPayloadAsBytes));
        //    }
        //    else
        //    {
        //        dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PwKA"), dataPayloadAsBytes));
        //    }

        //    Send(dataPackageAsBytes);

        //    WaitForAnswer();

        //    EnableReceivedEventHandler();
        //}


        private void SendStartCommand()
        {
            DisableReceivedEventHandler();
            Send(StringToByteArray("Start"));
            WaitForAnswer();
            EnableReceivedEventHandler();
        }

        private void SendIdentCommand()
        {
            DisableReceivedEventHandler();
            Send(AddCRC(StringToByteArray("IRxxxxxxxx")));
            WaitForAnswer();
            EnableReceivedEventHandler();
        }

        private void SendFirmwareVersionCommand()
        {
            DisableReceivedEventHandler();
            Send(AddCRC(StringToByteArray("Vd")));
            WaitForAnswer();
            EnableReceivedEventHandler();
        }

        private void SendSerialIDCommand()
        {
            DisableReceivedEventHandler();
            Send(AddCRC(StringToByteArray("X")));
            WaitForAnswer();
            EnableReceivedEventHandler();
        }

        public bool IsServiceProvidedInCurrentOperationMode(Services service)
        {
            if (operationMode == OperatingMode.Current)
            {
                if (service == Services.SensorControl)
                {
                    if (sensorControlCalibrationAvailable)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (operationMode == OperatingMode.FocalPower)
            {
                if (service == Services.SensorControl)
                {
                    if (communicationWithLensEstablished && sensorControlCalibrationAvailable)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }


            return false;
        }


        //private bool CheckModeRequisites(OperatingMode mode)
        //{
        //    bool check = false;


        //    switch (mode)
        //    {
        //        case OperatingMode.FocalPower:
        //            if (temperatureChannelAAvailable == true)
        //            {
        //                if (temperatureChannelA <= _maxOperationTemperatureInDegrees && temperatureChannelA >= _minOperationTemperatureInDegrees)
        //                {
        //                    check = true;
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Channel A temperature outside allowed lens temperature. Switiching back to DC operation.");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Channel A temperature or temperature calibration not available.");
        //            }

        //            break;
        //        case OperatingMode.DC:
        //            if (temperatureChannelAAvailable == true)
        //            {
        //                if (temperatureChannelA <= _maxOperationTemperatureInDegrees && temperatureChannelA >= _minOperationTemperatureInDegrees)
        //                {
        //                    check = true;
        //                }
        //            }
        //            break;
        //        default:
        //            check = true;
        //            break;
        //    }





        //    return check;
        //}

        public void SetOperationMode(OperatingMode newMode, bool zeroInitialization = false, bool triggerEvent = true)
        {
            StopJobQueue();
            StopDataAcquisition();

            DisableReceivedEventHandler();

            byte[] dataPackageAsBytes = new byte[6];

            //set default mode values
            //_virtualOperationModeChannelA = VirtualOperatingMode.None;
            DisableSensorControl(triggerEvent);


            switch (newMode)
            {
                case OperatingMode.Current:
                    //Send constant command
                    dataPackageAsBytes = AddCRC(StringToByteArray("MwDA"));
                    break;
                case OperatingMode.FocalPower:
                    //Send constant command
                    dataPackageAsBytes = AddCRC(StringToByteArray("MwCA"));
                    break;
                case OperatingMode.Rectangular:
                    //Send rectangular command
                    dataPackageAsBytes = AddCRC(StringToByteArray("MwQA"));
                    break;
                case OperatingMode.Sinusoidal:
                    //Send sinusoidal command
                    dataPackageAsBytes = AddCRC(StringToByteArray("MwSA"));
                    break;
                case OperatingMode.Triangular:
                    //Send triangular command
                    dataPackageAsBytes = AddCRC(StringToByteArray("MwTA"));
                    break;
                case OperatingMode.Analog:
                    //Send analog mode command
                    dataPackageAsBytes = AddCRC(StringToByteArray("LA"));
                    break;
                default:
                    break;
            }

            if (newMode != OperatingMode.Undefined)
            {
                Send(dataPackageAsBytes);

                WaitForAnswer();
            }

            // Send mode update message
            if (triggerEvent)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.OperationModeUpdate));
            }


            if (zeroInitialization)
            {
                if (operationMode == OperatingMode.Current)
                {
                    SetZeroCurrent();
                }
                else if (operationMode == OperatingMode.FocalPower)
                {
                    SetMinimumFocalPower();
                }
                else if (operationMode == OperatingMode.Sinusoidal ||
                            operationMode == OperatingMode.Triangular ||
                            operationMode == OperatingMode.Rectangular)
                {
                    SetZeroSignal();
                }
            }


            EnableReceivedEventHandler();

            StartJobQueue();
            StartDataAcquisition();

        }

        private void SetSerialMode()
        {
            Send(AddCRC(StringToByteArray("LS")));
            WaitForAnswer();
        }

        private void SetZeroCurrent()
        {
            SetCurrent(0, true);
        }

        private void SetZeroSignal()
        {
            SetFrequency(0, true);
            SetSignalCurrentLevel(0, LimitType.Lower, true);
            SetSignalCurrentLevel(0, LimitType.Upper, true);
        }

        private void SetMinimumFocalPower()
        {
            SetFocalPower((Int16)lowerFocalPowerLimit, true);
        }

        public void SetCurrent(Int16 newCurrent, bool useQueue = true, bool triggerEvent = true)
        {
            if (linkEstablished == false) return;

            //assure newCurrent is within softlimit bounds

            if (newCurrent < currentLowerSoftwareLimit && newCurrent != 0) newCurrent = (Int16)currentLowerSoftwareLimit;
            if (newCurrent > currentUpperSoftwareLimit && newCurrent != 0) newCurrent = (Int16)currentUpperSoftwareLimit;



            byte[] dataPayloadAsBytes = new byte[2] { (byte)(newCurrent >> 8), (byte)(newCurrent & 0xFF) };
            byte[] dataPackageAsBytes = new byte[6];

            // Support for 1.5 firmware
            if (firmwareVersionObject.minor < 7)
            {
                dataPackageAsBytes = AddCRC(Combine(StringToByteArray("A"), dataPayloadAsBytes));
            }
            else dataPackageAsBytes = AddCRC(Combine(StringToByteArray("Aw"), dataPayloadAsBytes));

            if (useQueue)
            {
                addSerialJob(dataPackageAsBytes);
            }
            else
            {
                Send(dataPackageAsBytes);
            }

            current = newCurrent;

            if (triggerEvent)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.CurrentUpdate));
            }
        }


        public void ReadCurrent()
        {
            if (linkEstablished == false) return;

            StopJobQueue();
            DisableReceivedEventHandler();

            byte[] dataPayloadAsBytes = new byte[2] { (byte)(0), (byte)(0) };
            byte[] dataPackageAsBytes;

            dataPackageAsBytes = AddCRC(Combine(StringToByteArray("Ar"), dataPayloadAsBytes));

            Send(dataPackageAsBytes);

            WaitForAnswer();

            StartJobQueue();
            EnableReceivedEventHandler();
        }

        public void SetFocalPower(Int16 newFocalPower, bool triggerEvent = true)
        {
            if (linkEstablished == false) return;

            //assure newFocalPower is within softlimit bounds

            //if (newFocalPower < lowerFocalPowerLimit) newFocalPower = (Int16)lowerFocalPowerLimit;
            //if (newFocalPower > upperFocalPowerLimit) newFocalPower = (Int16)upperFocalPowerLimit;



            byte[] dataPayloadAsBytes = new byte[4] { (byte)(newFocalPower >> 8), (byte)(newFocalPower & 0xFF), (byte)0, (byte)0 };
            byte[] dataPackageAsBytes;// = new byte[7];



            dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PwDA"), dataPayloadAsBytes));
            addSerialJob(dataPackageAsBytes);
            focalPower = newFocalPower;

            if (triggerEvent)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerUpdate));
            }
        }


        public void ReadFocalPower()
        {
            if (linkEstablished == false) return;

            StopJobQueue();
            DisableReceivedEventHandler();

            byte[] dataPayloadAsBytes = new byte[4] { (byte)(0), (byte)(0), (byte)0, (byte)0 };
            byte[] dataPackageAsBytes;

            dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PrDA"), dataPayloadAsBytes));

            Send(dataPackageAsBytes);

            WaitForAnswer();

            StartJobQueue();
            EnableReceivedEventHandler();
        }


        public void SetOperatingTemperatureRange(double lowerOperatingTemperature, double upperOperatingTemperature)
        {
            if (linkEstablished == false) return;

            DisableReceivedEventHandler();
            byte[] dataPackageAsBytes = new byte[10];
            uint lowerTemperatureBinary = convertTemperatureToBinary(lowerOperatingTemperature);
            uint upperTemperatureBinary = convertTemperatureToBinary(upperOperatingTemperature);
            byte[] dataPayloadAsBytes = new byte[4] { (byte)(upperTemperatureBinary >> 8), (byte)(upperTemperatureBinary & 0xFF), (byte)(lowerTemperatureBinary >> 8), (byte)(lowerTemperatureBinary & 0xFF) };

            dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PwTA"), dataPayloadAsBytes));

            Send(dataPackageAsBytes);

            WaitForAnswer();

            EnableReceivedEventHandler();

            //if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SignalUpdate));
        }

        public void SetFrequency(Int32 frequency, bool triggerEvent = true)
        {
            if (linkEstablished == false) return;

            byte[] dataPayloadAsBytes = new byte[4] { (byte)((frequency >> 24) & 0xFF), (byte)((frequency >> 16) & 0xFF), (byte)((frequency >> 8) & 0xFF), (byte)((frequency & 0xFF)) };
            byte[] dataPackageAsBytes = new byte[10];

            signalFrequency = frequency;
            dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PwFA"), dataPayloadAsBytes));
            addSerialJob(dataPackageAsBytes);

            if (triggerEvent)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SignalUpdate));
            }
        }


        public void SetSignalCurrentLevel(Int16 current, LimitType type, bool triggerEvent = true)
        {
            if (linkEstablished == false) return;

            //assure current is within softlimit bounds
            if (current < currentLowerSoftwareLimit && current != 0) current = (Int16)currentLowerSoftwareLimit;
            if (current > currentUpperSoftwareLimit && current != 0) current = (Int16)currentUpperSoftwareLimit;

            byte[] dataPayloadAsBytes = new byte[4] { (byte)(current >> 8), (byte)(current & 0xFF), (byte)0, (byte)0 };
            byte[] dataPackageAsBytes = new byte[10];

            if (type == LimitType.Upper)
            {
                signalUpperCurrentLevel = current;
                dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PwUA"), dataPayloadAsBytes));
                addSerialJob(dataPackageAsBytes);
            }
            else
            {
                singalLowerCurrentLevel = current;
                dataPackageAsBytes = AddCRC(Combine(StringToByteArray("PwLA"), dataPayloadAsBytes));
                addSerialJob(dataPackageAsBytes);
            }

            if (triggerEvent)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SignalUpdate));
            }
        }

        /// <summary>
        /// Reads temperature from LensDriver
        /// Only run this method from within DataAcquisitionTimer!
        /// </summary>
        /// <param name="channel">Channel to read temperature from</param>
        private void ReadTemperature()
        {
            if (linkEstablished == false) return;

            DisableReceivedEventHandler();

            FlushInBuffer();

            byte[] dataPackageAsBytes = new byte[4];

            dataPackageAsBytes = AddCRC(StringToByteArray("TA"));

            Send(dataPackageAsBytes);

            WaitForAnswer();

            EnableReceivedEventHandler();
        }

        /// <summary>
        /// Reads analog input from LensDriver.
        /// Only run this method from within DataAcquisitionTimer!
        /// </summary>
        /// <param name="channel">Channel to read analog input from</param>
        private void ReadAnalogInput()
        {
            if (linkEstablished == false) return;

            DisableReceivedEventHandler();

            FlushInBuffer();

            byte[] dataPackageAsBytes = new byte[4];


            dataPackageAsBytes = AddCRC(StringToByteArray("GAA"));

            Send(dataPackageAsBytes);

            WaitForAnswer();

            EnableReceivedEventHandler();
        }


        public void ReadAllCalibration()
        {
            CalibrationCommand(CalibrationCommandType.MaxCurrent, ReadOrWrite.Read);
            CalibrationCommand(CalibrationCommandType.LowerCurrentSoftwareLimit, ReadOrWrite.Read);
            CalibrationCommand(CalibrationCommandType.UpperCurrentSoftwareLimit, ReadOrWrite.Read);
            CalibrationCommand(CalibrationCommandType.DriftGain, ReadOrWrite.Read);
        }


        public void CalibrationCommand(CalibrationCommandType commandType, ReadOrWrite action, Int16 dataPayload = 0)
        {
            if (linkEstablished == false) return;

            StopJobQueue();
            StopDataAcquisition();

            DisableReceivedEventHandler();

            FlushInBuffer();

            byte[] dataPayloadAsBytes = new byte[2] { (byte)(dataPayload >> 8), (byte)(dataPayload & 0xFF) };
            byte[] dataPackageAsBytes = new byte[8];
            string commandIdentifier = String.Empty;

            if (commandType == CalibrationCommandType.LowerCurrentSoftwareLimit)
            {
                if (action == ReadOrWrite.Read)
                {
                    commandIdentifier = "CrLA";
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
                else if (action == ReadOrWrite.Write)
                {
                    commandIdentifier = "CwLA";
                    if (dataPayload > (Int16)currentUpperSoftwareLimit)
                    {
                        dataPayload = (Int16)(currentUpperSoftwareLimit - 1);
                    }
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
            }
            else if (commandType == CalibrationCommandType.UpperCurrentSoftwareLimit)
            {
                if (action == ReadOrWrite.Read)
                {
                    commandIdentifier = "CrUA";
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
                else if (action == ReadOrWrite.Write)
                {
                    commandIdentifier = "CwUA";
                    if (dataPayload < (Int16)currentLowerSoftwareLimit)
                    {
                        dataPayload = (Int16)(currentLowerSoftwareLimit + 1);
                        dataPayloadAsBytes = new byte[2] { (byte)(dataPayload >> 8), (byte)(dataPayload & 0xFF) };
                    }
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
            }
            else if (commandType == CalibrationCommandType.MaxCurrent)
            {
                if (action == ReadOrWrite.Read)
                {
                    commandIdentifier = "CrMA";
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
                else if (action == ReadOrWrite.Write)
                {
                    commandIdentifier = "CwMA";
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
            }

            else if (commandType == CalibrationCommandType.DriftGain)
            {
                if (action == ReadOrWrite.Read)
                {
                    commandIdentifier = "Or";
                    dataPayloadAsBytes = new byte[2] { (byte)(0), (byte)(0) };
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
                else if (action == ReadOrWrite.Write)
                {
                    commandIdentifier = "Ow";
                    dataPackageAsBytes = AddCRC(Combine(StringToByteArray(commandIdentifier), dataPayloadAsBytes));
                }
            }

            Send(dataPackageAsBytes);

            WaitForAnswer();

            EnableReceivedEventHandler();

            if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Calibration));

            StartJobQueue();
            StartDataAcquisition();
        }

        private void addSerialJob(byte[] job)
        {
            _jobQueue.Enqueue(job);
        }

        #endregion //commands and dependencies




        #region Sensor Control

        public void UpdateSensorControlConfiguration(List<DataPoint> calibrationList)
        {
            sensorControlCalibration = calibrationList;
            // Calibration list must have two points!
            if (sensorControlCalibration != null && sensorControlCalibration.Count > 1) sensorControlCalibrationAvailable = true;
            else sensorControlCalibrationAvailable = false;
        }

        
        
        private double ValueLookup(int analogInput)
        {
            /*
            if (sensorControlCalibrationAvailable == false) return 0;

            int upperIndex = 9999;
            int lowerIndex = 0;

            foreach (DataPoint calibrationPoint in sensorControlCalibration)
            {
                if (analogInput < calibrationPoint.analogInput)
                {
                    upperIndex = sensorControlCalibration.IndexOf(calibrationPoint);
                    lowerIndex = upperIndex - 1;
                    break;
                }
            }

            // Index for extrapolation
            if (upperIndex == 9999)
            {
                upperIndex = sensorControlCalibration.Count - 1;
                lowerIndex = sensorControlCalibration.Count - 2;
            }
            else if (lowerIndex == -1)
            {
                upperIndex = 1;
                lowerIndex = 0;
            }

            // Interpolation
            DataPoint lowerPoint = sensorControlCalibration[lowerIndex];
            DataPoint upperPoint = sensorControlCalibration[upperIndex];

            return lowerPoint.dataValue + (upperPoint.dataValue - lowerPoint.dataValue) * ((analogInput - lowerPoint.analogInput) / (upperPoint.analogInput - lowerPoint.analogInput));
            */
            return analogInput;
        }


        public void EnableSensorControl()
        {
            if (IsServiceProvidedInCurrentOperationMode(Services.SensorControl) == false)
            {
                MessageBox.Show("Lens Driver was unable to enable sensor control.\n\n\u2022 Please ensure that two calibration points have been set for the current mode." +
                    "\n\u2022 Calibration points can be set under Extras -> Sensor Configuration Form." +
                    "\n\n If error persists, please contact Optotune support at +41 58 856 3000");
                _sensorControlEnabled = false;
            }
            else
            {
                _sensorControlEnabled = true;
            }

            if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SensorControl));
        }

        public void DisableSensorControl(bool triggerEvent = true)
        {
            _sensorControlEnabled = false;
            if (triggerEvent)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SensorControl));
            }
        }

        public bool GetSensorControlStatus()
        {
            return _sensorControlEnabled;
        }

        #endregion Sensor control




        #region Timers


        public void StartDataAcquisition()
        {
            _dataAcquisitionTimer.Start();
        }

        public void StopDataAcquisition()
        {
            _dataAcquisitionTimer.Stop();
        }

        private void DataAcquisitionTimer_Tick(object sender, EventArgs e)
        {
            StopJobQueue();
            _dataAcquisitionTimer.Stop();

            //if (CheckModeRequisites(operationModeChannelA) == false && operationModeChannelA != OperatingMode.DC)
            //{
            //    SetOperationMode(OperatingMode.DC);
            //    SetCurrent(0);
            //}


            long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (now - _lastTemperatureUpdate >= _temperatureReadInterval)
            {
                //read temperature and set focal power if focal power lock is enabled

                bool oldcalibratedLensConnected = calibratedLensConnected;

                ReadTemperature();



                //read lens parameters once if lens has been connected
                if (oldcalibratedLensConnected == false && calibratedLensConnected == true)
                {
                    // SystemTimeConstants(ReadOrWrite.Read);
                    //if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SystemTimeConstantsUpdate));
                }

                // disable focal power mode if lens is disconnected
                if (calibratedLensConnected == false && operationMode == OperatingMode.FocalPower)
                {
                    SetOperationMode(OperatingMode.Current);
                }

                _lastTemperatureUpdate = now;

                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.DataAcquisition));
            }



            // firmwareVersionObject.major == 1 && firmwareVersionObject.minor == 31 && 
            if (now - _lastAnalogReadUpdate >= _analogInputReadInterval)
            {
                //read analog input and set focal power if sensor control is enabled
                ReadAnalogInput();

                if (_sensorControlEnabled)
                {

                    if (operationMode == OperatingMode.Current)
                    {
                        current = convertCurrentToBinary(ValueLookup(analog));
                        SetCurrent((Int16)current);
                    }
                    else if (operationMode == OperatingMode.FocalPower)
                    {
                        focalPower = convertDioptersToBinary(ValueLookup(analog));
                        SetFocalPower((Int16)focalPower);
                    }
                }

                _lastAnalogReadUpdate = now;
            }


            _dataAcquisitionTimer.Start();
            StartJobQueue();
        }






        private void StartConnectionCheckTimer()
        {
            _timerConnectionCheck.Enabled = true;
        }

        private void StopConnectionCheckTimer()
        {
            _timerConnectionCheck.Enabled = false;
            _timerConnectionCheck.Elapsed -= ConnectionCheck_Tick;
            bool gotLock = Monitor.TryEnter(_connectionCheckTimerLock, TimeSpan.FromSeconds(15));
        }

        private void ConnectionCheck_Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            _timerConnectionCheck.Enabled = false;

            bool deviceFound = false;

            if (!Monitor.TryEnter(_connectionCheckTimerLock))
            {
                return;
            }
            else
            {
                deviceFound = Service.DeviceManager.DetectDevice(_deviceManagerHardwareIdentificationString);
            }

            Monitor.Exit(_connectionCheckTimerLock);

            if (deviceFound == false)
            {
                LowLevelDeinitialize(true);
            }

            if (linkEstablished == true) _timerConnectionCheck.Enabled = true;
        }





        private void StopJobQueue(bool emptyQueue = false)
        {
            long startInTicks = DateTime.Now.Ticks;
            int timeoutPeriodMilliseconds = 1000;

            if (emptyQueue)
            {
                // wait for the job queue to become empty
                while (_jobQueue.Size > 0)
                {
                    if (DateTime.Now.Ticks - startInTicks > TimeSpan.TicksPerMillisecond * timeoutPeriodMilliseconds)
                    {
                        _jobTimer.Stop();
                        return;
                    }
                    Application.DoEvents();
                }
            }

            _jobTimer.Stop();
        }

        private void StartJobQueue()
        {
            _jobTimer.Start();
        }

        private void JobTimer_Tick(object sender, EventArgs e)
        {
            byte[] jobToSend;

            if (_jobQueue.Count > 0)
            {
                jobToSend = _jobQueue.Dequeue();

                Send(jobToSend);
            }
        }

        #endregion Timers




        #region Temperature and Focal Power

        //public void SetFocalPower(double focalPower)
        //{
        //    //if (!temperatureCalibrationAvailable) return;


        //    //if (!temperatureChannelAAvailable) temperatureChannelA = _ambientTemperatureInDegrees;
        //    //double deltaD = (temperatureChannelA - _ambientTemperatureInDegrees) * TCgetdFPdT(focalPower);

        //    //if (operationModeChannelA == OperatingMode.FocalPower)
        //    //{
        //    //    if ((int)(focalPower * 1000) > maxFocalPowerInMillis) focalPower = maxFocalPowerInMillis / 1000.0;
        //    //    if ((int)(focalPower * 1000) < minFocalPowerInMillis) focalPower = minFocalPowerInMillis / 1000.0;
        //    //}

        //    //SetCurrent((Int16)TCGetCurrent(focalPower, deltaD));


        //    //if (operationModeChannelA == OperatingMode.FocalPower)
        //    //{
        //    //    if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerUpdateChannelA));
        //    //}
        //}


        //public double GetFocalPower(int current = 1000000, double temperatureInDegrees = 1000)
        //{
        //    //if (!temperatureCalibrationAvailable) return 0;

        //    double diopters = 0;

        //    ////if (current == 1000000) current = currentChannelA; //override default value


        //    ////if (temperatureInDegrees == 1000) temperatureInDegrees = temperatureChannelA;  //override default value
        //    ////double ambientDiopters = TCGetAmbientFocalPower(current);
        //    ////diopters = ambientDiopters + TCgetdFPdT(ambientDiopters) * (temperatureInDegrees - _ambientTemperatureInDegrees);

        //    return diopters;
        //}


        //public void UnlockFocalPower()
        //{
        //   _focalPowerLockChannelA = false;

        //  if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocusLockStatusChannelA));
        //}

        //public bool GetFocalPowerLockStatus()
        //{
        //    _focalPowerLockChannelA = false;

        //    return _focalPowerLockChannelA;
        //}

        //public void LockFocalPower()
        //{
        //    if (IsServiceProvidedInCurrentOperationMode(Services.FocalPowerLock) == false)
        //    {
        //        MessageBox.Show("Lens Driver was unable to lock the focus.\n\nPossible Reasons:\n-The lens you are using is not properly connected\n-Temperature compensation parameters are not set up in Options\n-The lens you are using does not feature a temperature sensor\n-Focal power lock is enabled.\n-This service is not allowed in the current operation mode.");

        //         _focalPowerLockChannelA = false;

        //    }
        //    else
        //    {
        //        _lockTemperatureChannelA = temperatureChannelA;
        //        _lockCurrentChannelA = currentChannelA;
        //        _lockFocalPowerChannelA = GetFocalPower();
        //        _focalPowerLockChannelA = true;   
        //    }



        //  if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocusLockStatusChannelA));
        //}


        //private void UpdateTemperatureCompensationLimits()
        //{
        //    //update channel a min and max focal power (that is always achieveable due to min and max operation temperature)
        //    if (temperatureCalibrationAvailable)
        //    {
        //        //focal length trackbars
        //        double maxCurrentMaxTempFocalPower = GetFocalPower(upperSoftwareLimitChannelA, _maxOperationTemperatureInDegrees);
        //        double maxCurrentMinTempFocalPower = GetFocalPower(upperSoftwareLimitChannelA, _minOperationTemperatureInDegrees);

        //        double minCurrentMaxTempFocalPower = GetFocalPower(lowerSoftwareLimitChannelA, _maxOperationTemperatureInDegrees);
        //        double minCurrentMinTempFocalPower = GetFocalPower(lowerSoftwareLimitChannelA, _minOperationTemperatureInDegrees);

        //        if (maxCurrentMaxTempFocalPower > minCurrentMaxTempFocalPower)
        //        {
        //            maxFocalPowerInMillis = Math.Min((int)Math.Ceiling((maxCurrentMaxTempFocalPower * 1000)), (int)Math.Ceiling((maxCurrentMinTempFocalPower * 1000)));
        //            minFocalPowerInMillis = Math.Max((int)Math.Floor((minCurrentMaxTempFocalPower * 1000)), (int)Math.Floor((minCurrentMinTempFocalPower * 1000)));
        //        }
        //        else
        //        {
        //            maxFocalPowerInMillis = Math.Min((int)Math.Ceiling((minCurrentMaxTempFocalPower * 1000)), (int)Math.Ceiling((minCurrentMinTempFocalPower * 1000)));
        //            minFocalPowerInMillis = Math.Max((int)Math.Floor((maxCurrentMaxTempFocalPower * 1000)), (int)Math.Floor((maxCurrentMinTempFocalPower * 1000)));
        //        }
        //    }
        //}

        //public void UpdateTemperatureConfiguration(string focalPower, string focalPowerCurrents, string dFdT, string dFdTFocalPowers, double ambientTemperatureInDegrees, double minOperationTemperatureInDegrees, double maxOperationTemperatureInDegrees)
        //{
        //    temperatureCalibrationAvailable = true;

        //    _minOperationTemperatureInDegrees = minOperationTemperatureInDegrees;
        //    _maxOperationTemperatureInDegrees = maxOperationTemperatureInDegrees;


        //    string[] separator = new string[] { "," };

        //    double d = 0;
        //    List<double> FList = focalPower.Split(separator, StringSplitOptions.RemoveEmptyEntries).Where(a => Double.TryParse(a, out d)).Select(r => d).ToList();
        //    List<double> IToFList = focalPowerCurrents.Split(separator, StringSplitOptions.RemoveEmptyEntries).Where(a => Double.TryParse(a, out d)).Select(r => d).ToList();

        //    //check list lengths
        //    if (FList.Count != IToFList.Count || FList.Count < 2)
        //    {
        //        temperatureCalibrationAvailable = false;
        //        return;
        //    }

        //    _focalPowerCalibration.Clear();
        //    for (int i = 0; i < FList.Count(); i++)
        //    {
        //        _focalPowerCalibration.Add(new focalPowerCalibrationPoint { I = IToFList[i], IBinary = 0, F = FList[i], FShifted = FList[i] });
        //    }



        //    List<double> dFdTList = dFdT.Split(separator, StringSplitOptions.RemoveEmptyEntries).Where(a => Double.TryParse(a, out d)).Select(r => d).ToList();
        //    List<double> FTodFdTList = dFdTFocalPowers.Split(separator, StringSplitOptions.RemoveEmptyEntries).Where(a => Double.TryParse(a, out d)).Select(r => d).ToList();


        //    //check second set of lists
        //    if (dFdTList.Count != FTodFdTList.Count || dFdTList.Count < 2)
        //    {
        //        temperatureCalibrationAvailable = false;
        //        return;
        //    }

        //    _temperatureCompensation.Clear();
        //    for (int i = 0; i < dFdTList.Count(); i++)
        //    {
        //        _temperatureCompensation.Add(new tempratureCompensationPoint { dFdT = dFdTList[i], F = FTodFdTList[i] });
        //    }

        //    _ambientTemperatureInDegrees = ambientTemperatureInDegrees;

        //    //check ambient temperature
        //    if (_ambientTemperatureInDegrees == 0)
        //    {
        //        temperatureCalibrationAvailable = false;
        //        return;
        //    }

        //    UpdateTemperatureCompensationLimits();

        //    if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Calibration));
        //}


        //private int TCGetCurrent(double targetF, double focalPowerOffset)
        //{
        //    if (temperatureCalibrationAvailable == false) return 0;

        //    // y = a*x + b
        //    double a = 0;
        //    double b = 0;

        //    // update binary current values according to channel and shift calibration curve
        //    foreach (focalPowerCalibrationPoint calibrationPoint in _focalPowerCalibration)
        //    {
        //        calibrationPoint.IBinary = convertAmperageToBinary(calibrationPoint.I);
        //        calibrationPoint.FShifted = calibrationPoint.F + focalPowerOffset;
        //    }


        //    //find nearest datapoint in vector
        //    double delta = 9999;
        //    int index = 0;

        //    foreach (focalPowerCalibrationPoint calibrationPoint in _focalPowerCalibration)
        //    {
        //        if (Math.Abs(calibrationPoint.FShifted - targetF) < delta)
        //        {
        //            delta = Math.Abs(calibrationPoint.FShifted - targetF);
        //            index = _focalPowerCalibration.IndexOf(calibrationPoint);
        //        }
        //    }

        //    int endIndex = _focalPowerCalibration.Count - 1;
        //    bool calculationDone = false;

        //    if (index == 0)
        //    {
        //        a = (_focalPowerCalibration[1].FShifted - _focalPowerCalibration[0].FShifted) / (_focalPowerCalibration[1].IBinary - _focalPowerCalibration[0].IBinary);
        //        b = _focalPowerCalibration[1].FShifted - a * _focalPowerCalibration[1].IBinary;
        //        calculationDone = true;
        //    }
        //    else if (index == endIndex && calculationDone == false)
        //    {
        //        a = (_focalPowerCalibration[endIndex].FShifted - _focalPowerCalibration[endIndex - 1].FShifted) / (_focalPowerCalibration[endIndex].IBinary - _focalPowerCalibration[endIndex-1].IBinary);
        //        b = _focalPowerCalibration[endIndex].FShifted - a * _focalPowerCalibration[endIndex].IBinary;
        //        calculationDone = true;
        //    }
        //    else if (calculationDone == false)
        //    {
        //        if (targetF < _focalPowerCalibration[index].FShifted)
        //        {
        //            a = (_focalPowerCalibration[index].FShifted - _focalPowerCalibration[index - 1].FShifted) / (_focalPowerCalibration[index].IBinary - _focalPowerCalibration[index - 1].IBinary);
        //            b = _focalPowerCalibration[index].FShifted - a * _focalPowerCalibration[index].IBinary;
        //        }
        //        else
        //        {
        //            a = (_focalPowerCalibration[index+1].FShifted - _focalPowerCalibration[index].FShifted) / (_focalPowerCalibration[index+1].IBinary - _focalPowerCalibration[index].IBinary);
        //            b = _focalPowerCalibration[index].FShifted - a * _focalPowerCalibration[index].IBinary;
        //        }
        //    }

        //    return (int)((targetF-b)/a);
        //}


        //private double TCGetAmbientFocalPower(int current)
        //{
        //    if (temperatureCalibrationAvailable == false) return 0;

        //    // y = a*x + b
        //    double a = 0;
        //    double b = 0;

        //    // update binary current values according to channel and shift calibration curve
        //    foreach (focalPowerCalibrationPoint calibrationPoint in _focalPowerCalibration)
        //    {
        //        calibrationPoint.IBinary = convertAmperageToBinary(calibrationPoint.I);
        //        calibrationPoint.FShifted = calibrationPoint.F;
        //    }

        //    //find nearest datapoint in vector
        //    int delta = 9999;
        //    int index = 0;

        //    foreach (focalPowerCalibrationPoint calibrationPoint in _focalPowerCalibration)
        //    {
        //        if (Math.Abs(calibrationPoint.IBinary - current) < delta)
        //        {
        //            delta = Math.Abs(calibrationPoint.IBinary - current);
        //            index = _focalPowerCalibration.IndexOf(calibrationPoint);
        //        }
        //    }

        //    int endIndex = _focalPowerCalibration.Count - 1;
        //    bool calculationDone = false;

        //    if (index == 0)
        //    {
        //        a = (_focalPowerCalibration[1].F - _focalPowerCalibration[0].F) / (_focalPowerCalibration[1].IBinary - _focalPowerCalibration[0].IBinary);
        //        b = _focalPowerCalibration[1].F - a * _focalPowerCalibration[1].IBinary;
        //        calculationDone = true;
        //    }
        //    else if (index == endIndex && calculationDone == false)
        //    {
        //        a = (_focalPowerCalibration[endIndex].F - _focalPowerCalibration[endIndex - 1].F) / (_focalPowerCalibration[endIndex].IBinary - _focalPowerCalibration[endIndex - 1].IBinary);
        //        b = _focalPowerCalibration[endIndex].F - a * _focalPowerCalibration[endIndex].IBinary;
        //        calculationDone = true;
        //    }
        //    else if (calculationDone == false)
        //    {
        //        if (current < _focalPowerCalibration[index].IBinary)
        //        {
        //            a = (_focalPowerCalibration[index].F - _focalPowerCalibration[index - 1].F) / (_focalPowerCalibration[index].IBinary - _focalPowerCalibration[index - 1].IBinary);
        //            b = _focalPowerCalibration[index].F - a * _focalPowerCalibration[index].IBinary;
        //        }
        //        else
        //        {
        //            a = (_focalPowerCalibration[index + 1].F - _focalPowerCalibration[index].F) / (_focalPowerCalibration[index + 1].IBinary - _focalPowerCalibration[index].IBinary);
        //            b = _focalPowerCalibration[index].F - a * _focalPowerCalibration[index].IBinary;
        //        }
        //    }

        //    return a * current + b;
        //}


        //private double TCgetdFPdT(double targetF)
        //{
        //    if (temperatureCalibrationAvailable == false) return 0;

        //    // y = a*x + b
        //    double a = 0;
        //    double b = 0;

        //    //find nearest datapoint in vector
        //    double delta = 9999;
        //    int index = 0;

        //    foreach (tempratureCompensationPoint compensationPoint in _temperatureCompensation)
        //    {
        //        if (Math.Abs(compensationPoint.F - targetF) < delta)
        //        {
        //            delta = Math.Abs(compensationPoint.F - targetF);
        //            index = _temperatureCompensation.IndexOf(compensationPoint);
        //        }
        //    }

        //    int endIndex = _temperatureCompensation.Count - 1;
        //    bool calculationDone = false;

        //    if (index == 0)
        //    {
        //        a = (_temperatureCompensation[1].dFdT - _temperatureCompensation[0].dFdT) / (_temperatureCompensation[1].F - _temperatureCompensation[0].F);
        //        b = _temperatureCompensation[1].dFdT - a * _temperatureCompensation[1].F;
        //        calculationDone = true;
        //    }
        //    else if (index == endIndex && calculationDone == false)
        //    {
        //        a = (_temperatureCompensation[endIndex].dFdT - _temperatureCompensation[endIndex - 1].dFdT) / (_temperatureCompensation[endIndex].F - _temperatureCompensation[endIndex - 1].F);
        //        b = _temperatureCompensation[endIndex].dFdT - a * _temperatureCompensation[endIndex].F;
        //        calculationDone = true;
        //    }
        //    else if (calculationDone == false)
        //    {
        //        if (targetF < _temperatureCompensation[index].F)
        //        {
        //            a = (_temperatureCompensation[index].dFdT - _temperatureCompensation[index - 1].dFdT) / (_temperatureCompensation[index].F - _temperatureCompensation[index - 1].F);
        //            b = _temperatureCompensation[index].dFdT - a * _temperatureCompensation[index].F;
        //        }
        //        else
        //        {
        //            a = (_temperatureCompensation[index+1].dFdT - _temperatureCompensation[index].dFdT) / (_temperatureCompensation[index + 1].F - _temperatureCompensation[index].F);
        //            b = _temperatureCompensation[index].dFdT - a * _temperatureCompensation[index].F;
        //        }
        //    }

        //    return a * targetF + b;
        //}

        #endregion // Temperature and Focal Power




        #region SendData
        public void Send(string s)
        {
            if (_comport.IsOpen)
            {
                try
                {
                    _comport.Write(s);
                }
                catch (Exception)
                {
                    LowLevelDeinitialize(true);
                }
            }
        }


        public void Send(byte[] s)
        {
            if (_comport.IsOpen)
            {
                try
                {
                    _comport.DiscardOutBuffer();
                    _comport.Write(s, 0, s.Length);
                }
                catch (Exception)
                {
                    LowLevelDeinitialize(true);
                }
            }
        }
        #endregion





        #region ReceiveData
        private void WaitForAnswer()
        {
            if (_comport.IsOpen == false) return;

            string stringBuffer = String.Empty;
            long startInTicks = DateTime.Now.Ticks;
            byte[] byteBuffer = new byte[0];
            int timeoutPeriodMilliseconds = 1000;
            bool commandLengthError = false;
            bool timeoutError = false;

            while (true)
            {
                if (DateTime.Now.Ticks - startInTicks > TimeSpan.TicksPerMillisecond * timeoutPeriodMilliseconds)
                {
                    timeoutError = true;
                    break;
                }

                if (byteBuffer.Length > 16)
                {
                    commandLengthError = true;
                    break; //break if more than 10 bytes were received without termination characters
                }

                if (_comport.BytesToRead > 0)
                {
                    int bytesToRead = _comport.BytesToRead;
                    byte[] readBuffer = new byte[bytesToRead];

                    try
                    {
                        _comport.Read(readBuffer, 0, bytesToRead); //read existing bytes from input buffer
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not read from serial port.\nMessage: " + ex.Message);
                        break;
                    }

                    //combine newly read bytes from buffer with already stored bytes
                    byteBuffer = Combine(byteBuffer, readBuffer);

                    //check for message end and break while if found
                    if (byteBuffer.Length >= 2)
                    {
                        if ((byte)byteBuffer[byteBuffer.Length - 1] == (byte)10 && (byte)byteBuffer[byteBuffer.Length - 2] == (byte)13) // check for \r\n
                        {
                            break;
                        }
                    }
                }
            }





            if (timeoutError == true)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Timeout));
            }
            else if (commandLengthError == true)
            {
                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.CommunicationError));
            }
            else
            {
                // create string version of byte array to read out command type
                stringBuffer = System.Text.Encoding.UTF8.GetString(byteBuffer);

                //filter by command and do appropraite things
                if (stringBuffer == "N\r\n")
                {
                    if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.CommunicationCRCError));
                }
                else if (stringBuffer.Length >= 5 && stringBuffer.Substring(0, 5) == "Ready")
                {
                    debugMessage("Received Ready");
                }
                else if (stringBuffer.Length >= 12 && stringBuffer.Substring(0, 2) == "IR")
                {
                    debugMessage("Received ID");
                    if (byteBuffer.Length >= 12)
                    {
                        ID = stringBuffer.Substring(2, 8);

                        //TODO CHECK FOR HARDWARE ID COMPATIBILITY --> WrongHardwareID

                        _IDOK = true;
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.HardwareID));
                    }
                    else
                    {
                        // obviously wrong hardware since ID doesn't have the correct length
                        _IDOK = false;
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.WrongHardwareID));
                    }
                }
                else if (stringBuffer.Length >= 5 && stringBuffer.Substring(0, 1) == "V")
                {
                    debugMessage("Received Fimware version");

                    if (byteBuffer.Length >= 5)
                    {
                        firmwareVersionObject.major = (int)(byteBuffer[1]);
                        firmwareVersionObject.minor = (int)(byteBuffer[2]);

                        if (byteBuffer.Length >= 9)
                        {
                            firmwareVersionObject.build = (int)((byteBuffer[3] << 8) | byteBuffer[4]);
                            firmwareVersionObject.revision = (int)((byteBuffer[5] << 8) | byteBuffer[6]);
                        }


                        linkEstablished = true;


                        if (incompatibilityList.Contains(firmwareVersionObject) == true)
                        {
                            //this command has to come AFTER setting linkEstablished to true
                            if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.ExpiredFirmwareVersion));
                        }
                        else if (compatibilityList.Contains(firmwareVersionObject) == false)
                        {
                            //this command has to come AFTER setting linkEstablished to true
                            if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.OutdatedFirmwareVersion));
                        }
                        //TODO else
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Initialization));
                    }

                }

                else if (stringBuffer.Length >= 5 && stringBuffer.Substring(0, 1) == "X")
                {
                    serialID = stringBuffer.Substring(1, 8);
                }

                else if (stringBuffer.Length >= 5 && stringBuffer.Substring(0, 1) == "A")
                {
                    if (byteBuffer.Length >= 5)
                    {
                        current = (Int16)((byteBuffer[1] << 8) | byteBuffer[2]);

                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.CurrentUpdate));

                        debugMessage("Current received");
                    }
                }
                else if (stringBuffer.Length >= 5 && stringBuffer.Substring(0, 2) == "GA")
                {
                    debugMessage("Received analog A value");

                    if (stringBuffer.Substring(0, 3) == "GAA")
                    {
                        if (byteBuffer.Length >= 5)
                        {
                            analog = (Int16)((byteBuffer[3] << 8) | byteBuffer[4]);
                        }
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Analog));
                    }
                }
                else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 3) == "CLA")
                {
                    debugMessage("Received LA");
                    if (byteBuffer.Length >= 6)
                    {
                        currentLowerSoftwareLimit = (Int16)((byteBuffer[3] << 8) | byteBuffer[4]);
                    }
                }
                else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 3) == "CUA")
                {
                    debugMessage("Received UA");
                    if (byteBuffer.Length >= 6)
                    {
                        currentUpperSoftwareLimit = (Int16)((byteBuffer[3] << 8) | byteBuffer[4]);
                    }
                }
                else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 3) == "CMA")
                {
                    debugMessage("Received MA");
                    if (byteBuffer.Length >= 6)
                    {
                        maxCurrent = ((byteBuffer[3] << 8) | byteBuffer[4]);
                    }
                }
                else if (stringBuffer.Length >= 5 && stringBuffer.Substring(0, 2) == "Or")
                {
                    debugMessage("Received Drift Gain");
                    if (byteBuffer.Length >= 5)
                    {
                        currentDriftGain = (int)((byteBuffer[2] << 8) | byteBuffer[3]);
                    }
                }

                else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 2) == "Ow")
                {
                    if (byteBuffer.Length >= 6)
                    {
                        _hardwareErrors.SetErrorByte(byteBuffer[2]);

                        communicationWithLensEstablished = !_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.LensCommunicationError);
                        focalLengthOutsideFirmwareRange = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.FocalLengthOutsideFirmwareRange);
                        temperatureOutsideFirmwareRange = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.TemperatureOutsideFirmwareRange);
                        EEPROMAvailable = !_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.NoOrFaultyEEPROM);
                        cannotReachFocalPower = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.CannotHoldFocalLength);
                        calibratedLensConnected = EEPROMAvailable && communicationWithLensEstablished;

                        if (communicationWithLensEstablished && oldCommunicationEstablished != communicationWithLensEstablished)
                        {
                            SendSerialIDCommand();
                            serialIDErrorMessageShown = false;
                        }
                        oldCommunicationEstablished = communicationWithLensEstablished;

                        if (operationMode == OperatingMode.FocalPower)
                        {
                            if (_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.FocalLengthInversion) ||
                                    _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.LensCommunicationError) ||
                                    _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.NoOrFaultyEEPROM))
                            {
                                //cannot enable controlled mode
                                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerLimitsError, _hardwareErrors.GetErrorString()));
                                SetOperationMode(OperatingMode.Current);
                            }
                        }
                        //else
                        //{
                        upperFocalPowerLimit = (Int16)((byteBuffer[3] << 8) | byteBuffer[4]);
                        lowerFocalPowerLimit = (Int16)((byteBuffer[5] << 8) | byteBuffer[6]);
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerLimits));
                        //}

                        debugMessage("Diopter limits received");
                    }
                }

                else if (stringBuffer.Length >= 3 && stringBuffer.Substring(0, 2) == "MS")
                {

                    if (stringBuffer.Substring(0, 3) == "MSA")
                    {
                        operationMode = OperatingMode.Sinusoidal;
                    }

                    sensorControlAvailable = false;
                    debugMessage("Mode Sinusoidal Received");
                }
                else if (stringBuffer.Length >= 3 && stringBuffer.Substring(0, 2) == "MD")
                {
                    if (stringBuffer.Substring(0, 3) == "MDA")
                    {
                        operationMode = OperatingMode.Current;
                        sensorControlAvailable = true;
                    }

                    debugMessage("Mode DC Received");
                }

                else if (stringBuffer.Length >= 3 && stringBuffer.Substring(0, 1) == "L")
                {
                    if (byteBuffer[1] == 1)
                    {
                        operationMode = LensDriver.OperatingMode.Analog;
                        sensorControlAvailable = false;
                        debugMessage("Analog Mode Received");
                    }
                }
                else if (stringBuffer.Length >= 3 && stringBuffer.Substring(0, 2) == "MC")
                {
                    if (stringBuffer.Substring(0, 3) == "MCA")
                    {
                        if (byteBuffer.Length >= 7)
                        {
                            _hardwareErrors.SetErrorByte(byteBuffer[3]);

                            if (_hardwareErrors.ContainsNoControlledModeError())
                            {
                                //cannot enable controlled mode
                                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerModeError, _hardwareErrors.GetErrorString()));
                                SetOperationMode(OperatingMode.Current);
                            }
                            else
                            {
                                upperFocalPowerLimit = (int)((byteBuffer[4] << 8) | byteBuffer[5]);
                                lowerFocalPowerLimit = (int)((byteBuffer[6] << 8) | byteBuffer[7]);
                                operationMode = OperatingMode.FocalPower;
                                sensorControlAvailable = true;
                            }
                        }
                    }
                    debugMessage("Mode Focal Power Received");
                }
                else if (stringBuffer.Length >= 3 && stringBuffer.Substring(0, 2) == "MQ")
                {
                    if (stringBuffer.Substring(0, 3) == "MQA")
                    {
                        operationMode = OperatingMode.Rectangular;
                    }

                    sensorControlAvailable = false;
                    debugMessage("Mode Square Received");
                }
                else if (stringBuffer.Length >= 3 && stringBuffer.Substring(0, 2) == "MT")
                {
                    if (stringBuffer.Substring(0, 3) == "MTA")
                    {
                        operationMode = OperatingMode.Triangular;
                    }

                    sensorControlAvailable = false;

                    debugMessage("Mode Triangular Received");
                }
                else if (stringBuffer.Length >= 2 && stringBuffer.Substring(0, 2) == "TA")
                {
                    if (byteBuffer.Length >= 5)
                    {
                        _hardwareErrors.SetErrorByte(byteBuffer[2]);
                        communicationWithLensEstablished = !_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.LensCommunicationError);
                        focalLengthOutsideFirmwareRange = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.FocalLengthOutsideFirmwareRange);
                        temperatureOutsideFirmwareRange = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.TemperatureOutsideFirmwareRange);
                        EEPROMAvailable = !_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.NoOrFaultyEEPROM);
                        cannotReachFocalPower = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.CannotHoldFocalLength);
                        calibratedLensConnected = EEPROMAvailable && communicationWithLensEstablished;

                        if (communicationWithLensEstablished && oldCommunicationEstablished != communicationWithLensEstablished)
                        {
                            SendSerialIDCommand();
                            serialIDErrorMessageShown = false;
                            temperature = convertBinaryToTemperature((uint)(((byteBuffer[3] << 8) | byteBuffer[4])));
                        }
                        else if (communicationWithLensEstablished)
                        {
                            temperature = convertBinaryToTemperature((uint)(((byteBuffer[3] << 8) | byteBuffer[4])));
                        }
                        oldCommunicationEstablished = communicationWithLensEstablished;
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.TemperatureUpdate));
                    }
                }
                else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 2) == "PT")
                {
                    if (byteBuffer.Length >= 6)
                    {
                        _hardwareErrors.SetErrorByte(byteBuffer[2]);

                        communicationWithLensEstablished = !_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.LensCommunicationError);
                        focalLengthOutsideFirmwareRange = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.FocalLengthOutsideFirmwareRange);
                        temperatureOutsideFirmwareRange = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.TemperatureOutsideFirmwareRange);
                        EEPROMAvailable = !_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.NoOrFaultyEEPROM);
                        cannotReachFocalPower = _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.CannotHoldFocalLength);
                        calibratedLensConnected = EEPROMAvailable && communicationWithLensEstablished;

                        if (communicationWithLensEstablished && oldCommunicationEstablished != communicationWithLensEstablished)
                        {
                            SendSerialIDCommand();
                            serialIDErrorMessageShown = false;
                        }
                        oldCommunicationEstablished = communicationWithLensEstablished;

                        if (operationMode == OperatingMode.FocalPower)
                        {
                            if (_hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.FocalLengthInversion) ||
                                    _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.LensCommunicationError) ||
                                    _hardwareErrors.GetErrorStatus(HardwareErrors.ErrorDescriptor.NoOrFaultyEEPROM))
                            {
                                //cannot enable controlled mode
                                if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerLimitsError, _hardwareErrors.GetErrorString()));
                                SetOperationMode(OperatingMode.Current);
                            }
                        }
                        //else
                        //{
                        upperFocalPowerLimit = (int)((byteBuffer[3] << 8) | byteBuffer[4]);
                        lowerFocalPowerLimit = (int)((byteBuffer[5] << 8) | byteBuffer[6]);
                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerLimits));
                        //}

                        debugMessage("Diopter limits received");
                    }

                }
                else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 2) == "PD")
                {
                    if (byteBuffer.Length >= 6)
                    {
                        focalPower = (Int16)((byteBuffer[2] << 8) | byteBuffer[3]);

                        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.FocalPowerUpdate));
                        debugMessage("Focal power received");
                    }
                }
                //else if (stringBuffer.Length >= 6 && stringBuffer.Substring(0, 2) == "PK")
                //{
                //    if (byteBuffer.Length >= 6)
                //    {
                //        systemTimeConstantFalling = (int)(byteBuffer[3]);
                //        systemTimeConstantRising = (int)(byteBuffer[2]);


                //        if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.SystemTimeConstantsUpdate));
                //        debugMessage("System time constants received");
                //    }
                //}

            }
        }


        private void ReceivedData(Object sender, SerialDataReceivedEventArgs e)
        {
            if (_dontRunHandler) return;


            string stringBuffer;
            ArrayList entireBuffer = new ArrayList();
            List<byte[]> byteBuffers = new List<byte[]>();


            while (_comport.BytesToRead > 0)
            {
                byte[] readBuffer = new byte[_comport.BytesToRead];

                int resultOfRead = _comport.Read(readBuffer, 0, 1); //read byte by byte

                entireBuffer.Add(readBuffer[0]);

                if (entireBuffer.Count >= 3)
                {
                    if ((byte)entireBuffer[entireBuffer.Count - 1] == (byte)10 && (byte)entireBuffer[entireBuffer.Count - 2] == (byte)13) // check for \r\n
                    {
                        byte[] currentByteBuffer = new byte[1];
                        try
                        {
                            currentByteBuffer = (byte[])entireBuffer.ToArray(typeof(byte));  // Export our arraylist into a byte array.
                            byteBuffers.Add(currentByteBuffer);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't Convert byte [] in readSerialPort()\n" + ex.Message);
                        }
                        entireBuffer.Clear();
                    }


                    if (entireBuffer.Count > 100)
                    {
                        break; //break while if more than 10 bytes were received
                    }
                }
            }


            foreach (byte[] byteBuffer in byteBuffers)
            {
                // create string version of byte array to read out command type
                stringBuffer = System.Text.Encoding.UTF8.GetString(byteBuffer);
                if (stringBuffer.Contains("Info") == true)
                {
                    MessageBox.Show("Info: " + stringBuffer);
                }
                else
                {
                    //MessageBox.Show("Error: " + stringBuffer);
                    if (Message != null) this.Message(this, new LensDriverMessageEventArgs(MessageType.Timeout));
                }

            } // END FOREACH 
        }
        #endregion





        #region CRC Helpers
        private byte[] AddCRC(byte[] command)
        {
            UInt16 CRC = 0;

            byte[] commandWithCRC = new byte[command.Length + 2];

            CRC = CRC16IBMCalculator.ComputeChecksum(command);

            Array.Copy(command, 0, commandWithCRC, 0, command.Length);

            commandWithCRC[commandWithCRC.Length - 2] = (byte)(CRC & 0xFF);
            commandWithCRC[commandWithCRC.Length - 1] = (byte)(CRC >> 8);

            return commandWithCRC;
        }
        #endregion




        #region SerialPortHelpers
        private void DisableReceivedEventHandler()
        {
            _dontRunHandler = true;
        }

        private void EnableReceivedEventHandler()
        {
            _dontRunHandler = false;
        }

        public void FlushInBuffer()
        {
            if (_comport.IsOpen)
            {
                _comport.DiscardInBuffer();
            }
        }

        public void FlushOutBuffer()
        {
            if (_comport.IsOpen)
            {
                _comport.DiscardOutBuffer();
            }
        }
        #endregion




        #region String and Byte[] helpers
        private static byte[] StringToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        private static byte[] Combine(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            System.Buffer.BlockCopy(a, 0, c, 0, a.Length);
            System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }


        #endregion




        #region Converters
        public double convertBinaryToCurrent(int current)
        {
            double returnValue = 0;

            //constrain to max and min values
            if (current > 4095) current = 4095;
            if (current < -4095) current = -4095;

            returnValue = current / 4095.0 * maxCurrent / 100.0;
            return returnValue;
        }

        public int convertCurrentToBinary(double current)
        {
            int returnValue = (int)(100 * 4095 * current / maxCurrent);

            //constrain to max and min values
            if (returnValue > 4095) returnValue = 4095;
            if (returnValue < -4095) returnValue = -4095;

            return returnValue;
        }

        public uint convertTemperatureToBinary(double temperature)
        {
            //   if(serialID.Contains("AF")) return (uint)(temperature + 100)*256;
            //   else if(serialID.Contains("AE")) return (uint)((uint)(temperature * 16) & 0x1FFF);
            //  else return 0;
            return (uint)((uint)(temperature * 16) & 0x1FFF);
        }

        public double convertBinaryToTemperature(uint temperature)
        {
            //   if (serialID.Contains("AF")) return (double) (temperature) / 256 - 100;
            //  else if (serialID.Contains("AE")) return SignExtension(temperature) * 0.0625;
            //   else return 0;
            return SignExtension(temperature) * 0.0625;
        }

        public double decodeDriftGain(double driftGain)
        {
            return driftGain / 100;
        }

        public Int16 encodeDriftGain(double driftGain)
        {
            return (Int16)(driftGain * 100);
        }

        /// <summary>
        /// Converts uint containing 12bit signed integer data to signed 32bit integer
        /// </summary>
        /// <param name="input">uint containing 12bit signed integer data</param>
        /// <returns>signed 32bit integer</returns>
        private int SignExtension(uint input)
        {
            uint value = (0x00000FFF & input);
            int mask = 0x00000800;
            if ((mask & input) > 0)
            {
                value = value | 0xFFFFF000;
            }
            return (int)value;
        }


        public static Int32 convertFrequencyToBinary(double frequency)
        {
            int returnValue = 0;

            returnValue = (Int32)(frequency * 1000.0);

            if (returnValue <= 0) returnValue = 1;
            if (returnValue > Int32.MaxValue) returnValue = Int32.MaxValue;

            return returnValue;
        }

        public static double convertBinaryToFrequency(int frequencyAsBinary)
        {
            return frequencyAsBinary / 1000.0; ;
        }


        public double convertBinaryToDiopters(int diopters)
        {
            if (serialID.Contains("AF")) return (diopters / 200.0);
            else if (serialID.Contains("AE")) return ((diopters / 200.0) - 5);
            else
                // Serial ID is incorrect
                if (!serialIDErrorMessageShown && firmwareVersionObject.minor == 8)
                {
                    serialIDErrorMessageShown = true;
                    MessageBox.Show("The serial number of the connected lens is invalid.\n\n Please reconnect Lens Driver.\n If error persists, contact Optotune at +41 58 856 3000.");
                }
            return ((diopters / 200.0) - 5);
        }

        public int convertDioptersToBinary(double diopters)
        {
            if (serialID.Contains("AF")) return (int)(diopters * 200);
            else if (serialID.Contains("AE")) return (int)((diopters + 5) * 200);
            else
                // Serial ID is incorrect
                if (!serialIDErrorMessageShown && firmwareVersionObject.minor == 8)
                {
                    serialIDErrorMessageShown = true;
                    MessageBox.Show("The serial number of the connected lens is invalid.\n\n Please reconnect Lens Driver.\n If error persists, contact Optotune at +41 58 856 3000.");
                }
            return (int)((diopters + 5) * 200);
        }


        #endregion Converters




        #region Debug Helpers

        private void debugMessage(string message)
        {
            if (DEBUG_MODE)
            {
                Console.WriteLine(message);
            }
        }

        #endregion Debug Helpers
    }

}
