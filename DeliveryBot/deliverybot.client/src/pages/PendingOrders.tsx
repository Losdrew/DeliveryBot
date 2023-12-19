import {
    Button,
    Container,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography
} from '@mui/material';
import { useEffect, useState } from 'react';
import RobotSelectModal from '../components/RobotSelectModal';
import addressService from '../features/addressService';
import orderService from '../features/orderService';
import useAuth from '../hooks/useAuth';
import { OrderStatusColors, OrderStatusLabels } from '../interfaces/enums';
import { OrderFullInfo } from '../interfaces/order';

const PendingOrders = () => {
  const { auth } = useAuth();
  const [pendingOrders, setPendingOrders] = useState<OrderFullInfo[]>([]);
  const [selectedOrderId, setSelectedOrderId] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const fetchPendingOrders = async () => {
      try {
        const response = await orderService.getPendingOrders(auth.bearer!);
        setPendingOrders(response);
      } catch (error) {
        console.error('Error fetching pending orders:', error);
      }
    }
      
    fetchPendingOrders();
  }, [auth.bearer, isModalOpen]);

  const handleOpenModal = (orderId) => {
    setSelectedOrderId(orderId);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        Pending Orders
      </Typography>
      <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Placed Date</TableCell>
              <TableCell>Address</TableCell>
              <TableCell>Products</TableCell>
              <TableCell>Order Status</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {pendingOrders.map((order) => (
              <TableRow key={order.id}>
                <TableCell>{order.placedDateTime?.toLocaleString()}</TableCell>
                <TableCell>{addressService.getFullAddress(order.orderAddress)}</TableCell>
                <TableCell>{order.products?.map(p => p.name).join(", ")}</TableCell>
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
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={() => handleOpenModal(order.id)}
                  >
                    Create Delivery
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
      <RobotSelectModal open={isModalOpen} onClose={handleCloseModal} orderId={selectedOrderId} />
    </Container>
  );
};

export default PendingOrders;