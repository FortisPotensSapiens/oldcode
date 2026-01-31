import { PhotoCamera } from '@mui/icons-material';
import { Box } from '@mui/material';
import { forwardRef } from 'react';

import { useGetFileById } from '~/api';

import { useChange } from './useChange';

type PhotoFieldProps = {
  block?: boolean;
  name?: string;
  value?: string | null;
  onChange?: (value?: string) => void;
  onUploading?: (value: boolean) => void;
};

const dummy = () => {
  //
};

const PhotoField = forwardRef<HTMLInputElement, PhotoFieldProps>(function PhotoField(
  { block, value, onUploading = dummy, onChange = dummy, name },
  ref,
) {
  const [changeHandler, { isLoading }] = useChange(onChange, onUploading);
  const { data } = useGetFileById(value ?? '');

  const path = data?.path;

  return (
    <Box alignItems="center" display={block ? 'flex' : 'inline-flex'} justifyContent="center">
      <Box
        bgcolor="common.grey"
        borderRadius="50%"
        color="common.white"
        component="label"
        display="block"
        overflow="hidden"
        p={{ sm: 4.5, xs: 3 }}
        position="relative"
        style={path ? { backgroundImage: `url(${path})` } : {}}
        sx={{
          '& svg': {
            opacity: path ? 0 : 1,
          },

          '&:hover': {
            cursor: 'pointer',
          },

          '&:hover svg': {
            opacity: 1,
          },

          backgroundSize: 'cover',
        }}
      >
        <Box
          component={PhotoCamera}
          display="block"
          fontSize={{ sm: '5rem', xs: '3.25rem' }}
          sx={({ transitions }) => ({
            transitionDuration: transitions.duration.shortest,
            transitionProperty: 'opacity',
            transitionTimingFunction: transitions.easing.easeInOut,
          })}
        />

        <Box height="0" overflow="hidden" position="absolute" width="0">
          {!isLoading && <input ref={ref} name={name} onChange={changeHandler} type="file" />}
        </Box>
      </Box>
    </Box>
  );
});

export { PhotoField };
export type { PhotoFieldProps };
