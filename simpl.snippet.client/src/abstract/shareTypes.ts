export interface MessageObject {
    username: string;
    color: string;
    selection: Selection;
}

export interface Selection {
    start: Position;
    end: Position;
}

export interface Position {
    lineNumber: number;
    column: number;
}