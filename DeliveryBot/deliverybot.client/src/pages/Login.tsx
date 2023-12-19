import { Button, Container, Link, Paper, TextField, Typography } from '@mui/material';
import { useState } from 'react';
import { useNavigate } from 'react-router';
import authService from '../features/authService';
import useAuth from '../hooks/useAuth';
import { AuthResultDto } from '../interfaces/account';
import { useTranslation } from 'react-i18next';

const Login = () => {
 const { setAuth } = useAuth();
 const { t } = useTranslation();
 
 const navigate = useNavigate();

 const [email, setEmail] = useState<string>('');
 const [password, setPassword] = useState<string>('');

 const handleLogin = async () => {
   try {
     const result: AuthResultDto = await authService.signIn(email, password);
     setAuth(result);
     saveToLocalStorage(result);
     navigate("/");
   } catch (error) {
     console.error('Error');
   }
 };

 const saveToLocalStorage = async (authResult : AuthResultDto) => {
   localStorage.setItem('accessToken', authResult.bearer!);
   localStorage.setItem('userId', authResult.userId!);
   localStorage.setItem('role', authResult.role!);
 }

 return (
   <div className="bg-light pb-5" style={{ minHeight: "100vh" }}>
     <Container component="main" maxWidth="xs">
       <Paper elevation={3} style={{ padding: '30px', marginTop: '50px' }}>
         <Typography variant="h5" gutterBottom align="center">
           {t('signIn')}
         </Typography>
         <form>
           <TextField
             label={t('email')}
             variant="outlined"
             margin="normal"
             fullWidth
             value={email}
             onChange={(e) => setEmail(e.target.value)}
           />
           <TextField
             label={t('password')}
             variant="outlined"
             margin="normal"
             fullWidth
             type="password"
             value={password}
             onChange={(e) => setPassword(e.target.value)}
             sx={{mb: 2}}
           />
           <Link href="/sign-up">
             {t('createAnAccount')}
           </Link>
           <Button 
             variant="contained" 
             color="primary" 
             fullWidth 
             style={{ marginTop: '20px' }} 
             onClick={handleLogin}>
             {t('signIn')}
           </Button>
         </form>
       </Paper>
     </Container>
   </div>
 );
};

export default Login;
