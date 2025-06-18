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
    private static readonly BASE_URL = '/api/user';

    static async getUsers(params: PaginationParams): Promise<PaginatedResponse<User>> {
        try {
            const response = await axios.get<PaginatedResponse<User>>(this.BASE_URL, {
                params: {
                    page: params.page,
                    pageSize: params.pageSize
                }
            });
            return response.data;
        } catch (error) {
            console.error('Error fetching users:', error);
            throw error;
        }
    }
}
