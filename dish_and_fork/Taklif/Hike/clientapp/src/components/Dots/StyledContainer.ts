import { styled } from '@mui/material';

const StyledContainer = styled('ul', { name: 'StyledContainer' })(({ theme }) => ({
  display: 'flex',
  flexWrap: 'wrap',
  justifyContent: 'center',
  lineHeight: theme.spacing(1),
  listStyleType: 'none',
  margin: 0,
  marginLeft: theme.spacing(-1),
  padding: 0,
}));

export default StyledContainer;
