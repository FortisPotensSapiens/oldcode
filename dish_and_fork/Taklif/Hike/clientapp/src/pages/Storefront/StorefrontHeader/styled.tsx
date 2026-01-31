import FilterListIcon from '@mui/icons-material/FilterList';
import { Paper, styled } from '@mui/material';

export const StorefrontHeaderContainer = styled(Paper)(({ theme }) => ({
  marginBottom: theme.spacing(2),
  justifyContent: 'flex-end',
  display: 'flex',
  width: '100%',
  alignItems: 'flex-end',
  background: 'none',
  border: 'none',
  boxShadow: 'none',
  padding: '0',
}));

export const FilterListIconStyled = styled(FilterListIcon)(() => ({
  cursor: 'pointer',
}));
