import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { Box } from '@mui/material';
import { FC } from 'react';

import StyledBreadcrumbs from './StyledBreadcrumbs';

const Breadcrumbs: FC = ({ children }) => (
  <StyledBreadcrumbs separator={<Box component={ChevronRightIcon} ml={0.5} mr={0.5} />}>{children}</StyledBreadcrumbs>
);

export { Breadcrumbs };
