import { styled, Tab } from '@mui/material';

const StyledTab = styled(Tab, { name: 'StyledTab' })(({ theme }) => ({
  minHeight: 48,
  textTransform: 'none',

  [theme.breakpoints.up('sm')]: {
    flexBasis: 0,
    flexGrow: 1,
  },
}));

export { StyledTab };
