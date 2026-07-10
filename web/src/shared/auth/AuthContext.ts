import { createContext } from 'react';
import type { CurrentUser } from '../../features/auth/authTypes';
import type { LoginInput } from '../../features/auth/authApi';

export type AuthContextValue = {
  user: CurrentUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
  login: (input: LoginInput) => Promise<boolean>;
  logout: () => void;
};

export const AuthContext = createContext<AuthContextValue | null>(null);
