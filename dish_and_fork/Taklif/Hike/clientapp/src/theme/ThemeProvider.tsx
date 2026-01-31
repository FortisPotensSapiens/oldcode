import { CssBaseline, ThemeProvider as MuiThemeProvider } from '@mui/material';
import { FC, useState } from 'react';

import createTheme from './createTheme';

const ThemeProvider: FC = ({ children }) => {
  const [state] = useState(createTheme);

  return (
    <MuiThemeProvider theme={state}>
      <CssBaseline />
      {children}
    </MuiThemeProvider>
  );
};

export { ThemeProvider };
