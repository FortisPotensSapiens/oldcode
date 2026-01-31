import { styled } from '@mui/system';

const InfoBox = styled('div', { name: 'InfoBox' })(({ theme }) => ({
  boxSizing: 'border-box',
  padding: theme.spacing(1),
  paddingTop: theme.spacing(0),
  paddingBottom: 0,
  position: 'relative',
  display: 'flex',
  height: '100%',
  flexFlow: 'column',
  margin: theme.spacing(1),
  marginTop: 0,
  justifyContent: 'space-between',
  flex: '1',
}));

export default InfoBox;
