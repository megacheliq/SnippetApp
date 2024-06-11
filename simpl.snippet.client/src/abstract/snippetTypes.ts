export interface IAllSnippetsResponse {
    id: string;
    authorId: string;
    authorName: string;
    theme: string;
    createdDate: string;
    modifiedDate: string;
}

export enum Level {
    Junior = 1,
    Middle = 2,
    Senior = 3
}

export enum Direction {
    Backend = 1,
}

export interface ISnippet {
    id: string;
    theme: string;
    authorId: string;
    authorName: string;
    direction: Direction;
    level: Level;
    codeSnippet: string;
    mainQuestion: string;
    solution: string;
    additionalQuestions: string[];
}

export interface ICommandDto {
    theme: string,
    level: number,
    codeSnippet: string,
    mainQuestion: string,
    solution: string,
    additionalQuestions: string[]
}