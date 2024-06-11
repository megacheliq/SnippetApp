import React from 'react';
import {
    AlertDialog,
    AlertDialogContent,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogCancel,
    AlertDialogAction
} from "@/components/ui/alert-dialog";
import { useDialogContext } from "@/contexts/DialogContext";
import { IAllSnippetsResponse } from '@/abstract/snippetTypes';
import { LoadingSpinner } from "@/components/ui/loading-spinner";

interface DeleteDialogProps {
    selectedSnippet: IAllSnippetsResponse | null;
    deleteSnippet: (id: string) => void;
}

const DeleteDialog: React.FC<DeleteDialogProps> = ({ selectedSnippet, deleteSnippet }) => {
    const { isDeleteAlertDialogOpen, setIsDeleteAlertDialogOpen } = useDialogContext();

    return (
        <AlertDialog open={isDeleteAlertDialogOpen} onOpenChange={setIsDeleteAlertDialogOpen}>
            <AlertDialogContent>
                {selectedSnippet ? (
                    <div>
                        <AlertDialogHeader>
                            <AlertDialogTitle>Вы действительно хотите удалить сниппет?</AlertDialogTitle>
                            <AlertDialogDescription>
                                Удаленный сниппет нельзя будет вернуть
                            </AlertDialogDescription>
                        </AlertDialogHeader>
                        <AlertDialogFooter className='mt-8'>
                            <AlertDialogCancel>Отменить</AlertDialogCancel>
                            <AlertDialogAction onClick={() => deleteSnippet(selectedSnippet.id)}>Удалить</AlertDialogAction>
                        </AlertDialogFooter>
                    </div>
                ) : <LoadingSpinner />}
            </AlertDialogContent>
        </AlertDialog>
    );
};

export default DeleteDialog;
