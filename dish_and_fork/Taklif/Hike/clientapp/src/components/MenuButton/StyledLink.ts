import { styled } from '@mui/material';
import { Link } from 'react-router-dom';

const StyledLink = styled(Link, { name: 'StyledLink' })(({ theme }) => ({
  '&:hover': {
    textDecoration: 'none',
  },

  alignItems: 'center',
  color: theme.palette.common.black,
  display: 'flex',
  flexDirection: 'column',
  paddingBottom: theme.spacing(0.25),
  paddingLeft: theme.spacing(0.75),
  paddingRight: theme.spacing(0.75),
  paddingTop: theme.spacing(0.25),
  textDecoration: 'none',
}));

export { StyledLink };
