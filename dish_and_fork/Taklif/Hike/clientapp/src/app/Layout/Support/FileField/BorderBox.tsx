import { Box, styled } from '@mui/material';

const BorderBox = styled(Box, { name: 'StyledBordeerBox ' })(({ theme }) => {
  const color = theme.palette.common.grey;
  const padding = theme.spacing(2);
  const stroke = color.replace('#', '%23');
  const { borderRadius } = theme.shape;

  return {
    backgroundImage: `url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg'%3e%3crect width='100%25' height='100%25' fill='none' rx='${borderRadius}' ry='${borderRadius}' stroke='${stroke}' stroke-width='2' stroke-dasharray='4%2c 4' stroke-dashoffset='4' stroke-linecap='butt'/%3e%3c/svg%3e");`,
    backgroundRepeat: 'no-repeat',
    borderRadius,
    color,
    lineHeight: 2.5,
    padding,
  };
});

export { BorderBox };
