import { useQuery } from '@tanstack/react-query';
import {
  Container,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Typography,
  Button,
  Box,
  AppBar,
  Toolbar,
  IconButton,
} from '@mui/material';
import { ShoppingCart, Logout } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import api from '../services/api';
import { toast } from 'react-toastify';

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
}

interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
  totalPrice: number;
}

interface Order {
  id: number;
  userId: number;
  userName: string;
  status: number;
  totalAmount: number;
  items: OrderItem[];
  createdAt: string;
  updatedAt: string | null;
}

export function Home() {
  const navigate = useNavigate();
  const { logout } = useAuth();

  const { data: products, isLoading } = useQuery({
    queryKey: ['products'],
    queryFn: async () => {
      const response = await api.get('/Product/available');
      return response.data;
    }
  });

  const handleOrder = async (productId: number) => {
    try {
      const response = await api.post<Order[]>('/Order', {
        items: [
          {
            productId,
            quantity: 1
          }
        ]
      });
      toast.success('Pedido realizado com sucesso!');
      navigate('/orders');
    } catch (error) {
      toast.error('Erro ao realizar pedido. Tente novamente.');
    }
  };

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <Typography>Carregando produtos...</Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ 
      display: 'flex', 
      flexDirection: 'column',
      minHeight: '100vh'
    }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            E-commerce
          </Typography>
          <IconButton color="inherit" onClick={() => navigate('/orders')}>
            <ShoppingCart />
          </IconButton>
          <IconButton color="inherit" onClick={logout}>
            <Logout />
          </IconButton>
        </Toolbar>
      </AppBar>

      <Container 
        maxWidth="lg"
        sx={{ 
          flex: 1, 
          py: 6,
          px: { xs: 6, sm: 8, md: 12 },
          mt: 3
        }}
      >
        <Box sx={{ p: 4 }}>
          <Grid container spacing={4}>
            {products?.map((product: Product) => (
              <Grid item key={product.id} xs={12} sm={6} md={4}>
                <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                  <CardMedia
                    component="img"
                    height="200"
                    image={product.imageUrl}
                    alt={product.name}
                    sx={{
                      objectFit: 'cover',
                      backgroundColor: 'grey.100'
                    }}
                  />
                  <CardContent sx={{ flexGrow: 1 }}>
                    <Typography gutterBottom variant="h5" component="h2">
                      {product.name}
                    </Typography>
                    <Typography>
                      {product.description}
                    </Typography>
                    <Typography variant="h6" color="primary" sx={{ mt: 2 }}>
                      R$ {product.price.toFixed(2)}
                    </Typography>
                  </CardContent>
                  <Box sx={{ p: 2 }}>
                    <Button
                      fullWidth
                      variant="contained"
                      onClick={() => handleOrder(product.id)}
                    >
                      Comprar
                    </Button>
                  </Box>
                </Card>
              </Grid>
            ))}
          </Grid>
        </Box>
      </Container>
    </Box>
  );
} 