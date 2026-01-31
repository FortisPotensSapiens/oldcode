import { LoadingButton } from '@mui/lab';
import { styled } from '@mui/material';

const StyledIconButton = styled(LoadingButton, { name: 'StyledIconButton' })(({ theme }) => ({
  borderRadius: '50%',
  flexGrow: 0,
  marginRight: theme.spacing(1),
  minWidth: 'auto',
  padding: theme.spacing(0.5),
  width: 'auto',
}));

export { StyledIconButton };
