[System.Serializable()]
class InternalException : System.Exception
{
    public InternalException() : base() { }
    public InternalException(string message) : base(message) { }
    public InternalException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected InternalException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
    { }
}