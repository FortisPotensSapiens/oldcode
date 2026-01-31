import { styled } from '@mui/material';

const StyledControl = styled('button', { name: 'StyledControl' })(() => ({
  '&:hover': {
    cursor: 'pointer',
  },

  backgroundPosition: 'center center',
  backgroundSize: 'contain',
  backgroundRepeat: 'no-repeat',
  borderRadius: 10,
  borderWidth: 0,
  display: 'block',
  height: 100,
  padding: 0,
  width: 100,
}));

export default StyledControl;
