[System.Serializable()]
class LocationNotFoundException : System.Exception
{
    public LocationNotFoundException() : base() { }
    public LocationNotFoundException(string message) : base(message) { }
    public LocationNotFoundException(string message, System.Exception inner) : base(message, inner) { }

    // A constructor is needed for serialization when an
    // exception propagates from a remoting server to the client. 
    protected LocationNotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
    { }
}