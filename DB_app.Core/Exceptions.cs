namespace DB_app.Repository;

public class SaveDublicateRecordException : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public SaveDublicateRecordException() : base("Record already exists.") { }
}

public class RecordNotFound : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public RecordNotFound() : base("Record not found.") { }
}

public class RecordLinkedWithOrder : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public RecordLinkedWithOrder() : base("Record linked with order and can't be deleted until any links with orders exist") { }
}

public class ActiveOrganisationMissingLocation : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public ActiveOrganisationMissingLocation() : base("Active organisation must have at least one physical address") { }
}

public class InactiveOrganisationReadonly : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public InactiveOrganisationReadonly() : base("Inactive organisation can not be edited. Create new organisation instead.") { }
}

public class RecordAlreadyExists : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public RecordAlreadyExists() : base("Record already exists.") { }
}

public class LinkedMedicineTypeChange : Exception
{
    /// <summary>
    /// Initializes a new instance of the OrderSavingException class with a default error message.
    /// </summary>
    public LinkedMedicineTypeChange() : base("Linked medicine type can not be changed. Create another medicine with specified type instead") { }
}