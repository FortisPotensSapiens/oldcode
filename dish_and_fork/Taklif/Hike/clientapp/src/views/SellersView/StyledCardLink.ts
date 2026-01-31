import { styled } from '@mui/material';

import { CardLink } from '~/components';

const StyledCardLink = styled(CardLink)(({ theme }) => ({
  alignItems: 'center',
  display: 'flex',
  flexDirection: 'column',
  height: '100%',
  padding: theme.spacing(3),
  width: '100% !important',
  paddingBottom: theme.spacing(4),
}));

export default StyledCardLink;
