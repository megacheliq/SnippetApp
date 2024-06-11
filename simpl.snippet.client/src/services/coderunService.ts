import axiosClient from '@/axios-client';
import { toast } from "sonner"

export const runCode = async (code: string, language: number) => {
    try {
        const response = await axiosClient.post('CodeRunner/RunCode', {
            language: language,
            snippetCode: code
        });
        return response.data;
    } catch (error: any) {
        toast.error('Не удалось выполнить код');
        console.error('Failed to execute code:', error);

        if (error.response && error.response.data && error.response.data.message) {
            return { output: error.response.data.message };
        } else {
            return { output: 'Не удалось выполнить код' };
        }
    }
};