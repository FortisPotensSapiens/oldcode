import { Box, styled } from '@mui/material';

const StyledWrapper = styled(Box, { name: 'StyledWrapper' })(({ theme }) => ({
  backgroundColor: theme.palette.background.default,
  left: 0,
  position: 'fixed',
  right: 0,
  top: 0,
  zIndex: 99,
  height: `calc(100% - ${theme.spacing(7)})`,
  overflowY: 'auto',

  [theme.breakpoints.up('xs')]: {
    overflowY: 'scroll',
    '-webkit-overflow-scrolling': 'touch',
  },
}));

const StyledWrapperInside = styled('div')`
  height: 100%;
  position: relative;
`;

const ContactsWrapper = styled('div')`
  margin: 1rem;
`;

export { ContactsWrapper, StyledWrapper, StyledWrapperInside };
