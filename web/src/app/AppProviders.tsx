import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ConfigProvider } from 'antd';
import trTR from 'antd/locale/tr_TR';
import type { PropsWithChildren } from 'react';
import { AuthProvider } from '../shared/auth/AuthProvider';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
    },
  },
});

export function AppProviders({ children }: PropsWithChildren) {
  return (
    <ConfigProvider
      locale={trTR}
      theme={{
        token: {
          colorPrimary: '#1f7a5a',
          borderRadius: 6,
          fontFamily: 'Inter, Segoe UI, Arial, sans-serif',
        },
      }}
    >
      <QueryClientProvider client={queryClient}>
        <AuthProvider>{children}</AuthProvider>
      </QueryClientProvider>
    </ConfigProvider>
  );
}
