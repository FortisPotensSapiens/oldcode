import { styled, TextField } from '@mui/material';

const StyledTextField = styled(TextField, { name: 'StyledTextField' })(({ theme }) => ({
  '& .MuiFormHelperText-root': {
    boxSizing: 'border-box',
    mt: theme.spacing(-0.125),
    position: 'absolute',
    top: '100%',
    whiteSpace: 'nowrap',
    width: '100%',
  },
}));

export { StyledTextField };
