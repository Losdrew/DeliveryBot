import { useTranslation } from 'react-i18next';
import { OrderStatus, RobotStatus } from '../interfaces/enums';

const useStatusConverter = () => {
  const { t } = useTranslation();

  const OrderStatusLabels = {
    [OrderStatus.Pending]: t('pending'),
    [OrderStatus.Delivering]: t('delivering'),
    [OrderStatus.PickupAvailable]: t('pickupAvailable'),
    [OrderStatus.Delivered]: t('delivered'),
    [OrderStatus.Cancelled]: t('cancelled'),
  };

  const OrderStatusColors = {
    [OrderStatus.Pending]: '#DE5DF1',
    [OrderStatus.Delivering]: '#9C9405',
    [OrderStatus.PickupAvailable]: '#9631F5',
    [OrderStatus.Delivered]: '#34C42F',
    [OrderStatus.Cancelled]: '#F54C64',
  };

  const RobotStatusLabels = {
    [RobotStatus.Inactive]: t('inactive'),
    [RobotStatus.Idle]: t('idle'),
    [RobotStatus.WaitingForCargo]: t('waitingForCargo'),
    [RobotStatus.Delivering]: t('delivering'),
    [RobotStatus.ReadyForPickup]: t('readyForPickup'),
    [RobotStatus.Returning]: t('returning'),
    [RobotStatus.Maintenance]: t('maintenance'),
    [RobotStatus.Danger]: t('danger'),
  };

  const RobotStatusColors = {
    [RobotStatus.Inactive]: '#44474D',
    [RobotStatus.Idle]: '#C77a1C',
    [RobotStatus.WaitingForCargo]: '#DE5DF1',
    [RobotStatus.Delivering]: '#9C9405',
    [RobotStatus.ReadyForPickup]: '#9631F5',
    [RobotStatus.Returning]: '#34C42F',
    [RobotStatus.Maintenance]: '#2355A6',
    [RobotStatus.Danger]: '#F54C64',
  };

  return {
    OrderStatusLabels,
    OrderStatusColors,
    RobotStatusLabels,
    RobotStatusColors,
  };
};

export default useStatusConverter;