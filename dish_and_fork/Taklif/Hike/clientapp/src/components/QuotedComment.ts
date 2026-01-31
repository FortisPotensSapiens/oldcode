import { styled, Typography } from '@mui/material';

const QuotedComment = styled(Typography, { name: 'StyledComments' })(({ theme }) => ({
  borderColor: 'rgba(0, 0, 0, 0.12)',
  borderLeftStyle: 'solid',
  borderLeftWidth: 1,
  color: theme.palette.common.black,
  fontSize: '0.75rem',
  lineHeight: 1.167,
  paddingLeft: theme.spacing(1.5),
  wordBreak: 'break-all',
  wordWrap: 'break-word',
}));

export { QuotedComment };
