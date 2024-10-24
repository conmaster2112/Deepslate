using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Logs
{
    public partial class Logger: ILogger
    {
        public const string WARNTEXT = $"{ConsoleColors.RESET_FOREGROUND}[{ConsoleColors.BLINK + ConsoleColors.WARNING}WARNING{ConsoleColors.RESET_ALL}]";
        public const string SUCCESSTEXT = $"{ConsoleColors.RESET_FOREGROUND}[{ConsoleColors.SUCCESS}SUCCESS{ConsoleColors.RESET_FOREGROUND}]";
        public const string INFOTEXT = $"{ConsoleColors.RESET_FOREGROUND}[{ConsoleColors.INFO}INFO{ConsoleColors.RESET_FOREGROUND}]";
        public const string ERRORTEXT = $"{ConsoleColors.RESET_FOREGROUND}[{ConsoleColors.ERROR}ERROR{ConsoleColors.RESET_FOREGROUND}]";
        public const string LOGTEXT = $"{ConsoleColors.RESET_FOREGROUND}[LOG]";
        public const string DEBUGTEXT = $"{ConsoleColors.RESET_FOREGROUND}[{ConsoleColors.DEBUG}DEBUG{ConsoleColors.RESET_FOREGROUND}]";
        public readonly string Root;
        public static Logger Default { get; private set; } = new Logger(string.Empty);
        public Logger? Base { get; init; }
        public bool IsDebugEnabled { get; set; }
        protected Logger(string root, Logger? baseLogger = null)
        {
            Base = baseLogger;
            if (baseLogger != null) Root = $"{baseLogger.Root}{root} ";
            else Root = root + " ";
            IsDebugEnabled = Base?.IsDebugEnabled ?? false;
        }
        public void Error(object message, params object[] formatings)
        {
            Console.WriteLine($"{Root}{ERRORTEXT}{ConsoleColors.RESET_ALL} {message} {ConsoleColors.RESET_ALL}", formatings);
        }
        public void Warn(object message, params object[] formatings)
        {
            Console.WriteLine($"{Root}{WARNTEXT}{ConsoleColors.RESET_ALL} {message} {ConsoleColors.RESET_ALL}", formatings);
        }
        public void Success(object message, params object[] formatings)
        {
            Console.WriteLine($"{Root}{SUCCESSTEXT}{ConsoleColors.RESET_ALL} {message} {ConsoleColors.RESET_ALL}", formatings);
        }
        public void Info(object message, params object[] formatings)
        {
            Console.WriteLine($"{Root}{INFOTEXT}{ConsoleColors.RESET_ALL} {message} {ConsoleColors.RESET_ALL}", formatings);
        }
        public void Log(object message, params object[] formatings)
        {
            Console.WriteLine($"{ConsoleColors.DARKER}{Root}{LOGTEXT}{ConsoleColors.RESET_ALL}{ConsoleColors.DARKER} {message} {ConsoleColors.RESET_ALL}", formatings);
        }
        public void Debug(object message, params object[] formatings)
        {
            if(IsDebugEnabled) Console.WriteLine($" {DEBUGTEXT}{Root}{ConsoleColors.RESET_ALL}{message} {ConsoleColors.RESET_ALL}", formatings);
        }
        public Logger ChainCreate(string root, string color = "") => new($"[{color}{root}{ConsoleColors.RESET_ALL}]", this);
        public static Logger Create(string root) => Default.ChainCreate(root);
        public static void WriteError(object message, params object[] formatings) => Default.Error(message, formatings);
        public static void WriteWarn(object message, params object[] formatings) => Default.Warn(message, formatings);
        public static void WriteInfo(object message, params object[] formatings) => Default.Info(message, formatings);
        public static void WriteSuccess(object message, params object[] formatings) => Default.Success(message, formatings);
        public static void WriteLog(object message, params object[] formatings) => Default.Log(message, formatings);
        public static void WriteDebug(object message, params object[] formatings) => Default.Debug(message, formatings);
    }
}
