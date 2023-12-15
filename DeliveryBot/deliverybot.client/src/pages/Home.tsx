import { ArrowForward, Business, LocalShipping, Adb } from '@mui/icons-material';
import { Box, Button, Container, IconButton, Typography } from '@mui/material';
import { Link } from 'react-router-dom';

export function Home() {
  return (
    <Container>
      <Box textAlign="center" mt={10}>
        <Typography variant="h4" gutterBottom>
          Welcome to DeliveryBot
        </Typography>
        <Typography variant="subtitle1" color="textSecondary" paragraph>
          Where Innovation Meets Delivery
        </Typography>

        <Box mt={4}>
          <IconButton color="primary">
            <Business fontSize="large" />
          </IconButton>
          <IconButton color="primary">
            <Adb fontSize="large" />
          </IconButton>
          <IconButton color="primary">
            <LocalShipping fontSize="large" />
          </IconButton>
        </Box>

        <Box mt={5}>
          <Typography variant="h6" gutterBottom>
            Explore the Future of Deliveries with DeliveryBot
          </Typography>
          <Typography variant="body1" paragraph>
            Revolutionizing delivery services through advanced robotics and innovation.
          </Typography>
        </Box>

        <Box mt={5}>
          <Button
            variant="contained"
            color="primary"
            size="large"
            endIcon={<ArrowForward />}
            component={Link}
            to="/sign-up"
          >
            Get Started
          </Button>
        </Box>
      </Box>
    </Container>
  );
}