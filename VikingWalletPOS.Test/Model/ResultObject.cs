namespace VikingWalletPOS.Model
{
    #region ResultObject
    /// <summary>
    /// Abstract class for Viking Spots API responses
    /// </summary>
    public abstract class ResultObject
    {
        /// <summary>
        /// All the messages returned from the response
        /// </summary>
        public Message[] messages { get; set; }        
    }
    #endregion

    #region Message
    /// <summary>
    /// Message information. A message is usually an error
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Description of the message
        /// </summary>
        public string msg_text { get; set; }
        /// <summary>
        /// Identifier of the message
        /// </summary>
        public string msg_code { get; set; }
        /// <summary>
        /// Optional field type
        /// </summary>
        public string opt_field_type { get; set; }
        /// <summary>
        /// Optional field value
        /// </summary>
        public string opt_field_value { get; set; }
    }
    #endregion
}
