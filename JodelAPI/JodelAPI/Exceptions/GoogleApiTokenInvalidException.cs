[System.Serializable()]
class GoogleApiTokenInvalidException : System.Exception
{
    public GoogleApiTokenInvalidException() : base() { }
    public GoogleApiTokenInvalidException(string message) : base(message) { }
    public GoogleApiTokenInvalidException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected GoogleApiTokenInvalidException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
    { }
}