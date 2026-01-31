import { styled, Typography } from '@mui/material';

const StyledTypography = styled(Typography, { name: 'StyledTypography' })(({ theme }) => ({
  textAlign: 'center',

  [theme.breakpoints.up('sm')]: {
    textAlign: 'left',
  },
}));

export default StyledTypography;
