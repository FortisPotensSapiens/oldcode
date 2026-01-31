import { Typography } from '@mui/material';
import { FC } from 'react';

const Sup: FC = ({ children }) => (
  <Typography color="primary.main" display="inline-block" sx={{ transform: 'translateY(-0.5em)' }} variant="caption">
    {children}
  </Typography>
);

export { Sup };
