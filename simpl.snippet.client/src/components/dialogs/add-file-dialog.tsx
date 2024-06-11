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
import AddFileForm from '@/components/forms/addFile-form';
import { Button } from '@/components/ui/button';

interface AddFileDialogProps {
    fetchSnippets: () => void;
    downloadTemplate: () => void;
}

const AddFileDialog: React.FC<AddFileDialogProps> = ({ fetchSnippets, downloadTemplate }) => {
    const { isAddFileAlertDialogOpen, setIsAddFileAlertDialogOpen, triggerSubmit } = useDialogContext();

    return (
        <AlertDialog open={isAddFileAlertDialogOpen} onOpenChange={setIsAddFileAlertDialogOpen}>
            <AlertDialogContent >
                <AlertDialogHeader>
                    <AlertDialogTitle>Добавление сниппета из файла</AlertDialogTitle>
                    <AlertDialogDescription>
                        <div className='flex flex-col gap-6'>
                            <p>Для добавления файла, необходимо чтобы он соотвествовал шаблону</p>
                            <AddFileForm fetchSnippets={fetchSnippets} />
                        </div>
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <Button variant={'outline'} onClick={downloadTemplate}>Скачать шаблон</Button>
                    <AlertDialogCancel>Закрыть</AlertDialogCancel>
                    <Button onClick={triggerSubmit}>Загрузить</Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
};

export default AddFileDialog;
