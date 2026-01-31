import { Badge, styled } from '@mui/material';

const StyledBadge = styled(Badge)(({ theme }) => ({
  '& .MuiBadge-badge': {
    borderColor: theme.palette.common.white,
    borderStyle: 'solid',
    borderWidth: 1,
    bottom: 'auto',
    left: 'auto',
    right: 0,
    top: 17,
  },

  position: 'relative',
}));

export { StyledBadge };
