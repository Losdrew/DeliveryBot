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

const Cart = () => {
  const { cart, removeFromCart } = useCart();

  if (cart.length === 0) {
    return (
      <Container>
        <Typography variant="h5" gutterBottom align="center" mt={4}>
          Your shopping cart is empty
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
          Shopping Cart
        </Typography>
        <Table sx={{ mb: 2}}>
          <TableHead>
            <TableRow>
              <TableCell> </TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Volume</TableCell>
              <TableCell>Weight</TableCell>
              <TableCell>Total Price</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {cart.map((product) => (
              <TableRow>
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
                  aria-label="Remove from Cart"
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
            Checkout
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default Cart;