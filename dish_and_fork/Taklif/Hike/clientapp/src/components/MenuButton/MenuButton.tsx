import { Typography } from '@mui/material';
import { FC, MouseEventHandler, ReactNode } from 'react';

import { StyledLink } from './StyledLink';
import { useClick } from './useClick';

type MenuButtonProps = {
  icon: ReactNode;
  hideText?: boolean;
} & (
  | {
      onClick?: MouseEventHandler<HTMLAnchorElement>;
      to: string;
    }
  | {
      onClick: MouseEventHandler<HTMLAnchorElement>;
      to?: never;
    }
);

const MenuButton: FC<MenuButtonProps> = ({ children, hideText, icon, onClick, to = '' }) => {
  const click = useClick(onClick);

  return (
    <StyledLink onClick={click} to={to}>
      {icon}

      <Typography
        color="common.black"
        textAlign="center"
        variant="caption"
        visibility={hideText ? 'hidden' : 'visible'}
      >
        {children}
      </Typography>
    </StyledLink>
  );
};

export { MenuButton };
export type { MenuButtonProps };
