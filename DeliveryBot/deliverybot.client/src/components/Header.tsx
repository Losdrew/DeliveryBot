import LanguageIcon from '@mui/icons-material/Language';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { AppBar, Badge, Box, Button, IconButton, Menu, MenuItem, Toolbar, Typography } from '@mui/material';
import React from 'react';
import { Link as LinkRouter } from 'react-router-dom';
import useAuth from '../hooks/useAuth';
import useCart from '../hooks/useCart';
import { Roles } from '../interfaces/enums';
import LanguageChangerButton from './LanguageChanger';
import { useTranslation } from 'react-i18next';

export function Header() {
  const { t } = useTranslation();
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
    setAuth({ userId: undefined, bearer: undefined, role: undefined });
    handleMenuClose();
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography
          variant="h6"
          noWrap
          component={LinkRouter}
          to="/"
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
          {(!auth.userId || auth.role === Roles.Customer) && (
            <Button
              component={LinkRouter}
              to="/companies"
              sx={{ my: 2, color: 'white' }}
            >
              {t('companies')}
            </Button>
          )}
          {auth.role === Roles.Customer && (
            <Button
              component={LinkRouter}
              to="/customer-orders"
              sx={{ my: 2, color: 'white' }}
            >
              {t('orders')}
            </Button>
          )}
          {auth.role === Roles.Manager && (
            <Button
              component={LinkRouter}
              to="/my-company"
              sx={{ my: 2, color: 'white' }}
            >
              {t('company')}
            </Button>
          )}
          {auth.role === Roles.Administrator && (
            <Button
              component={LinkRouter}
              to="/admin-dashboard"
              sx={{ my: 2, color: 'white' }}
            >
              {t('dashboard')}
            </Button>
          )}
          {auth.role === Roles.CompanyEmployee && (
            <Button
              component={LinkRouter}
              to="/pending-orders"
              sx={{ my: 2, color: 'white' }}
            >
              {t('pendingOrders')}
            </Button>
          )}
          {auth.role === Roles.CompanyEmployee && (
            <Button
              component={LinkRouter}
              to="/active-deliveries"
              sx={{ my: 2, color: 'white' }}
            >
              {t('activeDeliveries')}
            </Button>
          )}
        </Box>
        <Box>
          <LanguageChangerButton />
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
            {t('signIn')}
          </Button>
        )}
        {auth.userId && (
          <Box display="flex" justifyContent="space-between">
            {auth.role === Roles.Customer && (
              <IconButton
                size="medium"
                color="inherit"
                aria-label="shopping cart"
                component={LinkRouter}
                to="/cart"
              >
                <Badge badgeContent={cart.length} color="error">
                  <ShoppingCartIcon sx={{ fontSize: 30 }} />
                </Badge>
              </IconButton>
            )}
            <IconButton
              size="medium"
              color="inherit"
              aria-label="user profile"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleMenuOpen}
            >
              <AccountCircleIcon sx={{ fontSize: 30 }} />
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
                {t('profile')}
              </MenuItem>
              <MenuItem onClick={handleLogout}>
                {t('logout')}
              </MenuItem>
            </Menu>
          </Box>
        )}
      </Toolbar>
    </AppBar>
  );
}
