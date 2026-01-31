import { styled } from '@mui/material';

import { Props } from './types';

const StyledContainer = styled('ul', { shouldForwardProp: (prop) => prop !== 'vertical' })<Props>(
  ({ theme, vertical }) => ({
    display: 'flex',
    flexDirection: vertical ? 'column' : 'row',
    margin: 0,
    padding: 0,
    width: vertical ? 94 : 'auto',
  }),
);

export default StyledContainer;
