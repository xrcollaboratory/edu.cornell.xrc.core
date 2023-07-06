using UnityEngine;

namespace XRC.Common.Logger
{
    public class Logger : ILogger
    {
        public enum DebugLevel
        {   INFO,
            WARNING,
            ERROR,
            ALL
        }
        private readonly Object m_Context;
        
        public DebugLevel logLevel { get; set; }

        public Logger(Object context, string tag, DebugLevel level = DebugLevel.ERROR)
        {
            this.m_Context = context;
            this.tag = tag;
            this.logLevel = level;
        }

        public Logger(string tag, DebugLevel level = DebugLevel.ERROR)
        {
            this.tag = tag;
            this.logLevel = level;
        }

        public string tag { get; set; }


        public bool isErrorEnabled
        {
            get { return this.logLevel >= DebugLevel.ERROR; }
        }

        public bool isWarningEnabled
        {
            get { return this.logLevel >= DebugLevel.WARNING; }
        }

        public bool isInfoEnabled
        {
            get { return this.logLevel >= DebugLevel.INFO; }
        }

        public bool isDebugEnabled { get { return this.logLevel == DebugLevel.ALL; } }
        public void LogError(string fmt, params object[] args) { Debug.LogErrorFormat(fmt, args); }
        public void LogWarning(string fmt, params object[] args) { Debug.LogWarningFormat(fmt, args); }
        public void LogInfo(string fmt, params object[] args) { Debug.LogFormat(fmt, args); }
        public void LogDebug(string fmt, params object[] args) { Debug.LogFormat(fmt, args); }
        

        
        
    }
}