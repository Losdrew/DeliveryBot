import { Business } from '@mui/icons-material';
import { Container, List, ListItemButton, ListItemIcon, ListItemText, Paper, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import companyService from '../features/companyService';
import { CompanyPreviewDto } from '../interfaces/company';

export function Companies() {
  const [companies, setCompanies] = useState<CompanyPreviewDto[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await companyService.getCompanies();
        setCompanies(response);
      } catch (error) {
        console.error('Error');
      }
    };

    fetchData();
  }, []);

  return (
    <Container sx={{ p: 4, textAlign: 'center' }}>
      <Paper elevation={3} style={{ padding: '20px' }}>
        <Typography variant="h4" gutterBottom>
          Available Companies
        </Typography>
        <List>
          {companies.map((company) => (
            <ListItemButton key={company.id} component={Link} to={`/company/${company.id}/products`}>
              <ListItemIcon>
                <Business fontSize="medium" color="primary" />
              </ListItemIcon>
              <ListItemText primary={company.name} secondary={company.description} />
            </ListItemButton>
          ))}
        </List>
      </Paper>
    </Container>
  );
}