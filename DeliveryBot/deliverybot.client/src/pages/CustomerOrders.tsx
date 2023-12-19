import { LocalMall } from '@mui/icons-material';
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
import addressService from '../features/addressService';
import orderService from '../features/orderService';
import useAuth from '../hooks/useAuth';
import { OrderStatus, OrderStatusColors, OrderStatusLabels, RobotStatusColors, RobotStatusLabels } from '../interfaces/enums';
import { OrderFullInfo } from '../interfaces/order';

export function CustomerOrders() {
  const { auth } = useAuth();
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
        My Orders
      </Typography>
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Date</TableCell>
              <TableCell>Order Status</TableCell>
              <TableCell>Total Price</TableCell>
              <TableCell>Actions</TableCell>
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
                      {OrderStatusLabels[order.orderStatus!]}
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
                      {expandedOrderId === order.id ? 'Hide Details' : 'View Details'}
                    </Button>
                  </TableCell>
                </TableRow>
                <TableRow>
                  <TableCell colSpan={5}>
                    <Collapse in={expandedOrderId === order.id} timeout="auto" unmountOnExit>
                      <Typography variant="h6" gutterBottom>
                        Order Details:
                      </Typography>
                      <Table sx={{ mb: 2}}>
                        <TableHead>
                          <TableRow>
                            <TableCell> </TableCell>
                            <TableCell>Product Name</TableCell>
                            <TableCell>Volume</TableCell>
                            <TableCell>Weight</TableCell>
                            <TableCell>Total Price</TableCell>
                          </TableRow>
                        </TableHead>
                        <TableBody>
                          {order.products?.map((product) => (
                            <TableRow>
                              <TableCell width="70">
                                <LocalMall fontSize="medium" color="primary" />
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
                        <Typography variant="h6" gutterBottom> Order info: </Typography>
                        <Typography gutterBottom>
                          Full Address: {addressService.getFullAddress(order.orderAddress!)}
                        </Typography>
                        {order.orderStatus !== OrderStatus.Pending && 
                          order.orderStatus !== OrderStatus.Cancelled && (
                          <Typography gutterBottom>
                            Shipped Date: {order.delivery?.shippedDateTime?.toLocaleString()}
                          </Typography>
                        )}
                        {order.orderStatus === OrderStatus.Delivered && (
                          <Typography gutterBottom>
                            Delivered Date: {order.delivery?.deliveredDateTime?.toLocaleString()}
                          </Typography>
                        )}
                        {order.delivery?.robot && (
                          <React.Fragment>
                            <Typography gutterBottom>
                              Delivery Robot Name: {order.delivery.robot.name}
                            </Typography>
                            <Typography gutterBottom>
                              Delivery Robot Status: 
                              <span
                                style={{
                                  marginLeft: '8px',
                                  padding: '5px',
                                  borderRadius: '10px',
                                  backgroundColor: RobotStatusColors[order.delivery.robot.status!],
                                }}
                              >
                                {RobotStatusLabels[order.delivery.robot.status!]}
                              </span>
                            </Typography>
                          </React.Fragment>
                        )}
                      </Box>
                      <Box display="flex" flexDirection="column" alignItems="flex-end">
                        <Typography variant="subtitle1" gutterBottom>
                          Total Price: $
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
                            Cancel Order
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
}
