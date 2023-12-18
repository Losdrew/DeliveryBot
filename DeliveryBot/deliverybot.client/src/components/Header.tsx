import { ShoppingCart } from '@mui/icons-material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { AppBar, Badge, Box, Button, IconButton, Menu, MenuItem, Toolbar, Typography } from '@mui/material';
import React from 'react';
import { Link as LinkRouter } from "react-router-dom";
import useAuth from '../hooks/useAuth';
import useCart from '../hooks/useCart';
import { Roles } from '../interfaces/enums';

export function Header() {
  const { auth, setAuth } = useAuth();
  const { cart } = useCart();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    if (auth.userId) {
      setAnchorEl(event.currentTarget);
    }
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    localStorage.clear();
    setAuth({userId: undefined, bearer: undefined, role: undefined});
    handleMenuClose();
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography
          variant="h6"
          noWrap
          component="a"
          href="/"
          sx={{
            mr: 2,
            fontWeight: 600,
            color: 'inherit',
            textDecoration: 'none',
          }}
        >
          DeliveryBot
        </Typography>
        <Box sx={{ flexGrow: 1, display: { md: 'flex' }, mr: 1 }}>
          {!auth.userId || auth.role === Roles.Customer && (
            <Button
              component={LinkRouter} 
              to="/companies"
              sx={{ my: 2, color: 'white'}}
            >
              Companies
            </Button>
          )}
          {auth.role === Roles.Customer && (
            <Button
              component={LinkRouter} 
              to="/customer-orders"
              sx={{ my: 2, color: 'white'}}
            >
              Orders
            </Button>
          )}
          {auth.role === Roles.Manager && (
            <Button
              component={LinkRouter} 
              to="/my-company"
              sx={{ my: 2, color: 'white'}}
            >
              Company
            </Button>
          )}
        </Box>
        {!auth.userId && (
          <Button
            variant="contained"
            color="primary"
            size="medium"
            onClick={handleMenuOpen}
            component={LinkRouter}
            to="/login"
          >
            Sign in
          </Button>
        )}
        {auth.userId && (
          <Box>
            {auth.role === Roles.Customer && (
              <IconButton
                  size="medium"
                  edge="start"
                  color="inherit"
                  aria-label="shopping cart"
                  component={LinkRouter}
                  to="/cart"
                >
                <Badge badgeContent={cart.length} color="error">
                  <ShoppingCart sx={{fontSize: 30}} />
                </Badge>
              </IconButton>
            )}
            <IconButton
              size="medium"
              edge="end"
              color="inherit"
              aria-label="user profile"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleMenuOpen}
            >
              <AccountCircleIcon sx={{fontSize: 30}} />
            </IconButton>
            <Menu
              sx={{ mt: '45px' }}
              id="menu-appbar"
              anchorEl={anchorEl}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorEl)}
              onClose={handleMenuClose}
            >
              <MenuItem component={LinkRouter} to="/profile" onClick={handleMenuClose}>
                Profile
              </MenuItem>
              <MenuItem onClick={handleLogout}>
                Logout
              </MenuItem>
            </Menu>
          </Box>
        )}
      </Toolbar>
    </AppBar>
  );
}
