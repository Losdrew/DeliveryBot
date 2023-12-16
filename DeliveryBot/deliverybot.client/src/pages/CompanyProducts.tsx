import { AddShoppingCart, LocalMall, RemoveShoppingCart } from '@mui/icons-material';
import { Container, IconButton, List, ListItem, ListItemIcon, ListItemText, Paper, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import productService from '../features/productService';
import useAuth from '../hooks/useAuth';
import useCart from '../hooks/useCart';
import { Roles } from '../interfaces/enums';
import { CompanyProductInfoDto } from '../interfaces/product';

export function CompanyProducts() {
  const { auth } = useAuth();
  const { companyId } = useParams();
  const { addToCart, removeFromCart } = useCart();
  const [companyProducts, setCompanyProducts] = useState<CompanyProductInfoDto[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await productService.getCompanyProducts(companyId!);
        setCompanyProducts(response);
      } catch (error) {
        console.error('Error');
      }
    };

    fetchData();
  }, [companyId]);

  return (
    <Container maxWidth="md" style={{ marginTop: '20px' }}>
      <Paper elevation={3} style={{ padding: '20px' }}>
        <Typography variant="h4" gutterBottom align="center">
          Products
        </Typography>
        <List>
          {companyProducts.map((product) => (
            <ListItem key={product.id}>
              <ListItemIcon>
                <LocalMall fontSize="medium" color="primary" />
              </ListItemIcon>
              <ListItemText
                primary={product.name}
                secondary={
                  <React.Fragment>
                    <Typography variant="body2" color="textSecondary">
                      Price: {product.price}
                    </Typography>
                    <Typography variant="body2" color="textSecondary">
                      Description: {product.description}
                    </Typography>
                  </React.Fragment>
                }
              />
            </ListItem>
          ))}
        </List>
      </Paper>
    </Container>
  );
}