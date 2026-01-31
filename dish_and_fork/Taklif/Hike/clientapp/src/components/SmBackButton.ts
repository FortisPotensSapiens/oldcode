import { styled } from '@mui/material';

import { BackIconButton, BackIconButtonProps } from './BackIconButton';

const SmBackButton = styled(BackIconButton)(({ theme }) => {
  const margin = theme.spacing(2);

  return {
    left: 0,
    marginLeft: margin,
    marginTop: margin,
    position: 'absolute',
    top: 0,
    zIndex: 999,

    [theme.breakpoints.up('sm')]: {
      display: 'none',
    },
  };
});

export { SmBackButton };
export type { BackIconButtonProps as SmBackButtonProps };
