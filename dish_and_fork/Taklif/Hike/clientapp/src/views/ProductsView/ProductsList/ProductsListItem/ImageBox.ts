import { styled } from '@mui/material';

const ImageBox = styled('div', { name: 'ImageBox' })(({ theme }) => {
  const borderRadius = theme.spacing(1.25);

  return {
    backgroundColor: theme.palette.grey[500],
    backgroundPosition: 'center center',
    backgroundRepeat: 'no-repeat',
    backgroundSize: 'contain',
    width: '100%',

    borderTopLeftRadius: borderRadius,
    borderTopRightRadius: borderRadius,
  };
});

export default ImageBox;
