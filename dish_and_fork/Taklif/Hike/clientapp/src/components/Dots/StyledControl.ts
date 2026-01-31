import { styled } from '@mui/material';

type StyledControlProps = { active?: boolean };

const StyledControl = styled('button', {
  name: 'StyledControl',
  shouldForwardProp: (prop) => prop !== 'active',
})<StyledControlProps>(({ active, theme }) => ({
  '&:hover': {
    cursor: 'pointer',
  },

  backgroundColor: active ? theme.palette.common.white : theme.palette.grey[400],
  borderRadius: '50%',
  borderWidth: 0,
  height: theme.spacing(1),
  opacity: active ? 0.8 : 1,
  padding: 0,
  width: theme.spacing(1),
}));

export default StyledControl;
export type { StyledControlProps };
