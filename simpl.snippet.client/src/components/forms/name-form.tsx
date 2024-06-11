"use client"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
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
import { registerUserOnSession } from '@/services/shareService';
import { useState, useEffect } from "react"
import {
    Alert,
    AlertDescription,
    AlertTitle,
} from "@/components/ui/alert"
import { ExclamationTriangleIcon } from "@radix-ui/react-icons"
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { useStateContext } from '@/contexts/ContextProvider'
import { useSnippetContext } from "@/contexts/SnippetContext"

interface NameFormProps {
    setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
    setSubmitted: React.Dispatch<React.SetStateAction<boolean>>;
}

const NameForm: React.FC<NameFormProps> = ({ setIsOpen, setSubmitted }) => {
    const [sessionId, setSessionId] = useState('');
    const [errors, setErrors] = useState(false);
    const { user } = useStateContext();
    const { setExpirationDate } = useSnippetContext();

    const parseTTL = (ttlString: string): Date => {
        const [hours, minutes, seconds] = ttlString.split(':');
        const wholeSeconds = parseInt(seconds.split('.')[0], 10);
    
        const ttlMilliseconds = (parseInt(hours, 10) * 60 * 60 * 1000) +
            (parseInt(minutes, 10) * 60 * 1000) +
            (wholeSeconds * 1000);
    
        const expirationDate = new Date(Date.now() + ttlMilliseconds);
        return expirationDate;
    }

    useEffect(() => {
        setSessionId(localStorage.getItem('sessionId') || '');
    }, []);

    const formSchema = z.object({
        username: z.string().min(2, 'Имя должно быть не меньше 2 символов')
            .max(20, 'Имя должно быть не больше 20 символов'),
        color: z.string().min(2, 'Выберите цвет')
    })

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            username: user?.email || '',
            color: ''
        },
    })

    const onSubmit = async (values: z.infer<typeof formSchema>) => {
        const response = await registerUserOnSession(sessionId, values.username, values.color);

        if (response.registratedUser) {
            const ttlString = response.ttl;
            const expirationDate = parseTTL(ttlString);
            setExpirationDate(expirationDate);

            document.cookie = `registrated_on_${sessionId}=true;expires=${expirationDate.toUTCString()};path=/`;
            document.cookie = `${sessionId}=${sessionId}_${values.username};expires=${expirationDate.toUTCString()};path=/`;
            setSubmitted(true);
            setIsOpen(false);
        } else {
            setErrors(true);
        }
    }

    return (
        <>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
                    <FormField
                        control={form.control}
                        name="username"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel className="text-lg">Введите своё имя</FormLabel>
                                <FormControl>
                                    <Input {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="color"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel className="text-lg">Выберите цвет</FormLabel>
                                <Select onValueChange={field.onChange} defaultValue={field.value}>
                                    <FormControl>
                                        <SelectTrigger>
                                            <SelectValue placeholder="Цвет" />
                                        </SelectTrigger>
                                    </FormControl>
                                    <SelectContent>
                                        <SelectItem value="#0000ff">Синий</SelectItem>
                                        <SelectItem value="#ffa500">Оранжевый</SelectItem>
                                        <SelectItem value="#ff0000">Красный</SelectItem>
                                        <SelectItem value="#008000">Зелёный</SelectItem>
                                        <SelectItem value="#a52a2a">Коричневый</SelectItem>
                                    </SelectContent>
                                </Select>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    {errors &&
                        <Alert variant="destructive">
                            <ExclamationTriangleIcon className="h-4 w-4" />
                            <AlertTitle>Ошибка</AlertTitle>
                            <AlertDescription>
                                Такой пользователь уже есть на сессии
                            </AlertDescription>
                        </Alert>
                    }
                    <Button className="w-full" type="submit">Войти</Button>
                </form>
            </Form>
        </>
    );
}

export default NameForm;