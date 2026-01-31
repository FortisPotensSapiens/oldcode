import { Box, BoxProps } from '@mui/material';
import { Property } from 'csstype';
import { FC, useMemo } from 'react';

type SmBoxProps = Omit<BoxProps, 'display'> & {
  display?: Property.Display;
  hideSmall?: boolean;
};

const SmBox: FC<SmBoxProps> = ({ display: sm = 'flex', hideSmall, ...props }) => {
  const display = useMemo(() => (hideSmall ? { sm, xs: 'none' } : sm), [sm, hideSmall]);

  return <Box display={display} {...props} />;
};

export { SmBox };
export type { SmBoxProps };
