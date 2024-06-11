import axios from 'axios';
import { toast } from "sonner"

export const login = async (username: string, password: string) => {
    const token_endpoint = import.meta.env.VITE_KEYCLOAK_URL;

    const params = new URLSearchParams();
    params.append('grant_type', 'password');
    params.append('client_id', 'default-public-client');
    params.append('username', username);
    params.append('password', password);

    try {
        const response = await axios.post(token_endpoint, params, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
        });
        return response.data;
    } catch (error) {
        console.error('Failed to login:', error);
        toast.error('Неверный логин или пароль');
        throw error;
    }
};

export const refreshToken = async () => {
    const token_endpoint = import.meta.env.VITE_KEYCLOAK_URL;

    const params = new URLSearchParams();
    params.append('grant_type', 'refresh_token');
    params.append('client_id', 'default-public-client');
    params.append('refresh_token', `${localStorage.getItem('REFRESH_TOKEN')}`);

    try {
        const response = await axios.post(token_endpoint, params, {
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
        });
        return response.data;
    } catch (error) {
        console.error('Failed to refresh:', error);
        toast.error('Не удалось обновить токен')
        throw error;
    }
}