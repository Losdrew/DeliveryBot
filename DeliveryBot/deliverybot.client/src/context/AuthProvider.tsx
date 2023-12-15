import { createContext, useState } from "react";
import { AuthResultDto } from "../interfaces/account";

interface AuthContextProps {
    auth: AuthResultDto;
    setAuth: React.Dispatch<React.SetStateAction<AuthResultDto>>;
}

interface AuthProviderProps {
    children: React.ReactNode;
}

const AuthContext = createContext({} as AuthContextProps);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [auth, setAuth] = useState<AuthResultDto>({});

    const [userId] = useState(localStorage.getItem("userId") || undefined);
    const [bearer] = useState(localStorage.getItem("bearer") || undefined);
    const [role] = useState(localStorage.getItem("role") || undefined);

    if (userId && !auth.userId) {
        setAuth({userId, bearer, role})
    }

    return (
        <AuthContext.Provider value={{ auth, setAuth }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;