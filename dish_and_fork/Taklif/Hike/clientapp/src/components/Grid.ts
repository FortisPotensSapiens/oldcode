import { Box, styled } from '@mui/material';

const Grid = styled(Box)(({ theme }) => {
  const gap = theme.spacing(3);

  return {
    columnGap: gap,
    display: 'grid',
    gridTemplateColumns: 'repeat(1, 1fr)',
    margin: 0,
    padding: 0,
    position: 'relative',
    rowGap: gap,

    [theme.breakpoints.up('xs')]: {
      gridTemplateColumns: 'repeat(2, 1fr)',
      marginBottom: gap,
    },

    [theme.breakpoints.up('md')]: { gridTemplateColumns: 'repeat(3, 1fr)' },
    [theme.breakpoints.up('lg')]: { gridTemplateColumns: 'repeat(4, 1fr)' },
  };
});

export { Grid };
