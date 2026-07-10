const tokenKey = 'pvc_atolye_access_token';

export const authStorage = {
  getToken: () => localStorage.getItem(tokenKey),
  setToken: (token: string) => localStorage.setItem(tokenKey, token),
  clear: () => localStorage.removeItem(tokenKey),
};
