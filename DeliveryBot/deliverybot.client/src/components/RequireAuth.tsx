import { Navigate, Outlet, useLocation } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import { AuthResultDto } from "../interfaces/account";

interface RequireAuthProps {
    allowedRoles?: string[];
}

const RequireAuth : React.FC<RequireAuthProps> = ({ allowedRoles }) => {
    const { auth } = useAuth() as { auth: AuthResultDto };
    const location = useLocation();

    return (
        auth?.role != undefined && allowedRoles?.includes(auth?.role)
            ? <Outlet />
            : auth?.userId
                ? <Navigate to="/unauthorized" state={{ from: location }} replace />
                : <Navigate to="/login" state={{ from: location }} replace />
    );
}

export default RequireAuth;