import { Box, BoxProps, IconButton, IconButtonProps } from '@mui/material';
import { FC, MouseEventHandler } from 'react';

import useClick from './useClick';

type WhiteIconButtonProps = BoxProps &
  Pick<IconButtonProps, 'color'> & {
    href?: string;
    onClick?: MouseEventHandler<HTMLButtonElement>;
  };

const WhiteIconButton: FC<WhiteIconButtonProps> = ({ children, color, href, onClick, ...props }) => {
  const anchorClick = useClick();

  return (
    <Box bgcolor="common.white" borderRadius="50%" display="inline-block" {...props}>
      {typeof href === 'string' ? (
        <IconButton color={color} href={href} onClick={anchorClick} size="large">
          {children}
        </IconButton>
      ) : (
        <IconButton color={color} onClick={onClick} size="large">
          {children}
        </IconButton>
      )}
    </Box>
  );
};

export { WhiteIconButton };
export type { WhiteIconButtonProps };
