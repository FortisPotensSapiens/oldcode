import { styled } from '@mui/material';

import { Link } from '~/components';

const StyledLink = styled(Link, { name: 'StyledLink' })(({ theme }) => {
  const paddingX = theme.spacing(2);
  const paddingY = theme.spacing(1.5);

  return {
    '& svg:first-of-type': {
      marginRight: theme.spacing(1),
    },

    alignItems: 'center',
    color: theme.palette.text.primary,
    display: 'flex',
    marginBottom: 1,
    paddingBottom: paddingY,
    paddingLeft: paddingX,
    paddingRight: paddingX,
    paddingTop: paddingY,
    textDecoration: 'none',
  };
});

const StyledA = styled('a', { name: 'StyledLink' })(({ theme }) => {
  const paddingX = theme.spacing(2);
  const paddingY = theme.spacing(1.5);

  return {
    '& svg:first-of-type': {
      marginRight: theme.spacing(1),
    },

    alignItems: 'center',
    color: theme.palette.text.primary,
    display: 'flex',
    marginBottom: 1,
    paddingBottom: paddingY,
    paddingLeft: paddingX,
    paddingRight: paddingX,
    paddingTop: paddingY,
    textDecoration: 'none',
  };
});

export { StyledA, StyledLink };
