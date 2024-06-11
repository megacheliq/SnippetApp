"use client"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm, useFieldArray } from "react-hook-form"
import { z } from "zod"
import { Button } from "@/components/ui/button"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { Editor } from "@monaco-editor/react"
import { LoadingSpinner } from "@/components/ui/loading-spinner"
import { useTheme } from "@/components/theme-provider";
import { Textarea } from "@/components/ui/textarea"
import { Trash } from "lucide-react";
import { createSnippet } from "@/services/snippetService"
import { ICommandDto } from "@/abstract/snippetTypes"
import { useDialogContext } from "@/contexts/DialogContext"

interface AddFormProps {
    fetchSnippets: () => void;
}

const AddForm: React.FC<AddFormProps> = ({ fetchSnippets }) => {
    const { theme } = useTheme();
    const { formRef, setIsAddAlertDialogOpen } = useDialogContext();

    const formSchema = z.object({
        theme: z.string().min(2, 'Тема должна быть не меньше 2 символов').max(60, 'Тема должна быть не больше 60 символов'),
        level: z.number(),
        codeSnippet: z.string().min(2, 'Код должен быть не менее 2 символов'),
        mainQuestion: z.string().min(2, 'Основной вопрос должен быть не менее 2 символов'),
        solution: z.string().min(2, 'Решение должно быть не менее 2 символов'),
        additionalQuestions: z.array(z.object({
            value: z.string().min(2, 'Дополнительный вопрос должен быть не менее 2 символов')
        })).optional(),
    });

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            theme: '',
            level: 1,
            codeSnippet: '',
            mainQuestion: '',
            solution: '',
            additionalQuestions: [{ value: '' }],
        },
    });

    const { fields, append, remove } = useFieldArray({
        control: form.control,
        name: "additionalQuestions",
    });

    const onSubmit = async (values: z.infer<typeof formSchema>) => {
        const formattedValues = {
            ...values,
            additionalQuestions: values.additionalQuestions?.map(question => question.value),
        };
        const response = await createSnippet(formattedValues as ICommandDto);
        if (response) {
            setIsAddAlertDialogOpen(false);
            fetchSnippets();
        }
    };

    return (
        <>
            <Form {...form}>
                <form ref={formRef} onSubmit={form.handleSubmit(onSubmit)} className="space-y-8 pr-4 pl-1 flex gap-4">
                    <div className="w-1/2 mt-8">
                        <FormField
                            control={form.control}
                            name="theme"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Введите тему</FormLabel>
                                    <FormControl>
                                        <Input {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            control={form.control}
                            name="level"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Выберите уровень</FormLabel>
                                    <Select onValueChange={(value) => field.onChange(parseInt(value))} defaultValue={field.value.toString()}>
                                        <FormControl>
                                            <SelectTrigger>
                                                <SelectValue placeholder="Сложность" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            <SelectItem value="1">Junior</SelectItem>
                                            <SelectItem value="2">Middle</SelectItem>
                                            <SelectItem value="3">Senior</SelectItem>
                                        </SelectContent>
                                    </Select>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            control={form.control}
                            name="mainQuestion"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Введите вопрос</FormLabel>
                                    <FormControl>
                                        <Input {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            control={form.control}
                            name="solution"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Введите решение</FormLabel>
                                    <FormControl>
                                        <Textarea
                                            placeholder="Решение"

                                            {...field}
                                        />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <div className="space-y-2">
                            <FormLabel>Дополнительные вопросы</FormLabel>
                            {fields.map((field, index) => (
                                <div key={field.id} className="flex items-center space-x-2 w-full">
                                    <FormField
                                        control={form.control}
                                        name={`additionalQuestions.${index}.value`}
                                        render={({ field }) => (
                                            <FormItem className="flex-1">
                                                <FormControl>
                                                    <Input {...field} />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                    <Button type="button" variant="destructive" onClick={() => remove(index)} className="flex-none">
                                        <Trash className="w-4 h-4" />
                                    </Button>
                                </div>
                            ))}
                            <br />
                            <Button type="button" variant="outline" onClick={() => append({ value: '' })}>
                                Добавить вопрос
                            </Button>
                        </div>
                    </div>
                    <FormField
                        control={form.control}
                        name="codeSnippet"
                        render={({ field }) => (
                            <FormItem className="w-1/2">
                                <FormLabel>Введите код</FormLabel>
                                <FormControl>
                                    <Editor
                                        height='40vh'
                                        language='csharp'
                                        defaultValue=""
                                        theme={theme === 'dark' ? 'vs-dark' : 'vs-light'}
                                        value={field.value}
                                        onChange={(value) => field.onChange(value)}
                                        options={{
                                            wordWrap: 'on',
                                            minimap: {
                                                enabled: false
                                            },
                                            fontSize: 10,
                                        }}
                                        loading={<LoadingSpinner />}
                                    />
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

export default AddForm;