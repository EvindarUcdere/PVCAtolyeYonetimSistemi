import { Spin } from 'antd';
import type { PropsWithChildren } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../features/auth/useAuth';

export function ProtectedRoute({ children }: PropsWithChildren) {
  const auth = useAuth();
  const location = useLocation();

  if (auth.isLoading) {
    return <Spin fullscreen tip="Oturum kontrol ediliyor" />;
  }

  if (!auth.isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return children;
}
