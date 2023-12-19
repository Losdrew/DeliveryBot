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
import { useTranslation } from 'react-i18next';

const Checkout = () => {
  const { t } = useTranslation();
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
          {t('orderPlacedSuccessfully')}
        </Typography>
      ) : 
      (
        <React.Fragment>
          <Typography variant="h5" gutterBottom align="center" mt={2} mb={2}>
            {t('checkout')}
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <Paper elevation={3} style={{ padding: '20px' }}>
                <Typography variant="h6" gutterBottom>
                  {t('shippingAddress')}
                </Typography>
                <Grid container spacing={1}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('addressLine1')}
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.addressLine1}
                      onChange={handleAddressChange('addressLine1')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('addressLine2')}
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.addressLine2}
                      onChange={handleAddressChange('addressLine2')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('addressLine3')}
                      fullWidth
                      margin="normal"
                      value={orderAddress.addressLine3}
                      onChange={handleAddressChange('addressLine3')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('addressLine4')}
                      fullWidth
                      margin="normal"
                      value={orderAddress.addressLine4}
                      onChange={handleAddressChange('addressLine4')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('townCity')}
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.townCity}
                      onChange={handleAddressChange('townCity')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('region')}
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.region}
                      onChange={handleAddressChange('region')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('country')}
                      fullWidth
                      margin="normal"
                      required
                      value={orderAddress.country}
                      onChange={handleAddressChange('country')}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      label={t('postcode')}
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
                    {t('placeOrder')}
                  </Button>
                </Box>
              </Paper>
            </Grid>
            <Grid item xs={12} md={6}>
              <Paper elevation={3} style={{ padding: '20px' }}>
                <Typography variant="h6" gutterBottom>
                  {t('orderSummary')}
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
