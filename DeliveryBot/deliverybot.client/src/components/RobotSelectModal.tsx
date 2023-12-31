import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    MenuItem,
    Select
} from '@mui/material';
import { useEffect, useState } from 'react';
import deliveryService from '../features/deliveryService';
import robotService from '../features/robotService';
import useAuth from '../hooks/useAuth';
import { RobotInfoDto } from '../interfaces/robot';
import { useTranslation } from 'react-i18next';

const RobotSelectModal = ({ open, onClose, orderId }) => {
  const { auth } = useAuth();
  const [availableRobots, setAvailableRobots] = useState<RobotInfoDto[]>([]);
  const [selectedRobotId, setSelectedRobotId] = useState<string>('');
  const { t } = useTranslation();

  useEffect(() => {
    const fetchAvailableRobots = async () => {
      try {
        const response = await robotService.getOwnCompanyRobots(auth.bearer!);
        setAvailableRobots(response);
      } catch (error) {
        console.error('Error fetching available robots:', error);
      }
    };

    if (open) {
      fetchAvailableRobots();
    }
  }, [auth.bearer, open]);

  const handleRobotSelect = (event) => {
    setSelectedRobotId(event.target.value);
  };

  const handleCreateDelivery = async () => {
    await deliveryService.createDelivery(
      orderId, 
      selectedRobotId, 
      auth.bearer!
    );
    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{t('selectRobotForDelivery')}</DialogTitle>
      <DialogContent>
        <Select
          fullWidth
          labelId="robot-select-label"
          id="robot-select"
          value={selectedRobotId}
          onChange={handleRobotSelect}
        >
          {availableRobots.map((robot) => (
            <MenuItem key={robot.id} value={robot.id}>
              {robot.name}
            </MenuItem>
          ))}
        </Select>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          {t('cancel')}
        </Button>
        <Button onClick={handleCreateDelivery} color="primary">
          {t('createDelivery')}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default RobotSelectModal;
