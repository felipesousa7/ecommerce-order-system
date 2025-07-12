import { createContext, useContext, useState } from 'react';
import type { ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

interface AuthContextData {
  user: User | null;
  login: (email: string, password: string) => Promise<void>;
  register: (data: { name: string; email: string; password: string; role: string }) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextData>({} as AuthContextData);

export function AuthProvider({ children }: { children: ReactNode }) {
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  });

  const login = async (email: string, password: string) => {
    const response = await api.post('/auth/login', { email, password });
    const { token, name, email: userEmail, role } = response.data;

    const userData = {
      id: parseInt(response.data.nameid),
      name,
      email: userEmail,
      role
    };

    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(userData));
    setUser(userData);
    navigate('/');
  };

  const register = async (data: { name: string; email: string; password: string; role: string }) => {
    const response = await api.post('/auth/register', data);
    const { token, name, email: userEmail, role } = response.data;

    const userData = {
      id: parseInt(response.data.nameid),
      name,
      email: userEmail,
      role
    };

    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(userData));
    setUser(userData);
    navigate('/');
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setUser(null);
    navigate('/login');
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
} 