import { Box, BoxProps } from '@mui/material';
import { FC } from 'react';

type SelectedImageProps = BoxProps & { path?: string };

const SelectedImage: FC<SelectedImageProps> = ({ path, ...props }) => {
  return (
    <Box color="rgba(255, 255, 255, 0.87)" display="flex" fontSize="5rem" position="relative" width="100%" {...props}>
      {path ? (
        <Box width="100%">
          <img src={path} alt="" width="100%" />
        </Box>
      ) : (
        <Box width="100%" />
      )}
    </Box>
  );
};

export { SelectedImage };
