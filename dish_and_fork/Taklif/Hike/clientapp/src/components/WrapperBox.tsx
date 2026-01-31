import { Box, BoxProps } from '@mui/material';
import { FC } from 'react';

type WrapperBoxProps = BoxProps;

const WrapperBox: FC<WrapperBoxProps> = (props) => (
  <Box
    alignItems="center"
    boxSizing="border-box"
    display="flex"
    justifyContent="space-between"
    maxWidth={({ breakpoints }) => breakpoints.values.xl}
    px={{ sm: 3, xs: 0 }}
    py={0}
    width={1}
    {...props}
  />
);

export type { WrapperBoxProps };
export { WrapperBox };
