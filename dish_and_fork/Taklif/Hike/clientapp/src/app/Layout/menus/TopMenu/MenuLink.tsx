import { Box } from '@mui/material';
import { FC } from 'react';

import { StyledLink, StyledLinkProps } from './StyledLink';

const MenuLink: FC<StyledLinkProps> = ({ children, ...props }) => (
  <StyledLink {...props}>
    {({ isActive }) =>
      isActive ? (
        <Box color="primary.main" component="span">
          {children}
        </Box>
      ) : (
        children
      )
    }
  </StyledLink>
);

export { MenuLink };
