using System;
using System.Runtime.Serialization;

namespace GenerateLogs.Models
{
    /// <summary>
    /// With the move to .NET Core, there is not a nice exception
    /// for the configuration key is present, but the value is unacceptable
    /// </summary>
    [Serializable]
    public class RequiredConfigurationBadValueException : Exception
    {
        private const string configurationKeyName = "configurationKey";
        
        /// <summary>
        /// Configuration Key 
        /// </summary>
        public string ConfigurationKey { get; set; }

        private const string configurationKeyValue = "configurationValue";

        /// <summary>
        /// Configuration Key 
        /// </summary>
        public object ConfigurationValue { get; set; }


        public RequiredConfigurationBadValueException() : base()
        {
        }

        public RequiredConfigurationBadValueException(string keyName, object value, string message) : base(message)
        {
            this.ConfigurationKey = keyName;
            this.ConfigurationValue = value;
        }

        public RequiredConfigurationBadValueException(string message) : base(message)
        {
        }

        public RequiredConfigurationBadValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RequiredConfigurationBadValueException(string keyName, object value, string message, Exception innerException) : base(message, innerException)
        {
            this.ConfigurationKey = keyName;
            this.ConfigurationValue = value;
        }

        protected RequiredConfigurationBadValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                this.ConfigurationKey = info.GetString(configurationKeyName);
                this.ConfigurationValue = info.GetValue(configurationKeyValue, typeof(object));
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue(configurationKeyName, this.ConfigurationKey);
                info.AddValue(configurationKeyValue, this.ConfigurationValue);
            }
        }

        public override string ToString()
        {
            return $"Configuration Key: '{this.ConfigurationKey}' has bad value '{this.ConfigurationValue}'";
        }

    }
}
