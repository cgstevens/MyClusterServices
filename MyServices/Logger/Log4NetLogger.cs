using System;
using System.Text.RegularExpressions;
using Akka.Actor;
using Akka.Event;
using log4net;
using Microsoft.ApplicationInsights;
using MyServices.Shared.Helpers;
using Log4Net = log4net.ILog;

namespace MyServices.Shared.Logger
{
    /// <summary>
    /// This class is used to receive log events and sends them to
    /// the configured Log4Net logger. The following log events are
    /// recognized: <see cref="Debug"/>, <see cref="Info"/>,
    /// <see cref="Warning"/> and <see cref="Error"/>.
    /// </summary>
    public class Log4NetLogger : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        private static void Log(LogEvent logEvent, Action<Log4Net> logStatement)
        {
            var logger = LogManager.GetLogger("CommissionsAkka");
            logStatement(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogger"/> class.
        /// </summary>
        public Log4NetLogger()
        {
            Receive<Error>(m => Log(m, logger =>
            {
                GlobalContext.Properties["ActorPath"] = m.LogSource;
                GlobalContext.Properties["EventID"] = ParseEventIdOutOfMessage(m.Message.ToString());

                logger.Error(string.Format("{0}", m.Message), m.Cause);
                
                GlobalContext.Properties["ActorPath"] = null;
                GlobalContext.Properties["EventID"] = 0;
            }));
            Receive<Fatal>(m => Log(m, logger =>
            {
                GlobalContext.Properties["ActorPath"] = m.LogSource;
                GlobalContext.Properties["EventID"] = ParseEventIdOutOfMessage(m.Message.ToString());

                logger.Fatal(string.Format("{0}", m.Message), m.Cause);
                
                GlobalContext.Properties["ActorPath"] = null;
                GlobalContext.Properties["EventID"] = 0;
            }));
            Receive<Warning>(m => Log(m, logger =>
            {
                GlobalContext.Properties["ActorPath"] = m.LogSource;
                GlobalContext.Properties["EventID"] = ParseEventIdOutOfMessage(m.Message.ToString());

                logger.Warn(string.Format("{0}", m.Message));

                GlobalContext.Properties["ActorPath"] = null;
                GlobalContext.Properties["EventID"] = 0;
            }));
            Receive<Info>(m => Log(m, logger =>
            {
                GlobalContext.Properties["ActorPath"] = m.LogSource;
                GlobalContext.Properties["EventID"] = ParseEventIdOutOfMessage(m.Message.ToString());

                logger.Info(string.Format("{0}", m.Message));

                GlobalContext.Properties["ActorPath"] = null;
                GlobalContext.Properties["EventID"] = 0;
            }));
            Receive<Debug>(m => Log(m, logger =>
            {
                GlobalContext.Properties["ActorPath"] = m.LogSource;
                GlobalContext.Properties["EventID"] = ParseEventIdOutOfMessage(m.Message.ToString());

                logger.Debug(string.Format("{0}", m.Message));

                GlobalContext.Properties["ActorPath"] = null;
                GlobalContext.Properties["EventID"] = 0;
            }));
            Receive<InitializeLogger>(m =>
            {
                _log.Debug("Log4NetLogger started");
                Sender.Tell(new LoggerInitialized());
            });
        }

        private int ParseEventIdOutOfMessage(string message)
        {
            int eventId = 0;

            Regex reg = new Regex(@"EventId:[0-9]");
            foreach (Match match in reg.Matches(message))
            {
                var e = match.Value.Split(':');
                int.TryParse(e[1], out eventId);
            }

            return eventId;
        }
    }

    public class Fatal : LogEvent
    {
        public Exception Cause { get; private set; }

        public Fatal(Exception cause, string logSource, Type logClass, object message)
        {
            Cause = cause;
            LogSource = logSource;
            LogClass = logClass;
            Message = message;
        }
        public override LogLevel LogLevel()
        {
            return Akka.Event.LogLevel.InfoLevel;
        }

        public override string ToString()
        {
            var cause = Cause;
            var causeStr = cause?.ToString() ?? "Unknown";
            var errorStr = string.Format("[{0}][{1}][Thread {2}][{3}] {4}{5}Cause: {6}",
                LogLevel().ToString().Replace("Level", "").ToUpperInvariant(), Timestamp,
                Thread.ManagedThreadId.ToString().PadLeft(4, '0'), LogSource, Message, Environment.NewLine, causeStr);
            return errorStr;
        }
    }
}
