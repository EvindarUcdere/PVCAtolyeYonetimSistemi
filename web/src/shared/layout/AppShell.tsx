import {
  AppstoreOutlined,
  CalendarOutlined,
  DashboardOutlined,
  DollarOutlined,
  FileTextOutlined,
  InboxOutlined,
  LogoutOutlined,
  SettingOutlined,
  ToolOutlined,
  TruckOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { Button, Layout, Menu, Space, Typography } from 'antd';
import { Outlet, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../features/auth/useAuth';

const { Header, Sider, Content } = Layout;

const menuItems = [
  { key: 'dashboard', icon: <DashboardOutlined />, label: 'Dashboard' },
  { key: 'customers', icon: <UserOutlined />, label: 'Musteriler' },
  { key: 'surveys', icon: <CalendarOutlined />, label: 'Kesif ve Olcu' },
  { key: 'quotes', icon: <FileTextOutlined />, label: 'Teklifler' },
  { key: 'orders', icon: <AppstoreOutlined />, label: 'Siparisler' },
  { key: 'inventory', icon: <InboxOutlined />, label: 'Stok' },
  { key: 'production', icon: <ToolOutlined />, label: 'Uretim' },
  { key: 'installation', icon: <TruckOutlined />, label: 'Montaj' },
  { key: 'payments', icon: <DollarOutlined />, label: 'Tahsilat' },
  { key: 'settings', icon: <SettingOutlined />, label: 'Ayarlar' },
];

export function AppShell() {
  const auth = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const selectedKey = location.pathname.startsWith('/settings') ? 'settings' : 'dashboard';

  return (
    <Layout className="app-shell">
      <Sider width={248} breakpoint="lg" collapsedWidth="0" className="app-sidebar">
        <div className="brand-block">
          <Typography.Title level={4}>PVC Atolye</Typography.Title>
          <Typography.Text>Yonetim Sistemi</Typography.Text>
        </div>
        <Menu
          mode="inline"
          selectedKeys={[selectedKey]}
          items={menuItems}
          onClick={({ key }) => {
            if (key === 'dashboard') {
              navigate('/');
            }

            if (key === 'settings') {
              navigate('/settings');
            }
          }}
        />
      </Sider>
      <Layout>
        <Header className="app-header">
          <Space>
            <Typography.Text strong>{auth.user?.displayName}</Typography.Text>
            <Button icon={<LogoutOutlined />} onClick={auth.logout}>
              Cikis
            </Button>
          </Space>
        </Header>
        <Content className="app-content">
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
