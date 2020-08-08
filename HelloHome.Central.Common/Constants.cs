namespace HelloHome.Central.Common
{
    public static class Constants
    {
        public static class Message
        {
            public static class Report
            {
                public const byte SendingStatusReport = 0 + (0 << 2);
                public const byte NodeStartedReport = 0 + (1 << 2);
                public const byte NodeInfoReport = 0 + (2 << 2);
                public const byte EnvironmentReport = 0 + (3 << 2);
                public const byte PulseReport = 0 + (4 << 2);
                public const byte PushButtonPressedReport = 0 + (5 << 2);
                public const byte SwitchButtonActivatedReport = 0 + (6 << 2);
                public const byte VarioButtonChangedReport = 0 + (7 << 2);
            }

            public static class Command
            {
                public const byte NodeConfigCommand = 2 + (0 << 2);
                public const byte NodeRestartCommand = 2 + (1 << 2);
                public const byte SetRelayStateCommand = 2 + (2 << 2);
            }
        }
    }
}