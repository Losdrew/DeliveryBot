import { AddShoppingCart, LocalMall, RemoveShoppingCart } from '@mui/icons-material';
import {
    Box,
    Container,
    IconButton,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography
} from '@mui/material';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import productService from '../features/productService';
import useCart from '../hooks/useCart';
import { CompanyProductInfoDto } from '../interfaces/product';

export function CompanyProducts() {
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

  const handleAddToCart = (
    productId: string,
    productName: string,
    productDescription: string,
    productVolume : string,
    productWeight : string,
    productPrice: number
  ) => {
    addToCart({ 
      id: productId, 
      name: productName, 
      description: productDescription, 
      volume: productVolume,
      weight: productWeight,
      price: productPrice 
    });
  };

  const handleRemoveFromCart = (productId: string) => {
    removeFromCart(productId);
  };

  return (
    <Container maxWidth="md" style={{ marginTop: '20px' }}>
      <Paper elevation={3} style={{ padding: '20px' }}>
        <Typography variant="h4" gutterBottom align="center">
          Products
        </Typography>
        <Table sx={{ mb: 2}}>
          <TableHead>
            <TableRow>
              <TableCell> </TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Volume</TableCell>
              <TableCell>Weight</TableCell>
              <TableCell>Total Price</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {companyProducts?.map((product) => (
              <TableRow>
                <TableCell width="70">
                  <LocalMall fontSize="medium" color="primary" />
                </TableCell>
                <TableCell>{product.name}</TableCell>
                <TableCell>{product.description}</TableCell>
                <TableCell>{product.volumeCm3}</TableCell>
                <TableCell>{product.weightG}</TableCell>
                <TableCell>${product.price}</TableCell>
                <TableCell>
                  <Box display="flex" flexDirection="row">
                    <IconButton
                        color="primary"
                        aria-label="Add to Cart"
                        onClick={() => handleAddToCart(
                          product.id, 
                          product.name, 
                          product.description, 
                          product.volumeCm3,
                          product.weightG,
                          product.price
                        )}
                    >
                      <AddShoppingCart />
                    </IconButton>
                    <IconButton
                        color="secondary"
                        aria-label="Remove from Cart"
                        onClick={() => handleRemoveFromCart(product.id)}
                    >
                      <RemoveShoppingCart />
                    </IconButton>
                  </Box>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
    </Container>
  );
}