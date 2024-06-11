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
import { login } from "@/services/authService"
import { useStateContext } from '@/contexts/ContextProvider'

const LoginForm: React.FC = () => {
    const { setKcResponse} = useStateContext()

    const formSchema = z.object({
        username: z.string().min(2, 'Имя должно быть не меньше 2 символов')
            .max(20, 'Имя должно быть не больше 20 символов'),
        password: z.string().min(2, 'Пароль должен быть не менее 2 символов')
    });

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            username: '',
            password: ''
        },
    });

    const onSubmit = async (values: z.infer<typeof formSchema>) => {
        const response = await login(values.username, values.password);
        setKcResponse(response);
    };

    return (
        <>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
                    <FormField
                        control={form.control}
                        name="username"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Введите имя пользователя</FormLabel>
                                <FormControl>
                                    <Input {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="password"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Введите пароль</FormLabel>
                                <FormControl>
                                    <Input {...field} type="password"/>
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />                   
                    <Button className="w-full" type="submit">Войти</Button>
                </form>
            </Form>
        </>
    );
}

export default LoginForm;