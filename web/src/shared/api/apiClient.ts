import axios from 'axios';
import { authStorage } from '../auth/authStorage';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '',
  timeout: 15_000,
});

apiClient.interceptors.request.use((config) => {
  const token = authStorage.getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export type ApiResponse<T> = {
  success: boolean;
  message: string;
  data: T | null;
  errors: string[];
};
