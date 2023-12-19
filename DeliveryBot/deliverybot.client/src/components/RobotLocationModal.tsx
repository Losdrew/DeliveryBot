import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import 'mapbox-gl/dist/mapbox-gl.css';
import MapboxMap from './MapboxMap';
import { useEffect, useState } from 'react';
import geolocationService from '../features/geolocationService';
import { useTranslation } from 'react-i18next';

const RobotLocationModal = ({ open, handleClose }) => {
  const [route, setRoute] = useState();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchRoute = async () => {
      try {
        const response = await geolocationService.getRoute(
          [35.871651,48.531681],
          [35.868051,48.529541]
        );
        setRoute(response.routes[0].geometry.coordinates);
      } catch (error) {
        console.error('Error fetching robots:', error);
      }
    };

    fetchRoute();
  }, []) 

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>{t('robotLocation')}</DialogTitle>
      <DialogContent>
        <MapboxMap routeCoordinates={route} />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="primary">
          {t('close')}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default RobotLocationModal;
