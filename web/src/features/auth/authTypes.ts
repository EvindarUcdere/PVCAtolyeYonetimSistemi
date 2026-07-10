export type CurrentUser = {
  id: string;
  username: string;
  displayName: string;
  mustChangePassword: boolean;
  roles: string[];
  permissions: string[];
};

export type LoginResponse = {
  accessToken: string;
  expiresAt: string;
  user: CurrentUser;
};
