import {
    ExpandMore as ExpandMoreIcon
} from '@mui/icons-material';
import {
    Box,
    Button,
    Collapse,
    Container,
    Divider,
    IconButton,
    List,
    Paper,
    TextField,
    Typography
} from '@mui/material';
import { GridColDef } from '@mui/x-data-grid';
import { useEffect, useState } from 'react';
import EditableDataGrid from '../components/EditableDataGrid';
import companyService from '../features/companyService';
import productService from '../features/productService';
import useAuth from '../hooks/useAuth';
import { OwnCompanyInfoDto } from '../interfaces/company';
import { GridCompanyAddress, GridCompanyEmployee, GridCompanyProduct } from '../interfaces/grid';
import { Link } from 'react-router-dom';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import EditToolbar from '../components/EditToolbar';

export function OwnCompany() {
  const { auth } = useAuth();
  const [company, setCompany] = useState<OwnCompanyInfoDto>();
  
  const [companyName, setCompanyName] = useState<string | undefined>();
  const [companyDescription, setCompanyDescription] = useState<string | undefined>();
  const [companyWebsiteUrl, setCompanyWebsiteUrl] = useState<string | undefined>();
  const [companyAddresses, setCompanyAddresses] = useState<GridCompanyAddress[]>();
  const [companyEmployees, setCompanyEmployees] = useState<GridCompanyEmployee[]>();
  const [companyProducts, setCompanyProducts] = useState<GridCompanyProduct[]>();

  const [expandAddresses, setExpandAddresses] = useState(true);
  const [expandEmployees, setExpandEmployees] = useState(true);
  const [expandProducts, setExpandProducts] = useState(true);

  const [saveStatus, setSaveStatus] = useState<'success' | 'error' | null>(null);

  useEffect(() => {
    const fetchCompanyInfo = async () => {
      try {
        const companyResponse = await companyService.getOwnCompany(auth.bearer!);
        setCompany(companyResponse);

        setCompanyName(company?.name);
        setCompanyDescription(company?.description);
        setCompanyWebsiteUrl(company?.websiteUrl);
        setCompanyAddresses(company?.companyAddresses);
        setCompanyEmployees(company?.companyEmployees);

        const productResponse = await productService.getCompanyProducts(company.id);
        setCompanyProducts(productResponse);
      } catch (error) {
        console.error('Error fetching company info:', error);
      }
    };

    fetchCompanyInfo();
  }, [auth.bearer, company?.id]) 

  const handleCloseSnackbar = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setSaveStatus(null);
  };

  const saveChanges = async () => {
    try {
      const response = await companyService.editCompany(
        auth.bearer!,
        companyName,
        companyDescription,
        companyWebsiteUrl,
        companyAddresses,
        companyEmployees
      );
      setCompany(response);
      setSaveStatus('success');
    } catch (error) {
      console.error('Error saving company info:', error);
      setSaveStatus('error');
    }
  };

  const companyAddressColumns: GridColDef[] = [
    { field: 'addressLine1', headerName: 'Address Line 1', width: 170, editable: true },
    { field: 'addressLine2', headerName: 'Address Line 2', width: 150, editable: true },
    { field: 'addressLine3', headerName: 'Address Line 3', width: 120, editable: true },
    { field: 'addressLine4', headerName: 'Address Line 4', width: 120, editable: true },
    { field: 'townCity', headerName: 'Town/City', width: 125, editable: true },
    { field: 'region', headerName: 'Region', width: 125, editable: true },
    { field: 'country', headerName: 'Country', width: 125, editable: true },
    { field: 'postcode', headerName: 'Postcode', width: 70, editable: true },
  ];

  const companyEmployeeColumns: GridColDef[] = [
    { field: 'email', headerName: 'Email', width: 170, editable: true },
    { field: 'firstName', headerName: 'First Name', width: 150, editable: true },
    { field: 'lastName', headerName: 'Last Name', width: 150, editable: true, },
  ];

  const companyProductColumns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 170, editable: true },
    { field: 'description', headerName: 'Description', width: 150, editable: true },
    { field: 'volumeCm3', headerName: 'Volume', width: 100, editable: true, },
    { field: 'weightG', headerName: 'Weight', width: 100, editable: true },
    { field: 'price', headerName: 'Price', width: 100, editable: true },
  ];

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        My Company
      </Typography>
      {company ? (
        <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
          <Typography variant="h5" gutterBottom sx={{ mb: 2 }}>
            Company Details:
          </Typography>
          <TextField
            fullWidth
            label="Name"
            variant="outlined"
            margin="normal"
            value={companyName || ''}
            onChange={(e) => setCompanyName(e.target.value)}
          />
          <TextField
            fullWidth
            label="Description"
            variant="outlined"
            margin="normal"
            value={companyDescription || ''}
            onChange={(e) => setCompanyDescription(e.target.value)}
          />
          <TextField
            fullWidth
            label="Website URL"
            variant="outlined"
            margin="normal"
            value={companyWebsiteUrl || ''}
            onChange={(e) => setCompanyWebsiteUrl(e.target.value)}
          />
          <Divider />
          <Typography variant="h6" gutterBottom mt={3}>
            Company Addresses
            <IconButton onClick={() => setExpandAddresses(!expandAddresses)}>
              <ExpandMoreIcon />
            </IconButton>
          </Typography>
          <Collapse in={expandAddresses}>
            <EditableDataGrid
              toolbar={EditToolbar}
              toolbarProps={{
                rows: companyAddresses || [],
                setRows: setCompanyAddresses
              }}
              rows={companyAddresses || []}
              setRows={setCompanyAddresses}
              initialColumns={companyAddressColumns}
            />
          </Collapse>
          <Divider />
          <Typography variant="h6" gutterBottom mt={3}>
            Company Employees
            <IconButton onClick={() => setExpandEmployees(!expandEmployees)}>
              <ExpandMoreIcon />
            </IconButton>
          </Typography>
          <Collapse in={expandEmployees}>
            <EditableDataGrid
              toolbar={EditToolbar}
              toolbarProps={{
                rows: companyEmployees || [],
                setRows: setCompanyEmployees
              }}
              rows={companyEmployees || []}
              setRows={setCompanyEmployees}
              initialColumns={companyEmployeeColumns}
            />
          </Collapse>
          <Divider />
          <Typography variant="h6" gutterBottom mt={3}>
            Company Products
            <IconButton onClick={() => setExpandProducts(!expandProducts)}>
              <ExpandMoreIcon />
            </IconButton>
          </Typography>
          <Collapse in={expandProducts}>
            <EditableDataGrid
              toolbar={EditToolbar}
              toolbarProps={{
                rows: companyProducts || [],
                setRows: setCompanyProducts
              }}
              rows={companyProducts || []}
              setRows={setCompanyProducts}
              initialColumns={companyProductColumns}
            />
          </Collapse>
          <Divider />
          <Box sx={{mt: 4}} display="flex" justifyContent="center">
            <Button  variant="contained" color="primary" onClick={saveChanges}>
              Save Changes
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
                  ? 'Changes saved successfully!'
                  : 'Error saving changes. Please try again.'}
              </Alert>
          </Snackbar>
          </Box>
        </Paper>
      ) : (
        <Paper elevation={3} style={{ padding: '20px', paddingBottom: '30px' }}>
          <Typography variant="subtitle1" gutterBottom align="center">
            You haven't created your company yet.
          </Typography>
          <Box sx={{mt: 2}} display="flex" justifyContent="center">
            <Button variant="contained" color="primary" component={Link} to="/my-company/create">
              Create Company
            </Button>
          </Box>
        </Paper>
      )}
    </Container>
  );
};
