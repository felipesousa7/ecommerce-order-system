import { useQuery } from '@tanstack/react-query';
import {
  Container,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Box,
  AppBar,
  Toolbar,
  IconButton,
  Grid,
  Card,
  CardContent,
  Divider,
} from '@mui/material';
import { ArrowBack, Logout, Home } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import api from '../services/api';

interface Order {
  id: number;
  productName: string;
  status: number;
  createdAt: string;
  totalAmount: number;
  userName: string;
  items: OrderItem[];
}

interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
  totalPrice: number;
}

enum OrderStatus {
  Received = 0,
  AwaitingPayment = 1,
  PaymentApproved = 2,
  PaymentRejected = 3,
  StockReserved = 4,
  StockCancelled = 5,
  Error = 6
}

const getStatusText = (status: OrderStatus): string => {
  switch (status) {
    case OrderStatus.Received:
      return 'Recebido';
    case OrderStatus.AwaitingPayment:
      return 'Aguardando Pagamento';
    case OrderStatus.PaymentApproved:
      return 'Pagamento Aprovado';
    case OrderStatus.PaymentRejected:
      return 'Pagamento Rejeitado';
    case OrderStatus.StockReserved:
      return 'Estoque Reservado';
    case OrderStatus.StockCancelled:
      return 'Reserva de Estoque Cancelada';
    case OrderStatus.Error:
      return 'Erro';
    default:
      return 'Status Desconhecido';
  }
};

export function Orders() {
  const navigate = useNavigate();
  const { logout } = useAuth();

  const { data: orders, isLoading } = useQuery({
    queryKey: ['orders'],
    queryFn: async () => {
      const response = await api.get('/Order');
      return response.data;
    }
  });

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
        <Typography>Carregando pedidos...</Typography>
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
            Meus Pedidos
          </Typography>
          <IconButton color="inherit" onClick={() => navigate('/')}>
            <Home />
          </IconButton>
          <IconButton color="inherit" onClick={logout}>
            <Logout />
          </IconButton>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ flex: 1, mt: 3 }}>
        <Box sx={{ p: 4 }}>
          {isLoading ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
              <Typography>Carregando pedidos...</Typography>
            </Box>
          ) : orders && orders.length > 0 ? (
            <Grid container spacing={4}>
              {orders.map((order: Order) => (
                <Grid item xs={12} md={6} lg={4} key={order.id}>
                  <Card>
                    <CardContent>
                      <Typography variant="h6" gutterBottom>
                        Pedido #{order.id}
                      </Typography>
                      <Typography variant="body2" color="text.secondary" gutterBottom>
                        Cliente: {order.userName}
                      </Typography>
                      <Typography variant="body2" color="text.secondary" gutterBottom>
                        Status: {getStatusText(order.status as OrderStatus)}
                      </Typography>
                      <Typography variant="body2" color="text.secondary" gutterBottom>
                        Total: R$ {order.totalAmount.toFixed(2)}
                      </Typography>
                      <Typography variant="body2" color="text.secondary" gutterBottom>
                        Data: {new Date(order.createdAt).toLocaleDateString()}
                      </Typography>
                      <Divider sx={{ my: 2 }} />
                      <Typography variant="subtitle2" gutterBottom>
                        Itens:
                      </Typography>
                      {order.items.map((item) => (
                        <Box key={item.id} sx={{ mb: 1 }}>
                          <Typography variant="body2">
                            {item.productName} - {item.quantity}x R$ {item.unitPrice.toFixed(2)}
                          </Typography>
                        </Box>
                      ))}
                    </CardContent>
                  </Card>
                </Grid>
              ))}
            </Grid>
          ) : (
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
              <Typography>Nenhum pedido encontrado.</Typography>
            </Box>
          )}
        </Box>
      </Container>
    </Box>
  );
} 