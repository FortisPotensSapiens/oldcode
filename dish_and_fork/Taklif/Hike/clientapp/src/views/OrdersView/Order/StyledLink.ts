import { styled } from '@mui/material';
import { Link } from 'react-router-dom';

const StyledLink = styled(Link, { name: 'StyledLink' })(({ theme }) => ({
  color: theme.palette.text.primary,
  textDecoration: 'none',
}));

export { StyledLink };
