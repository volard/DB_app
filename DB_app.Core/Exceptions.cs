using System.Runtime.Serialization;

namespace DB_app.Repository;


// https://stackoverflow.com/questions/94488/what-is-the-correct-way-to-make-a-custom-net-exception-serializable#100369
// TODO Maybe its better to have one DatabaseOperationException and work around localized messages idk yet




//[Serializable]
//public class DatabaseOperationException : Exception
//{
//    /// <summary>
//    /// Initializes a new instance of the RecordNotFoundException class with a default error message.
//    /// </summary>
//    public DatabaseOperationException()
//        : base("There was an error while performing database related operation")
//    {
//    }

//    /// <summary>
//    /// Initializes a new instance of the RecordNotFoundException class with provided error message.
//    /// </summary>
//    public DatabaseOperationException(string message)
//            : base(message)
//    {
//    }

//    public DatabaseOperationException(string message, Exception innerException)
//        : base(message, innerException)
//    {
//    }

//    // Without this constructor, deserialization will fail
//    protected DatabaseOperationException(SerializationInfo info, StreamingContext context)
//        : base(info, context)
//    {
//    }
//}





[Serializable]
public class RecordNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the RecordNotFoundException class with a default error message.
    /// </summary>
    public RecordNotFoundException()
        : base("Record not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the RecordNotFoundException class with provided error message.
    /// </summary>
    public RecordNotFoundException(string message)
            : base(message)
    {
    }

    public RecordNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Without this constructor, deserialization will fail
    protected RecordNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}




[Serializable]
public class ActiveOrganisationMissingLocationException : Exception
{

    /// <summary>
    /// Initializes a new instance of the ActiveOrganisationMissingLocationException class with a default error message.
    /// </summary>
    public ActiveOrganisationMissingLocationException()
        : base("Active organisation must have at least one physical address")
    {
    }

    /// <summary>
    /// Initializes a new instance of the ActiveOrganisationMissingLocationException class with provided error message.
    /// </summary>
    public ActiveOrganisationMissingLocationException(string message)
            : base(message)
    {
    }

    public ActiveOrganisationMissingLocationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Without this constructor, deserialization will fail
    protected ActiveOrganisationMissingLocationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}




[Serializable]
public class InactiveOrganisationReadonlyException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ActiveOrganisationMissingLocationException class with a default error message.
    /// </summary>
    public InactiveOrganisationReadonlyException()
        : base("Inactive organisation can not be edited. Create new organisation instead.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the ActiveOrganisationMissingLocationException class with provided error message.
    /// </summary>
    public InactiveOrganisationReadonlyException(string message)
            : base(message)
    {
    }

    public InactiveOrganisationReadonlyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Without this constructor, deserialization will fail
    protected InactiveOrganisationReadonlyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }


}





[Serializable]
public class RecordAlreadyExistsException : Exception
{

    /// <summary>
    /// Initializes a new instance of the RecordAlreadyExistsException class with a default error message.
    /// </summary>
    public RecordAlreadyExistsException()
        : base("Record already exists.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the ActiveOrganisationMissingLocationException class with provided error message.
    /// </summary>
    public RecordAlreadyExistsException(string message)
            : base(message)
    {
    }

    public RecordAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Without this constructor, deserialization will fail
    protected RecordAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

}





[Serializable]
public class LinkedRecordOperationException : Exception
{

    /// <summary>
    /// Initializes a new instance of the LinkedRecordOperationException class with a default error message.
    /// </summary>
    public LinkedRecordOperationException()
        : base("Operation doesn't allow for this record because of its relations with other record(s)")
    {
    }

    /// <summary>
    /// Initializes a new instance of the LinkedRecordOperationException class with provided error message.
    /// </summary>
    public LinkedRecordOperationException(string message)
            : base(message)
    {
    }

    public LinkedRecordOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    // Without this constructor, deserialization will fail
    protected LinkedRecordOperationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

}