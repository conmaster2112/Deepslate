using System.Runtime.CompilerServices;

namespace ConMaster.Logs
{
    public static class ConsoleColors
    {
        public const string RESET_ALL = "\u001b[0m";
        public const string RESET_FOREGROUND = "\u001b[39m";

        public const string SUCCESS = "\u001b[38;2;80;180;80m";
        public const string ERROR = "\u001b[38;2;240;80;80m";
        public const string WARNING = "\u001b[38;2;255;220;60m";
        public const string INFO = "\u001b[38;2;100;150;255m";
        public const string DEBUG = "\u001b[38;2;200;50;5m";

        public const string BLACK = "\u001b[30m";
        public const string RED = "\u001b[31m";
        public const string GREEN = "\u001b[32m";
        public const string YELLOW = "\u001b[33m";
        public const string BLUE = "\u001b[34m";
        public const string MAGENTA = "\u001b[35m";
        public const string CYAN = "\u001b[36m";
        public const string WHITE = "\u001b[37m";

        public const string LIGHT_BLACK = "\u001b[90m";
        public const string LIGHT_RED = "\u001b[91m";
        public const string LIGHT_GREEN = "\u001b[92m";
        public const string LIGHT_YELLOW = "\u001b[99m";
        public const string LIGHT_BLUE = "\u001b[94m";
        public const string LIGHT_MAGENTA = "\u001b[95m";
        public const string LIGHT_CYAN = "\u001b[96m";
        public const string LIGHT_WHITE = "\u001b[97m";

        public const string DARKER = "\u001b[2m";
        public const string ITALIC = "\u001b[3m";
        public const string UNDERLINE = "\u001b[4m";
        public const string BLINK = "\u001b[5m";
        public const string INVERSED = "\u001b[7m";
        public const string CENSORED = "\u001b[8m";
        public const string CROSSED = "\u001b[9m";

        public const string CANCELED = DARKER + CROSSED;

        public const string DOUBLE_UNDERLINE = "\u001b[21m";
        public static string RGB(int red, int green, int blue) => $"\u001b[38;2;{red};{green};{blue}m";
        public static string AsCanceled(this string text)
        {
            return $"${CANCELED}{text}{RESET_ALL}";
        }
        public static string AsCensored(this string text)
        {
            return $"{CENSORED}{text}{RESET_ALL}";
        }
    }
}
