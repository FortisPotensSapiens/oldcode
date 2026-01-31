import { useOidc } from '@axa-fr/react-oidc';
import { FC, useEffect } from 'react';
import { Outlet, To, useLocation, useNavigate } from 'react-router-dom';

import { Error, LoadingSpinner } from '~/components';
import { useConfig } from '~/contexts';
import { useIsLogged, useMyUserProfile } from '~/hooks';

type ProtectedProps = {
  role?: string;
  redirect?: To;
};

const Protected: FC<ProtectedProps> = ({ children, redirect, role }) => {
  const { storageKeys } = useConfig();
  const { login } = useOidc();
  const { state } = useLocation();
  const isLogged = useIsLogged();
  const navigate = useNavigate();
  const { data, isLoading } = useMyUserProfile();

  useEffect(() => {
    if (!isLogged) {
      login();
    }
  }, [isLogged, login, state, navigate, storageKeys.logoutCallback]);

  if (!isLogged || isLoading) {
    return <LoadingSpinner />;
  }

  if (role && !data?.roles?.includes(role)) {
    return <Error title="Запрещено">У Вас нет доступа к данной странице</Error>;
  }

  return <>{children ?? <Outlet />}</>;
};

export { Protected };
