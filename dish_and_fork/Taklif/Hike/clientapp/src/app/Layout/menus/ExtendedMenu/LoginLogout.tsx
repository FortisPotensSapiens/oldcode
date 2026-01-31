import { useOidc } from '@axa-fr/react-oidc';
import { Login, Logout } from '@mui/icons-material';
import { FC } from 'react';

import { useIsLogged, useLogout } from '~/hooks';

import { StyledLink } from './StyledLink';

const LoginLogout: FC = () => {
  const isLogged = useIsLogged();
  const logout = useLogout();
  const { login } = useOidc();

  if (isLogged) {
    return (
      <StyledLink
        onClick={(event) => {
          event.preventDefault();
          event.stopPropagation();
          logout();
        }}
        to="#"
      >
        <Logout /> Выйти
      </StyledLink>
    );
  }

  return (
    <StyledLink
      onClick={(event) => {
        event.preventDefault();
        login();
      }}
      to="#"
    >
      <Login /> Войти
    </StyledLink>
  );
};

export { LoginLogout };
