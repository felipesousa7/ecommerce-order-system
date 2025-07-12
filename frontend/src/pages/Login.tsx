import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import {
  Container,
  Box,
  TextField,
  Button,
  Typography,
  Link,
  Paper,
  Grid,
} from '@mui/material';
import { useNavigate, Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { toast } from 'react-toastify';

const loginSchema = z.object({
  email: z.string().email('Email inválido'),
  password: z.string().min(1, 'Senha é obrigatória'),
});

type LoginFormData = z.infer<typeof loginSchema>;

export function Login() {
  const navigate = useNavigate();
  const { login, user } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginFormData) => {
    try {
      await login(data.email, data.password);
      toast.success('Login realizado com sucesso!');
    } catch (error) {
      toast.error('Erro ao realizar login. Verifique suas credenciais.');
    }
  };

  if (user) {
    return <Navigate to="/" replace />;
  }

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(45deg, #1976d2 30%, #42a5f5 90%)',
        p: 2,
      }}
    >
      <Paper
        elevation={3}
        sx={{
          width: '100%',
          maxWidth: 400,
          p: 4,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          borderRadius: 2,
        }}
      >
        <Typography
          component="h1"
          variant="h4"
          sx={{
            mb: 3,
            fontWeight: 'bold',
            color: 'primary.main',
          }}
        >
          Bem-vindo
        </Typography>
        <Typography
          variant="body1"
          sx={{
            mb: 4,
            color: 'text.secondary',
            textAlign: 'center',
          }}
        >
          Faça login para acessar sua conta
        </Typography>
        <Box
          component="form"
          onSubmit={handleSubmit(onSubmit)}
          sx={{ width: '100%' }}
        >
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email"
            autoComplete="email"
            autoFocus
            {...register('email')}
            error={!!errors.email}
            helperText={errors.email?.message}
            sx={{ mb: 2 }}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            label="Senha"
            type="password"
            id="password"
            autoComplete="current-password"
            {...register('password')}
            error={!!errors.password}
            helperText={errors.password?.message}
            sx={{ mb: 3 }}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            size="large"
            sx={{
              py: 1.5,
              mb: 2,
              borderRadius: 2,
              textTransform: 'none',
              fontSize: '1.1rem',
            }}
          >
            Entrar
          </Button>
          <Grid container justifyContent="center">
            <Grid item>
              <Link
                href="/register"
                variant="body2"
                sx={{
                  textDecoration: 'none',
                  '&:hover': {
                    textDecoration: 'underline',
                  },
                }}
              >
                Não tem uma conta? Registre-se
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Paper>
    </Box>
  );
} 