using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LensDriverController.Logic
{
    public class HardwareErrors
    {
        public enum ErrorDescriptor
        {
            FocalLengthOutsideFirmwareRange,
            TemperatureOutsideFirmwareRange,
            TemperatureOutsideProductRange,
            FocalLengthInversion,
            CannotHoldFocalLength,
            NoTemperatureLimits,
            NoOrFaultyEEPROM,
            LensCommunicationError
        }


        private class ErrorFlag
        {
            public int bitNumber { get; private set; }
            public bool statusFlag = false;
            public string errorMessage { get; private set; }

            public ErrorFlag(int mBitNumber, string mErrorMessage)
            {
                bitNumber = mBitNumber;
                errorMessage = mErrorMessage;
            }
        }


        private Dictionary<ErrorDescriptor, ErrorFlag> errorDictionary = new Dictionary<ErrorDescriptor, ErrorFlag>();

        private byte _errorByte;

        public HardwareErrors(byte errorByte = 0)
        {
            _errorByte = errorByte;

            errorDictionary.Add(ErrorDescriptor.TemperatureOutsideFirmwareRange, new ErrorFlag(8, "\n\u2022 Temperature is outside range defined in firmware settings."));
            errorDictionary.Add(ErrorDescriptor.FocalLengthOutsideFirmwareRange, new ErrorFlag(7, "\n\u2022 Focal length is outside range defined by current temperature settings."));
            errorDictionary.Add(ErrorDescriptor.TemperatureOutsideProductRange, new ErrorFlag(6, "\n\u2022 Temperature is outside product specifications." +
                "-Please visit http://optotune.com/downloads to obtain the latest product specifications \n"));

            errorDictionary.Add(ErrorDescriptor.FocalLengthInversion, new ErrorFlag(5, "\n\u2022 High focal length limit is lower than low focal length limit. \n" + 
                "   -Ensure max/min current limits are positive. \n"+
                "   -Temperature limit range must not be set too high. \n" +
                "   -Drift compensation gain parameter must not be set too high. \n"));
            errorDictionary.Add(ErrorDescriptor.CannotHoldFocalLength, new ErrorFlag(4, "\n\u2022 Cannot hold focal length.\n"));
            errorDictionary.Add(ErrorDescriptor.NoTemperatureLimits, new ErrorFlag(3, "\n\u2022 No temperature limits set.\n"));

            errorDictionary.Add(ErrorDescriptor.NoOrFaultyEEPROM, new ErrorFlag(2, "\n\u2022 Empty or invalid EEPROM structure. \n" +
                "   -Firmware version of driver might be out of date.\n" +
                "   -Please visit http://optotune.com/downloads for the latest version.\n"));

            errorDictionary.Add(ErrorDescriptor.LensCommunicationError, new ErrorFlag(1, "\n\u2022No lens communication possible.\n" + 
                "   -Please try reconnecting lens. \n"));

            Evaluate();
        }

        public bool GetErrorStatus(ErrorDescriptor descriptor)
        {
            return errorDictionary[descriptor].statusFlag;
        }

        public void SetErrorByte(byte errorByte)
        {
            _errorByte = errorByte;
            Evaluate();
        }

        public string GetErrorString()
        {
            string returnValue = String.Empty;

            foreach (KeyValuePair<ErrorDescriptor, ErrorFlag> entry in errorDictionary)
            {
                if (entry.Value.statusFlag == true)
                {
                    returnValue = returnValue + entry.Value.errorMessage;
                }
            }

            // Add last section
            string errorSupportMessage = "\nIf error persists, please contact Optotune support at +41 58 856 3000.";

            return returnValue + errorSupportMessage;
        }


        public bool ContainsNoControlledModeError()
        {
            return ((_errorByte & Convert.ToByte("00010111", 2)) > 0);
        }

        private void Evaluate()
        {
            foreach (KeyValuePair<ErrorDescriptor, ErrorFlag> entry in errorDictionary)
            {
                entry.Value.statusFlag = GetBit(entry.Value.bitNumber);
            }
        }

        private bool GetBit(int bitNumber)
        {
            return (_errorByte & (1 << bitNumber - 1)) != 0;
        }
    }
}
