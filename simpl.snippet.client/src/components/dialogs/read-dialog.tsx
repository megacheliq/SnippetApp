import React from 'react';
import {
    AlertDialog,
    AlertDialogContent,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogCancel
} from "@/components/ui/alert-dialog";
import { useDialogContext } from "@/contexts/DialogContext";
import { LoadingSpinner } from "@/components/ui/loading-spinner";
import { Snippet } from "@/models/Snippet";
import { ScrollArea } from '@/components/ui/scroll-area';
import Editor from '@monaco-editor/react';
import { useTheme } from "@/components/theme-provider";
import { IAllSnippetsResponse } from '@/abstract/snippetTypes';

interface ReadDialogProps {
    selectedSnippet: IAllSnippetsResponse | null;
    loadingSnippet: string | null;
    snippetDetails: { [key: string]: Snippet };
}

const ReadDialog: React.FC<ReadDialogProps> = ({ selectedSnippet, loadingSnippet, snippetDetails }) => {
    const { isReadAlertDialogOpen, setIsReadAlertDialogOpen } = useDialogContext();
    const { theme } = useTheme();

    return (
        <AlertDialog open={isReadAlertDialogOpen} onOpenChange={setIsReadAlertDialogOpen}>
            <AlertDialogContent className="max-w-[75%]">
                <AlertDialogHeader>
                    <AlertDialogTitle>Детали сниппета</AlertDialogTitle>
                    <AlertDialogDescription>
                        {selectedSnippet ? (
                            loadingSnippet === selectedSnippet.id ? (
                                <LoadingSpinner />
                            ) : (
                                <ScrollArea className="h-[400px]">
                                    {snippetDetails[selectedSnippet.id] && (
                                        <div className='flex gap-4'>
                                            <div className='w-1/2'>
                                                <p><span className="text-accent-foreground">Автор:</span> {snippetDetails[selectedSnippet.id].authorName}</p>
                                                <p><span className="text-accent-foreground">Направление:</span> {snippetDetails[selectedSnippet.id].getDirectionLabel()}</p>
                                                <p><span className="text-accent-foreground">Уровень:</span> {snippetDetails[selectedSnippet.id].getLevelLabel()}</p>
                                                <p><span className="text-accent-foreground">Главный вопрос:</span> {snippetDetails[selectedSnippet.id].mainQuestion}</p>
                                                <p><span className="text-accent-foreground">Ответ:</span> {snippetDetails[selectedSnippet.id].solution}</p>
                                                <div>
                                                    <p className="text-accent-foreground">Дополнительные вопросы:</p>
                                                    {snippetDetails[selectedSnippet.id].additionalQuestions.map((additional, index) => (
                                                        <p key={index}>{additional}</p>
                                                    ))}
                                                </div>

                                            </div>
                                            <div className='w-1/2'>
                                                <Editor
                                                    className='pr-4 mt-2'
                                                    height='38vh'
                                                    language='csharp'
                                                    defaultValue=""
                                                    theme={theme === 'dark' ? 'vs-dark' : 'vs-light'}
                                                    value={snippetDetails[selectedSnippet.id].codeSnippet}
                                                    options={{
                                                        wordWrap: 'on',
                                                        minimap: {
                                                            enabled: false
                                                        },
                                                        fontSize: 10,
                                                        readOnly: true
                                                    }}
                                                    loading={<LoadingSpinner />}
                                                />
                                            </div>

                                        </div>
                                    )}
                                </ScrollArea>
                            )
                        ) : <LoadingSpinner />}
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Закрыть</AlertDialogCancel>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
};

export default ReadDialog;
