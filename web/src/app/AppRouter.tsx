import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { DashboardPage } from '../features/dashboard/DashboardPage';
import { AppShell } from '../shared/layout/AppShell';

const router = createBrowserRouter([
  {
    path: '/',
    element: <AppShell />,
    children: [
      {
        index: true,
        element: <DashboardPage />,
      },
    ],
  },
]);

export function AppRouter() {
  return <RouterProvider router={router} />;
}
