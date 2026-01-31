import { TextFieldProps } from '@mui/material';
import { forwardRef } from 'react';

import { StyledTextField } from './StyledTextField';

type InputProps = Omit<TextFieldProps, 'fullWidth' | 'variant'>;

const Input = forwardRef<HTMLDivElement, InputProps>(function Input(props, ref) {
  return <StyledTextField ref={ref} fullWidth variant="outlined" {...props} />;
});

export { Input };
export type { InputProps };
