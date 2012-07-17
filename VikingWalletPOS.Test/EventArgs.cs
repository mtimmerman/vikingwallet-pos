using System;

namespace VikingWalletPOS
{
    #region LogEventArgs
    /// <summary>
    /// Event args used for when the server logs something
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// The message that was logged
        /// </summary>
        public string Message { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="LogEventArgs"/>
        /// </summary>
        /// <param name="message">The message that was logged</param>
        public LogEventArgs(string message)
        {
            this.Message = message;
        }
        #endregion
    }
    #endregion

    #region ServerMessageEventArgs
    /// <summary>
    /// Event args used for when the server sent a message
    /// </summary>
    public class ServerMessageEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// The message that was received
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The Id for the message that was received
        /// </summary>
        public string MessageId { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of <see cref="ServerMessageEventArgs"/>
        /// </summary>
        /// <param name="message">The message that was received</param>
        /// <param name="messageId">The Id for the message that was received</param>
        public ServerMessageEventArgs(string message, string messageId = "")
        {
            this.Message = message;
            this.MessageId = messageId;
        }
        #endregion
    }
    #endregion
}
