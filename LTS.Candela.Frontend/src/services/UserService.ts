import axios from 'axios';
import type { User } from '@/types/User';

export interface PaginatedResponse<T> {
    items: T[];
    totalItems: number;
    currentPage: number;
    totalPages: number;
}

export interface PaginationParams {
    page: number;
    pageSize: number;
}

export class UserService {
    static async createUser(user: { name: string; email: string; tokens: number }): Promise<void> {
        await axios.post(`${this.BASE_URL}/create`, user);
    }

    private static readonly BASE_URL = '/api/user';

    static async getUsers(params: PaginationParams): Promise<PaginatedResponse<User>> {
        
            const response = await axios.get<PaginatedResponse<User>>(this.BASE_URL, {
                params: {
                    page: params.page,
                    pageSize: params.pageSize
                }
            });
            return response.data;
    }
}
