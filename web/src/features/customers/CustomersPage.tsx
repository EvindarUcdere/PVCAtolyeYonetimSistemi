import { EditOutlined, PlusOutlined, SearchOutlined } from '@ant-design/icons';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  Button,
  Card,
  Checkbox,
  Col,
  Form,
  Input,
  Modal,
  Row,
  Select,
  Space,
  Switch,
  Table,
  Tag,
  Typography,
  message,
} from 'antd';
import { useMemo, useState } from 'react';
import { customersApi, type CustomerDetail, type CustomerPayload } from './customersApi';

const emptyCustomer: CustomerPayload = {
  type: 'Individual',
  displayName: '',
  phone: '',
  email: '',
  taxNumber: '',
  taxOffice: '',
  identityNumber: '',
  notes: '',
  isActive: true,
  addresses: [],
  contacts: [],
};

export function CustomersPage() {
  const queryClient = useQueryClient();
  const [form] = Form.useForm<CustomerPayload>();
  const [search, setSearch] = useState('');
  const [includePassive, setIncludePassive] = useState(false);
  const [page, setPage] = useState(1);
  const [editingCustomer, setEditingCustomer] = useState<CustomerDetail | null>(null);
  const [isModalOpen, setModalOpen] = useState(false);

  const customersQuery = useQuery({
    queryKey: ['customers', search, includePassive, page],
    queryFn: () => customersApi.getCustomers({ search, includePassive, page, pageSize: 10 }),
  });

  const saveMutation = useMutation({
    mutationFn: (values: CustomerPayload) =>
      editingCustomer ? customersApi.updateCustomer(editingCustomer.id, values) : customersApi.createCustomer(values),
    onSuccess: async () => {
      message.success('Musteri kaydedildi.');
      setModalOpen(false);
      setEditingCustomer(null);
      await queryClient.invalidateQueries({ queryKey: ['customers'] });
    },
  });

  const customerType = Form.useWatch('type', form);
  const modalTitle = useMemo(() => (editingCustomer ? 'Musteri Duzenle' : 'Yeni Musteri'), [editingCustomer]);

  const openCreateModal = () => {
    setEditingCustomer(null);
    form.setFieldsValue(emptyCustomer);
    setModalOpen(true);
  };

  const openEditModal = async (id: string) => {
    const detail = await customersApi.getCustomer(id);
    setEditingCustomer(detail);
    form.setFieldsValue(detail);
    setModalOpen(true);
  };

  return (
    <Space direction="vertical" size={18} className="page-stack">
      <div className="page-heading-row">
        <div>
          <Typography.Title level={2}>Musteriler</Typography.Title>
          <Typography.Text type="secondary">Musteri kartlari, adresler ve yetkili kisiler.</Typography.Text>
        </div>
        <Button type="primary" icon={<PlusOutlined />} onClick={openCreateModal}>
          Yeni Musteri
        </Button>
      </div>

      <Card>
        <Space className="customer-toolbar" wrap>
          <Input
            allowClear
            prefix={<SearchOutlined />}
            placeholder="Ad, telefon, e-posta, vergi no ara"
            value={search}
            onChange={(event) => {
              setSearch(event.target.value);
              setPage(1);
            }}
          />
          <Checkbox checked={includePassive} onChange={(event) => setIncludePassive(event.target.checked)}>
            Pasifleri dahil et
          </Checkbox>
        </Space>

        <Table
          rowKey="id"
          loading={customersQuery.isLoading}
          dataSource={customersQuery.data?.items ?? []}
          pagination={{
            current: customersQuery.data?.page ?? page,
            pageSize: customersQuery.data?.pageSize ?? 10,
            total: customersQuery.data?.totalCount ?? 0,
            onChange: setPage,
          }}
          columns={[
            { title: 'Musteri', dataIndex: 'displayName' },
            {
              title: 'Tip',
              dataIndex: 'type',
              width: 130,
              render: (type: string) => (type === 'Corporate' ? 'Kurumsal' : 'Bireysel'),
            },
            { title: 'Telefon', dataIndex: 'phone', width: 160 },
            { title: 'E-posta', dataIndex: 'email', width: 220 },
            { title: 'Vergi No', dataIndex: 'taxNumber', width: 150 },
            {
              title: 'Durum',
              dataIndex: 'isActive',
              width: 110,
              render: (isActive: boolean) => <Tag color={isActive ? 'blue' : 'default'}>{isActive ? 'Aktif' : 'Pasif'}</Tag>,
            },
            {
              title: '',
              width: 80,
              render: (_, record) => <Button icon={<EditOutlined />} onClick={() => openEditModal(record.id)} aria-label="Musteri duzenle" />,
            },
          ]}
        />
      </Card>

      <Modal
        title={modalTitle}
        open={isModalOpen}
        onCancel={() => setModalOpen(false)}
        onOk={() => form.submit()}
        confirmLoading={saveMutation.isPending}
        width={920}
        destroyOnHidden
      >
        <Form layout="vertical" form={form} onFinish={(values) => saveMutation.mutate(values)}>
          <Row gutter={16}>
            <Col xs={24} md={8}>
              <Form.Item label="Musteri tipi" name="type" rules={[{ required: true, message: 'Musteri tipi zorunlu.' }]}>
                <Select
                  options={[
                    { value: 'Individual', label: 'Bireysel' },
                    { value: 'Corporate', label: 'Kurumsal' },
                  ]}
                />
              </Form.Item>
            </Col>
            <Col xs={24} md={16}>
              <Form.Item label="Musteri adi" name="displayName" rules={[{ required: true, message: 'Musteri adi zorunlu.' }]}>
                <Input maxLength={200} />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item label="Telefon" name="phone">
                <Input maxLength={50} />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item label="E-posta" name="email">
                <Input maxLength={150} />
              </Form.Item>
            </Col>
            <Col xs={24} md={8}>
              <Form.Item label="Aktif" name="isActive" valuePropName="checked">
                <Switch />
              </Form.Item>
            </Col>
            {customerType === 'Corporate' ? (
              <>
                <Col xs={24} md={12}>
                  <Form.Item label="Vergi dairesi" name="taxOffice">
                    <Input maxLength={100} />
                  </Form.Item>
                </Col>
                <Col xs={24} md={12}>
                  <Form.Item label="Vergi no" name="taxNumber" rules={[{ required: true, message: 'Vergi no zorunlu.' }]}>
                    <Input maxLength={50} />
                  </Form.Item>
                </Col>
              </>
            ) : (
              <Col xs={24} md={12}>
                <Form.Item label="Kimlik no" name="identityNumber" rules={[{ required: true, message: 'Kimlik no zorunlu.' }]}>
                  <Input maxLength={20} />
                </Form.Item>
              </Col>
            )}
            <Col xs={24}>
              <Form.Item label="Notlar" name="notes">
                <Input.TextArea rows={3} maxLength={1000} />
              </Form.Item>
            </Col>
          </Row>

          <Typography.Title level={5}>Adresler</Typography.Title>
          <Form.List name="addresses">
            {(fields, { add, remove }) => (
              <Space direction="vertical" size={12} className="page-stack">
                {fields.map((field) => (
                  <Card key={field.key} size="small">
                    <Row gutter={12}>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Baslik" name={[field.name, 'title']} rules={[{ required: true, message: 'Baslik zorunlu.' }]}>
                          <Input maxLength={80} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Ilce" name={[field.name, 'district']}>
                          <Input maxLength={100} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Il" name={[field.name, 'city']}>
                          <Input maxLength={100} />
                        </Form.Item>
                      </Col>
                      <Col xs={24}>
                        <Form.Item {...field} label="Adres" name={[field.name, 'addressLine']} rules={[{ required: true, message: 'Adres zorunlu.' }]}>
                          <Input.TextArea rows={2} maxLength={500} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Posta kodu" name={[field.name, 'postalCode']}>
                          <Input maxLength={20} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Varsayilan" name={[field.name, 'isDefault']} valuePropName="checked">
                          <Switch />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Button danger onClick={() => remove(field.name)}>
                          Kaldir
                        </Button>
                      </Col>
                    </Row>
                  </Card>
                ))}
                <Button icon={<PlusOutlined />} onClick={() => add({ title: 'Adres', addressLine: '', isDefault: fields.length === 0 })}>
                  Adres Ekle
                </Button>
              </Space>
            )}
          </Form.List>

          <Typography.Title level={5}>Yetkili Kisiler</Typography.Title>
          <Form.List name="contacts">
            {(fields, { add, remove }) => (
              <Space direction="vertical" size={12} className="page-stack">
                {fields.map((field) => (
                  <Card key={field.key} size="small">
                    <Row gutter={12}>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Ad soyad" name={[field.name, 'fullName']} rules={[{ required: true, message: 'Ad soyad zorunlu.' }]}>
                          <Input maxLength={150} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Gorev" name={[field.name, 'title']}>
                          <Input maxLength={100} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Telefon" name={[field.name, 'phone']}>
                          <Input maxLength={50} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="E-posta" name={[field.name, 'email']}>
                          <Input maxLength={150} />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Form.Item {...field} label="Birincil" name={[field.name, 'isPrimary']} valuePropName="checked">
                          <Switch />
                        </Form.Item>
                      </Col>
                      <Col xs={24} md={8}>
                        <Button danger onClick={() => remove(field.name)}>
                          Kaldir
                        </Button>
                      </Col>
                    </Row>
                  </Card>
                ))}
                <Button icon={<PlusOutlined />} onClick={() => add({ fullName: '', isPrimary: fields.length === 0 })}>
                  Yetkili Ekle
                </Button>
              </Space>
            )}
          </Form.List>
        </Form>
      </Modal>
    </Space>
  );
}
