import * as monaco from 'monaco-editor';
import { csharp } from '@/keywords';

export function loadCSharp() {
    monaco.languages.setMonarchTokensProvider('csharp', {
        csharp,
        tokenizer: {
            root: [
                [/@?[a-zA-Z][\w$]*/, {
                    cases: {
                        '@csharp': 'keyword',
                        '@default': 'variable',
                    }
                }],
                [/[{}()\[\]]/, '@brackets'],
                [/[<>](?!@symbols)/, '@brackets'],
                [/@symbols/, 'operator'],
                [/[0-9_]*\.[0-9_]+([eE][\-+]?\d+)?[fFdD]?/, 'number.float'],
                [/0[xX][0-9a-fA-F_]+/, 'number.hex'],
                [/0[bB][01_]+/, 'number.hex'],
                [/[0-9_]+/, 'number'],
                [/[;,.]/, 'delimiter'],
                [/\/\*/, 'comment', '@comment'],
                [/\/\/.*$/, 'comment']
            ],
            comment: [
                [/[^/*]+/, 'comment'],
                [/\/\*/, 'comment', '@push'],
                ['\\*/', 'comment', '@pop'],
                [/[/]/, 'comment']
            ],
        },
        symbols: /[=><!~?:&|+\-*\/\^%]+/,
        operators: ['=', '>', '<', '!', '~', '?', ':', '&', '|', '+', '-', '*', '/', '^', '%']
    });

    monaco.languages.registerCompletionItemProvider('csharp', {
        provideCompletionItems: (model, position) => {
            const wordUntil = model.getWordUntilPosition(position);
            const range = {
                startLineNumber: position.lineNumber,
                endLineNumber: position.lineNumber,
                startColumn: wordUntil.startColumn,
                endColumn: wordUntil.endColumn
            };

            const suggestions = csharp.map(k => ({
                label: k,
                kind: monaco.languages.CompletionItemKind.Keyword,
                insertText: k,
                range: range
            }));

            return { suggestions: suggestions };
        }
    });
}