import { styled } from '@mui/material';
import { Link } from 'react-router-dom';

const StyledLink = styled(Link)(({ theme }) => ({
  alignSelf: 'flex-start',
  display: 'block',

  [theme.breakpoints.up('xs')]: {
    height: 48,
    marginLeft: 0,
  },

  [theme.breakpoints.up('sm')]: {
    height: 70,
    marginLeft: theme.spacing(-2.5),
  },
}));

export default StyledLink;
