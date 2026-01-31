import { OidcProvider, TokenRenewMode, useOidc } from '@axa-fr/react-oidc';
import styled from '@emotion/styled';
import { Spin } from 'antd';
import { FC, useEffect, useMemo, useState } from 'react';

import { useAuthConfig } from '~/api';
import { LoadingSpinner } from '~/components';

const SpinContainer = styled.div`
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const LoadingComponent = () => {
  return (
    <SpinContainer>
      <Spin size="large" />
    </SpinContainer>
  );
};

const SessionLost = () => {
  const { login } = useOidc();

  useEffect(() => {
    login();
  }, [login]);

  return <></>;
};

const AuthProvider: FC = ({ children }) => {
  const { data, isLoading } = useAuthConfig();
  const [isSessionLost, setIsSessionLost] = useState(false);

  const configuration = useMemo(
    () => ({
      storage: localStorage,
      silent_redirect_uri: `${window.location.origin}/authentication/silent-callback`,
      service_worker_relative_url: '/OidcServiceWorker.js',
      service_worker_only: false,
      token_renew_mode: TokenRenewMode.access_token_or_id_token_invalid,
      ...data,
    }),
    [data],
  );

  const onSessionLost = () => {
    setIsSessionLost(true);
  };

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <OidcProvider
      authenticatingComponent={LoadingComponent}
      onSessionLost={onSessionLost}
      loadingComponent={LoadingComponent}
      configuration={configuration as any}
    >
      {isSessionLost && <SessionLost />}
      {children}
    </OidcProvider>
  );
};

export { AuthProvider };
