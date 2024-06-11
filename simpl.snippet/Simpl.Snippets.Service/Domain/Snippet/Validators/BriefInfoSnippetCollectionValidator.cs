using FluentValidation;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries;

namespace Simpl.Snippets.Service.Domain.Snippet.Validators
{
    public class BriefInfoSnippetCollectionValidator : AbstractValidator<BriefInfoSnippetsQuery>
    {
        public BriefInfoSnippetCollectionValidator()
        {
            RuleFor(x => x.Direction)
                .IsInEnum()
                .WithMessage("Недопустимое значение для направления сниппета");

            RuleFor(x => x.Level)
                .IsInEnum()
                .When(x => x.Level.HasValue)
                .WithMessage("Недопустимое значение для уровня сниппета");

            RuleFor(x => x.CreatedDateStart)
                .LessThanOrEqualTo(x => x.CreatedDateEnd.Value)
                .When(x => x.CreatedDateStart.HasValue && x.CreatedDateEnd.HasValue)
                .WithMessage("Начальная дата создания сниппета должна быть меньше или равна конечной дате создания");

            RuleFor(x => x.ModifiedDateStart)
                .LessThanOrEqualTo(x => x.ModifiedDateEnd.Value)
                .When(x => x.ModifiedDateStart.HasValue && x.ModifiedDateEnd.HasValue)
                .WithMessage("Начальная дата изменения сниппета должна быть меньше или равна конечной дате изменения");
        }
    }
}
