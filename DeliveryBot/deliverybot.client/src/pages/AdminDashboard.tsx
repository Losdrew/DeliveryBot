import { Box, Button, Container, Divider, Paper, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import EditableDataGrid from '../components/EditableDataGrid';
import dataService from '../features/dataService';
import robotService from '../features/robotService';
import useAuth from '../hooks/useAuth';
import { GridCompanyRobot } from '../interfaces/grid';
import RobotEditToolbar from '../components/RobotEditToolbar';
import RobotLocationModal from '../components/RobotLocationModal';
import { RobotStatus, RobotStatusColors, RobotStatusLabels } from '../interfaces/enums';
import { GridActionsCellItem, GridColDef, GridRenderCellParams, GridValueFormatterParams } from '@mui/x-data-grid';

export function AdminDashboard() {
  const { auth } = useAuth();
  const [companyRobots, setCompanyRobots] = useState<GridCompanyRobot[]>();
  const [isModalOpen, setIsModalOpen] = useState(false);

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

  const companyRobotColumns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 170, editable: true },
    { field: 'volumeCapacityCm3', headerName: 'Volume Capacity', width: 100, editable: true },
    { field: 'weightCapacityG', headerName: 'Weight Capacity', width: 100, editable: true, },
    { field: 'deviceId', headerName: 'Device Id', width: 170, editable: true },
    { 
      field: 'batteryCharge', 
      headerName: 'Battery', 
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
      headerName: 'Status',
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
        Administrator Dashboard
      </Typography>
      <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
        <Box mb={2}>
          <Typography variant="h6" gutterBottom mb={2}>
            Export current system data:
          </Typography>
          <Button variant="contained" color="primary" onClick={handleExportData}>
            Export Data
          </Button>
        </Box>
        <Divider />
          <Typography variant="h6" gutterBottom mt={2} mb={2}>
            Company Robots
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
      </Paper>
      <RobotLocationModal open={isModalOpen} handleClose={handleCloseModal} />
    </Container>
  );
}
