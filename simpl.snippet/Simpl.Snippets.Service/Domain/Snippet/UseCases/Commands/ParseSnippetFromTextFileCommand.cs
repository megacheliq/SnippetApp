using MediatR;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Snippet.Models;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.Domain.Snippet.UseCases.Commands
{
    public class ParseSnippetFromTextFileCommand : IRequest<AddOrUpdateSnippetDto>
    {
        /// <summary>
        /// Текстовый файл
        /// </summary>
        public Stream TextFile { get; set; }
    }

    public class ParseSnippetFromTextFileCommandHandler : IRequestHandler<ParseSnippetFromTextFileCommand, AddOrUpdateSnippetDto>
    {
        private const string ThemeKey = "// Тема:";
        private const string LevelKey = "// Уровень:";
        private const string MainQuestionKey = "// Основной вопрос:";
        private const string SolutionKey = "// Ответ:";
        private const string AdditionalQuestionsKey = "// Дополнительные вопросы:";

        public async Task<AddOrUpdateSnippetDto> Handle(ParseSnippetFromTextFileCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.TextFile == null)
            {
                throw new BusinessLogicException("Не передан файл");
            }

            request.TextFile.Position = 0;
            using var reader = new StreamReader(request.TextFile, leaveOpen: true);
            var lines = (await reader.ReadToEndAsync()).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return new AddOrUpdateSnippetDto
            {
                Theme = GetValue(lines, ThemeKey),
                Level = ParseLevel(GetValue(lines, LevelKey)),
                MainQuestion = GetValue(lines, MainQuestionKey),
                Solution = ExtractSolution(lines),
                AdditionalQuestions = ExtractAdditionalQuestions(lines),
                CodeSnippet = ExtractCodeSnippet(lines)
            };
        }

        private static string GetValue(string[] lines, string key)
        {
            return lines.FirstOrDefault(line => line.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                        ?.Substring(key.Length).Trim() ?? string.Empty;
        }

        private static Level ParseLevel(string levelStr)
        {
            return Enum.TryParse(levelStr, true, out Level level) ? level : Level.Junior;
        }

        private static string ExtractSolution(string[] lines)
        {
            return ExtractSection(lines, SolutionKey, AdditionalQuestionsKey)
                    .Select(line => line.StartsWith("//") ? line.Substring(2).Trim() : line)
                    .Aggregate((current, next) => $"{current} {next}");
        }

        private static List<string> ExtractAdditionalQuestions(string[] lines)
        {
            return ExtractSection(lines, AdditionalQuestionsKey, null)
                    .Where(line => line.StartsWith("//"))
                    .Select(line => line.Trim('/', '#', ' ', '\r', '\n'))
                    .ToList();
        }

        private static string ExtractCodeSnippet(string[] lines)
        {
            var codeSnippet = new List<string>();
            bool codeStarted = false;

            foreach (var line in lines)
            {
                if (!line.StartsWith("//") && !codeStarted)
                {
                    codeStarted = true;
                }

                if (codeStarted)
                {
                    codeSnippet.Add(line);
                }
            }

            return string.Join("\n", codeSnippet).Trim();
        }

        private static IEnumerable<string> ExtractSection(string[] lines, string startKey, string endKey)
        {
            bool sectionStarted = false;

            foreach (var line in lines)
            {
                if (line.StartsWith(startKey, StringComparison.OrdinalIgnoreCase))
                {
                    sectionStarted = true;
                    yield return line.Substring(startKey.Length).Trim();
                }
                else if (sectionStarted)
                {
                    if (endKey != null && line.StartsWith(endKey, StringComparison.OrdinalIgnoreCase))
                    {
                        yield break;
                    }
                    yield return line.Trim();
                }
            }
        }
    }
}
