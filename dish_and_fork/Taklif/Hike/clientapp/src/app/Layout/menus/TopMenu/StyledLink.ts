import { styled } from '@mui/material';
import { NavLink, NavLinkProps } from 'react-router-dom';

const StyledLink = styled(NavLink, { name: 'StyledLink' })(({ theme }) => ({
  '&:first-of-type': {
    marginLeft: 0,
  },

  color: theme.palette.common.black,
  display: 'block',
  listStyleType: 'none',
  marginLeft: theme.spacing(2),
  textDecoration: 'none',
  whiteSpace: 'nowrap',
}));

export { StyledLink };
export type { NavLinkProps as StyledLinkProps };
