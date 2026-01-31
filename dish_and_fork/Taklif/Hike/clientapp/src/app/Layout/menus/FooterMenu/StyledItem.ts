import { Button, styled } from '@mui/material';

type StyledItemProps = { active?: boolean };

const StyledItem = styled(Button, {
  name: 'StyledItem',
  shouldForwardProp: (prop) => prop !== 'active',
})<StyledItemProps>(({ active, theme }) => ({
  alignItems: 'center',
  borderRadius: 0,
  color: active ? theme.palette.primary.main : theme.palette.text.secondary,
  display: 'flex',
  flexBasis: 0,
  flexDirection: 'column',
  flexGrow: 1,
  justifyContent: 'center',
  margin: 0,
  padding: 0,
  minWidth: 'auto',
  textTransform: 'none',
}));

export { StyledItem };
export type { StyledItemProps };
