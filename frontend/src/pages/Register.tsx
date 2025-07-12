import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import {
  Box,
  TextField,
  Button,
  Typography,
  Link,
  Paper,
  Grid,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { toast } from 'react-toastify';

const registerSchema = z.object({
  name: z.string().min(1, 'Nome é obrigatório'),
  email: z.string().email('Email inválido'),
  password: z.string().min(6, 'Senha deve ter no mínimo 6 caracteres'),
});

type RegisterFormData = z.infer<typeof registerSchema>;

export function Register() {
  const navigate = useNavigate();
  const { register: registerUser } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema)
  });

  const onSubmit = async (data: RegisterFormData) => {
    try {
      await registerUser({ ...data, role: 'User' });
      toast.success('Registro realizado com sucesso!');
      navigate('/');
    } catch (error) {
      toast.error('Erro ao realizar registro. Tente novamente.');
    }
  };

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
          Criar Conta
        </Typography>
        <Typography
          variant="body1"
          sx={{
            mb: 4,
            color: 'text.secondary',
            textAlign: 'center',
          }}
        >
          Preencha os dados para se registrar
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
            id="name"
            label="Nome"
            autoComplete="name"
            autoFocus
            {...register('name')}
            error={!!errors.name}
            helperText={errors.name?.message}
            sx={{ mb: 2 }}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email"
            autoComplete="email"
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
            autoComplete="new-password"
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
            Registrar
          </Button>
          <Grid container justifyContent="center">
            <Grid item>
              <Link
                href="/login"
                variant="body2"
                sx={{
                  textDecoration: 'none',
                  '&:hover': {
                    textDecoration: 'underline',
                  },
                }}
              >
                Já tem uma conta? Faça login
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Paper>
    </Box>
  );
} 