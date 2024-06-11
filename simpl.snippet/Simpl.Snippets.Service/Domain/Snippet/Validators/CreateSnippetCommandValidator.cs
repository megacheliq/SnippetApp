using FluentValidation;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands;

namespace Simpl.Snippets.Service.Domain.Snippet.Validators;

public class CreateSnippetCommandValidator : AbstractValidator<CreateSnippetCommand>
{
    public CreateSnippetCommandValidator()
    {
        RuleFor(command => command.Direction)
            .IsInEnum()
            .WithMessage("Недопустимое значение для направления сниппета");

        RuleFor(command => command.CommandDto)
            .NotNull()
            .SetValidator(new AddOrUpdateSnippetDtoValidator());
    }
}
