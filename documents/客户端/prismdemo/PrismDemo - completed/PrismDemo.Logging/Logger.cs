using log4net;
using log4net.Config;
using Microsoft.Practices.Prism.Logging;

namespace PrismDemo.Logging
{
  /// <summary>
  /// A log4Net implementation of PRISM' ILoggerFacade.
  /// *Note: Any logging framework could be plugged in here as long as we implement the ILoggerFacade interface.
  /// </summary>
  public class Logger : ILoggerFacade
  {
    // Member variables
    private static readonly ILog _logger = LogManager.GetLogger(typeof(Logger));

    public Logger()
    {
      XmlConfigurator.Configure();
    }

    #region ILoggerFacade Members

    /// <summary>
    /// Prism Log routine.
    /// </summary>
    /// <param name="message">The message to write.</param>
    /// <param name="category">The message category.</param>
    /// <param name="priority">Not used by Log4Net; pass Priority.None.</param>
    public void Log(string message, Category category, Priority priority)
    {
      switch (category)
      {
        case Category.Debug:
          _logger.Debug(message);
          break;

        case Category.Warn:
          _logger.Warn(message);
          break;

        case Category.Exception:
          _logger.Error(message);
          break;

        case Category.Info:
          _logger.Info(message);
          break;
      }
    }

    #endregion ILoggerFacade Members
  }
}