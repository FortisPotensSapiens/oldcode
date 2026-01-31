import { styled } from '@mui/material';
import { NavLink } from 'react-router-dom';

const StyledLink = styled(NavLink, { name: 'StyledLink' })(({ theme }) => {
  const marginV = theme.spacing(-0.75);
  const marginH = theme.spacing(-2);
  const paddingV = theme.spacing(0.75);
  const paddingH = theme.spacing(2);

  return {
    '& svg:first-of-type': {
      marginRight: theme.spacing(0.5),
      position: 'relative',
    },

    alignItems: 'center',
    boxSizing: 'content-box',
    color: theme.palette.text.primary,
    display: 'flex',
    justifyContent: 'stretch',
    justifySelf: 'stretch',
    marginBottom: marginV,
    marginLeft: marginH,
    marginRight: marginH,
    marginTop: marginV,
    paddingBottom: paddingV,
    paddingLeft: paddingH,
    paddingRight: paddingH,
    paddingTop: paddingV,
    textDecoration: 'none',
    width: '100%',
  };
});

export { StyledLink };
