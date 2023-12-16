import { LocalMall, RemoveShoppingCart } from '@mui/icons-material';
import { Box, Button, Container, IconButton, List, ListItem, ListItemIcon, ListItemText, Paper, Typography } from '@mui/material';
import React from 'react';
import { Link } from 'react-router-dom';
import useCart from '../hooks/useCart';

export function Cart() {
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
        <List>
          {cart.map((product) => (
            <ListItem key={product.id}>
              <ListItemIcon>
                <LocalMall fontSize="medium" color="primary" />
              </ListItemIcon>
              <ListItemText
                primary={product.name}
                secondary={
                  <React.Fragment>
                    <Typography variant="body2" color="textSecondary">
                      Price: {product.price}
                    </Typography>
                    <Typography variant="body2" color="textSecondary">
                      Description: {product.description}
                    </Typography>
                  </React.Fragment>
                }
              />
              <IconButton
                  color="secondary"
                  aria-label="Remove from Cart"
                  onClick={() => handleRemoveFromCart(product.id)}
              >
                <RemoveShoppingCart />
              </IconButton>
            </ListItem>
          ))}
        </List>
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
}
