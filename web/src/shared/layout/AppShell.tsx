import {
  AppstoreOutlined,
  CalendarOutlined,
  DashboardOutlined,
  DollarOutlined,
  FileTextOutlined,
  InboxOutlined,
  SettingOutlined,
  ToolOutlined,
  TruckOutlined,
  UserOutlined,
} from '@ant-design/icons';
import { Layout, Menu, Typography } from 'antd';
import { Outlet } from 'react-router-dom';

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
  return (
    <Layout className="app-shell">
      <Sider width={248} breakpoint="lg" collapsedWidth="0" className="app-sidebar">
        <div className="brand-block">
          <Typography.Title level={4}>PVC Atolye</Typography.Title>
          <Typography.Text>Yonetim Sistemi</Typography.Text>
        </div>
        <Menu mode="inline" selectedKeys={['dashboard']} items={menuItems} />
      </Sider>
      <Layout>
        <Header className="app-header">
          <Typography.Text strong>Faz 1 Altyapi</Typography.Text>
        </Header>
        <Content className="app-content">
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
