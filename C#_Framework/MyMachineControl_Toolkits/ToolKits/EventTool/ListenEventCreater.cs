using System;

namespace ToolKits.EventTool
{
    public class ListenEventCreater
    {
        public event EventHandler<ListenEventArgs> ListenEvent;

        public void ChangeValue(EventData iData)
        {
            ListenEventArgs args = new ListenEventArgs(iData);
            this.SendEvent(args);
        }

        protected virtual void SendEvent(ListenEventArgs args)
        {
            EventHandler<ListenEventArgs> listenEvent = this.ListenEvent;
            if (listenEvent != null)
            {
                listenEvent(this, args);
            }
        }
    }
}

