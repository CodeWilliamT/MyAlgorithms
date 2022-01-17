using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LensDriverController.Dto
{
    public class FirmwareVersion
    {
        public int major { get; set; }
        public int minor { get; set; }
        public int build { get; set; }
        public int revision { get; set; }

        public FirmwareVersion(int mMajor = 1, int mMinor = 0, int mBuild = 0, int mRevision = 0)
        {
            major = mMajor;
            minor = mMinor;
            build = mBuild;
            revision = mRevision;
        }

        public string GetFullString()
        {
            return major.ToString() + "." + minor.ToString() + "." + build.ToString() + "." + revision.ToString();
        }

        public string GetPartialString()
        {
            return major.ToString() + "." + minor.ToString() + "." + build.ToString();
        }


        public override bool Equals(object obj)
        {
            if (!(obj is LensDriverController.Dto.FirmwareVersion)) return false;

            if (((LensDriverController.Dto.FirmwareVersion)obj).major == major && ((LensDriverController.Dto.FirmwareVersion)obj).minor == minor) return true;
            else return false;
        }
    }
}
