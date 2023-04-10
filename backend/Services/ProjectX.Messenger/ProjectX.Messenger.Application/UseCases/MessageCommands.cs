using FluentValidation;
using FluentValidation.Results;
using ProjectX.Core;
using ProjectX.Core.Validation;

namespace ProjectX.Messenger.Application.UseCases;

public class SendMessageCommand : ICommand, IValidatable
{
    /// <summary>
    /// Receiver's id.
    /// </summary>
    public int UserId { get; set; }

    public string Content { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.UserId).GreaterThan(0);
            command.RuleFor(x => x.Content).NotEmpty();
        });
    }
}

public class UpdateMessageCommand : ICommand, IValidatable
{
    public string ConversationId { get; set; }
    
    public Guid MessageId { get; set; }
    
    public string Content { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.ConversationId).NotEmpty();
            command.RuleFor(x => x.MessageId).NotEmpty();
            command.RuleFor(x => x.Content).NotEmpty();
        });
    }
}

public class DeleteMessageCommand : ICommand, IValidatable
{
    public string ConversationId { get; set; }

    public Guid MessageId { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.ConversationId).NotEmpty();
            command.RuleFor(x => x.MessageId).NotEmpty();
        });
    }
}