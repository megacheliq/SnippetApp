"use client"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { z } from "zod"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { ICommandDto } from "@/abstract/snippetTypes"
import { createSnippet, parseFile } from "@/services/snippetService"
import { useDialogContext } from "@/contexts/DialogContext"

interface AddFileFormProps {
    fetchSnippets: () => void;
}

const AddFileForm: React.FC<AddFileFormProps> = ({ fetchSnippets }) => {
    const { formRef, setIsAddFileAlertDialogOpen } = useDialogContext();

    const formSchema = z.object({
        file: z
            .instanceof(FileList)
            .refine((files) => files.length === 1, 'Вы должны загрузить один файл'),
    });

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            file: undefined
        },
    });

    const onSubmit = async (values: z.infer<typeof formSchema>) => {
        const response = await parseFile(values.file[0]);
        if (response?.status === 200) {
            await createSnippet(response.data as ICommandDto);
            fetchSnippets();
            setIsAddFileAlertDialogOpen(false);
        }
        
    };

    return (
        <>
            <Form {...form}>
                <form ref={formRef} onSubmit={form.handleSubmit(onSubmit)} >
                    <FormField
                        control={form.control}
                        name="file"
                        render={({ field }) => (
                            <FormItem>
                                <FormControl>
                                    <Input type="file" onChange={(e) => field.onChange(e.target.files)} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                </form>
            </Form>
        </>
    );
}

export default AddFileForm;