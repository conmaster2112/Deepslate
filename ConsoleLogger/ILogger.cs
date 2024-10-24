namespace ConMaster.Logs
{
    public interface ILogger
    {
        public bool IsDebugEnabled { get; set; }
        public void Error(object message, params object[] formatings);
        public void Warn(object message, params object[] formatings);
        public void Success(object message, params object[] formatings);
        public void Info(object message, params object[] formatings);
        public void Log(object message, params object[] formatings);
        public void Debug(object message, params object[] formatings);
    }
}
