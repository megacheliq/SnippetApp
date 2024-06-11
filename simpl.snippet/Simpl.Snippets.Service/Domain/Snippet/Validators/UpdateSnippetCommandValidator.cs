using FluentValidation;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands;

namespace Simpl.Snippets.Service.Domain.Snippet.Validators
{
    public class UpdateSnippetCommandValidator : AbstractValidator<UpdateSnippetCommand>
    {
        public UpdateSnippetCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty()
                .WithMessage("Идентификатор не должен быть пустым");


            RuleFor(command => command.Dto)
                .NotNull()
                .SetValidator(new AddOrUpdateSnippetDtoValidator());
        }
    }
}
