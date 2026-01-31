import { useOidc } from '@axa-fr/react-oidc';
import { Login } from '@mui/icons-material';
import { Box } from '@mui/material';
import { FC, ReactNode } from 'react';

import { MenuButton } from '~/components';

type AuthButtonProps = { text?: ReactNode };

const AuthButton: FC<AuthButtonProps> = ({ text }) => {
  const { login } = useOidc();

  return (
    <MenuButton icon={<Login />} onClick={() => login()}>
      Войти
      <Box height={0} overflow="hidden">
        {text}
      </Box>
    </MenuButton>
  );
};

export { AuthButton };
export type { AuthButtonProps };
