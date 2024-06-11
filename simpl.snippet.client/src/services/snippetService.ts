import { ICommandDto } from '@/abstract/snippetTypes';
import axiosClient from '@/axios-client';
import { toast } from "sonner"

export const getAllSnippets = async () => {
    try {
        const response = await axiosClient.get('Snippet/GetAll?Direction=Backend');
        return response.data;
    } catch (error) {
        console.error('Failed to get snippets:', error);
        toast.error('Не удалось получить сниппеты');
    }
};

export const getSnippetById = async (id: string) => {
    try {
        const response = await axiosClient.get(`Snippet/Get/${id}`);
        return response.data;
    } catch (error) {
        console.error('Failed to get snippet:', error);
        toast.error('Не удалось получить сниппет');
    }
};

export const getRandomSnippet = async (level: string) => {
    try {
        const response = await axiosClient.get(`Snippet/GetRandom?Direction=Backend&Level=${level}`);
        return response.data;
    } catch (error) {
        console.error('Failed to get snippet:', error);
        toast.error('Не удалось получить сниппет');
    }
};

export const deleteSnippetById = async (id: string) => {
    try {
        const response = await axiosClient.delete(`Snippet/Delete/${id}`);
        toast.success('Сниппет успешно удален');
        return response.data;
    } catch (error) {
        console.error('Failed to delete snippet:', error);
        toast.error('Не удалось удалить сниппет');
    }
};

export const createSnippet = async (commandDto: ICommandDto) => {
    try {
        await axiosClient.post('Snippet/Create', {
            direction: 1,
            commandDto: commandDto
        });
        toast.success('Сниппет успешно добавлен');
        return true;
    } catch (error: any) {
        toast.error('Не удалось добавить сниппет');
        console.error('Failed to add snippet:', error);
    }
};

export const updateSnippet = async (commandDto: ICommandDto, id: string) => {
    try {
        await axiosClient.put(`Snippet/Update/${id}`, {
            theme: commandDto.theme,
            level: commandDto.level,
            codeSnippet: commandDto.codeSnippet,
            mainQuestion: commandDto.mainQuestion,
            solution: commandDto.solution,
            additionalQuestions: commandDto.additionalQuestions
        });
        toast.success('Сниппет успешно обновлен');
        return true;
    } catch (error: any) {
        toast.error('Не удалось обновить сниппет');
        console.error('Failed to add snippet:', error);
    }
};

export const parseFile = async (file: File) => {
    try {
        const formData = new FormData();
        formData.append('file', file);

        const response = await axiosClient.post('Snippet/ParseFromTextFile', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        });
        return response;
    } catch (error: any) {
        toast.error('Не удалось распарсить сниппет');
        console.error('Failed to parse snippet:', error);
    }
}