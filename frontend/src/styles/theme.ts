import { createTheme } from '@mui/material/styles';

export const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
  typography: {
    fontFamily: '"Roboto", "Helvetica", "Arial", sans-serif',
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8,
        },
      },
    },
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          margin: 0,
          padding: 0,
          minHeight: '100vh',
          width: '100%',
        },
        '#root': {
          minHeight: '100vh',
          width: '100%',
          display: 'flex',
          flexDirection: 'column',
        },
      },
    },
    MuiContainer: {
      styleOverrides: {
        root: {
          minHeight: '100vh',
          width: '100%',
          maxWidth: '100% !important',
          padding: '0 !important',
          margin: '0 !important',
        },
      },
    },
  },
}); 