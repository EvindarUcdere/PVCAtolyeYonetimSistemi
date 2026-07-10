import axios from 'axios';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '',
  timeout: 15_000,
});

export type ApiResponse<T> = {
  success: boolean;
  message: string;
  data: T | null;
  errors: string[];
};
