import { Box, BoxProps } from '@mui/material';
import { FC, ReactNode } from 'react';

import { BasicHeader, BasicHeaderProps, SmBackButton, SmBackButtonProps } from '~/components';
import { getHomepagePath } from '~/routing';

type PageLayoutProps = Omit<BoxProps, 'title'> &
  Pick<SmBackButtonProps, 'href'> &
  Pick<BasicHeaderProps, 'noHeaderTopPadding'> & {
    title: ReactNode;
  } & {
    beforeHeader?: ReactNode;
  };

const margins = { sm: 0, xs: 7 };

const PageLayout: FC<PageLayoutProps> = ({
  beforeHeader,
  children,
  href = getHomepagePath(),
  title,
  noHeaderTopPadding,
  ...props
}) => {
  return (
    <Box display="flex" flexDirection="column" height={1} mb={3} mx="auto" position="relative" {...props}>
      <SmBackButton href={href} />

      {beforeHeader && beforeHeader}

      <BasicHeader
        flexDirection="column"
        justifyContent={{ sm: 'flex-start', xs: 'center' }}
        ml={margins}
        mr={margins}
        noHeaderTopPadding={noHeaderTopPadding}
        textAlign="center"
      >
        {title}
      </BasicHeader>

      {children}
    </Box>
  );
};

export { PageLayout };
export type { PageLayoutProps };
