import { styled } from '@mui/material';

import { Props } from './types';

type StyledItemProps = Props & { hideMd?: boolean };

const StyledItem = styled('li', {
  shouldForwardProp: (prop) => prop !== 'hideMd' && prop !== 'vertical',
})<StyledItemProps>(({ hideMd, theme, vertical }) => {
  const rest = vertical
    ? {
        marginLeft: 0,
        marginTop: theme.spacing(3),
      }
    : {
        marginLeft: theme.spacing(3),
        marginTop: 0,
      };

  return {
    '&:first-of-type': {
      marginLeft: 0,
      marginTop: 0,
    },

    display: hideMd ? 'none' : 'block',
    margin: 0,
    padding: 0,

    ...rest,
  };
});

export type { StyledItemProps };
export default StyledItem;
