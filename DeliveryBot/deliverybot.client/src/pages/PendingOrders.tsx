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
import { useTranslation } from 'react-i18next';
import RobotSelectModal from '../components/RobotSelectModal';
import addressService from '../features/addressService';
import orderService from '../features/orderService';
import useAuth from '../hooks/useAuth';
import useStatusConverter from '../hooks/useStatusConverter';
import { OrderFullInfo } from '../interfaces/order';

const PendingOrders = () => {
  const { t } = useTranslation();
  const { auth } = useAuth();
  const { OrderStatusColors, OrderStatusLabels } = useStatusConverter();
  const [pendingOrders, setPendingOrders] = useState<OrderFullInfo[]>([]);
  const [selectedOrderId, setSelectedOrderId] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const fetchPendingOrders = async () => {
      try {
        const response = await orderService.getPendingOrders(auth.bearer!);
        setPendingOrders(response);
      } catch (error) {
        console.error('Error fetching pending orders:', error);
      }
    };

    fetchPendingOrders();
  }, [auth.bearer, isModalOpen]);

  const handleOpenModal = (orderId: string) => {
    setSelectedOrderId(orderId);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        {t('pendingOrders')}
      </Typography>
      <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{t('placedDate')}</TableCell>
              <TableCell>{t('address')}</TableCell>
              <TableCell>{t('products')}</TableCell>
              <TableCell>{t('orderStatus')}</TableCell>
              <TableCell>{t('actions')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {pendingOrders.map((order) => (
              <TableRow key={order.id}>
                <TableCell>{order.placedDateTime?.toLocaleString()}</TableCell>
                <TableCell>{addressService.getFullAddress(order.orderAddress)}</TableCell>
                <TableCell>{order.products?.map((p) => p.name).join(', ')}</TableCell>
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
                    {t('createDelivery')}
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
