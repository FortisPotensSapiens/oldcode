import { styled } from '@mui/material';

type StyledContainerProps = { zIndex?: number };

const StyledContainer = styled('nav', {
  name: 'StyledContainer',
  shouldForwardProp: (prop) => prop !== 'zIndex',
})<StyledContainerProps>(({ theme, zIndex }) => ({
  alignItems: 'stretch',
  backgroundColor: theme.palette.common.white,
  bottom: 0,
  boxShadow: theme.shadows[1],
  display: 'flex',
  height: 56,
  listStyleType: 'none',
  margin: 0,
  padding: 0,
  position: 'fixed',
  width: '100%',
  zIndex: 99,
  transform: 'translate3d(0,0,0)',
}));

export { StyledContainer };
export type { StyledContainerProps };
