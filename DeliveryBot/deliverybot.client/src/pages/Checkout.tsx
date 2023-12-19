import LocalMallIcon from '@mui/icons-material/LocalMall';
import {
    Box,
    Button,
    Container,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TextField,
    Typography
} from '@mui/material';
import React, { useState } from "react";
import orderService from "../features/orderService";
import useAuth from '../hooks/useAuth';
import useCart from '../hooks/useCart';
import { AddressDto } from '../interfaces/address';
import { OrderInfoDto } from "../interfaces/order";

const Checkout = () => {
  const { auth } = useAuth();
  const { cart, clearCart } = useCart();

  const initialAddress: AddressDto = {
    addressLine1: '',
    addressLine2: '',
    addressLine3: '',
    addressLine4: '',
    townCity: '',
    region: '',
    country: '',
    postcode: '',
  };

  const [orderAddress, setOrderAddress] = useState<AddressDto>(initialAddress);
  const [order, setOrder] = useState<OrderInfoDto>();

  const handleAddressChange = (field: keyof AddressDto) => (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setOrderAddress((previousAddress) => ({ ...previousAddress, [field]: event.target.value }));
  };

  const handlePlaceOrder = async () => {
    try {
      const orderProducts = cart.map((product) => 
      {
        return {productId : product.id, quantity : 1 } 
      });
      
      const response = await orderService.createOrder(
        orderAddress, 
        orderProducts, 
        auth.bearer!
      );
      setOrder(response);
      clearCart();
    } catch (error) {
      console.error('Error');
    }
  };

  return (
    <Container>
      {order ? (
        <Typography variant="h5" gutterBottom align="center" mt={4} color="primary">
          Order placed successfully!
        </Typography>
      ) : 
      (
        <React.Fragment>
          <Typography variant="h5" gutterBottom align="center" mt={2} mb={2}>
            Checkout
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <Paper elevation={3} style={{ padding: '20px' }}>
                <Typography variant="h6" gutterBottom>
                  Shipping Address
                </Typography>
                <Grid container spacing={1}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Address First Line"
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.addressLine1}
                      onChange={handleAddressChange('addressLine1')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Address Second Line"
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.addressLine2}
                      onChange={handleAddressChange('addressLine2')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Address Third Line (Optional)"
                      fullWidth
                      margin="normal"
                      value={orderAddress.addressLine3}
                      onChange={handleAddressChange('addressLine3')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Address Fourth Line (Optional)"
                      fullWidth
                      margin="normal"
                      value={orderAddress.addressLine4}
                      onChange={handleAddressChange('addressLine4')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Town/City"
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.townCity}
                      onChange={handleAddressChange('townCity')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Region"
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.region}
                      onChange={handleAddressChange('region')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Country"
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.country}
                      onChange={handleAddressChange('country')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label="Postcode"
                      fullWidth
                      type="number"
                      margin="normal"
                      required
                      value={orderAddress.postcode}
                      onChange={handleAddressChange('postcode')}
                    />
                  </Grid>
                </Grid>
                <Box display="flex" justifyContent="center" mt={2}>
                  <Button variant="contained" color="primary" onClick={handlePlaceOrder}>
                    Place Order
                  </Button>
                </Box>
              </Paper>
            </Grid>
            <Grid item xs={12} md={6}>
              <Paper elevation={3} style={{ padding: '20px' }}>
                <Typography variant="h6" gutterBottom>
                  Order Summary
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
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </Paper>
            </Grid>
          </Grid>
        </React.Fragment>
      )}
    </Container>
  )
};

export default Checkout;