import { Route, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import RequireAuth from './components/RequireAuth';
import { Roles } from './interfaces/enums';
import { Companies } from './pages/Companies';
import { CompanyProducts } from './pages/CompanyProducts';
import { Home } from './pages/Home';
import { Login } from './pages/Login';
import { SignUp } from './pages/SignUp';
import { Cart } from './pages/Cart';

function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<Home />} />
        <Route path="/sign-up" element={<SignUp />} />
        <Route path="/login" element={<Login />} />
        <Route path="/companies" element={<Companies />} />
        <Route path="/company/:companyId/products" element={<CompanyProducts />} />
      
        <Route element={<RequireAuth allowedRoles={[Roles.Customer]} />}>
          <Route path="/cart" element={<Cart />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
