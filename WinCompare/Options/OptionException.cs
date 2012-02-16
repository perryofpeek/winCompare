using System;
using System.Runtime.Serialization;

namespace NDesk.Options
{   
    [Serializable]
    public class OptionException : Exception {
        private string option;

        public OptionException (string message, string optionName)
            : base (message)
        {
            this.option = optionName;
        }

        public OptionException (string message, string optionName, Exception innerException)
            : base (message, innerException)
        {
            this.option = optionName;
        }

        protected OptionException (SerializationInfo info, StreamingContext context)
            : base (info, context)
        {
            this.option = info.GetString ("OptionName");
        }

        public string OptionName {
            get {return this.option;}
        }

        public override void GetObjectData (SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData (info, context);
            info.AddValue ("OptionName", option);
        }
    }
}