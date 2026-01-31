import { styled } from '@mui/material';

type StyledContainerProps = { scrolled?: boolean };

const StyledContainer = styled('ul', {
  name: 'StyledContainer',
  shouldForwardProp: (prop) => prop !== 'scrolled',
})<StyledContainerProps>(({ scrolled, theme }) => ({
  '&::-webkit-scrollbar': {
    direction: 'rtl',
    height: 5,
    width: 5,
  },
  '&::-webkit-scrollbar-thumb': {
    background: 'rgba(0, 0, 0, 0.1)',
    borderRadius: 4,
    direction: 'rtl',
  },
  '&::-webkit-scrollbar-track': {
    borderRadius: 4,
    direction: 'rtl',
  },
  listStyleType: 'none',
  margin: 0,
  overflowY: scrolled ? 'scroll' : undefined,
  padding: 0,
}));

export type { StyledContainerProps };
export default StyledContainer;
