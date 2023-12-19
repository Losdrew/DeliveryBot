import { useTranslation } from 'react-i18next';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import BusinessIcon from '@mui/icons-material/Business';
import LocalShippingIcon from '@mui/icons-material/LocalShipping';
import AdbIcon from '@mui/icons-material/Adb';
import { Box, Button, Container, IconButton, Typography } from '@mui/material';
import { Link } from 'react-router-dom';

const Home = () => {
 const { t } = useTranslation();

 return (
   <Container>
     <Box textAlign="center" mt={10}>
       <Typography variant="h4" gutterBottom>
         {t('homePageTitle')}
       </Typography>
       <Typography variant="subtitle1" color="textSecondary" paragraph>
         {t('homePageSubtitle1')}
       </Typography>

       <Box mt={4}>
         <IconButton color="primary">
           <BusinessIcon fontSize="large" />
         </IconButton>
         <IconButton color="primary">
           <AdbIcon fontSize="large" />
         </IconButton>
         <IconButton color="primary">
           <LocalShippingIcon fontSize="large" />
         </IconButton>
       </Box>

       <Box mt={5}>
         <Typography variant="h6" gutterBottom>
           {t('homePageSubtitle3')}
         </Typography>
         <Typography variant="body1" paragraph>
           {t('homePageSubtitle4')}
         </Typography>
       </Box>

       <Box mt={5}>
         <Button
           variant="contained"
           color="primary"
           size="large"
           endIcon={<ArrowForwardIcon />}
           component={Link}
           to="/sign-up"
         >
           {t('getStarted')}
         </Button>
       </Box>
     </Box>
   </Container>
 );
};

export default Home;
