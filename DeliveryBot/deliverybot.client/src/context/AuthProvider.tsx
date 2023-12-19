import { createContext, useEffect, useState } from "react";
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

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    const bearer = localStorage.getItem("accessToken");
    const role = localStorage.getItem("role");

    if (!auth.userId && userId && bearer && role) {
      setAuth({userId, bearer, role})
    }
  }, [auth.userId]);

  return (
    <AuthContext.Provider value={{ auth, setAuth }}>
      {children}
    </AuthContext.Provider>
  )
}

export default AuthContext;