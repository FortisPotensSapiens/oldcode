import AuthLayout from "@/components/auth-layout";
import Layout from "@/components/layout";
import { AUTH_PATHS, NOT_FOUND_PATH } from "@/constants/paths";
import type { AppProps } from "next/app";
import { useRouter } from "next/router";
import "@/styles/global.scss";
import { SnackbarProvider, closeSnackbar } from 'notistack';
import { Button } from "@mui/material";
import { useEffect } from "react";
import * as amplitude from '@amplitude/analytics-browser';

export default function App({ Component, pageProps }: AppProps) {
  const router = useRouter();
  useEffect(() => {
    // Perform localStorage action
    if (window)
      amplitude.init('f032d7653493fe2f5ae96e737fedbab1');
    const registrationQueryParams = router.asPath;
    console.log("REGISTRATION QUERY PARAMS");
    console.log(registrationQueryParams);
    if (registrationQueryParams && !(localStorage.getItem('registrationQueryParams')))
      localStorage.setItem("registrationQueryParams", registrationQueryParams);
  }, [])
  if (AUTH_PATHS.includes(router.pathname)) {
    return (
      <SnackbarProvider
        preventDuplicate={true}
        action={(snackbarId) => (
          <Button onClick={() => closeSnackbar(snackbarId)} variant="contained" color="success">
            Close
          </Button>
        )}
      >
        <AuthLayout>
          <Component {...pageProps} />
        </AuthLayout>
      </ SnackbarProvider>
    );
  }

  if (router.pathname === NOT_FOUND_PATH) {
    return (
      <SnackbarProvider
        preventDuplicate={true}
        action={(snackbarId) => (
          <Button onClick={() => closeSnackbar(snackbarId)} variant="contained" color="success">
            Close
          </Button>
        )}
      >
        <Component {...pageProps} />
      </SnackbarProvider>
    );
  }

  return (
    <SnackbarProvider
      preventDuplicate={true}
      action={(snackbarId) => (
        <Button onClick={() => closeSnackbar(snackbarId)} variant="contained" color="success">
          Close
        </Button>
      )}
    >
      <Layout>
        <Component {...pageProps} />
      </Layout>
    </SnackbarProvider>
  );
}
