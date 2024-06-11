import React, { createContext, useContext, useState, ReactNode, useEffect, useRef } from "react";
import { jwtDecode } from 'jwt-decode';
import { refreshToken } from "@/services/authService";
import { IKeycloakResponse, IUser } from "@/abstract/authTypes";

interface StateContextProps {
  user: IUser | null;
  token: string | null;
  kcResponse: IKeycloakResponse | null;
  role: string | null;
  setKcResponse: (kcResponse: IKeycloakResponse) => void;
  logout: () => void;
}

interface ContextProviderProps {
  children: ReactNode;
}

const StateContext = createContext<StateContextProps>({
  user: null,
  token: null,
  kcResponse: null,
  role: null,
  setKcResponse: () => { },
  logout: () => { }
});

export const ContextProvider: React.FC<ContextProviderProps> = ({ children }) => {
  const [kcResponse, setKcResponse] = useState<IKeycloakResponse | null>(null);
  const [token, setToken] = useState<string | null>(localStorage.getItem('ACCESS_TOKEN'));
  const [user, setUser] = useState<IUser | null>(null);
  const [role, setRole] = useState<string | null>(null);
  const timerRef = useRef<NodeJS.Timeout | null>(null);

  const initializeState = () => {
    const storedToken = localStorage.getItem('ACCESS_TOKEN');
    const storedRefreshToken = localStorage.getItem('REFRESH_TOKEN');
    const storedKcResponse = localStorage.getItem('KC_RESPONSE');

    if (storedToken && storedRefreshToken && storedKcResponse) {
      const parsedKcResponse: IKeycloakResponse = JSON.parse(storedKcResponse);
      const parsedUser = jwtDecode<IUser>(parsedKcResponse.access_token);
      setKcResponse(parsedKcResponse);
      setToken(storedToken);
      setUser(parsedUser);
      setRole(parsedUser.realm_access.roles.includes("applications-simpl-snippet-administrator") ? "admin" : "user");
    }
  };

  const scheduleTokenRefresh = (expiresIn: number) => {
    const expirationTime = (expiresIn - 60) * 1000;

    if (timerRef.current) {
      clearTimeout(timerRef.current);
    }

    timerRef.current = setTimeout(async () => {
      try {
        const newToken: IKeycloakResponse = await refreshToken();
        setKcResponse(newToken);
      } catch (error) {
        console.error('Failed to refresh token:', error);
        logout();
      }
    }, expirationTime);
  };

  useEffect(() => {
    initializeState();
  }, []);

  useEffect(() => {
    if (kcResponse) {
      const parsedUser = jwtDecode(kcResponse.access_token) as IUser;
      localStorage.setItem('KC_RESPONSE', JSON.stringify(kcResponse));
      localStorage.setItem('ACCESS_TOKEN', kcResponse.access_token);
      localStorage.setItem('REFRESH_TOKEN', kcResponse.refresh_token);
      
      setToken(kcResponse.access_token);
      setUser(parsedUser);
      setRole(parsedUser.realm_access.roles.includes("applications-simpl-snippet-administrator") ? "admin" : "user");

      scheduleTokenRefresh(kcResponse.expires_in);
    }
  }, [kcResponse]);

  useEffect(() => {
    const handleVisibilityChange = () => {
      if (document.visibilityState === "visible" && kcResponse) {
        scheduleTokenRefresh(kcResponse.expires_in);
      }
    };

    document.addEventListener("visibilitychange", handleVisibilityChange);
    return () => {
      document.removeEventListener("visibilitychange", handleVisibilityChange);
      if (timerRef.current) {
        clearTimeout(timerRef.current);
      }
    };
  }, [kcResponse]);

  const logout = () => {
    setKcResponse(null);
    setToken(null);
    setUser(null);
    setRole(null);
    localStorage.removeItem('KC_RESPONSE');
    localStorage.removeItem('ACCESS_TOKEN');
    localStorage.removeItem('REFRESH_TOKEN');
  };

  return (
    <StateContext.Provider value={{ user, token, kcResponse, role, setKcResponse, logout }}>
      {children}
    </StateContext.Provider>
  );
};

export const useStateContext = (): StateContextProps => useContext(StateContext);
