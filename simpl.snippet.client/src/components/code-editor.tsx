import React, { useEffect, useState } from 'react';
import Editor, { OnChange } from '@monaco-editor/react';
import { LoadingSpinner } from '@/components/ui/loading-spinner';
import * as monaco from 'monaco-editor';
import { useTheme } from "@/components/theme-provider"
import { getMessage } from '@/services/shareService';
import NameDialog from './name-dialog';
import { MessageObject } from '@/abstract/shareTypes';
import * as MonacoCollabExt from "@convergencelabs/monaco-collab-ext";
import { RemoteSelection } from '@convergencelabs/monaco-collab-ext/typings/RemoteSelection';
import { RemoteCursor } from '@convergencelabs/monaco-collab-ext/typings/RemoteCursor';
import { useSnippetContext } from '@/contexts/SnippetContext';

interface CodeEditorProps {
    sessionId: string,
    messageObject: MessageObject;
    codeValue: string;
    language: string;
    sendMessageToGroup: (message: string) => void;
    sendSelectionToGroup: (username: string, messageObject: MessageObject) => void;
}

const CodeEditor: React.FC<CodeEditorProps> = ({ codeValue, messageObject, language, sendMessageToGroup, sendSelectionToGroup, sessionId }) => {
    const { theme } = useTheme();
    const [submitted, setSubmitted] = useState(false);
    const [editor, setEditor] = useState<monaco.editor.IStandaloneCodeEditor | null>(null);
    const [selections, setSelections] = useState<{ [key: string]: RemoteSelection }>({});
    const [cursors, setCursors] = useState<{ [key: string]: RemoteCursor }>({});
    const { mainSnippet } = useSnippetContext();

    const getCookie = (name: string): string | null => {
        return document.cookie.split('; ').reduce((acc, cookie) => {
            const [key, value] = cookie.split('=');
            return key === name ? decodeURIComponent(value) : acc;
        }, null as string | null);
    };

    const getUsernameFromCookie = (sessionId: string): string | null => {
        const cookieValue = getCookie(sessionId);
        if (cookieValue) {
            const parts = cookieValue.split('_');
            return parts[1] || null;
        }
        return null;
    };

    const handleChange: OnChange = (value) => {
        sendMessageToGroup(value || "");       
    };

    const handleOnMount = async (editor: monaco.editor.IStandaloneCodeEditor) => {
        const initialMessageString = await getMessage(getCookie(sessionId) || "");
        const initialMessageJson = JSON.parse(initialMessageString);
        const initialMessage: MessageObject = {
            username: initialMessageJson.Username,
            color: initialMessageJson.Color,
            selection: {
                start: {
                    lineNumber: initialMessageJson.Selection.Start.LineNumber,
                    column: initialMessageJson.Selection.Start.Column,
                },
                end: {
                    lineNumber: initialMessageJson.Selection.End.LineNumber,
                    column: initialMessageJson.Selection.End.Column,
                },
            },
        };

        setEditor(editor);
        const editorElement = editor.getDomNode();

        if (editorElement) {
            editor.onDidChangeCursorSelection(_ => {
                const selection = editor.getSelection();
                if (selection) {
                    const newMessage: MessageObject = {
                        ...initialMessage,
                        selection: {
                            start: {
                                lineNumber: selection.getStartPosition().lineNumber,
                                column: selection.getStartPosition().column,
                            },
                            end: {
                                lineNumber: selection.getEndPosition().lineNumber,
                                column: selection.getEndPosition().column,
                            },
                        }
                    };

                    sendSelectionToGroup(initialMessage.username, newMessage)
                }
            });
        }
    };

    useEffect(() => {
        if (mainSnippet) {
            sendMessageToGroup(mainSnippet.codeSnippet);
        }
    }, [mainSnippet])

    useEffect(() => {
        if (editor && messageObject) {
            const remoteSelectionManager = new MonacoCollabExt.RemoteSelectionManager({ editor: editor });
            const remoteCursorManager = new MonacoCollabExt.RemoteCursorManager({
                editor: editor,
                showTooltipOnHover: true,
                className: ""
            });

            const select = selections[messageObject.username];
            const curs = cursors[messageObject.username];

            if (select && curs) {
                select.dispose();
                curs.dispose();
            }

            const selection = remoteSelectionManager.addSelection(messageObject.username, messageObject.color);
            const cursor = remoteCursorManager.addCursor(messageObject.username, messageObject.color, messageObject.username);

            const start: monaco.IPosition = { lineNumber: messageObject.selection.start.lineNumber, column: messageObject.selection.start.column };
            const end: monaco.IPosition = { lineNumber: messageObject.selection.end.lineNumber, column: messageObject.selection.end.column };

            const cookieUsername = getUsernameFromCookie(sessionId);

            if (messageObject.username != cookieUsername) {
                selection.setPositions(start, end);
                cursor.setPosition(end);
            }

            setSelections({
                ...selections,
                [messageObject.username]: selection
            });

            setCursors({
                ...cursors,
                [messageObject.username]: cursor
            })
        }

    }, [messageObject]);

    return (
        <>
            {
                submitted ? (
                    <Editor
                        language={language}
                        defaultValue=""
                        theme={theme === 'dark' ? 'vs-dark' : 'vs-light'}
                        value={codeValue}
                        options={{
                            wordWrap: 'on',
                            minimap: {
                                enabled: false
                            },
                        }}
                        loading={<LoadingSpinner />}
                        onChange={handleChange}
                        onMount={handleOnMount}
                    />
                ) : (
                    <NameDialog setSubmitted={setSubmitted} />
                )
            }
        </>
    );
};

export default CodeEditor;