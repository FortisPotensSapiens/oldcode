import { Paper, styled } from '@mui/material';

const StyledPaper = styled((props) => <Paper elevation={0} {...props} />, { name: 'StyledPaper' })(({ theme }) => ({
  borderRadius: 10,
  height: '100%',
  padding: theme.spacing(2),

  [theme.breakpoints.up('md')]: {
    padding: theme.spacing(4),
  },
}));

export { StyledPaper };
