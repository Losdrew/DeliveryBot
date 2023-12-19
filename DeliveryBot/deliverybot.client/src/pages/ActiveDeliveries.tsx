import {
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
import deliveryService from '../features/deliveryService';
import useAuth from '../hooks/useAuth';
import useStatusConverter from '../hooks/useStatusConverter';
import { DeliveryFullInfo } from '../interfaces/delivery';

const ActiveDeliveries = () => {
  const { auth } = useAuth();
  const { t } = useTranslation();
  const { RobotStatusColors, RobotStatusLabels } = useStatusConverter();
  const [activeDeliveries, setActiveDeliveries] = useState<DeliveryFullInfo[]>([]);

  useEffect(() => {
    const fetchActiveDeliveries = async () => {
      try {
        const response = await deliveryService.getActiveDeliveries(auth.bearer!);
        setActiveDeliveries(response);
      } catch (error) {
        console.error('Error fetching pending orders:', error);
      }
    }
      
    fetchActiveDeliveries();
  }, [auth.bearer]);

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        {t('activeDeliveries')}
      </Typography>
      <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>{t('shippedDate')}</TableCell>
              <TableCell>{t('robotName')}</TableCell>
              <TableCell>{t('robotStatus')}</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {activeDeliveries.map((delivery) => (
              <TableRow key={delivery.id}>
                <TableCell>{delivery.shippedDateTime?.toLocaleString()}</TableCell>
                <TableCell>{delivery.robot.name}</TableCell>
                <TableCell>
                  <span
                    style={{
                      padding: '5px',
                      borderRadius: '10px',
                      backgroundColor: RobotStatusColors[delivery.robot.status!],
                    }}
                  >
                    {t(RobotStatusLabels[delivery.robot.status!])}
                  </span>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
    </Container>
  );
};

export default ActiveDeliveries;
