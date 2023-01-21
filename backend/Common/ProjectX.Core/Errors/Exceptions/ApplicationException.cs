using System.Runtime.Serialization;

namespace ProjectX.Core.Errors.Exceptions;

[Serializable]
/// <summary>
/// Important: This attribute is NOT inherited from Exception, and MUST be specified 
/// otherwise serialization will fail with a SerializationException stating that
/// "Type X in Assembly Y is not marked as serializable."
/// Instance fields that are declared in a type that inherits 
/// the<see cref="System.Runtime.Serialization.ISerializable"/> interface are not automatically included in the serialization process.
/// To include the fields, the type must implement the GetObjectData method and the serialization constructor.
/// If the fields should not be serialized, apply the NonSerializedAttribute attribute
/// to the fields to explicitly indicate the decision.
/// ISerializable doc: <see cref="https://learn.microsoft.com/en-us/visualstudio/code-quality/ca2240?view=vs-2022&tabs=csharp"/>
/// </summary>
public class ApplicationException : Exception, ISerializable
{
    public ApplicationError Error { get; }

    public ApplicationException(ApplicationError error)
        : base(error.Message)
    {
        Error = error;
    }

    public ApplicationException(ErrorType type, ErrorCode code, string? message = null)
        : this(new ApplicationError(type, code, message))
    {
    }

    #pragma warning disable CS8618
    public ApplicationException(SerializationInfo info, StreamingContext context)
    #pragma warning restore CS8618
        : base(info, context)
    {
        Error = (ApplicationError)info.GetValue(nameof(Error), typeof(ApplicationError));
    }

    /// <summary>
    /// In types that are not sealed, implementations of the GetObjectData method should be externally visible. 
    /// Therefore, the method can be called by derived types, and is overridable.
    /// </summary>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.ThrowIfNull();

        // Note: if "ApplicationError" isn't serializable you may need to work out another
        // method of adding your list, this is just for show...
        info.AddValue(nameof(Error), Error, typeof(ApplicationError));

        // MUST call through to the base class to let it save its own state
        base.GetObjectData(info, context);
    }
}



public class InvalidPermissionException : ApplicationException
{
    public InvalidPermissionException()
        : this(ErrorCode.InvalidPermission)
    {
    }
    
    public InvalidPermissionException(ApplicationError error)
        : base(error)
    {
    }
    
    public InvalidPermissionException(ErrorCode code, string? message = null)
        : base(ErrorType.InvalidPermission, code, message)
    {
    }
}

public class NotFoundException : ApplicationException
{
    public NotFoundException() 
        : this(ErrorCode.NotFound) 
    {
    }

    public NotFoundException(ApplicationError error) 
        : base(error)
    {
    }
    
    public NotFoundException(ErrorCode code, string? message = null)
        : base(ErrorType.NotFound, code, message)
    {
    }
}