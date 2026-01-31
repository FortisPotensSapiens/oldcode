import { Button, ButtonProps } from '@mui/material';
import { FC } from 'react';

import { useConfig } from '~/contexts';
import { useDownSm } from '~/hooks';

type TelegramButtonLinkProps = Pick<ButtonProps, 'size' | 'variant' | 'fullWidth' | 'color'>;

const TelegramButtonLink: FC<TelegramButtonLinkProps> = ({ children = 'Telegram', ...props }) => {
  const { contacts } = useConfig();
  const isXs = useDownSm();

  return (
    <Button
      color="info"
      fullWidth={isXs}
      href={contacts.telegram}
      size="large"
      target="_blank"
      variant="contained"
      {...props}
    >
      {children}
    </Button>
  );
};

export { TelegramButtonLink };
export type { TelegramButtonLinkProps };
