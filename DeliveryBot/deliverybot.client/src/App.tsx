import { Route, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import RequireAuth from './components/RequireAuth';
import { Roles } from './interfaces/enums';
import ActiveDeliveries from './pages/ActiveDeliveries';
import AdminDashboard from './pages/AdminDashboard';
import Cart from './pages/Cart';
import Checkout from './pages/Checkout';
import Companies from './pages/Companies';
import CompanyProducts from './pages/CompanyProducts';
import CustomerOrders from './pages/CustomerOrders';
import Home from './pages/Home';
import Login from './pages/Login';
import OwnCompany from './pages/OwnCompany';
import PendingOrders from './pages/PendingOrders';
import SignUp from './pages/SignUp';

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
          <Route path="/checkout" element={<Checkout />} />
          <Route path="/customer-orders" element={<CustomerOrders />} />
        </Route>

        <Route element={<RequireAuth allowedRoles={[Roles.Manager]} />}>
          <Route path="/my-company" element={<OwnCompany />} />
        </Route>

        <Route element={<RequireAuth allowedRoles={[Roles.Administrator]} />}>
          <Route path="/admin-dashboard" element={<AdminDashboard />} />
        </Route>

        <Route element={<RequireAuth allowedRoles={[Roles.CompanyEmployee]} />}>
          <Route path="/pending-orders" element={<PendingOrders />} />
          <Route path="/active-deliveries" element={<ActiveDeliveries />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
