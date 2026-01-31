import { styled, Typography } from '@mui/material';

const EllipsisText = styled(Typography, { name: 'EllipsisText' })(({ theme }) => ({
  marginTop: theme.spacing(0.5),
  overflow: 'hidden',
  textOverflow: 'ellipsis',
  whiteSpace: 'nowrap',
}));

export { EllipsisText };
