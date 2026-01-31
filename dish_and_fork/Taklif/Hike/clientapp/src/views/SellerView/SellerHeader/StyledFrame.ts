import { styled } from '@mui/material';

const StyledFrame = styled('div', { name: 'StyledFrame' })(({ theme }) => ({
  boxShadow: theme.shadows[1],
  display: 'flex',
  marginTop: theme.spacing(11.25),
  maxWidth: 630,
  padding: theme.spacing(3),
  position: 'relative',

  [theme.breakpoints.down('sm')]: {
    paddingTop: theme.spacing(7.75),
  },

  [theme.breakpoints.up('sm')]: {
    backgroundColor: theme.palette.common.white,
    borderRadius: theme.spacing(2.5),
  },
}));

export default StyledFrame;
