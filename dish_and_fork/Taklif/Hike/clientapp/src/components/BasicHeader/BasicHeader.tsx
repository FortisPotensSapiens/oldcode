import { Typography, TypographyProps } from '@mui/material';
import { FC } from 'react';

import useVariant from './useVariant';

type BasicHeaderProps = TypographyProps & {
  noHeaderTopPadding?: boolean;
  storefront?: boolean;
};

const BasicHeader: FC<BasicHeaderProps> = ({ noHeaderTopPadding, ref, storefront, ...props }) => {
  const variant = useVariant();

  return (
    <Typography
      component="h1"
      display="flex"
      mb={storefront ? 2 : 3}
      pt={{ sm: noHeaderTopPadding ? 0 : 6, xs: 3 }}
      px={{ sm: 0, xs: 2 }}
      sx={{
        wordBreak: 'break-all',
        wordWrap: 'break-word',
      }}
      variant={variant}
      {...props}
    />
  );
};

export { BasicHeader };
export type { BasicHeaderProps };
