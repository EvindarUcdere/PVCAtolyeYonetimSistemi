import { apiClient, type ApiResponse } from '../../shared/api/apiClient';

export type CustomerType = 'Individual' | 'Corporate';

export type CustomerAddress = {
  id?: string;
  title: string;
  addressLine: string;
  district?: string | null;
  city?: string | null;
  postalCode?: string | null;
  isDefault: boolean;
};

export type CustomerContact = {
  id?: string;
  fullName: string;
  title?: string | null;
  phone?: string | null;
  email?: string | null;
  isPrimary: boolean;
};

export type CustomerListItem = {
  id: string;
  type: CustomerType;
  displayName: string;
  phone?: string | null;
  email?: string | null;
  taxNumber?: string | null;
  isActive: boolean;
};

export type CustomerDetail = CustomerListItem & {
  taxOffice?: string | null;
  identityNumber?: string | null;
  notes?: string | null;
  addresses: CustomerAddress[];
  contacts: CustomerContact[];
};

export type CustomerPayload = Omit<CustomerDetail, 'id'>;

export type PagedResponse<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};

async function unwrap<T>(request: Promise<{ data: ApiResponse<T> }>): Promise<T> {
  const response = await request;
  if (!response.data.success || response.data.data === null) {
    throw new Error(response.data.message);
  }

  return response.data.data;
}

export const customersApi = {
  getCustomers: (params: { search?: string; page?: number; pageSize?: number; includePassive?: boolean }) =>
    unwrap(apiClient.get<ApiResponse<PagedResponse<CustomerListItem>>>('/api/customers', { params })),
  getCustomer: (id: string) => unwrap(apiClient.get<ApiResponse<CustomerDetail>>(`/api/customers/${id}`)),
  createCustomer: (payload: CustomerPayload) => unwrap(apiClient.post<ApiResponse<CustomerDetail>>('/api/customers', payload)),
  updateCustomer: (id: string, payload: CustomerPayload) =>
    unwrap(apiClient.put<ApiResponse<CustomerDetail>>(`/api/customers/${id}`, payload)),
};
