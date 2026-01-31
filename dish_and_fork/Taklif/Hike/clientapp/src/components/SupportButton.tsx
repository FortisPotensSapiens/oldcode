import { Button, ButtonProps } from '@mui/material';
import { FC } from 'react';

import { useSupportActions } from '~/contexts';
import { useDownSm } from '~/hooks';

type SupportButtonProps = Pick<ButtonProps, 'size' | 'variant' | 'fullWidth' | 'color'>;

const SupportButton: FC<SupportButtonProps> = ({ children, ...props }) => {
  const isXs = useDownSm();
  const { show } = useSupportActions();

  return (
    <Button color="info" fullWidth={isXs} onClick={show} size="large" variant="contained" {...props}>
      {children ?? (isXs ? 'Связаться' : 'Связаться с нами')}
    </Button>
  );
};

export { SupportButton };
