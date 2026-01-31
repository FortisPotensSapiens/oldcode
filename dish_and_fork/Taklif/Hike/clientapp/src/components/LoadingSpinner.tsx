import { Box, BoxProps, CircularProgress } from '@mui/material';
import { FC } from 'react';

type LoadingSpinnerProps = Omit<BoxProps, 'alignItems' | 'display' | 'justifyContent'>;

const LoadingSpinner: FC<LoadingSpinnerProps> = (props) => (
  <Box
    alignItems="center"
    display="flex"
    height="100%"
    justifyContent="center"
    sx={{ opacity: 0.15 }}
    width="100%"
    {...props}
  >
    <CircularProgress color="primary" />
  </Box>
);

export type { LoadingSpinnerProps };
export { LoadingSpinner };
