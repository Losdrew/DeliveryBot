import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import {
  Box,
  Button,
  Collapse,
  Container,
  Divider,
  IconButton,
  Paper,
  TextField,
  Typography
} from '@mui/material';
import Alert from '@mui/material/Alert';
import Snackbar from '@mui/material/Snackbar';
import { GridColDef } from '@mui/x-data-grid';
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import EditToolbar from '../components/EditToolbar';
import EditableDataGrid from '../components/EditableDataGrid';
import companyService from '../features/companyService';
import productService from '../features/productService';
import useAuth from '../hooks/useAuth';
import { OwnCompanyInfoDto } from '../interfaces/company';
import { GridCompanyAddress, GridCompanyEmployee, GridCompanyProduct } from '../interfaces/grid';
import { useTranslation } from 'react-i18next';

const OwnCompany = () => {
  const { t } = useTranslation();
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
  }, [auth.bearer, company?.id]);

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
    { field: 'addressLine1', headerName: t('addressLine1'), width: 170, editable: true },
    { field: 'addressLine2', headerName: t('addressLine2'), width: 150, editable: true },
    { field: 'addressLine3', headerName: t('addressLine3'), width: 120, editable: true },
    { field: 'addressLine4', headerName: t('addressLine4'), width: 120, editable: true },
    { field: 'townCity', headerName: t('townCity'), width: 125, editable: true },
    { field: 'region', headerName: t('region'), width: 125, editable: true },
    { field: 'country', headerName: t('country'), width: 125, editable: true },
    { field: 'postcode', headerName: t('postcode'), width: 70, editable: true },
  ];

  const companyEmployeeColumns: GridColDef[] = [
    { field: 'email', headerName: t('email'), width: 170, editable: true },
    { field: 'firstName', headerName: t('firstName'), width: 150, editable: true },
    { field: 'lastName', headerName: t('lastName'), width: 150, editable: true, },
  ];

  const companyProductColumns: GridColDef[] = [
    { field: 'name', headerName: t('name'), width: 170, editable: true },
    { field: 'description', headerName: t('description'), width: 150, editable: true },
    { field: 'volumeCm3', headerName: t('volume'), width: 100, editable: true, },
    { field: 'weightG', headerName: t('weight'), width: 100, editable: true },
    { field: 'price', headerName: t('price'), width: 100, editable: true },
  ];

  return (
    <Container>
      <Typography variant="h5" gutterBottom align="center" mt={3} mb={2}>
        {t('myCompany')}
      </Typography>
      {company ? (
        <Paper elevation={3} style={{ padding: '20px', paddingBottom: '20px' }}>
          <Typography variant="h5" gutterBottom sx={{ mb: 2 }}>
            {t('companyDetails')}
          </Typography>
          <TextField
            fullWidth
            label={t('name')}
            variant="outlined"
            margin="normal"
            value={companyName || ''}
            onChange={(e) => setCompanyName(e.target.value)}
          />
          <TextField
            fullWidth
            label={t('description')}
            variant="outlined"
            margin="normal"
            value={companyDescription || ''}
            onChange={(e) => setCompanyDescription(e.target.value)}
          />
          <TextField
            fullWidth
            label={t('websiteUrl')}
            variant="outlined"
            margin="normal"
            value={companyWebsiteUrl || ''}
            onChange={(e) => setCompanyWebsiteUrl(e.target.value)}
          />
          <Divider />
          <Typography variant="h6" gutterBottom mt={3}>
            {t('companyAddresses')}
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
            {t('companyEmployees')}
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
            {t('companyProducts')}
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
          <Box sx={{ mt: 4 }} display="flex" justifyContent="center">
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
      ) : (
        <Paper elevation={3} style={{ padding: '20px', paddingBottom: '30px' }}>
          <Typography variant="subtitle1" gutterBottom align="center">
            {t('noCompanyMessage')}
          </Typography>
          <Box sx={{ mt: 2 }} display="flex" justifyContent="center">
            <Button variant="contained" color="primary" component={Link} to="/my-company/create">
              {t('createCompany')}
            </Button>
          </Box>
        </Paper>
      )}
    </Container>
  );
};

export default OwnCompany;
