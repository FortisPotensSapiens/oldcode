import { Box } from '@mui/material';
import { FC, ReactNode } from 'react';

type StorefrontLayoutProps = {
  isLoading?: boolean;
  pagination?: ReactNode;
  title: ReactNode;
  height?: string;
};

const StorefrontLayout: FC<StorefrontLayoutProps> = ({ children, isLoading, pagination, title, height = '100%' }) => (
  <Box display="flex" flexDirection="column" height={height}>
    {title}

    <Box flexGrow={1} mb={2} position="relative">
      {children}

      <Box display="flex" justifyContent="center" paddingTop={3}>
        {pagination}
      </Box>

      {isLoading && null}
    </Box>
  </Box>
);

export { StorefrontLayout };
export type { StorefrontLayoutProps };
