import { zodResolver } from '@hookform/resolvers/zod';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography } from 'antd';
import { Controller, useForm } from 'react-hook-form';
import { Navigate, useLocation, useNavigate } from 'react-router-dom';
import { z } from 'zod';
import { useAuth } from './useAuth';

const schema = z.object({
  username: z.string().min(1, 'Kullanici adi zorunludur.'),
  password: z.string().min(1, 'Sifre zorunludur.'),
});

type FormValues = z.infer<typeof schema>;

export function LoginPage() {
  const auth = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const from = (location.state as { from?: { pathname?: string } } | null)?.from?.pathname ?? '/';
  const form = useForm<FormValues>({ resolver: zodResolver(schema), defaultValues: { username: 'admin', password: '' } });

  if (auth.user) {
    return <Navigate to="/" replace />;
  }

  const onSubmit = form.handleSubmit(async (values) => {
    const success = await auth.login(values);
    if (success) {
      navigate(from, { replace: true });
    }
  });

  return (
    <main className="login-page">
      <Card className="login-card">
        <Typography.Title level={2}>PVC Atolye</Typography.Title>
        <Typography.Text type="secondary">Yonetim paneline giris yapin.</Typography.Text>

        {auth.error ? <Alert type="error" showIcon message={auth.error} /> : null}

        <Form layout="vertical" onFinish={onSubmit} className="login-form">
          <Form.Item label="Kullanici adi" validateStatus={form.formState.errors.username ? 'error' : undefined} help={form.formState.errors.username?.message}>
            <Controller
              control={form.control}
              name="username"
              render={({ field }) => <Input {...field} prefix={<UserOutlined />} autoComplete="username" />}
            />
          </Form.Item>

          <Form.Item label="Sifre" validateStatus={form.formState.errors.password ? 'error' : undefined} help={form.formState.errors.password?.message}>
            <Controller
              control={form.control}
              name="password"
              render={({ field }) => <Input.Password {...field} prefix={<LockOutlined />} autoComplete="current-password" />}
            />
          </Form.Item>

          <Button type="primary" htmlType="submit" block loading={auth.isLoading}>
            Giris Yap
          </Button>
        </Form>
      </Card>
    </main>
  );
}
