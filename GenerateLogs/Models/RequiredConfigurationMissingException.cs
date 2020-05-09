using System;
using System.Runtime.Serialization;

namespace GenerateLogs.Models
{
    /// <summary>
    /// With the move to .NET Core, there is not a nice exception
    /// for this key is missing and it can't be
    /// </summary>
    [Serializable]
    public class RequiredConfigurationMissingException : Exception
    {
        private const string configurationKeyName = "configurationKey";

        public string ConfigurationKey { get; set; }

        public RequiredConfigurationMissingException() : base()
        {
        }

        public RequiredConfigurationMissingException(string keyName, string message) : base(message)
        {
            this.ConfigurationKey = keyName;
        }

        public RequiredConfigurationMissingException(string message) : base(message)
        {
        }

        public RequiredConfigurationMissingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RequiredConfigurationMissingException(string keyName, string message, Exception innerException) : base(message, innerException)
        {
            this.ConfigurationKey = keyName;
        }

        protected RequiredConfigurationMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                this.ConfigurationKey = info.GetString(configurationKeyName);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue(configurationKeyName, this.ConfigurationKey);
            }
        }

        public override string ToString()
        {
            return $"Configuration Missing Required Key: '{this.ConfigurationKey}'";
        }

    }
}
