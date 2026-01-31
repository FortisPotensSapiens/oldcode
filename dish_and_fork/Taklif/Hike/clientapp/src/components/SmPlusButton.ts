import { styled } from '@mui/material';

import { PlusIconButton, PlusIconButtonProps } from './PlusIconButton';

const SmPlusButton = styled(PlusIconButton)(({ theme }) => {
  const margin = theme.spacing(2);

  return {
    marginLeft: margin,
    marginTop: margin,
    position: 'absolute',
    right: 0,
    top: 0,

    [theme.breakpoints.up('sm')]: {
      display: 'none',
    },
  };
});

export { SmPlusButton };
export type { PlusIconButtonProps as SmPlusButtonProps };
