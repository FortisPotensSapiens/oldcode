import { Paper, styled } from '@mui/material';

const StyledPaper = styled(Paper, { name: 'StyledPaper' })(({ theme }) => ({
  borderRadius: Number(theme.shape.borderRadius) * 2.5,
  padding: theme.spacing(3),
  width: '100%',

  [theme.breakpoints.down('sm')]: {
    // backgroundColor: 'transparent',
    borderRadius: 0,
    borderWidth: 0,
    boxShadow: 'none',
    paddingBottom: theme.spacing(1),
    paddingLeft: theme.spacing(2),
    paddingRight: theme.spacing(2),
    paddingTop: theme.spacing(1),
  },
}));

export { StyledPaper };
