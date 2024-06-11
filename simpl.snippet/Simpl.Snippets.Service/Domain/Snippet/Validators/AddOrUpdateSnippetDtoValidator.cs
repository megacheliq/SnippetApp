using FluentValidation;
using Simpl.Snippets.Service.Domain.Snippet.Models;


namespace Simpl.Snippets.Service.Domain.Snippet.Validators;
public class AddOrUpdateSnippetDtoValidator : AbstractValidator<AddOrUpdateSnippetDto>
{
    public AddOrUpdateSnippetDtoValidator()
    {
        RuleFor(snippet => snippet.Theme)
            .NotEmpty()
            .WithMessage("Тема сниппета должна быть указана");

        RuleFor(snippet => snippet.Level)
            .IsInEnum()
            .WithMessage("Недопустимое значение для уровня сниппета");

        RuleFor(snippet => snippet.CodeSnippet)
            .NotEmpty()
            .WithMessage("Код сниппета должен быть указан");

        RuleFor(snippet => snippet.MainQuestion)
            .NotEmpty()
            .WithMessage("Основной вопрос должен быть указан");
    }
}

