import { ApiOutlined, CheckCircleOutlined, DatabaseOutlined, HddOutlined } from '@ant-design/icons';
import { Alert, Card, Col, Row, Space, Statistic, Table, Tag, Typography } from 'antd';

const modules = [
  { key: 'auth', name: 'Kimlik ve Yetki', phase: 'Faz 2', status: 'Tamamlandi' },
  { key: 'settings', name: 'Ayarlar ve Tanimlar', phase: 'Faz 3', status: 'Tamamlandi' },
  { key: 'customers', name: 'Musteri Yonetimi', phase: 'Faz 4', status: 'Hazirlaniyor' },
  { key: 'quotes', name: 'Teklif ve Siparis', phase: 'Faz 6-7', status: 'Planlandi' },
];

const columns = [
  { title: 'Modul', dataIndex: 'name', key: 'name' },
  { title: 'Faz', dataIndex: 'phase', key: 'phase', width: 120 },
  {
    title: 'Durum',
    dataIndex: 'status',
    key: 'status',
    width: 140,
    render: (status: string) => <Tag color="processing">{status}</Tag>,
  },
];

export function DashboardPage() {
  return (
    <Space direction="vertical" size={20} className="page-stack">
      <div>
        <Typography.Title level={2}>Proje Altyapisi</Typography.Title>
        <Typography.Text type="secondary">
          Backend, frontend, kimlik altyapisi ve ana tanimlar yerel calisma icin hazirlaniyor.
        </Typography.Text>
      </div>

      <Alert
        type="success"
        showIcon
        message="Sistem temeli"
        description="Kimlik dogrulama, yetki politikasi, veritabani migrationlari ve ana ayarlar modulu sonraki is fazlari icin temel olusturur."
      />

      <Row gutter={[16, 16]}>
        <Col xs={24} md={12} xl={6}>
          <Card>
            <Statistic title="API" value="Hazirlaniyor" prefix={<ApiOutlined />} />
          </Card>
        </Col>
        <Col xs={24} md={12} xl={6}>
          <Card>
            <Statistic title="PostgreSQL" value="Docker" prefix={<DatabaseOutlined />} />
          </Card>
        </Col>
        <Col xs={24} md={12} xl={6}>
          <Card>
            <Statistic title="Yerel Kurulum" value="LAN" prefix={<HddOutlined />} />
          </Card>
        </Col>
        <Col xs={24} md={12} xl={6}>
          <Card>
            <Statistic title="Mimari" value="Monolith" prefix={<CheckCircleOutlined />} />
          </Card>
        </Col>
      </Row>

      <Card title="Fazlara gore ilk moduller">
        <Table columns={columns} dataSource={modules} pagination={false} />
      </Card>
    </Space>
  );
}
