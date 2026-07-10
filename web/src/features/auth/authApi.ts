import { apiClient, type ApiResponse } from '../../shared/api/apiClient';
import type { CurrentUser, LoginResponse } from './authTypes';

export type LoginInput = {
  username: string;
  password: string;
};

export async function login(input: LoginInput) {
  const response = await apiClient.post<ApiResponse<LoginResponse>>('/api/auth/login', input);
  return response.data;
}

export async function getCurrentUser() {
  const response = await apiClient.get<ApiResponse<CurrentUser>>('/api/auth/me');
  return response.data;
}
