namespace ToolKits.EventTool
{
    public class EventData
    {
        private object data;

        public EventData()
        {
            this.data = new object();
        }

        public EventData(object obj)
        {
            this.data = obj;
        }

        public object Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }

}

