import { Box, styled } from '@mui/material';

const CardLink = styled(Box)(({ theme }) => ({
  backgroundColor: theme.palette.common.white,
  borderRadius: 0,
  boxShadow: theme.shadows[1],
  boxSizing: 'border-box',
  color: theme.palette.text.primary,
  display: 'flex',
  flexDirection: 'column',
  overflow: 'hidden',
  position: 'relative',
  textDecoration: 'none',
  width: '210px',
  margin: '0px auto',

  [theme.breakpoints.up('sm')]: {
    borderRadius: theme.spacing(1.25),
    '&:hover': {
      boxShadow: theme.shadows[2],
      color: theme.palette.text.primary,
      cursor: 'pointer',
      textDecoration: 'none',
    },
  },

  [theme.breakpoints.down('sm')]: {
    width: '150px',
  },

  borderTopLeftRadius: theme.spacing(1.25),
  borderTopRightRadius: theme.spacing(1.25),
}));

export { CardLink };
