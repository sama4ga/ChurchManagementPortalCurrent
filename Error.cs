namespace ChurchManagementPortal
{
    class Error
    {
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorType { get; set; }

        /// <summary>
        /// Returns information about an error
        /// </summary>
        /// <param name="errorMessage">Message about the error</param>
        /// <param name="errorCode">Code associated with the error</param>
        /// <param name="errorType">The type of error</param>
        public Error(string errorMessage, int errorCode = 0, string errorType = "")
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ErrorType = errorType;
        }
    }
}
