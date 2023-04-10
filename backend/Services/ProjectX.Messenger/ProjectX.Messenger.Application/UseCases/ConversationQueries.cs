using FluentValidation;
using FluentValidation.Results;
using ProjectX.Core;
using ProjectX.Core.Validation;
using ProjectX.Messenger.Domain;

namespace ProjectX.Messenger.Application.UseCases;

public sealed class ConversationViewQuery : IQuery<ConversationView>, IValidatable
{
    /// <summary>
    /// Id of conversation's member.
    /// </summary>
    public int UserId { get; set; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.UserId).GreaterThan(0);
        });
    }
}

public sealed class UserConversationsQuery : IQuery<IEnumerable<UserConversationsView.Conversation>>
{
}