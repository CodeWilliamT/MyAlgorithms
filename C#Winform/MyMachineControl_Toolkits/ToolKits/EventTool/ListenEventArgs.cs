using System;

namespace ToolKits.EventTool
{
    public class ListenEventArgs : EventArgs
    {
        private EventData iData;

        public ListenEventArgs(EventData iData)
        {
            this.iData = iData;
        }

        public EventData IData
        {
            get
            {
                return this.iData;
            }
            set
            {
                this.iData = value;
            }
        }
    }
}

