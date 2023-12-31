import { Alert, Box, Button, Container, Divider, Paper, Snackbar, Typography } from '@mui/material';
import { GridColDef, GridRenderCellParams, GridValueFormatterParams } from '@mui/x-data-grid';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import EditableDataGrid from '../components/EditableDataGrid';
import RobotEditToolbar from '../components/RobotEditToolbar';
import RobotLocationModal from '../components/RobotLocationModal';
import dataService from '../features/dataService';
import robotService from '../features/robotService';
import useAuth from '../hooks/useAuth';
import useStatusConverter from '../hooks/useStatusConverter';
import { RobotStatus } from '../interfaces/enums';
import { GridCompanyRobot } from '../interfaces/grid';

const AdminDashboard = () => {
  const { auth } = useAuth();
  const { t } = useTranslation();
  const { RobotStatusColors, RobotStatusLabels } = useStatusConverter();
  const [companyRobots, setCompanyRobots] = useState<GridCompanyRobot[]>();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [saveStatus, setSaveStatus] = useState<'success' | 'error' | null>(null);
 
  const handleExportData = async () => {
    try {
      const response = await dataService.getExportedData(auth.bearer!);
      
      const link = document.createElement('a');
      const blob = new Blob([response]);

      link.href = window.URL.createObjectURL(blob);
      link.download = 'Data.xlsx';
      link.click();
      window.URL.revokeObjectURL(link.href);
    } catch (error) {
      console.error('Error exporting data', error);
    }
  };

  useEffect(() => {
    const fetchCompanyRobots = async () => {
      try {
        const response = await robotService.getOwnCompanyRobots(auth.bearer!);
        setCompanyRobots(response);
      } catch (error) {
        console.error('Error fetching robots:', error);
      }
    };

    fetchCompanyRobots();
  }, [auth.bearer]) 

  const handleCloseSnackbar = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setSaveStatus(null);
  };

  const saveChanges = async () => {
    try {
      for (const robot of companyRobots) {
        if (robot.isNew) {
          await robotService.createRobot(
            robot.name,
            robot.weightCapacityG,
            robot.volumeCapacityCm3,
            robot.deviceId,
            auth.bearer
          );
        }
        else {
          await robotService.editRobot(
            auth.bearer,
            robot.id,
            robot.name,
            robot.weightCapacityG,
            robot.volumeCapacityCm3,
            robot.deviceId,
          );
        }
      }
      setSaveStatus('success');
    } catch (error) {
      console.error('Error saving company info:', error);
      setSaveStatus('error');
    }
  };

  const companyRobotColumns: GridColDef[] = [
    { field: 'name', headerName: t('name'), width: 170, editable: true },
    { field: 'volumeCapacityCm3', headerName: t('volumeCapacity'), width: 100, editable: true },
    { field: 'weightCapacityG', headerName: t('weightCapacity'), width: 100, editable: true, },
    { field: 'deviceId', headerName: t('deviceId'), width: 170, editable: true },
    { 
      field: 'batteryCharge', 
      headerName: t('battery'), 
      width: 100, 
      valueFormatter: (params: GridValueFormatterParams<number>) => {
        if (params.value == null) {
          return '';
        }
        return `${params.value} %`;
      }
    },
    { 
      field: 'status',
      headerName: t('status'),
      renderCell: (params: GridRenderCellParams<any, RobotStatus>) => (
      <span
        style={{
          padding: '5px',
          borderRadius: '10px',
          backgroundColor: RobotStatusColors[params.value],
        }}
      >
        {RobotStatusLabels[params.value]}
      </span>
    ),
    }
  ];

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        {t('administratorDashboard')}
      </Typography>
      <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
        <Box mb={2}>
          <Typography variant="h6" gutterBottom mb={2}>
            {t('exportCurrentSystemData')}
          </Typography>
          <Button variant="contained" color="primary" onClick={handleExportData}>
            {t('exportData')}
          </Button>
        </Box>
        <Divider />
        <Typography variant="h6" gutterBottom mt={2} mb={2}>
          {t('companyRobots')}
        </Typography>
        <EditableDataGrid
          toolbar={RobotEditToolbar}
          toolbarProps={{
            setModal: setIsModalOpen, 
            rows: companyRobots || [],
            setRows: setCompanyRobots
          }}
          rows={companyRobots || []}
          setRows={setCompanyRobots}
          initialColumns={companyRobotColumns}
        />
        <Divider />
        <Box sx={{mt: 4}} display="flex" justifyContent="center">
          <Button variant="contained" color="primary" onClick={saveChanges}>
            {t('saveChanges')}
          </Button>
          <Snackbar
            open={saveStatus !== null}
            autoHideDuration={5000}
            onClose={handleCloseSnackbar}
          >
            <Alert
              elevation={6}
              variant="filled"
              severity={(saveStatus === 'success' || saveStatus === null) ? 'success' : 'error'}
              onClose={handleCloseSnackbar}
            >
              {saveStatus === 'success' || saveStatus === null
                ? t('changesSavedSuccessfully')
                : t('errorSavingChanges')}
            </Alert>
          </Snackbar>
        </Box>
      </Paper>
      <RobotLocationModal open={isModalOpen} handleClose={handleCloseModal} />
    </Container>
  );
};

export default AdminDashboard;