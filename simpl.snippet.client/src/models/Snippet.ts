import { Direction, ISnippet, Level } from "@/abstract/snippetTypes";

export class Snippet implements ISnippet {
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

    constructor(
        id: string,
        theme: string,
        authorId: string,
        authorName: string,
        direction: Direction,
        level: Level,
        codeSnippet: string,
        mainQuestion: string,
        solution: string,
        additionalQuestions: string[]
    ) {
        this.id = id;
        this.theme = theme;
        this.authorId = authorId;
        this.authorName = authorName;
        this.direction = direction;
        this.level = level;
        this.codeSnippet = codeSnippet;
        this.mainQuestion = mainQuestion;
        this.solution = solution;
        this.additionalQuestions = additionalQuestions;
    }

    getLevelLabel(): string {
        switch (this.level) {
            case Level.Junior:
                return "Junior";
            case Level.Middle:
                return "Middle";
            case Level.Senior:
                return "Senior";
            default:
                return "Unknown";
        }
    }

    getDirectionLabel(): string {
        switch (this.direction) {
            case Direction.Backend:
                return "Backend";
            default:
                return "Unknown";
        }
    }
}