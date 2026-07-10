import { useQueryClient } from '@tanstack/react-query';
import type { PropsWithChildren } from 'react';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { getCurrentUser, login as loginRequest, type LoginInput } from '../../features/auth/authApi';
import type { CurrentUser } from '../../features/auth/authTypes';
import { authStorage } from './authStorage';
import { AuthContext } from './AuthContext';

export function AuthProvider({ children }: PropsWithChildren) {
  const queryClient = useQueryClient();
  const [user, setUser] = useState<CurrentUser | null>(null);
  const [isLoading, setIsLoading] = useState(Boolean(authStorage.getToken()));
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const token = authStorage.getToken();
    if (!token) {
      setIsLoading(false);
      return;
    }

    getCurrentUser()
      .then((response) => {
        if (response.success && response.data) {
          setUser(response.data);
        } else {
          authStorage.clear();
        }
      })
      .catch(() => authStorage.clear())
      .finally(() => setIsLoading(false));
  }, []);

  const login = useCallback(async (input: LoginInput) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await loginRequest(input);
      if (!response.success || !response.data) {
        setError(response.message);
        return false;
      }

      authStorage.setToken(response.data.accessToken);
      setUser(response.data.user);
      return true;
    } catch {
      setError('Giris islemi tamamlanamadi. Bilgileri kontrol edin.');
      return false;
    } finally {
      setIsLoading(false);
    }
  }, []);

  const logout = useCallback(() => {
    authStorage.clear();
    setUser(null);
    void queryClient.clear();
  }, [queryClient]);

  const value = useMemo(() => ({ user, isAuthenticated: Boolean(user), isLoading, error, login, logout }), [error, isLoading, login, logout, user]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
