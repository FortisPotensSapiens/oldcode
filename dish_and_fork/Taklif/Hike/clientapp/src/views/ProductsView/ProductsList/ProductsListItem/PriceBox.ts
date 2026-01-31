import { styled } from '@mui/system';

const PriceBox = styled('div', { name: 'PriceBox' })(({ theme }) => {
  return {
    alignItems: 'baseline',
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    marginBottom: theme.spacing(0.1),
    marginTop: 0,

    padding: theme.spacing(2),
    paddingTop: theme.spacing(1),
    paddingBottom: 0,
  };
});

export default PriceBox;
