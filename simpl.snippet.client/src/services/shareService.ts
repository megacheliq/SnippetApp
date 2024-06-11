import axiosClient from '@/axios-client';
import { toast } from "sonner"

export const createSession = async () => {
    try {
        const response = await axiosClient.post('CodeShare/CreateSession');
        return response.data;
    } catch (error) {
        console.error('Failed to create session:', error);
        toast.error('Не удалось создать сессию')
    }
};

export const checkMessage = async (key: string) => {
    try {
        const response = await axiosClient.get(`CodeShare/IsMessageExists?key=${key}`);
        return response.data.sessionExist;
    } catch (error) {
        console.error('Failed to check message:', error);
        toast.error('Не удалось проверить сообщение')
    }
};

export const getMessage = async (key: string) => {
    try {
        const response = await axiosClient.get(`CodeShare/GetMessage?key=${key}`);
        return response.data.message;
    } catch (error) {
        console.error('Failed to get message:', error);
        toast.error('Не удалось получить сообщение')
    }
}

export const registerUserOnSession = async (sessionId: string, username: string, color: string) => {
    try {
        const response = await axiosClient.post('CodeShare/RegisterUserOnSession', null, {
            params: {
                sessionId: sessionId,
                user: username,
                color: color
            }
        });
        return response.data;
    } catch (error) {
        console.error('Failed to register user:', error);
        toast.error('Не удалось создать пользователя в сессии')
    }
}

export const deleteMessage = async (key: string) => {
    try {
        await axiosClient.post('CodeShare/DeleteMessage', null, {
            params: {
                key: key
            }
        });
    } catch (error) {
        console.error('Failed to delete message:', error);
        toast.error('Не удалось удалить сообщение')
    }
}