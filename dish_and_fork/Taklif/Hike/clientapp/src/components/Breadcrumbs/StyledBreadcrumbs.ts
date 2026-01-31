import { Breadcrumbs, styled } from '@mui/material';

const StyledBreadcrumbs = styled(Breadcrumbs, { name: 'StyledBreadcrumbs' })(({ theme }) => ({
  display: 'none',
  marginBottom: theme.spacing(3),
  marginTop: theme.spacing(6),

  [theme.breakpoints.up('sm')]: {
    display: 'block',
  },
}));

export default StyledBreadcrumbs;
