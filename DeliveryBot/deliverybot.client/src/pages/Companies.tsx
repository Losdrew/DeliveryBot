import { Business } from '@mui/icons-material';
import { Container, Icon, List, ListItemButton, ListItemText, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import apiClient from '../config/apiClient';
import { CompanyPreviewDto } from '../interfaces/company';

export function Companies() {
  const [companies, setCompanies] = useState<CompanyPreviewDto[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await apiClient<CompanyPreviewDto[]>('/api/Company/companies');
        setCompanies(response.data);
      } catch (error) {
        console.error('Error');
      }
    };

    fetchData();
  }, []);

  return (
    <Container sx={{ p: 4, textAlign: 'center' }}>
      <Typography variant="h4" gutterBottom>
        Available Companies
      </Typography>
      <List>
        {companies.map((company) => (
          <ListItemButton key={company.id} component={Link} to={`/company/${company.id}`} alignItems="flex-start">
            <Icon color="primary" sx={{ mr: 2, mt: 1.3 }}>
              <Business fontSize="small" />
            </Icon>
            <ListItemText primary={company.name} secondary={company.description} />
          </ListItemButton>
        ))}
      </List>
    </Container>
  );
}