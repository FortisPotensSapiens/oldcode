import { Paper, styled } from '@mui/material';

const StyledPaper = styled(Paper, { name: 'StyledPaper' })(({ theme }) => ({
  [theme.breakpoints.down('sm')]: {
    backgroundColor: 'transparent',
    borderWidth: 0,
    boxShadow: 'none',
  },
}));

export { StyledPaper };
