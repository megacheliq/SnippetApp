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
import { ScrollArea } from '@/components/ui/scroll-area';
import AddForm from '@/components/forms/add-form';
import { Button } from '@/components/ui/button';

interface AddDialogProps {
    fetchSnippets: () => void;
}

const AddDialog: React.FC<AddDialogProps> = ({ fetchSnippets }) => {
    const { isAddAlertDialogOpen, setIsAddAlertDialogOpen, triggerSubmit } = useDialogContext();

    return (
        <AlertDialog open={isAddAlertDialogOpen} onOpenChange={setIsAddAlertDialogOpen}>
            <AlertDialogContent className="max-w-[80%]">
                <AlertDialogHeader>
                    <AlertDialogTitle>Добавление сниппета</AlertDialogTitle>
                    <AlertDialogDescription>
                        <ScrollArea className="h-[550px]">
                            <AddForm fetchSnippets={fetchSnippets} />
                        </ScrollArea>
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Закрыть</AlertDialogCancel>
                    <Button onClick={triggerSubmit}>Добавить</Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
};

export default AddDialog;
