import LocalMallIcon from '@mui/icons-material/LocalMall';
import {
    Box,
    Button,
    Collapse,
    Container,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography
} from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import addressService from '../features/addressService';
import orderService from '../features/orderService';
import useAuth from '../hooks/useAuth';
import useStatusConverter from '../hooks/useStatusConverter';
import { OrderStatus } from '../interfaces/enums';
import { OrderFullInfo } from '../interfaces/order';

const CustomerOrders = () => {
  const { t } = useTranslation();
  const { auth } = useAuth();
  const { 
    RobotStatusColors, 
    RobotStatusLabels, 
    OrderStatusColors, 
    OrderStatusLabels 
  } = useStatusConverter();
  const [orders, setOrders] = useState<OrderFullInfo[]>([]);
  const [expandedOrderId, setExpandedOrderId] = useState<string | null>(null);

  const handleExpand = (orderId: string) => {
    setExpandedOrderId((prev) => (prev === orderId ? null : orderId));
  };

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        if (auth.bearer) {
          const response = await orderService.getOwnOrders(auth.bearer);
          setOrders(response);
        }
      } catch (error) {
        console.error('Error fetching orders:', error);
      }
    };

    fetchOrders();
  }, [auth.bearer, orders]);

  const handleCancelOrder = async (orderId: string) => {
    try {
      await orderService.cancelOrder(orderId, auth.bearer!);
    } catch (error) {
      console.error('Error');
    }
  };

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        {t('myOrders')}
      </Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{t('date')}</TableCell>
              <TableCell>{t('orderStatus')}</TableCell>
              <TableCell>{t('totalPrice')}</TableCell>
              <TableCell>{t('actions')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {orders.map((order) => (
              <React.Fragment key={order.id}>
                <TableRow>
                  <TableCell>{order.placedDateTime!.toLocaleString()}</TableCell>
                  <TableCell>
                    <span
                      style={{
                        padding: '5px',
                        borderRadius: '10px',
                        backgroundColor: OrderStatusColors[order.orderStatus!],
                      }}
                    >
                      {t(OrderStatusLabels[order.orderStatus!])}
                    </span>
                  </TableCell>
                  <TableCell>
                    $
                    {order.products
                      ?.reduce((accumulated, product) => accumulated + product.price, 0)
                      .toFixed(2)}
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="outlined"
                      color="primary"
                      onClick={() => handleExpand(order.id)}
                    >
                      {expandedOrderId === order.id ? t('hideDetails') : t('viewDetails')}
                    </Button>
                  </TableCell>
                </TableRow>
                <TableRow>
                  <TableCell colSpan={5}>
                    <Collapse in={expandedOrderId === order.id} timeout="auto" unmountOnExit>
                      <Typography variant="h6" gutterBottom>
                        {t('orderDetails')}
                      </Typography>
                      <Table sx={{ mb: 2}}>
                        <TableHead>
                          <TableRow>
                            <TableCell> </TableCell>
                            <TableCell>{t('productName')}</TableCell>
                            <TableCell>{t('volume')}</TableCell>
                            <TableCell>{t('weight')}</TableCell>
                            <TableCell>{t('totalPrice')}</TableCell>
                          </TableRow>
                        </TableHead>
                        <TableBody>
                          {order.products?.map((product) => (
                            <TableRow key={product.id}>
                              <TableCell width="70">
                                <LocalMallIcon fontSize="medium" color="primary" />
                              </TableCell>
                              <TableCell width="150">{product.name}</TableCell>
                              <TableCell>{product.volumeCm3}</TableCell>
                              <TableCell>{product.weightG}</TableCell>
                              <TableCell>${product.price}</TableCell>
                            </TableRow>
                          ))}
                        </TableBody>
                      </Table>
                      <Box>
                        <Typography variant="h6" gutterBottom> {t('orderInfo')} </Typography>
                        <Typography gutterBottom>
                          {t('fullAddress')}: {addressService.getFullAddress(order.orderAddress!)}
                        </Typography>
                        {order.orderStatus !== OrderStatus.Pending && 
                          order.orderStatus !== OrderStatus.Cancelled && (
                          <Typography gutterBottom>
                            {t('shippedDate')}: {order.delivery?.shippedDateTime?.toLocaleString()}
                          </Typography>
                        )}
                        {order.orderStatus === OrderStatus.Delivered && (
                          <Typography gutterBottom>
                            {t('deliveredDate')}: {order.delivery?.deliveredDateTime?.toLocaleString()}
                          </Typography>
                        )}
                        {order.delivery?.robot && (
                          <React.Fragment>
                            <Typography gutterBottom>
                              {t('deliveryRobotName')}: {order.delivery.robot.name}
                            </Typography>
                            <Typography gutterBottom>
                              {t('deliveryRobotStatus')}: 
                              <span
                                style={{
                                  marginLeft: '8px',
                                  padding: '5px',
                                  borderRadius: '10px',
                                  backgroundColor: RobotStatusColors[order.delivery.robot.status!],
                                }}
                              >
                                {t(RobotStatusLabels[order.delivery.robot.status!])}
                              </span>
                            </Typography>
                          </React.Fragment>
                        )}
                      </Box>
                      <Box display="flex" flexDirection="column" alignItems="flex-end">
                        <Typography variant="subtitle1" gutterBottom>
                          {t('totalPrice')}: $
                          {order.products
                            ?.reduce((accumulated, product) => accumulated + product.price, 0)
                            .toFixed(2)}
                        </Typography>
                        {order.orderStatus === OrderStatus.Pending && (
                          <Button
                          variant="outlined"
                          color="error"
                          onClick={() => handleCancelOrder(order.id!)}
                          >
                            {t('cancelOrder')}
                          </Button>
                        )}
                      </Box>
                    </Collapse>
                  </TableCell>
                </TableRow>
              </React.Fragment>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default CustomerOrders;
