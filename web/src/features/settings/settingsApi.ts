import { apiClient, type ApiResponse } from '../../shared/api/apiClient';

export type CompanyProfile = {
  id: string;
  companyName: string;
  taxOffice?: string | null;
  taxNumber?: string | null;
  address?: string | null;
  phone?: string | null;
  email?: string | null;
  currencyCode: string;
  defaultVatRate: number;
  quoteFooterNote?: string | null;
};

export type Warehouse = {
  id: string;
  code: string;
  name: string;
  address?: string | null;
  isDefault: boolean;
  isActive: boolean;
};

export type DefinitionItem = {
  id: string;
  code: string;
  name: string;
  description?: string | null;
  sortOrder: number;
  isActive: boolean;
};

export type NumberSequence = {
  id: string;
  documentType: string;
  prefix: string;
  nextNumber: number;
  paddingLength: number;
  isActive: boolean;
};

export type DefinitionType = {
  key: string;
  label: string;
};

export const definitionTypes: DefinitionType[] = [
  { key: 'units', label: 'Olcu Birimleri' },
  { key: 'material-categories', label: 'Malzeme Kategorileri' },
  { key: 'product-types', label: 'Urun Tipleri' },
  { key: 'colors', label: 'Renkler' },
  { key: 'glass-types', label: 'Cam Tipleri' },
  { key: 'profile-series', label: 'Profil Serileri' },
  { key: 'accessory-types', label: 'Aksesuar Tipleri' },
  { key: 'production-stages', label: 'Uretim Asamalari' },
  { key: 'payment-methods', label: 'Odeme Yontemleri' },
];

async function unwrap<T>(request: Promise<{ data: ApiResponse<T> }>): Promise<T> {
  const response = await request;
  if (!response.data.success || response.data.data === null) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export const settingsApi = {
  getCompanyProfile: () => unwrap(apiClient.get<ApiResponse<CompanyProfile>>('/api/settings/company-profile')),
  updateCompanyProfile: (payload: Omit<CompanyProfile, 'id'>) =>
    unwrap(apiClient.put<ApiResponse<CompanyProfile>>('/api/settings/company-profile', payload)),
  getWarehouses: () => unwrap(apiClient.get<ApiResponse<Warehouse[]>>('/api/settings/warehouses')),
  createWarehouse: (payload: Omit<Warehouse, 'id'>) =>
    unwrap(apiClient.post<ApiResponse<Warehouse>>('/api/settings/warehouses', payload)),
  updateWarehouse: (id: string, payload: Omit<Warehouse, 'id'>) =>
    unwrap(apiClient.put<ApiResponse<Warehouse>>(`/api/settings/warehouses/${id}`, payload)),
  getDefinitions: (definitionType: string) =>
    unwrap(apiClient.get<ApiResponse<DefinitionItem[]>>(`/api/settings/definitions/${definitionType}`)),
  createDefinition: (definitionType: string, payload: Omit<DefinitionItem, 'id'>) =>
    unwrap(apiClient.post<ApiResponse<DefinitionItem>>(`/api/settings/definitions/${definitionType}`, payload)),
  updateDefinition: (definitionType: string, id: string, payload: Omit<DefinitionItem, 'id'>) =>
    unwrap(apiClient.put<ApiResponse<DefinitionItem>>(`/api/settings/definitions/${definitionType}/${id}`, payload)),
  getNumberSequences: () => unwrap(apiClient.get<ApiResponse<NumberSequence[]>>('/api/settings/number-sequences')),
  updateNumberSequence: (documentType: string, payload: Omit<NumberSequence, 'id' | 'documentType'>) =>
    unwrap(apiClient.put<ApiResponse<NumberSequence>>(`/api/settings/number-sequences/${documentType}`, payload)),
};
