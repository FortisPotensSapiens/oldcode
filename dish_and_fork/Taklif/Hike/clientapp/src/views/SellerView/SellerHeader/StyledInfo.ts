import { styled } from '@mui/material';

const StyledInfo = styled('div', { name: 'StyledInfo' })(({ theme }) => ({
  columnGap: theme.spacing(2),
  display: 'block',
  flexGrow: 1,
  gridTemplateAreas: '"title type" "description description"',
  gridTemplateColumns: '1fr auto',
  gridTemplateRows: 'auto 1fr',
  rowGap: theme.spacing(1),

  [theme.breakpoints.down('sm')]: {
    marginTop: theme.spacing(0.5),
  },

  [theme.breakpoints.up('sm')]: {
    display: 'grid',
    marginLeft: theme.spacing(3),
  },
}));

export default StyledInfo;
