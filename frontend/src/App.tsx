import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import { CssBaseline, Box } from '@mui/material';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import { theme } from './styles/theme';
import { AuthProvider } from './contexts/AuthContext';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { Home } from './pages/Home';
import { Orders } from './pages/Orders';
import { PrivateRoute } from './components/PrivateRoute';

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Box sx={{ 
          minHeight: '100vh',
          display: 'flex',
          flexDirection: 'column'
        }}>
          <BrowserRouter>
            <AuthProvider>
              <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route
                  path="/"
                  element={
                    <PrivateRoute>
                      <Home />
                    </PrivateRoute>
                  }
                />
                <Route
                  path="/orders"
                  element={
                    <PrivateRoute>
                      <Orders />
                    </PrivateRoute>
                  }
                />
              </Routes>
            </AuthProvider>
          </BrowserRouter>
          <ToastContainer />
        </Box>
      </ThemeProvider>
    </QueryClientProvider>
  );
}

export default App;
