import CssBaseline from "@mui/material/CssBaseline";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { Outlet } from "react-router-dom";
import { Header } from "./Header";

const darkTheme = createTheme({
  palette: {
    mode: "dark",
  },
});

const Layout = () => {
  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <Header />
      <main className="App">
        <Outlet />
      </main>
    </ThemeProvider>
  );
};

export default Layout;
