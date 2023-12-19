import LocalMallIcon from '@mui/icons-material/LocalMall';
import RemoveShoppingCartIcon from '@mui/icons-material/RemoveShoppingCart';
import {
  Box,
  Button,
  Container,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography
} from '@mui/material';
import { Link } from 'react-router-dom';
import useCart from '../hooks/useCart';
import { useTranslation } from 'react-i18next';

const Cart = () => {
  const { t } = useTranslation();
  const { cart, removeFromCart } = useCart();

  if (cart.length === 0) {
    return (
      <Container>
        <Typography variant="h5" gutterBottom align="center" mt={4}>
          {t('emptyShoppingCart')}
        </Typography>
      </Container>
    );
  }

  const handleRemoveFromCart = (productId: string) => {
    removeFromCart(productId);
  };

  return (
    <Container maxWidth="md" style={{ marginTop: '20px' }}>
      <Paper elevation={3} style={{ padding: '20px' }}>
        <Typography variant="h4" gutterBottom align="center">
          {t('shoppingCart')}
        </Typography>
        <Table sx={{ mb: 2 }}>
          <TableHead>
            <TableRow>
              <TableCell> </TableCell>
              <TableCell>{t('name')}</TableCell>
              <TableCell>{t('description')}</TableCell>
              <TableCell>{t('volume')}</TableCell>
              <TableCell>{t('weight')}</TableCell>
              <TableCell>{t('totalPrice')}</TableCell>
              <TableCell>{t('actions')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {cart.map((product) => (
              <TableRow key={product.id}>
                <TableCell width="70">
                  <LocalMallIcon fontSize="medium" color="primary" />
                </TableCell>
                <TableCell>{product.name}</TableCell>
                <TableCell>{product.description}</TableCell>
                <TableCell>{product.volume}</TableCell>
                <TableCell>{product.weight}</TableCell>
                <TableCell>${product.price}</TableCell>
                <TableCell>
                  <IconButton
                    color="secondary"
                    aria-label={t('removeFromCart')}
                    onClick={() => handleRemoveFromCart(product.id)}
                  >
                    <RemoveShoppingCartIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <Box display="flex" justifyContent="flex-end" mt={2} mb={2}>
          <Button
            variant="contained"
            color="primary"
            component={Link}
            to={`/checkout`}
          >
            {t('checkout')}
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default Cart;
