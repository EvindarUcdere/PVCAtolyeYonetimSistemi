import { EditOutlined, PlusOutlined, SaveOutlined } from '@ant-design/icons';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  Button,
  Card,
  Col,
  Form,
  Input,
  InputNumber,
  Modal,
  Row,
  Select,
  Space,
  Switch,
  Table,
  Tabs,
  Tag,
  Typography,
  message,
} from 'antd';
import { useEffect, useMemo, useState } from 'react';
import {
  definitionTypes,
  settingsApi,
  type CompanyProfile,
  type DefinitionItem,
  type NumberSequence,
  type Warehouse,
} from './settingsApi';

type DefinitionFormValues = Omit<DefinitionItem, 'id'>;
type WarehouseFormValues = Omit<Warehouse, 'id'>;
type NumberSequenceFormValues = Omit<NumberSequence, 'id' | 'documentType'>;

export function SettingsPage() {
  const queryClient = useQueryClient();
  const [companyForm] = Form.useForm<Omit<CompanyProfile, 'id'>>();
  const [warehouseForm] = Form.useForm<WarehouseFormValues>();
  const [definitionForm] = Form.useForm<DefinitionFormValues>();
  const [sequenceForm] = Form.useForm<NumberSequenceFormValues>();
  const [selectedDefinitionType, setSelectedDefinitionType] = useState(definitionTypes[0].key);
  const [editingWarehouse, setEditingWarehouse] = useState<Warehouse | null>(null);
  const [isWarehouseModalOpen, setWarehouseModalOpen] = useState(false);
  const [editingDefinition, setEditingDefinition] = useState<DefinitionItem | null>(null);
  const [isDefinitionModalOpen, setDefinitionModalOpen] = useState(false);
  const [editingSequence, setEditingSequence] = useState<NumberSequence | null>(null);

  const companyQuery = useQuery({
    queryKey: ['settings', 'company-profile'],
    queryFn: settingsApi.getCompanyProfile,
  });
  const warehousesQuery = useQuery({
    queryKey: ['settings', 'warehouses'],
    queryFn: settingsApi.getWarehouses,
  });
  const definitionsQuery = useQuery({
    queryKey: ['settings', 'definitions', selectedDefinitionType],
    queryFn: () => settingsApi.getDefinitions(selectedDefinitionType),
  });
  const sequencesQuery = useQuery({
    queryKey: ['settings', 'number-sequences'],
    queryFn: settingsApi.getNumberSequences,
  });

  useEffect(() => {
    if (companyQuery.data) {
      companyForm.setFieldsValue(companyQuery.data);
    }
  }, [companyForm, companyQuery.data]);

  const selectedDefinitionLabel = useMemo(
    () => definitionTypes.find((item) => item.key === selectedDefinitionType)?.label ?? 'Tanimlar',
    [selectedDefinitionType],
  );

  const companyMutation = useMutation({
    mutationFn: settingsApi.updateCompanyProfile,
    onSuccess: async () => {
      message.success('Isletme profili kaydedildi.');
      await queryClient.invalidateQueries({ queryKey: ['settings', 'company-profile'] });
    },
  });

  const warehouseMutation = useMutation({
    mutationFn: (values: WarehouseFormValues) =>
      editingWarehouse
        ? settingsApi.updateWarehouse(editingWarehouse.id, values)
        : settingsApi.createWarehouse(values),
    onSuccess: async () => {
      message.success('Depo kaydedildi.');
      setWarehouseModalOpen(false);
      setEditingWarehouse(null);
      await queryClient.invalidateQueries({ queryKey: ['settings', 'warehouses'] });
    },
  });

  const definitionMutation = useMutation({
    mutationFn: (values: DefinitionFormValues) =>
      editingDefinition
        ? settingsApi.updateDefinition(selectedDefinitionType, editingDefinition.id, values)
        : settingsApi.createDefinition(selectedDefinitionType, values),
    onSuccess: async () => {
      message.success('Tanim kaydedildi.');
      setDefinitionModalOpen(false);
      setEditingDefinition(null);
      await queryClient.invalidateQueries({ queryKey: ['settings', 'definitions', selectedDefinitionType] });
    },
  });

  const sequenceMutation = useMutation({
    mutationFn: (values: NumberSequenceFormValues) => {
      if (!editingSequence) {
        throw new Error('Numara ayari secilmedi.');
      }

      return settingsApi.updateNumberSequence(editingSequence.documentType, values);
    },
    onSuccess: async () => {
      message.success('Numara ayari kaydedildi.');
      setEditingSequence(null);
      await queryClient.invalidateQueries({ queryKey: ['settings', 'number-sequences'] });
    },
  });

  const openWarehouseModal = (warehouse?: Warehouse) => {
    setEditingWarehouse(warehouse ?? null);
    warehouseForm.setFieldsValue(
      warehouse ?? {
        code: '',
        name: '',
        address: '',
        isDefault: false,
        isActive: true,
      },
    );
    setWarehouseModalOpen(true);
  };

  const openDefinitionModal = (definition?: DefinitionItem) => {
    setEditingDefinition(definition ?? null);
    definitionForm.setFieldsValue(
      definition ?? {
        code: '',
        name: '',
        description: '',
        sortOrder: 0,
        isActive: true,
      },
    );
    setDefinitionModalOpen(true);
  };

  const openSequenceModal = (sequence: NumberSequence) => {
    setEditingSequence(sequence);
    sequenceForm.setFieldsValue(sequence);
  };

  return (
    <Space direction="vertical" size={18} className="page-stack">
      <div>
        <Typography.Title level={2}>Ayarlar ve Tanimlar</Typography.Title>
        <Typography.Text type="secondary">Isletme, depo, ana tanim ve numara ayarlari.</Typography.Text>
      </div>

      <Tabs
        items={[
          {
            key: 'company',
            label: 'Isletme',
            children: (
              <Card>
                <Form layout="vertical" form={companyForm} onFinish={(values) => companyMutation.mutate(values)}>
                  <Row gutter={16}>
                    <Col xs={24} md={12}>
                      <Form.Item label="Isletme adi" name="companyName" rules={[{ required: true, message: 'Isletme adi zorunlu.' }]}>
                        <Input maxLength={200} />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={6}>
                      <Form.Item label="Para birimi" name="currencyCode" rules={[{ required: true, message: 'Para birimi zorunlu.' }]}>
                        <Input maxLength={3} />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={6}>
                      <Form.Item label="Varsayilan KDV" name="defaultVatRate" rules={[{ required: true, message: 'KDV zorunlu.' }]}>
                        <InputNumber min={0} max={100} precision={2} className="full-width" />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={8}>
                      <Form.Item label="Vergi dairesi" name="taxOffice">
                        <Input maxLength={100} />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={8}>
                      <Form.Item label="Vergi no" name="taxNumber">
                        <Input maxLength={50} />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={8}>
                      <Form.Item label="Telefon" name="phone">
                        <Input maxLength={50} />
                      </Form.Item>
                    </Col>
                    <Col xs={24} md={12}>
                      <Form.Item label="E-posta" name="email">
                        <Input maxLength={150} />
                      </Form.Item>
                    </Col>
                    <Col xs={24}>
                      <Form.Item label="Adres" name="address">
                        <Input.TextArea rows={3} maxLength={500} />
                      </Form.Item>
                    </Col>
                    <Col xs={24}>
                      <Form.Item label="Teklif alt notu" name="quoteFooterNote">
                        <Input.TextArea rows={3} maxLength={1000} />
                      </Form.Item>
                    </Col>
                  </Row>
                  <Button type="primary" htmlType="submit" icon={<SaveOutlined />} loading={companyMutation.isPending || companyQuery.isLoading}>
                    Kaydet
                  </Button>
                </Form>
              </Card>
            ),
          },
          {
            key: 'warehouses',
            label: 'Depolar',
            children: (
              <Card
                extra={
                  <Button type="primary" icon={<PlusOutlined />} onClick={() => openWarehouseModal()}>
                    Yeni Depo
                  </Button>
                }
              >
                <Table
                  rowKey="id"
                  loading={warehousesQuery.isLoading}
                  dataSource={warehousesQuery.data ?? []}
                  pagination={false}
                  columns={[
                    { title: 'Kod', dataIndex: 'code', width: 140 },
                    { title: 'Depo', dataIndex: 'name' },
                    { title: 'Adres', dataIndex: 'address' },
                    {
                      title: 'Durum',
                      key: 'state',
                      width: 180,
                      render: (_, record) => (
                        <Space>
                          {record.isDefault && <Tag color="green">Varsayilan</Tag>}
                          <Tag color={record.isActive ? 'blue' : 'default'}>{record.isActive ? 'Aktif' : 'Pasif'}</Tag>
                        </Space>
                      ),
                    },
                    {
                      title: '',
                      width: 80,
                      render: (_, record) => (
                        <Button icon={<EditOutlined />} onClick={() => openWarehouseModal(record)} aria-label="Depo duzenle" />
                      ),
                    },
                  ]}
                />
              </Card>
            ),
          },
          {
            key: 'definitions',
            label: 'Ana Tanimlar',
            children: (
              <Card
                extra={
                  <Button type="primary" icon={<PlusOutlined />} onClick={() => openDefinitionModal()}>
                    Yeni Tanim
                  </Button>
                }
              >
                <Space direction="vertical" size={16} className="page-stack">
                  <Select className="definition-select" value={selectedDefinitionType} options={definitionTypes.map((item) => ({ value: item.key, label: item.label }))} onChange={setSelectedDefinitionType} />
                  <Table
                    rowKey="id"
                    loading={definitionsQuery.isLoading}
                    dataSource={definitionsQuery.data ?? []}
                    pagination={{ pageSize: 10 }}
                    columns={[
                      { title: 'Sira', dataIndex: 'sortOrder', width: 90 },
                      { title: 'Kod', dataIndex: 'code', width: 180 },
                      { title: selectedDefinitionLabel, dataIndex: 'name' },
                      { title: 'Aciklama', dataIndex: 'description' },
                      {
                        title: 'Durum',
                        dataIndex: 'isActive',
                        width: 110,
                        render: (isActive: boolean) => <Tag color={isActive ? 'blue' : 'default'}>{isActive ? 'Aktif' : 'Pasif'}</Tag>,
                      },
                      {
                        title: '',
                        width: 80,
                        render: (_, record) => (
                          <Button icon={<EditOutlined />} onClick={() => openDefinitionModal(record)} aria-label="Tanim duzenle" />
                        ),
                      },
                    ]}
                  />
                </Space>
              </Card>
            ),
          },
          {
            key: 'numbering',
            label: 'Numaralandirma',
            children: (
              <Card>
                <Table
                  rowKey="id"
                  loading={sequencesQuery.isLoading}
                  dataSource={sequencesQuery.data ?? []}
                  pagination={false}
                  columns={[
                    { title: 'Belge', dataIndex: 'documentType', width: 160 },
                    { title: 'On ek', dataIndex: 'prefix', width: 160 },
                    { title: 'Siradaki no', dataIndex: 'nextNumber', width: 160 },
                    { title: 'Hane', dataIndex: 'paddingLength', width: 120 },
                    {
                      title: 'Durum',
                      dataIndex: 'isActive',
                      width: 110,
                      render: (isActive: boolean) => <Tag color={isActive ? 'blue' : 'default'}>{isActive ? 'Aktif' : 'Pasif'}</Tag>,
                    },
                    {
                      title: '',
                      width: 80,
                      render: (_, record) => (
                        <Button icon={<EditOutlined />} onClick={() => openSequenceModal(record)} aria-label="Numara ayari duzenle" />
                      ),
                    },
                  ]}
                />
              </Card>
            ),
          },
        ]}
      />

      <Modal
        title={editingWarehouse ? 'Depo Duzenle' : 'Yeni Depo'}
        open={isWarehouseModalOpen}
        onCancel={() => setWarehouseModalOpen(false)}
        onOk={() => warehouseForm.submit()}
        confirmLoading={warehouseMutation.isPending}
        destroyOnHidden
      >
        <Form layout="vertical" form={warehouseForm} onFinish={(values) => warehouseMutation.mutate(values)}>
          <Form.Item label="Kod" name="code" rules={[{ required: true, message: 'Kod zorunlu.' }]}>
            <Input maxLength={50} />
          </Form.Item>
          <Form.Item label="Depo adi" name="name" rules={[{ required: true, message: 'Depo adi zorunlu.' }]}>
            <Input maxLength={150} />
          </Form.Item>
          <Form.Item label="Adres" name="address">
            <Input.TextArea rows={3} maxLength={500} />
          </Form.Item>
          <Space size={24}>
            <Form.Item label="Varsayilan" name="isDefault" valuePropName="checked">
              <Switch />
            </Form.Item>
            <Form.Item label="Aktif" name="isActive" valuePropName="checked">
              <Switch />
            </Form.Item>
          </Space>
        </Form>
      </Modal>

      <Modal
        title={editingDefinition ? 'Tanim Duzenle' : 'Yeni Tanim'}
        open={isDefinitionModalOpen}
        onCancel={() => setDefinitionModalOpen(false)}
        onOk={() => definitionForm.submit()}
        confirmLoading={definitionMutation.isPending}
        destroyOnHidden
      >
        <Form layout="vertical" form={definitionForm} onFinish={(values) => definitionMutation.mutate(values)}>
          <Form.Item label="Kod" name="code" rules={[{ required: true, message: 'Kod zorunlu.' }]}>
            <Input maxLength={50} />
          </Form.Item>
          <Form.Item label="Ad" name="name" rules={[{ required: true, message: 'Ad zorunlu.' }]}>
            <Input maxLength={150} />
          </Form.Item>
          <Form.Item label="Aciklama" name="description">
            <Input.TextArea rows={3} maxLength={500} />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item label="Sira" name="sortOrder" rules={[{ required: true, message: 'Sira zorunlu.' }]}>
                <InputNumber min={0} className="full-width" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item label="Aktif" name="isActive" valuePropName="checked">
                <Switch />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      <Modal
        title={`${editingSequence?.documentType ?? ''} Numaralandirma`}
        open={Boolean(editingSequence)}
        onCancel={() => setEditingSequence(null)}
        onOk={() => sequenceForm.submit()}
        confirmLoading={sequenceMutation.isPending}
        destroyOnHidden
      >
        <Form layout="vertical" form={sequenceForm} onFinish={(values) => sequenceMutation.mutate(values)}>
          <Form.Item label="On ek" name="prefix" rules={[{ required: true, message: 'On ek zorunlu.' }]}>
            <Input maxLength={20} />
          </Form.Item>
          <Form.Item label="Siradaki no" name="nextNumber" rules={[{ required: true, message: 'Siradaki no zorunlu.' }]}>
            <InputNumber min={1} className="full-width" />
          </Form.Item>
          <Form.Item label="Hane" name="paddingLength" rules={[{ required: true, message: 'Hane zorunlu.' }]}>
            <InputNumber min={1} max={12} className="full-width" />
          </Form.Item>
          <Form.Item label="Aktif" name="isActive" valuePropName="checked">
            <Switch />
          </Form.Item>
        </Form>
      </Modal>
    </Space>
  );
}
