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
import { Snippet } from "@/models/Snippet";
import { ScrollArea } from '@/components/ui/scroll-area';
import EditForm from '@/components/forms/edit-form';
import { LoadingSpinner } from "@/components/ui/loading-spinner";
import { Button } from '@/components/ui/button';
import { IAllSnippetsResponse } from '@/abstract/snippetTypes';

interface EditDialogProps {
    selectedSnippet: IAllSnippetsResponse | null;
    loadingSnippet: string | null;
    snippetDetails: { [key: string]: Snippet };
    fetchSnippets: () => void;
}

const EditDialog: React.FC<EditDialogProps> = ({ selectedSnippet, loadingSnippet, snippetDetails, fetchSnippets }) => {
    const { isEditAlertDialogOpen, setIsEditAlertDialogOpen, triggerSubmit } = useDialogContext();

    return (
        <AlertDialog open={isEditAlertDialogOpen} onOpenChange={setIsEditAlertDialogOpen}>
            <AlertDialogContent className="max-w-[80%]">
                <AlertDialogHeader>
                    <AlertDialogTitle>Редактирование сниппета</AlertDialogTitle>
                    <AlertDialogDescription>
                        {selectedSnippet ? (
                            loadingSnippet === selectedSnippet.id ? (
                                <LoadingSpinner />
                            ) : (
                                <ScrollArea className="h-[550px]">
                                    <EditForm fetchSnippets={fetchSnippets} snippet={snippetDetails[selectedSnippet.id]} />
                                </ScrollArea>
                            )
                        ) : <LoadingSpinner />}
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Закрыть</AlertDialogCancel>
                    <Button onClick={triggerSubmit}>Обновить</Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
};

export default EditDialog;
