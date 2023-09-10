namespace UserDataValidation
{

    /// <summary>
    /// Parent class for the types of results that are generated from the
    /// validation process.
    /// </summary>
    public abstract class ValidationResult<TSuccess, TError>
    {

    }

    /// <summary>
    /// The result if the validation was successful, containing the output value.
    /// </summary>
    public class OKResult<TSuccess, TError> : ValidationResult<TSuccess, TError>
    {

        public TSuccess Success { get; private set; }

        public OKResult(TSuccess success)
        {
            this.Success = success;
        }



    }

    /// <summary>
    /// The result if the validation was unsuccessful, containing an object
    /// of the corresponding error type.
    /// </summary>
    public class ErrorResult<TSuccess, TError> : ValidationResult<TSuccess, TError>
    {

        public TError Error { get; private set; }

        public ErrorResult(TError error)
        {
            this.Error = error;
        }


    }



}