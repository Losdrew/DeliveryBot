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

    return (
        <AuthContext.Provider value={{ auth, setAuth }}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext;