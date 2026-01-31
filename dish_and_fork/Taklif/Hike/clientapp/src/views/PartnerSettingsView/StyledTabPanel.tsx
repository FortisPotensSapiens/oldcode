import { TabPanel } from '@mui/lab';
import { styled } from '@mui/material';

const StyledTabPanel = styled(TabPanel, { name: 'StyledTabPanel' })(({ theme }) => ({
  maxWidth: '100%',
  width: '100%',

  [theme.breakpoints.up('sm')]: {
    paddingLeft: 0,
    paddingRight: 0,
  },
}));

export { StyledTabPanel };
