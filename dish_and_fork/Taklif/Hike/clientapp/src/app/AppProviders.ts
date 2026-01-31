import { SnackbarProvider } from 'notistack';
import { combineProviders } from 'react-combine-provider';
import { HelmetProvider } from 'react-helmet-async';
import { BrowserRouter } from 'react-router-dom';

import { ApiV1Provider } from '~/api';
import { CartProvider, ConfigProvider, SupportProvider } from '~/contexts';
import { ThemeProvider } from '~/theme';
import AntdThemeProvider from '~/theme/antd';

import { AuthProvider, LocalizationProvider, QueryProvider } from './providers';
import { DndAppProvider } from './providers/DndAppProvider';

const AppProviders = combineProviders(
  [
    SnackbarProvider,
    ConfigProvider,
    LocalizationProvider,
    ThemeProvider,
    QueryProvider,
    AuthProvider,
    ApiV1Provider,
    CartProvider,
    HelmetProvider,
    BrowserRouter,
    SupportProvider,
    AntdThemeProvider,
    DndAppProvider,
  ].reverse(),
);

export { AppProviders };
