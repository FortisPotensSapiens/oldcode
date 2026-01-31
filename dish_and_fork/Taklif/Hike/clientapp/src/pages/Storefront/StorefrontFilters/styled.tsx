import { Box, styled } from '@mui/material';
import { Skeleton } from 'antd';

export const StorefrontFiltersBlockContainer = styled(Box)(({ theme }) => ({
  paddingBottom: theme.spacing(4),
}));

export const StyledPapaer = styled(Box)(({ theme }) => ({
  marginBottom: theme.spacing(4),
  position: 'sticky',
  top: '0',
  zIndex: '10',
  padding: theme.spacing(2),
}));

export const StyledSkeleton = styled(Skeleton)`
  min-width: 150px;
`;
