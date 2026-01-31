import { styled } from '@mui/material';

import backgroundImage from '~/assets/images/seller-bg.png';

const StyledFone = styled('div', { name: 'StyledFone' })(({ theme }) => ({
  backgroundImage: `url(${backgroundImage})`,
  backgroundPosition: 'center center',
  backgroundSize: 'cover',
  height: 90,
  left: 0,
  position: 'absolute',
  right: 0,

  [theme.breakpoints.up('sm')]: {
    height: 150,
  },
}));

export default StyledFone;
