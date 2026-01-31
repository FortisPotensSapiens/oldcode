import { Badge, styled } from '@mui/material';

const StyledBadge = styled(Badge, { name: 'StyledBadge' })(({ theme }) => ({
  '& .MuiBadge-badge': {
    borderColor: theme.palette.common.white,
    borderStyle: 'solid',
    borderWidth: 1,
    top: 10,
  },
}));

export { StyledBadge };
