import { styled } from '@mui/material';

const StyledImage = styled('div', { name: 'StyledImage' })(({ theme }) => ({
  backgroundColor: theme.palette.grey['400'],
  backgroundPosition: 'center center',
  backgroundSize: 'contain',
  borderRadius: '50%',
  boxSizing: 'border-box',
  flexShrink: 0,
  height: 120,
  width: 120,

  [theme.breakpoints.down('sm')]: {
    borderColor: theme.palette.common.white,
    borderStyle: 'solid',
    borderWidth: 3,
    height: 126,
    left: '50%',
    marginLeft: -63,
    marginTop: -126,
    position: 'absolute',
    width: 126,
  },
}));

export default StyledImage;
